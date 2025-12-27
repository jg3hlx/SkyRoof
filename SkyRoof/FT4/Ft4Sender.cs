using System.Security.Cryptography;
using System.Text;
using VE3NEA;

namespace SkyRoof
{
  public class Ft4Sender : IDisposable
  {  
    public enum Mode { Idle, Tuning, Sending }


    private byte[] MessageChars = new byte[NativeFT4Coder.ENCODE_MESSAGE_LENGTH + 1];
    private float[] AudioSamples = new float[NativeFT4Coder.ENCODE_SAMPLE_COUNT];
    private float[] Waveform = new float[NativeFT4Coder.ENCODE_SAMPLE_COUNT];

    private float txAudioFrequency = 1500;
    private object lockObj = new();
    private Thread? WorkerThread;
    private bool Stopping;
    private Ft4Slot Slot = new();


    public int PttOnMargin = 500; // milliseconds
    public int PttOffMargin = 500;

    public OutputSoundcard<float> Soundcard = new();
    public Mode SenderMode { get; private set; } = Mode.Idle;

    public event EventHandler? BeforeTransmit;
    public event EventHandler? AfterTransmit;

    public float TxAudioFrequency { get => txAudioFrequency; set { lock (lockObj) { txAudioFrequency = value; } }}

    public void StartTuning() => SetMode(Mode.Tuning);
    public void StartSending() => SetMode(Mode.Sending);
    public void Stop() => SetMode(Mode.Idle);
    public bool TxOdd;

    public void SetMessage(string message)
    {
      // generate samples
      int len = Math.Min(message.Length, MessageChars.Length - 1);
      Array.Clear(MessageChars);
      Encoding.ASCII.GetBytes(message, 0, len, MessageChars, 0);

      NativeFT4Coder.encode_ft4(MessageChars, ref txAudioFrequency, AudioSamples);

      // copy to waveform
      lock (lockObj)
        Array.Copy(AudioSamples, Waveform, NativeFT4Coder.ENCODE_SAMPLE_COUNT);
    }

    public void SetMode(Mode newMode)
    {
      if (SenderMode == newMode) return;

      Mode oldMode = SenderMode;
      SenderMode = newMode;

      // stopping audio
      if (newMode == Mode.Idle)
      {
        lock (lockObj) { Stopping = true; }
        Stopping = true;
        WorkerThread!.Join();
        Soundcard.Enabled = false;
      }

      // starting audio
      else if (oldMode == Mode.Idle)
      {
        Soundcard.Enabled = true;

        if (!Soundcard.IsRunning)
        {
          MessageBox.Show("Unable to open the soundcard.", "SkyRoof",
            MessageBoxButtons.OK, MessageBoxIcon.Error);

          SenderMode = Mode.Idle;
          return;
        }

        Stopping = false;
        if (newMode == Mode.Tuning)
          WorkerThread = new(TuneThreadProcedure);
        else
          WorkerThread = new(SendThreadProcedure);

        WorkerThread.IsBackground = true;
        WorkerThread.Name = GetType().Name;
        WorkerThread.Priority = ThreadPriority.Highest;
        WorkerThread.Start();
      }

      // no switching between Tuning and Sending
      else
      {
        Console.Beep();
        SenderMode = oldMode;
      }
    }

    private const int leadSampleCount = NativeFT4Coder.SAMPLING_RATE;
    private readonly TimeSpan leadTime = TimeSpan.FromSeconds(leadSampleCount / (double)NativeFT4Coder.SAMPLING_RATE);
    private readonly float[] txBuffer = new float[leadSampleCount];
    private readonly float[] silence = new float[leadSampleCount];

    private double sinePhase;

    private void TuneThreadProcedure()
    {
      BeforeTransmit?.Invoke(this, EventArgs.Empty);
      Thread.Sleep(PttOnMargin);

      int samplesNeeded = 0;
      double phaseInc = 0;

      while (!Stopping)
      {
        lock (lockObj)
        {
          samplesNeeded = Math.Max(0, leadSampleCount - Soundcard.GetBufferedSampleCount());
          phaseInc = 2.0 * Math.PI * txAudioFrequency / NativeFT4Coder.SAMPLING_RATE;
        }

        for (int i = 0; i < samplesNeeded; i++)
          {
            txBuffer[i] = (float)Math.Sin(sinePhase);
            sinePhase += phaseInc;
            if (sinePhase > 2 * Math.PI) sinePhase -= 2 * Math.PI;
          }

        if (samplesNeeded > 0) Soundcard.AddSamples(txBuffer, 0, samplesNeeded);
        Thread.Sleep(50);
      }

      Soundcard.ClearBuffer();
      Thread.Sleep(PttOffMargin);
      AfterTransmit?.Invoke(this, EventArgs.Empty);
    }



    private enum SendStage { Idle, Scheduled, Sending }

    private void SendThreadProcedure()
    { 
      int sampleIndex = 0;
      int sampleCount;
      DateTime now;
      SendStage stage = SendStage.Idle;


      SetMessage("CQ VE3NEA FN03"); //{!}
      int len = Waveform.Length;
      //for (int i = 0; i < len; i++) Waveform[i] *= i / (float)len;
      Soundcard.ClearBuffer();


      while (!Stopping)
      {
        now = DateTime.UtcNow;
        Slot.Utc = now;
        var startTime = Slot.GetTxStartTime(TxOdd);
        var endTime = startTime + TimeSpan.FromSeconds(NativeFT4Coder.ENCODE_SECONDS);

        switch (stage)
        {
          case SendStage.Idle:
            sampleCount = (int)((startTime - now).TotalSeconds * NativeFT4Coder.SAMPLING_RATE);

            // time to schedule
            if (now < startTime && sampleCount < leadSampleCount)
            {
              Soundcard.AddSamples(silence, 0, sampleCount);
              sampleCount = leadSampleCount - sampleCount;
              Soundcard.AddSamples(Waveform, 0, sampleCount);
              sampleIndex = sampleCount;
              stage = SendStage.Scheduled;
            }

            // starting in the middle of transmission time
            else if (now > startTime)
            {
              double missedSeconds = (now - startTime).TotalSeconds + PttOnMargin * 1e-3;

              if (missedSeconds < NativeFT4Coder.ENCODE_SECONDS - 1)
              {
                // silence for PttOnMargin 
                sampleCount = (int)(PttOnMargin * 1e-3 * NativeFT4Coder.SAMPLING_RATE);
                Soundcard.AddSamples(silence, 0, sampleCount);

                // samples, skipping the missed time
                sampleIndex = (int)(missedSeconds * NativeFT4Coder.SAMPLING_RATE);
                sampleCount = leadSampleCount - Soundcard.GetBufferedSampleCount();
                sampleCount = Math.Min(Waveform.Length - sampleIndex, sampleCount); //{!} not needed?
                Soundcard.AddSamples(Waveform, sampleIndex, sampleCount);

                // PTT switch
                stage = SendStage.Sending;
                BeforeTransmit?.Invoke(this, EventArgs.Empty);
                Thread.Sleep(PttOnMargin);
              }
            }
            break;

          case SendStage.Scheduled:
            // keep buffer full
            sampleCount = leadSampleCount - Soundcard.GetBufferedSampleCount();
            Soundcard.AddSamples(Waveform, sampleIndex, sampleCount);
            sampleIndex += sampleCount;

            // scheduled starts now
            if (now > startTime)
            {
              stage = SendStage.Sending;
              BeforeTransmit?.Invoke(this, EventArgs.Empty);
            }
            break;

          case SendStage.Sending:
            // keep buffer full
            if (sampleIndex < Waveform.Length)
            {
              sampleCount = leadSampleCount - Soundcard.GetBufferedSampleCount();
              sampleCount = Math.Min(Waveform.Length - sampleIndex, sampleCount);
              Soundcard.AddSamples(Waveform, sampleIndex, sampleCount);
              sampleIndex += sampleCount;
            }
            // all samples in the buffer, switch to idle
            else if (Soundcard.GetBufferedSampleCount() == 0)
            {
              stage = SendStage.Idle;
              Thread.Sleep(PttOffMargin);
              AfterTransmit?.Invoke(this, EventArgs.Empty);
            }
            break;
        }

        Thread.Sleep(10);
      }

      // stop audio and fire event if stopped during transmission
      Soundcard.ClearBuffer();
      if (stage == SendStage.Sending)
      {
        stage = SendStage.Idle;
        Thread.Sleep(PttOffMargin);
        AfterTransmit?.Invoke(this, EventArgs.Empty);
      }
    }

    public void Dispose()
    {
      Stop();
      Soundcard.Dispose();
    }
  }
}
