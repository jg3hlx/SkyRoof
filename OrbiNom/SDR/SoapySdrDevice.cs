using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VE3NEA
{
  public class SoapySdrDevice : IDisposable
  {
    public readonly SoapySdrDeviceInfo Info;

    public double Frequency { get => Info.Frequency; set => SetFrequency(value); }
    public double Gain { get => Info.Gain; set => SetGain(value); }
    

    public SoapySdrDevice(SoapySdrDeviceInfo info)
    { 
      Info = info;
    }


    private void SetGain(double value)
    {
      //gain = Math.Min(GainRange.Maximum, Math.Max(GainRange.Minimum, value));
      //Device.SetGain(Direction.Rx, 0, gain);
    }

    private void SetFrequency(double value)
    {
      //if (FrequencyRange.FirstOrDefault(r => value >= r.Minimum && value <= r.Maximum) != null)
      //{
      //  frequency = value;
      //  Device.SetFrequency(Direction.Rx, 0, frequency);
      //}
      //else
      //  Debug.WriteLine($"Invalid frequency: {value}");
    }



    public void Start()
    {
      /*
      Thread = new Thread(new ThreadStart(ProcessingThreadProcedure));
      Thread.IsBackground = true;
      Thread.Name = GetType().Name;
      Stopping = false;
      Thread.Start();
      Thread.Priority = ThreadPriority.Highest;
      */
    }

    public void Stop()
    {
      /*
      Stopping = true;
      Thread.Join();
      */

      //if (Device != IntPtr.Zero) SoapySDRDevice_unmake(Device);
    }


    private void ProcessingThreadProcedure()
    {
      /*
      SetSampleRate(Direction.Rx, 0, SampleRate);
      SetBandwidth(Direction.Rx, 0, Bandwidth);
      SetFrequency(Frequency);
      SetGain(Gain);

      var channels = new uint[] { 0 };
      Stream = SetupRxStream("CF32", channels, "");
      //Stream = Device.SetupRxStream("CF32", [0], "");

      int timeout = 1000000;
      ulong samplesPerBlock = Stream.MTU;
      ulong floatsPerBlock = samplesPerBlock * 2;
      Samples = new Complex32[samplesPerBlock];
      FloatSamples = new float[floatsPerBlock];
      DataEventArgs<Complex32> Args = new();

      Stream.Activate();

      while (!Stopping)
      {
        StreamResult streamResult = new();
        var errorCode = Stream.Read(ref FloatSamples, timeout, out streamResult);
        if (errorCode != ErrorCode.None || streamResult.NumSamples != samplesPerBlock)
        {
          Debug.WriteLine($"Stream.Read error: {errorCode}  StreamResult: {streamResult}");
          break;
        }

        for (int i = 0; i < Samples.Count(); i++) Samples[i] = new Complex32(FloatSamples[i * 2], FloatSamples[i * 2 + 1]);
        //Buffer.BlockCopy(FloatSamples, 0, Samples, 0, FloatSamples.Length * sizeof(float));
        Args.SetValues(Samples);
        DataAvailable?.Invoke(this, Args);
      }

      Stream.Deactivate();
      Stream.Close();
      Stream = null;

      StateChanged?.Invoke(this, EventArgs.Empty);
      */
    }

    public void Dispose()
    {
      Stop();
    }
  }
}
