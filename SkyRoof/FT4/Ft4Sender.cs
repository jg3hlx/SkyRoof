using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
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

    public void SetMessage(string message)
    {
      // generate samples
      int len = Math.Min(message.Length, MessageChars.Length - 1);
      Array.Clear(MessageChars);
      Encoding.ASCII.GetBytes(message, 0, len, MessageChars, 0);

      NativeFT4Coder.encode_ft4_f(MessageChars, ref txAudioFrequency, AudioSamples);

      // copy to waveform
      lock (lockObj)
        Array.Copy(AudioSamples, Waveform, NativeFT4Coder.ENCODE_SAMPLE_COUNT);
    }

    private void SetMode(Mode newMode)
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
    private float[] txBuffer = new float[leadSampleCount]; 
    private double sinePhase;

    private void TuneThreadProcedure()
    {
      BeforeTransmit?.Invoke(this, EventArgs.Empty);
      Thread.Sleep(PttOnMargin);

      int samplesNeeded = 0;

      while (!Stopping)
      {
        samplesNeeded = Math.Max(0, leadSampleCount - Soundcard.GetBufferedSampleCount());
        double phaseInc = 2.0 * Math.PI * txAudioFrequency / NativeFT4Coder.SAMPLING_RATE;

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

    private void SendThreadProcedure()
    {


    }

    public void Dispose()
    {
      Stop();
      Soundcard.Dispose();
    }
  }
}
