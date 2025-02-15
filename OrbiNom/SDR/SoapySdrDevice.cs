using System.Diagnostics;
using MathNet.Numerics;
using Serilog;

namespace VE3NEA
{
  public class SoapySdrDevice : IDisposable
  {
    private IntPtr Device;
    private Thread? Thread;
    private bool Stopping;
    DataEventArgs<Complex32> Args = new();
    protected Complex32[] Data = Array.Empty<Complex32>();
    private bool enabled;
    private bool Started;

    public readonly SoapySdrDeviceInfo Info;
    public double Frequency { get => Info.Frequency; set => SetFrequency(value); }
    public double Gain { get => Info.Gain; set => SetGain(value); }
    public bool Enabled { get => enabled; set => SetEnabled(value); }

    public event EventHandler? StateChanged;
    public event EventHandler<DataEventArgs<Complex32>>? DataAvailable;


    public SoapySdrDevice(SoapySdrDeviceInfo info)
    { 
      Info = info;
    }




    //----------------------------------------------------------------------------------------------
    //                                     start / stop
    //----------------------------------------------------------------------------------------------
    private void SetEnabled(bool value)
    {
      if (value == enabled) return;
      enabled = value;

      if (value)
      {
        TryStart(true);
        if (!Started)
          Debug.WriteLine($"Failed to start {Info.Name}, will keep trying");
      }
      else
      {
        Stop();
        Started = false;
        StateChanged?.Invoke(this, EventArgs.Empty);
      }
    }

    private void TryStart(bool logErrors)
    {
      try
      {
        Start();

        Started = true;
        StateChanged?.Invoke(this, EventArgs.Empty);
      }
      catch (Exception ex)
      {
        Stop();
        Started = false;
        if (logErrors) Log.Error(ex, $"Error starting {Info.Name}");
      }
    }

    public void Retry()
    {
      if (IsRunning()) return;

      if (Started) // was running but then failed
      {
        Started = false;
        Log.Error($"{Info.Name} failed, restarting");
        StateChanged?.Invoke(this, EventArgs.Empty);
      }

      if (enabled && !Started) TryStart(false);
    }

    public bool IsRunning()
    {
      return Thread != null;
    }




    //----------------------------------------------------------------------------------------------
    //                                     worker thread
    //----------------------------------------------------------------------------------------------
    public void Start()
    {
      Device = SoapySdr.CreateDevice(Info.KwArgs);

      Thread = new Thread(new ThreadStart(ThreadProcedure));
      Thread.IsBackground = true;
      Thread.Name = GetType().Name;
      Stopping = false;
      Thread.Start();
      Thread.Priority = ThreadPriority.Highest;
    }

    public void Stop()
    {
      if (!IsRunning()) return;

      Stopping = true;
      Thread!.Join();
      Thread = null;

      SoapySdr.ReleaseDevice(Device);
      Device = IntPtr.Zero;
    }

    private void ThreadProcedure()
    {
      SetAllParams();

      //Stream = Device.SetupRxStream("CF32", [0], "");

      int timeout = 1000000;
//      ulong samplesPerBlock = Stream.MTU;
//      ulong floatsPerBlock = samplesPerBlock * 2;
//      Samples = new Complex32[samplesPerBlock];
//      FloatSamples = new float[floatsPerBlock];
//      DataEventArgs<Complex32> Args = new();
//
//      Stream.Activate();

      while (!Stopping)
      {
        Thread.Sleep(300);
        //StreamResult streamResult = new();
        //var errorCode = Stream.Read(ref FloatSamples, timeout, out streamResult);
        //if (errorCode != ErrorCode.None || streamResult.NumSamples != samplesPerBlock)
        //{
        //  Debug.WriteLine($"Stream.Read error: {errorCode}  StreamResult: {streamResult}");
        //  break;
        //}
        //
        //for (int i = 0; i < Samples.Count(); i++) Samples[i] = new Complex32(FloatSamples[i * 2], FloatSamples[i * 2 + 1]);
        ////Buffer.BlockCopy(FloatSamples, 0, Samples, 0, FloatSamples.Length * sizeof(float));
        //Args.SetValues(Samples);
        DataAvailable?.Invoke(this, Args);
      }

//      Stream.Deactivate();
//      Stream.Close();
//      Stream = null;

      StateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetAllParams()
    {
      // set all properties

      
      NativeSoapySdr.SoapySDRDevice_setSampleRate(Device, NativeSoapySdr.Direction.Rx, 0, Info.SampleRate);
      SoapySdr.CheckError();
      NativeSoapySdr.SoapySDRDevice_setBandwidth(Device, NativeSoapySdr.Direction.Rx, 0, Info.Bandwidth);
      SoapySdr.CheckError();

      SetFrequency(Frequency);
      SetGain(Gain);
    }






    //----------------------------------------------------------------------------------------------
    //                                     set params
    //----------------------------------------------------------------------------------------------
    private void SetGain(double value)
    {
      Info.Gain = (float)Math.Min(Info.GainRange.maximum, Math.Max(Info.GainRange.minimum, value));

      if (Device != IntPtr.Zero)
      {
        NativeSoapySdr.SoapySDRDevice_setGain(Device, NativeSoapySdr.Direction.Rx, 0, Info.Gain);
        SoapySdr.CheckError();
      }
    }

    private void SetFrequency(double value)
    {
      if (Info.FrequencyRange.Any(r => value >= r.minimum && value <= r.maximum))
      {
        Info.Frequency = value;
        NativeSoapySdr.SoapySDRDevice_setFrequency(Device, NativeSoapySdr.Direction.Rx, 0, value, IntPtr.Zero);
      }
      else
        Log.Error($"Attempted to set an invalid frequency for {Info.Name}: {value} Hz");
    }




    //----------------------------------------------------------------------------------------------
    //                                     IDispose
    //----------------------------------------------------------------------------------------------
    public void Dispose()
    {
      Stop();
    }
  }
}
