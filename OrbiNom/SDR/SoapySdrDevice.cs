using System.Diagnostics;
using System.Runtime.InteropServices;
using MathNet.Numerics;
using Serilog;
using static VE3NEA.NativeSoapySdr;

namespace VE3NEA
{
  public class SoapySdrDevice : IDisposable
  {
    private IntPtr Device;
    private Thread? Thread;
    private bool Stopping;
    DataEventArgs<Complex32> Args = new();
    private bool enabled;
    private bool Started;
    private IntPtr Stream;
    private float[] FloatSamples;
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
      return Thread?.IsAlive ?? false;
    }




    //----------------------------------------------------------------------------------------------
    //                                     worker thread
    //----------------------------------------------------------------------------------------------
    public void Start()
    {      
      if (!SoapySdr.DeviceExists(Info.KwArgs)) throw new Exception($"Device {Info.Name} is no longer available");

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

    private nint samplesPerBlock, floatsPerBlock;

    private void ThreadProcedure()
    {
      Stream = SetupStream();

      long timeout = 300_000; // microseconds

      samplesPerBlock = SoapySDRDevice_getStreamMTU(Device, Stream);
      floatsPerBlock = 2 * samplesPerBlock;
      Args.Data = new Complex32[samplesPerBlock];
      FloatSamples = new float[floatsPerBlock];

      SoapySDRDevice_activateStream(Device, Stream, SoapySdrFlags.None, 0, 0);

      while (!Stopping)
        try
        {
          int floatCount = ReadStream(FloatSamples, out SoapySdrFlags flags, timeout);
          SoapySdr.CheckError();
          if (floatCount < 0) throw new Exception($"Error code {(SoapySdrError)floatCount}");

          Args.Count = floatCount / 2;
          for (int i = 0; i < Args.Count; i++) 
            Args.Data[i] = new Complex32(FloatSamples[i * 2], FloatSamples[i * 2 + 1]);

          DataAvailable?.Invoke(this, Args);
        }
        catch (Exception ex)
        {
          Log.Error(ex, $"{ Info.Name} read failed");
          break;
        }

      SoapySDRDevice_deactivateStream(Device, Stream, 0, out long timeNs);
      SoapySDRDevice_closeStream(Device, Stream);
      Stream = IntPtr.Zero;
      SoapySdr.ReleaseDevice(Device);
      Device = IntPtr.Zero;

      StateChanged?.Invoke(this, EventArgs.Empty);
    }

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



    //----------------------------------------------------------------------------------------------
    //                                     stream
    //----------------------------------------------------------------------------------------------
    public IntPtr SetupStream()
    {
      nint[] channels = [0];
      IntPtr ptr = IntPtr.Zero;

      try
      {
        int size = Marshal.SizeOf(typeof(nint));
        ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(channels, 0, ptr, 1);
        return SoapySDRDevice_setupStream(Device, Direction.Rx, "CF32", ptr, 1, IntPtr.Zero);
      }
      finally
      {
        SoapySdr.CheckError();
        if (ptr != IntPtr.Zero) Marshal.FreeHGlobal(ptr);
      }
    }

    public int ReadStream(float[] buff, out SoapySdrFlags flags, long timeoutUs)
    {
      IntPtr buffsPtr = IntPtr.Zero;
      GCHandle buffHandle = default;

      // {!} todo: do this once
      try
      {
        if (buff != null && buff.Length > 0)
        {
          buffHandle = GCHandle.Alloc(buff, GCHandleType.Pinned);
          IntPtr[] buffsArray = [buffHandle.AddrOfPinnedObject()];
          buffsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * buffsArray.Length);
          Marshal.Copy(buffsArray, 0, buffsPtr, buffsArray.Length);
        }
        return SoapySDRDevice_readStream(Device, Stream, buffsPtr, buff!.Length, out flags, out long timeNs, timeoutUs);
      }
      finally
      {
        if (buffsPtr != IntPtr.Zero) Marshal.FreeHGlobal(buffsPtr);
        if (buffHandle.IsAllocated) buffHandle.Free();
      }
    }




    //----------------------------------------------------------------------------------------------
    //                                     set params
    //----------------------------------------------------------------------------------------------
    private void SetGain(double value)
    {
      Info.Gain = (float)Math.Min(Info.GainRange.maximum, Math.Max(Info.GainRange.minimum, value));

      if (Device != IntPtr.Zero)
      {
        SoapySDRDevice_setGain(Device, Direction.Rx, 0, Info.Gain);
        SoapySdr.CheckError();
      }
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




    //----------------------------------------------------------------------------------------------
    //                                     IDispose
    //----------------------------------------------------------------------------------------------
    public void Dispose()
    {
      Stop();
    }
  }
}
