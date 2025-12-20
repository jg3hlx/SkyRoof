using NAudio.CoreAudioApi;
using NAudio.Wave;
using MathNet.Numerics;

namespace VE3NEA
{
  //-----------------------------------------------------------------------------------------------
  //            base class
  //-----------------------------------------------------------------------------------------------
  
  // <T> is float or Complex32
  
  public abstract class SoundcardNA : IDisposable
  {
    protected const int DEFAULT_SAMPLING_RATE = 48_000;
    protected readonly int SamplingRate;
    protected MMDevice? mmDevice;
    protected bool enabled;
    private System.Timers.Timer? Timer;

    public bool Retry;
    public bool Enabled { get => enabled; set => SetEnabled(value); }
    public bool IsRunning { get; private set; }

    public event EventHandler? StateChanged;

    public SoundcardNA(string? audioDeviceId = null, int? samplingRate = null)
    {
      SetDeviceId(audioDeviceId);
      SamplingRate = samplingRate ?? DEFAULT_SAMPLING_RATE;
    }

 private void SetEnabled(bool value)
    {
      if (value == Enabled) return;
      enabled = value;

      // will call OnStateChanged
      if (value) Start(); else Stop();
    }

    private void EnableRetry(bool value)
    {
      Timer?.Stop();
      Timer = null;

    if (value)
      {
        Timer = new();
        Timer.Interval = 3000;
    Timer.Elapsed += (s, a) => RunRetry();
        Timer.Start();
      }
    }

    private void RunRetry()
    {
  Start();
      EnableRetry(!IsRunning);
    }

    public void SetDeviceId(string? deviceId)
    {
      bool wasEnabled = enabled;
      Enabled = false;
      if (deviceId == null) return;

      try
 {
        using (var deviceEnumerator = new MMDeviceEnumerator())
        {
    mmDevice = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active)
   .FirstOrDefault(d => d.ID == deviceId);
        }
      }
      catch { }

      Enabled = wasEnabled;
    }

    internal string GetDisplayName()
    {
  try
 {
        return mmDevice?.FriendlyName ?? "None Selected";
      }
      catch
      {
        return "Device Failed";
      }
    }

    protected void OnStateChanged(bool running)
    {
      IsRunning = running;

      StateChanged?.Invoke(this, EventArgs.Empty);

      EnableRetry(Enabled && Retry && !running);
    }

    public static Entry[] ListDevices(DataFlow direction)
    {
      using (var deviceEnumerator = new MMDeviceEnumerator())
      {
        var devices = deviceEnumerator.EnumerateAudioEndPoints(direction, DeviceState.Active);
 return devices.Select(s => new Entry(s.ID, s.FriendlyName)).ToArray();
      }
    }

    public static string? GetDefaultSoundcardId(DataFlow direction)
    {
    try
      {
      using (var deviceEnumerator = new MMDeviceEnumerator())
        {
          return deviceEnumerator.GetDefaultAudioEndpoint(direction, Role.Multimedia)?.ID;
        }
  }
      catch
 {
        return null;
      }
    }

    public static string? GetFirstVacId(DataFlow direction)
    {
      try
      {
        using (var deviceEnumerator = new MMDeviceEnumerator())
        {
          var devices = deviceEnumerator.EnumerateAudioEndPoints(direction, DeviceState.Active);
    return devices.FirstOrDefault(d => d.FriendlyName.Contains("Virtual"))?.ID;
   }
      }
      catch
      {
        return null;
      }
    }

  public void Dispose()
    {
      Enabled = false;
    }

    protected abstract void Start();
    protected abstract void Stop();
  }




  //-----------------------------------------------------------------------------------------------
  //      WaveSource wrapper for NAudio
  //-----------------------------------------------------------------------------------------------
  public class WaveSourceNA<T> : IWaveProvider
  {
    private readonly WaveFormat waveFormat;
    private RingBuffer<T> ringBuffer;

    public WaveFormat WaveFormat => waveFormat;

public WaveSourceNA(int samplingRate)
    {
      int channelCount = typeof(T) == typeof(Complex32) ? 2 : 1;
      waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(samplingRate, channelCount);
      ringBuffer = new RingBuffer<T>(samplingRate * 2); // 2 second buffer
    }

    public void AddSamples(T[] samples, int offset = 0, int? count = null)
    {
      count ??= samples.Length - offset;
 ringBuffer.Write(samples, offset, count.Value);
    }

 public void ClearBuffer()
    {
      ringBuffer.Clear();
    }

    public int Read(byte[] buffer, int offset, int count)
    {
      int sampleSize = sizeof(float) * (typeof(T) == typeof(Complex32) ? 2 : 1);
      int sampleCount = count / sampleSize;
      
      if (sampleCount <= 0) return 0;

      var tempBuffer = new T[sampleCount];
  int samplesRead = ringBuffer.Read(tempBuffer, 0, sampleCount);
    int bytesRead = samplesRead * sampleSize;

   Buffer.BlockCopy(tempBuffer, 0, buffer, offset, bytesRead);
      
      return bytesRead;
    }
  }




  //-----------------------------------------------------------------------------------------------
  //      output soundcard
  //-----------------------------------------------------------------------------------------------
  public class OutputSoundcardNA<T> : SoundcardNA
  {
    private WaveSourceNA<T> waveSource;
    private IWavePlayer? wasapiOut;
    private float volume;
    public float Volume { get => volume; set => SetVolume(value); }


    public OutputSoundcardNA(string? audioDeviceId = null, int? samplingRate = null) : base(audioDeviceId, samplingRate)
    {
      waveSource = new WaveSourceNA<T>(SamplingRate);
    }

    protected override void Start()
    {
      if (mmDevice?.State != DeviceState.Active) throw new Exception();

      try
      {
        waveSource.ClearBuffer();

        wasapiOut = new WasapiOut(mmDevice, AudioClientShareMode.Shared, useEventSync: false, latency: 100);
        wasapiOut.Init(waveSource);
        wasapiOut.Volume = volume;
        wasapiOut.PlaybackStopped += (s, a) => OnStateChanged(false);
   wasapiOut.Play();

        OnStateChanged(true);
      }
      catch
      {
        Stop();
 }
    }

    protected override void Stop()
    {
      wasapiOut?.Stop();
      wasapiOut?.Dispose();
      wasapiOut = null;

      OnStateChanged(false);
    }

    private void SetVolume(float value)
{
      volume = Math.Clamp(value, 0f, 1f);
      if (wasapiOut != null) wasapiOut.Volume = volume;
    }

    public void AddSamples(T[] samples)
    {
      if (Enabled) waveSource.AddSamples(samples);
    }

    public new void Dispose()
    {
      base.Dispose();
      wasapiOut?.Dispose();
    }
  }




  //-----------------------------------------------------------------------------------------------
  //       Sample-based resampler wrapper
  //-----------------------------------------------------------------------------------------------
  internal class SimpleResampler : ISampleProvider
  {
    private readonly IWaveProvider source;
    private readonly WaveFormat targetFormat;
    private byte[] byteBuffer;
    private float[] floatBuffer;
    private readonly int bytesPerSample;

  public WaveFormat WaveFormat => targetFormat;

    public SimpleResampler(IWaveProvider source, int targetSampleRate)
    {
      this.source = source;
      bytesPerSample = source.WaveFormat.BitsPerSample / 8 * source.WaveFormat.Channels;
      
    if (source.WaveFormat.SampleRate == targetSampleRate)
      {
        targetFormat = source.WaveFormat;
      }
      else
      {
        targetFormat = WaveFormat.CreateIeeeFloatWaveFormat(targetSampleRate, source.WaveFormat.Channels);
 }
      
      byteBuffer = new byte[source.WaveFormat.AverageBytesPerSecond / 10];
      floatBuffer = new float[byteBuffer.Length / bytesPerSample];
    }

    public int Read(float[] buffer, int offset, int count)
    {
      // For now, direct passthrough if rates match
      if (source.WaveFormat.SampleRate == WaveFormat.SampleRate)
      {
   int bytesNeeded = count * bytesPerSample;
      if (byteBuffer.Length < bytesNeeded)
        byteBuffer = new byte[bytesNeeded];

        int bytesRead = source.Read(byteBuffer, 0, bytesNeeded);
        int samplesRead = bytesRead / bytesPerSample;

        Buffer.BlockCopy(byteBuffer, 0, buffer, offset * sizeof(float), bytesRead);
        return samplesRead;
      }

      return 0;
    }

    public void Dispose()
    {
      (source as IDisposable)?.Dispose();
    }
  }




  //-----------------------------------------------------------------------------------------------
  //     input soundcard
  //-----------------------------------------------------------------------------------------------
  public class InputSoundcardNA<T> : SoundcardNA
  {
    private WasapiCapture? soundIn;
    private DataEventArgsPool<float> ArgsPool = new();
    private byte[] captureBuffer = new byte[4096];
    private float[] floatBuffer = new float[2048];

    public event EventHandler<DataEventArgs<float>>? SamplesAvailable;

    public InputSoundcardNA(string? audioDeviceId = null, int? samplingRate = null) : base(audioDeviceId, samplingRate)
    {
    }

    protected override void Start()
    {
      try
      {
        if (mmDevice?.State != DeviceState.Active) throw new Exception();

        // init audio device
    int channelCount = typeof(T) == typeof(Complex32) ? 2 : 1;
        WaveFormat targetFormat = WaveFormat.CreateIeeeFloatWaveFormat(SamplingRate, channelCount);

        soundIn = new WasapiCapture(mmDevice);
        soundIn.WaveFormat = targetFormat;
        soundIn.RecordingStopped += (s, a) => OnStateChanged(false);
        soundIn.DataAvailable += SoundIn_DataAvailable;

        soundIn.StartRecording();

  OnStateChanged(true);
      }
      catch
      {
        Stop();
      }
    }

    protected override void Stop()
    {
      if (IsRunning && soundIn != null)
      {
        soundIn.StopRecording();
      }
      soundIn?.Dispose();
      soundIn = null;

      OnStateChanged(false);
    }

    private void SoundIn_DataAvailable(object? sender, WaveInEventArgs e)
 {
      if (soundIn == null) return;

    int bytesPerSample = sizeof(float) * (typeof(T) == typeof(Complex32) ? 2 : 1);
      int sampleCount = e.BytesRecorded / bytesPerSample;
      
      if (sampleCount <= 0) return;

      // Ensure float buffer is large enough
      if (floatBuffer.Length < sampleCount)
        Array.Resize(ref floatBuffer, sampleCount);

      // Convert bytes to float samples
    Buffer.BlockCopy(e.Buffer, 0, floatBuffer, 0, e.BytesRecorded);

      // Emit samples
      var args = ArgsPool.Rent(sampleCount);
   Array.Copy(floatBuffer, 0, args.Data, 0, sampleCount);
      args.Count = sampleCount;
      SamplesAvailable?.Invoke(this, args);
      ArgsPool.Return(args);
    }

    public new void Dispose()
    {
      base.Dispose();
      soundIn?.Dispose();
    }
  }
}