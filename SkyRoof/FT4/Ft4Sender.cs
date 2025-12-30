using System.Drawing.Text;
using System.Security.Cryptography;
using System.Text;
using VE3NEA;

namespace SkyRoof
{
  public class Ft4Sender : IDisposable
  {  
    public enum Mode { Idle, Tuning, Sending }

    public const int PttOnMargin = 150; // milliseconds
    public const int PttOffMargin = 300;
    private const double PttOnSeconds = PttOnMargin * 1e-3;
    private const int PttOnSampleCount = (int)(PttOnSeconds * NativeFT4Coder.SAMPLING_RATE);
    private const int RampSamples = NativeFT4Coder.SAMPLES_PER_SYMBOL;
    private const double RampSeconds = RampSamples / (double)NativeFT4Coder.SAMPLING_RATE;
    private const int LeadSampleCount = 2 * NativeFT4Coder.SAMPLING_RATE / 3;

    private byte[] MessageChars = new byte[NativeFT4Coder.ENCODE_MESSAGE_LENGTH + 1];
    private float[] AudioSamples = new float[NativeFT4Coder.ENCODE_SAMPLE_COUNT];
    private float[] Waveform = new float[NativeFT4Coder.ENCODE_SAMPLE_COUNT];
    private float[] Ramp = new float[RampSamples];
    private readonly float[] TxBuffer = new float[LeadSampleCount];
    private readonly float[] Silence = new float[LeadSampleCount];

    private int txAudioFrequency = 1500;
    private object lockObj = new();
    private Thread? WorkerThread;
    private bool Stopping;
    private Ft4Slot Slot = new();



    public OutputSoundcard<float> Soundcard = new();
    public Mode SenderMode { get; private set; } = Mode.Idle;

    public event EventHandler? BeforeTransmit;
    public event EventHandler? AfterTransmit;

    public int TxAudioFrequency { get => txAudioFrequency; set { lock (lockObj) { txAudioFrequency = value; } }}

    public void StartTuning() => SetMode(Mode.Tuning);
    public void StartSending() => SetMode(Mode.Sending);
    public void Stop() => SetMode(Mode.Idle);
    public bool TxOdd;

    public enum SendStage { Idle, Scheduled, Sending }

    public SendStage stage = SendStage.Idle;

    public Ft4Sender()
    {
      GenerateRamp();
      SetMessage("CQ VE3NEA FN03"); //{!}
    }

    private void GenerateRamp()
    {
      for (int i = 0; i < RampSamples; i++) 
        Ramp[i] = (float)(0.5 - 0.5 * Math.Cos(Math.PI * i / RampSamples));
    }

    public void SetMessage(string message)
    {
      // generate samples
      int len = Math.Min(message.Length, MessageChars.Length - 1);
      Array.Clear(MessageChars);
      Encoding.ASCII.GetBytes(message, 0, len, MessageChars, 0);
      float frequency = txAudioFrequency;

      NativeFT4Coder.encode_ft4(MessageChars, ref frequency, AudioSamples);

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
          samplesNeeded = Math.Max(0, LeadSampleCount - Soundcard.Buffer.Count);
          phaseInc = 2.0 * Math.PI * txAudioFrequency / NativeFT4Coder.SAMPLING_RATE;
        }

        for (int i = 0; i < samplesNeeded; i++)
          {
            TxBuffer[i] = (float)Math.Sin(sinePhase);
            sinePhase += phaseInc;
            if (sinePhase > 2 * Math.PI) sinePhase -= 2 * Math.PI;
          }

        if (samplesNeeded > 0) Soundcard.AddSamples(TxBuffer, 0, samplesNeeded);
        Thread.Sleep(50);
      }

      Soundcard.Buffer.Clear();
      Thread.Sleep(PttOffMargin);
      AfterTransmit?.Invoke(this, EventArgs.Empty);
    }

    private void SendThreadProcedure()
    {
      int sampleIndex = 0;
      int sampleCount;
      DateTime now;
      stage = SendStage.Idle;

      void RampDown()
      {
        Soundcard.Buffer.ExecuteLocked(() =>
        {
          sampleIndex -= Soundcard.Buffer.Count;
          Soundcard.Buffer.Clear();
          if (sampleIndex >= 0 && sampleIndex + RampSamples <= Waveform.Length)
          {
            for (int i = 0; i < RampSamples; i++) TxBuffer[i] = Waveform[sampleIndex + i] * Ramp[RampSamples - 1 - i];
            Soundcard.Buffer.Write(TxBuffer, 0, RampSamples);
          }
        });
  
        sampleIndex = Waveform.Length;
      }

      Soundcard.Buffer.Clear();

      while (!Stopping)
      {
        now = DateTime.UtcNow;
        Slot.Utc = now;
        var startTime = Slot.GetTxStartTime(TxOdd);

        switch (stage)
        {
          case SendStage.Idle:
            sampleCount = (int)((startTime - now).TotalSeconds * NativeFT4Coder.SAMPLING_RATE);

            // time to schedule
            if (now < startTime && sampleCount < LeadSampleCount)
            {
              Soundcard.AddSamples(Silence, 0, sampleCount);
              Soundcard.AddSamples(Silence, 0, PttOnSampleCount);
              sampleCount = LeadSampleCount - sampleCount;
              Soundcard.AddSamples(Waveform, 0, sampleCount);
              sampleIndex = sampleCount;
              stage = SendStage.Scheduled;
            }

            // starting in the middle of transmission time
            else if (now > startTime)
            {
              double missedSeconds = (now - startTime).TotalSeconds + PttOnSeconds;

              if (missedSeconds < NativeFT4Coder.ENCODE_SECONDS - PttOnSeconds - RampSeconds)
              {
                // silence for PttOnMargin 
                Soundcard.Buffer.Clear();
                sampleCount = (int)(PttOnSeconds * NativeFT4Coder.SAMPLING_RATE);
                Soundcard.AddSamples(Silence, 0, sampleCount);

                // current position in waveform
                sampleIndex = (int)(missedSeconds * NativeFT4Coder.SAMPLING_RATE);

                // ramped waveform
                for (int i = 0; i < RampSamples; i++) TxBuffer[i] = Waveform[sampleIndex + i] * Ramp[i];
                Soundcard.Buffer.Write(TxBuffer, 0, RampSamples);
                sampleIndex += RampSamples;

                // waveform
                sampleCount = LeadSampleCount - Soundcard.Buffer.Count;
                sampleCount = Math.Min(Waveform.Length - sampleIndex, sampleCount);
                Soundcard.AddSamples(Waveform, sampleIndex, sampleCount);
                sampleIndex += sampleCount;

                // PTT switch
                stage = SendStage.Sending;
                BeforeTransmit?.Invoke(this, EventArgs.Empty);
                Thread.Sleep(PttOnMargin);
              }
            }
            break;

          case SendStage.Scheduled:
            // keep buffer full
            sampleCount = LeadSampleCount - Soundcard.Buffer.Count;
            sampleCount = Math.Max(0, Math.Min(Waveform.Length - sampleIndex, sampleCount)); 
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
              sampleCount = LeadSampleCount - Soundcard.Buffer.Count;
              sampleCount = Math.Min(Waveform.Length - sampleIndex, sampleCount);
              Soundcard.AddSamples(Waveform, sampleIndex, sampleCount);
              sampleIndex += sampleCount;
            }

            // all samples in the buffer, switch to idle
            else if (Soundcard.Buffer.Count == 0)
            {
              stage = SendStage.Idle;
              Thread.Sleep(PttOffMargin);
              AfterTransmit?.Invoke(this, EventArgs.Empty);
            }

            // Odd/Even changed, stop sending
            if (sampleIndex < Waveform.Length && startTime > now) RampDown();

            break;
        }

        Thread.Sleep(10);
      }

      // aborted, stop audio and fire event
      if (stage == SendStage.Sending)
      {
        RampDown();
        stage = SendStage.Idle;
        Thread.Sleep(PttOffMargin);
        AfterTransmit?.Invoke(this, EventArgs.Empty);
      }

      Soundcard.Buffer.Clear();
    }

    public void Dispose()
    {
      Stop();
      Soundcard.Dispose();
    }
  }
}
