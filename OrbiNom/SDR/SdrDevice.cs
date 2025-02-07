using System.Diagnostics;
using VE3NEA;
using Pothosware.SoapySDR;
using Range = Pothosware.SoapySDR.Range;
using Stream = Pothosware.SoapySDR.Stream;
using MathNet.Numerics;


namespace OrbiNom
{
  internal class SdrDevice : IDisposable
  {
    private Device Device;
    private RxStream Stream;
    private float[] FloatSamples;
    private Complex32[] Samples;
    private Thread Thread;
    private bool Stopping;
    private double frequency;
    private double gain;

    public readonly Kwargs Kwargs;
    public SdrProperties Properties;

    public string Name { get => Kwargs["label"]; }

    public double Gain { get => gain; set => SetGain(value);  }
    public double Frequency { get => frequency; set => SetFrequency(value); }
    public double SampleRate;
    public double Bandwidth;
    public readonly Range GainRange;
    public readonly RangeList FrequencyRange;
    public readonly RangeList SampleRateRange;
    public readonly RangeList BandwidthRange;

    public event EventHandler<DataEventArgs<Complex32>>? DataAvailable;
    public event EventHandler? StateChanged;


    public SdrDevice(Kwargs kwargs, SdrProperties? properties = null)
      {
      Kwargs = kwargs;
      Device = new(kwargs);

      GainRange = Device.GetGainRange(Direction.Rx, 0);
      FrequencyRange = Device.GetFrequencyRange(Direction.Rx, 0);
      SampleRateRange = Device.GetSampleRateRange(Direction.Rx, 0);
      BandwidthRange = Device.GetBandwidthRange(Direction.Rx, 0);

      gain = Device.GetGain(Direction.Rx, 0);
      frequency = Device.GetFrequency(Direction.Rx, 0);
      SampleRate = Device.GetSampleRate(Direction.Rx, 0);
      Bandwidth = Device.GetSampleRate(Direction.Rx, 0);

      // todo: load not only properties but gain, etc.
      Properties = properties ?? GetProperties(Device);
    }

    public void Start()
    {
      Thread = new Thread(new ThreadStart(ProcessingThreadProcedure));
      Thread.IsBackground = true;
      Thread.Name = GetType().Name;
      Stopping = false;
      Thread.Start();
      Thread.Priority = ThreadPriority.Highest;
    }

    public void Stop()
    {
      Stopping = true;
      Thread.Join();
    }

    
    private void ProcessingThreadProcedure()
    {
      Device.SetSampleRate(Direction.Rx, 0, SampleRate);
      Device.SetBandwidth(Direction.Rx, 0, Bandwidth);
      SetFrequency(Frequency);
      SetGain(Gain);
      
      var channels = new uint[] { 0 };
      Stream = Device.SetupRxStream("CF32", channels, "");
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

        for (int i=0; i < Samples.Count(); i++) Samples[i] = new Complex32(FloatSamples[i*2], FloatSamples[i*2+1]);
        //Buffer.BlockCopy(FloatSamples, 0, Samples, 0, FloatSamples.Length * sizeof(float));
        Args.SetValues(Samples);
        DataAvailable?.Invoke(this, Args);
      }

      Stream.Deactivate();
      Stream.Close();
      Stream = null;

      StateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetGain(double value)
    {
      gain = Math.Min(GainRange.Maximum, Math.Max(GainRange.Minimum, value));
      Device.SetGain(Direction.Rx, 0, gain);
    }

    private void SetFrequency(double value)
    {
      if (FrequencyRange.FirstOrDefault(r => value >= r.Minimum && value <= r.Maximum) != null)
      {
        frequency = value;
        Device.SetFrequency(Direction.Rx, 0, frequency);
      }
      else
        Debug.WriteLine($"Invalid frequency: {value}");
    }


    private SdrProperties GetProperties(Device device)
    {
      // properties from list

      var settingInfo = device.GetSettingInfo();
      SdrProperties sdrProperties = new();
      var properties = settingInfo.Select(s => new SdrProperty(s, device.ReadSetting(s.Key))).ToList();
      foreach (SdrProperty p in properties) sdrProperties.Add(p);

      // static properties

      var argInfo = new ArgInfo();
      argInfo.ArgType = ArgInfo.Type.String;
      argInfo.Name = "Antenna";
      argInfo.Description = "Antenna Input Selection";
      argInfo.Options = device.ListAntennas(Direction.Rx, 0);
      string value = device.GetAntenna(Direction.Rx, 0);
      var property = new SdrProperty(argInfo, value);
      sdrProperties.Add(property);

      if (device.HasDCOffsetMode(Direction.Rx, 0))
      {
        argInfo = new ArgInfo();
        argInfo.ArgType = ArgInfo.Type.Bool;
        argInfo.Name = "DCOffsetMode";
        argInfo.Description = "Enable or disable automatic frontend DC offset correction";
        value = device.GetDCOffsetMode(Direction.Rx, 0) ? "True" : "False";
        property = new SdrProperty(argInfo, value);
        sdrProperties.Add(property);
      }

      if (device.HasIQBalanceMode(Direction.Rx, 0))
      {
        argInfo = new ArgInfo();
        argInfo.ArgType = ArgInfo.Type.Bool;
        argInfo.Name = "IQBalanceMode";
        argInfo.Description = "Enable or disable automatic frontend IQ balance correction";
        value = device.GetIQBalanceMode(Direction.Rx, 0) ? "True" : "False";
        property = new SdrProperty(argInfo, value);
        sdrProperties.Add(property);
      }

      if (device.HasGainMode(Direction.Rx, 0))
      {
        argInfo = new ArgInfo();
        argInfo.ArgType = ArgInfo.Type.Bool;
        argInfo.Name = "GainMode";
        argInfo.Description = "Enable or disable automatic gain control on the chain";
        value = device.GetGainMode(Direction.Rx, 0) ? "True" : "False";
        property = new SdrProperty(argInfo, value);
        sdrProperties.Add(property);
      }

      // gains
      var gainNames = device.ListGains(Direction.Rx, 0);
      foreach (string gainName in gainNames)
      {
        argInfo = new ArgInfo();
        argInfo.ArgType = ArgInfo.Type.Float;
        argInfo.Name = gainName + " Gain";
        argInfo.Key = gainName;
        argInfo.Range = device.GetGainRange(Direction.Rx, 0, gainName);
        value = device.GetGain(Direction.Rx, 0, gainName).ToString();
        property = new SdrProperty(argInfo, value);
        sdrProperties.Add(property);
      }

      return sdrProperties;
    }

    public void Dispose()
    {
      Stop();
      Device?.Dispose();
      Device = null;
    }
  }
}
