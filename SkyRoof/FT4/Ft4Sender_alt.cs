namespace VE3NEA
{
  public class Ft4Sender_alt : IDisposable
  {
    private const int SAMPLING_RATE = NativeFT4Coder.SAMPLING_RATE;
    private const int BLOCK_SIZE = NativeFT4Coder.ENCODE_SAMPLE_COUNT;

    private readonly OutputSoundcard<float> soundcard;
    private readonly float[] waveform = new float[BLOCK_SIZE];

    private CancellationTokenSource? cts;
    private Task? worker;
    private int txAudioFrequency = 1500;
    private bool tuning;
    private bool running;

    private double sinePhase;
    private readonly object lockObj = new();

    public int PttMargin { get; set; } = 100; // milliseconds
    public bool Odd { get; set; }
    public int TxAudioFrequency { get => txAudioFrequency; set => SetTxAudioFrequency(value); }
    public bool Tuning { get => tuning; set => SetTuning(value); }

    public event EventHandler? BeforeTransmit;
    public event EventHandler? AfterTransmit;

    public Ft4Sender_alt()
    {
      soundcard = new();
    }

    // ------------------------------------------------------------
    // Public control
    // ------------------------------------------------------------

    public void SetWaveform(float[] samples)
    {
      if (samples.Length != BLOCK_SIZE)
        throw new ArgumentException(nameof(samples));

      lock (lockObj)
        Array.Copy(samples, waveform, BLOCK_SIZE);
    }

    public void Start()
    {
      if (running) return;

      running = true;
      cts = new CancellationTokenSource();
      worker = Task.Run(() => RunAsyncLoop(cts.Token));
    }

    public void Stop()
    {
      if (!running) return;

      running = false;
      cts?.Cancel();

      Task.Delay(PttMargin).ContinueWith(_ => { AfterTransmit?.Invoke(this, EventArgs.Empty); });
    }

    public void Dispose()
    {
      Stop();
    }


    // ------------------------------------------------------------
    // Internal setters
    // ------------------------------------------------------------

    private void SetTxAudioFrequency(int value)
    {
      lock (lockObj)
        txAudioFrequency = value;
    }

    private void SetTuning(bool value)
    {
      bool fireEvent;

      lock (lockObj)
      {
        if (tuning == value) return;
        tuning = value;
        fireEvent = running;
      }

      if (fireEvent)
        Task.Run(async () =>
        {
          BeforeTransmit?.Invoke(this, EventArgs.Empty);
          await Task.Delay(PttMargin);
        });
    }

    // ------------------------------------------------------------
    // Core scheduler loop
    // ------------------------------------------------------------

    private async Task RunAsyncLoop(CancellationToken token)
    {
      AlignToTimeBase();

      soundcard.Enabled = true;
      if (!soundcard.IsRunning) return;

      while (!token.IsCancellationRequested)
      {
        bool doTuning;
        lock (lockObj) doTuning = tuning;

        if (doTuning)
          await RunTuningAsync(token);
        else
          await RunWaveformAsync(token);
      }

      soundcard.Enabled = false;
    }

    // ------------------------------------------------------------
    // Waveform playback
    // ------------------------------------------------------------

    private async Task RunWaveformAsync(CancellationToken token)
    {
      DateTime startTime = ComputeCurrentSlotStart();

      DateTime now = DateTime.UtcNow;
      if (now < startTime)
        await Task.Delay(startTime - now, token);

      BeforeTransmit?.Invoke(this, EventArgs.Empty);
      await Task.Delay(PttMargin, token);

      int samplesSent = 0;

      while (samplesSent < BLOCK_SIZE && !token.IsCancellationRequested)
      {
        int chunk = Math.Min(2048, BLOCK_SIZE - samplesSent);

        float[] buf = new float[chunk];
        lock (lockObj)
          Array.Copy(waveform, samplesSent, buf, 0, chunk);

        soundcard.AddSamples(buf);
        samplesSent += chunk;

        await Task.Delay(5, token);
      }

      await Task.Delay(PttMargin, token);
      AfterTransmit?.Invoke(this, EventArgs.Empty);

      await DelayUntilNextSlot(token);
    }

    // ------------------------------------------------------------
    // Continuous tuning tone
    // ------------------------------------------------------------

    private async Task RunTuningAsync(CancellationToken token)
    {
      BeforeTransmit?.Invoke(this, EventArgs.Empty);
      await Task.Delay(PttMargin, token);

      const int chunk = 2048;
      float[] buffer = new float[chunk];

      while (true)
      {
        lock (lockObj)
        {
          if (!tuning) break;

          double phaseInc = 2.0 * Math.PI * txAudioFrequency / SAMPLING_RATE;

          for (int i = 0; i < chunk; i++)
          {
            buffer[i] = (float)Math.Sin(sinePhase);
            sinePhase += phaseInc;
            if (sinePhase > 2 * Math.PI) sinePhase -= 2 * Math.PI;
          }
        }

        soundcard.AddSamples(buffer);
        await Task.Delay(5, token);
      }

      await Task.Delay(PttMargin, token);
      AfterTransmit?.Invoke(this, EventArgs.Empty);
    }

    // ------------------------------------------------------------
    // Timing helpers
    // ------------------------------------------------------------

    private void AlignToTimeBase()
    {
      DateTime now = DateTime.UtcNow;
      int slot = now.Second / 15;
      int baseSec = slot * 15 + (Odd ? 7 : 0);
      DateTime next = new DateTime(now.Year, now.Month, now.Day,
                                   now.Hour, now.Minute, baseSec,
                                   DateTimeKind.Utc);
      if (next <= now)
        next = next.AddSeconds(15);

      Thread.Sleep(next - now);
    }

    private DateTime ComputeCurrentSlotStart()
    {
      DateTime now = DateTime.UtcNow;
      int slot = now.Second / 15;
      int sec = slot * 15 + (Odd ? 7 : 0);

      return new DateTime(now.Year, now.Month, now.Day,
                          now.Hour, now.Minute, sec,
                          DateTimeKind.Utc);
    }

    private async Task DelayUntilNextSlot(CancellationToken token)
    {
      DateTime next = ComputeCurrentSlotStart().AddSeconds(15);
      TimeSpan delay = next - DateTime.UtcNow;
      if (delay > TimeSpan.Zero)
        await Task.Delay(delay, token);
    }
  }
}
