//using System;
//using System.Collections.Concurrent;
//using System.Diagnostics;
//using System.Numerics;
//using System.Runtime.InteropServices;
//using System.Threading;
//using PortAudioSharp;
//using Serilog;

//namespace VE3NEA
//{
//  //-----------------------------------------------------------------------------------------------
//  //                                     base class
//  //-----------------------------------------------------------------------------------------------
//  public abstract class SoundcardPa : IDisposable
//  {
//    protected const int DEFAULT_SAMPLING_RATE = 48000;
//    protected readonly int SamplingRate;
//    protected bool enabled;
//    private System.Timers.Timer? Timer;

//    public bool Retry;
//    public bool Enabled { get => enabled; set => SetEnabled(value); }
//    public bool IsRunning { get; private set; }
//    public event EventHandler? StateChanged;

//    protected SoundcardPa(int? samplingRate = null)
//    {
//      SamplingRate = samplingRate ?? DEFAULT_SAMPLING_RATE;
//    }

//    private void SetEnabled(bool value)
//    {
//      if (value == Enabled) return;
//      enabled = value;
//      if (value) Start(); else Stop();
//    }

//    private void EnableRetry(bool value)
//    {
//      Timer?.Stop();
//      Timer = null;
//      if (value)
//      {
//        Timer = new();
//        Timer.Interval = 3000;
//        Timer.Elapsed += (s, a) => RunRetry();
//        Timer.Start();
//      }
//    }

//    private void RunRetry()
//    {
//      Start();
//      EnableRetry(!IsRunning);
//    }

//    protected void OnStateChanged(bool running)
//    {
//      IsRunning = running;
//      StateChanged?.Invoke(this, EventArgs.Empty);
//      EnableRetry(Enabled && Retry && !running);
//    }

//    public void Dispose() => Enabled = false;

//    protected abstract void Start();
//    protected abstract void Stop();
//  }

//  //-----------------------------------------------------------------------------------------------
//  //                                     output soundcard
//  //-----------------------------------------------------------------------------------------------
//  public class OutputSoundcardPa<T> : SoundcardPa
//  {
//    private PortAudioSharp.Stream? stream;
//    private readonly WaveSource<T> waveSource;
//    private GCHandle callbackHandle;
//    private float volume;
//    public float Volume { get => volume; set => SetVolume(value); }

//    public OutputSoundcardPa(int? samplingRate = null) : base(samplingRate)
//    {
//      waveSource = new WaveSource<T>(SamplingRate);
//    }

//    protected override void Start()
//    {
//      try
//      {
//        waveSource.ClearBuffer();
//        PortAudio.Initialize();

//        int channels = typeof(T) == typeof(Complex) ? 2 : 1;
//        SampleFormat format = SampleFormat.Float32;

//        StreamCallback callback = OutputCallback;

//        callbackHandle = GCHandle.Alloc(callback);
//        stream = PortAudio.OpenDefaultStream(0, channels, format, SamplingRate, 256, callbackHandle);
//        stream.Start();

//        OnStateChanged(true);
//      }
//      catch (Exception e)
//      {
//        Log.Error(e, "Error starting OutputSoundcardPa");
//        Stop();
//      }
//    }

//    protected override void Stop()
//    {
//      if (stream != null)
//      {
//        stream.Stop();
//        stream.Close();
//        stream = null;
//        callbackHandle.Free();
//      }
//      PortAudio.Terminate();
//      OnStateChanged(false);
//    }

//    private void SetVolume(float value)
//    {
//      volume = value;
//    }

//    private int OutputCallback(IntPtr input, IntPtr output, uint frameCount, IntPtr timeInfo, PaStreamCallbackFlags statusFlags, IntPtr userData)
//    {
//      int channels = typeof(T) == typeof(Complex) ? 2 : 1;
//      int sampleCount = (int)frameCount * channels;

//      float[] buffer = new float[sampleCount];
//      int read = waveSource.Read(buffer, 0, sampleCount);

//      for (int i = 0; i < read; i++)
//        buffer[i] *= volume;

//      Marshal.Copy(buffer, 0, output, read);

//      // zero remaining if any
//      if (read < sampleCount)
//        for (int i = read; i < sampleCount; i++)
//          Marshal.WriteInt32(output, i * 4, 0);

//      return 0; // continue
//    }

//    public void AddSamples(T[] samples)
//    {
//      if (Enabled) waveSource.AddSamples(samples);
//    }
//  }

//  //-----------------------------------------------------------------------------------------------
//  //                                     input soundcard
//  //-----------------------------------------------------------------------------------------------
//  public class InputSoundcardPa<T> : SoundcardPa
//  {
//    private Stream? stream;
//    private GCHandle callbackHandle;
//    private DataEventArgsPool<float> ArgsPool = new();
//    public static long TotalSampleCount = 0;

//    public event EventHandler<DataEventArgs<float>>? SamplesAvailable;

//    public InputSoundcardPa(int? samplingRate = null) : base(samplingRate)
//    {
//    }

//    protected override void Start()
//    {
//      try
//      {
//        PortAudio.Initialize();

//        int channels = typeof(T) == typeof(Complex) ? 2 : 1;
//        PaSampleFormat format = PaSampleFormat.Float32;

//        StreamCallback callback = InputCallback;
//        callbackHandle = GCHandle.Alloc(callback);

//        stream = PortAudio.OpenDefaultStream(channels, 0, format, SamplingRate, 256, callbackHandle);
//        stream.Start();

//        OnStateChanged(true);
//      }
//      catch (Exception e)
//      {
//        Log.Error(e, "Error starting InputSoundcardPa");
//        Stop();
//      }
//    }

//    protected override void Stop()
//    {
//      if (stream != null)
//      {
//        stream.Stop();
//        stream.Close();
//        stream = null;
//        callbackHandle.Free();
//      }
//      PortAudio.Terminate();
//      OnStateChanged(false);
//    }

//    private int InputCallback(IntPtr input, IntPtr output, uint frameCount, IntPtr timeInfo, PaStreamCallbackFlags statusFlags, IntPtr userData)
//    {
//      int channels = typeof(T) == typeof(Complex) ? 2 : 1;
//      int sampleCount = (int)frameCount * channels;

//      var args = ArgsPool.Rent(sampleCount);
//      Marshal.Copy(input, args.Data, 0, sampleCount);
//      args.Count = sampleCount;
//      TotalSampleCount += sampleCount;

//      SamplesAvailable?.Invoke(this, args);
//      ArgsPool.Return(args);

//      return 0; // continue
//    }
//  }
//}
