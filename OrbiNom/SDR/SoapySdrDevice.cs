using System.Diagnostics;
using MathNet.Numerics;
using Serilog;
using static VE3NEA.NativeSoapySdr;

namespace VE3NEA
{
  public class SoapySdrDevice : IDisposable
  {
    private IntPtr Device;
    private SoapySdrStream? Stream;
    private Thread? Thread;

    public readonly SoapySdrDeviceInfo Info;
    private bool Started;
    private bool Stopping;

    public double Frequency { get => Info.Frequency; set => SetFrequency(value); }
    public double Gain { get => Info.Gain; set => SetGain(value); }
    public bool Enabled { get => enabled; set => SetEnabled(value); }
    private bool enabled;

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

      // was started but is no longer running
      if (Started) 
      {
        Started = false;
        Log.Error($"{Info.Name} failed, restarting");
        StateChanged?.Invoke(this, EventArgs.Empty);
      }

      if (Enabled && !Started) TryStart(false);
    }

    public bool IsRunning()
    {
      return Thread?.IsAlive ?? false;
    }




    //----------------------------------------------------------------------------------------------
    //                                     worker thread
    //----------------------------------------------------------------------------------------------
    public void Start()
    {      
      if (!SoapySdr.DeviceExists(Info.KwArgs)) 
        throw new Exception($"Device {Info.Name} is no longer available");

      Device = SoapySdr.CreateDevice(Info.KwArgs);
      SetAllParams();

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
    }

    private void ThreadProcedure()
    {
      Stream = new(Device);

      while (!Stopping)
        try
        {
          Stream.ReadStream();
          DataAvailable?.Invoke(this, Stream.Args);
        }
        catch (Exception ex)
        {
          Log.Error(ex, $"{Info.Name} read failed");
          break;
        }

      Stream.Dispose();
      Stream = null;
      SoapySdr.ReleaseDevice(Device);
      Device = IntPtr.Zero;

      StateChanged?.Invoke(this, EventArgs.Empty);
    }




    //----------------------------------------------------------------------------------------------
    //                                     set params
    //----------------------------------------------------------------------------------------------
    private void SetAllParams()
    {
      // {!} set all properties here


      SoapySDRDevice_setSampleRate(Device, Direction.Rx, 0, Info.SampleRate);
      SoapySdr.CheckError();
      SoapySDRDevice_setBandwidth(Device, Direction.Rx, 0, Info.Bandwidth);
      SoapySdr.CheckError();

      SetFrequency(Frequency);
      SetGain(Gain);
    }

    private void SetFrequency(double value)
    {
      if (Info.FrequencyRange.Any(r => value >= r.minimum && value <= r.maximum))
      {
        Info.Frequency = value;
        SoapySDRDevice_setFrequency(Device, Direction.Rx, 0, value, IntPtr.Zero);
      }
      else
        Log.Error($"Attempted to set an invalid frequency for {Info.Name}: {value} Hz");
    }

    private void SetGain(double value)
    {
      Info.Gain = (float)Math.Min(Info.GainRange.maximum, Math.Max(Info.GainRange.minimum, value));

      if (Device != IntPtr.Zero)
      {
        SoapySDRDevice_setGain(Device, Direction.Rx, 0, Info.Gain);
        SoapySdr.CheckError();
      }
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
