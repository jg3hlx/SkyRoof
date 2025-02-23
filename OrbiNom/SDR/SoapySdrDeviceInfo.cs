using System.ComponentModel;
using System.Runtime.InteropServices;
using static VE3NEA.NativeSoapySdr;

namespace VE3NEA
{
  public class SoapySdrDeviceInfo
  {
    private IntPtr Device;
    internal bool Present;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Name { get => KwArgs["label"]; }
    public SoapySDRKwargs KwArgs;
    public SoapySDRRange[] FrequencyRange, SampleRateRange, BandwidthRange;
    public SoapySDRRange GainRange;
    public double SampleRate, Bandwidth, Frequency, Gain;
    public double Ppm => GetPpm();

    public SdrProperties Properties = new();

    public SoapySdrDeviceInfo() { }

    public SoapySdrDeviceInfo(SoapySDRKwargs kwArgs)
    {
      KwArgs = kwArgs;

      Device = SoapySdr.CreateDevice(kwArgs);
      ReadCapabilities();
      ReadProperties();
      SoapySdr.ReleaseDevice(Device);

      Present = true;
    }




    //----------------------------------------------------------------------------------------------
    //                                 private methods
    //----------------------------------------------------------------------------------------------
    private void ReadCapabilities()
    {
      // gain range
      GainRange = SoapySDRDevice_getGainRange(Device, Direction.Rx, 0);

      // frequency range
      IntPtr ptr, length;
      ptr = SoapySDRDevice_getFrequencyRange(Device, Direction.Rx, 0, out length);
      SoapySdr.CheckError();
      FrequencyRange = SoapySdrHelper.MarshalRangeArray(ptr, length);

      // sampling rate range
      ptr = SoapySDRDevice_getSampleRateRange(Device, Direction.Rx, 0, out length);
      SoapySdr.CheckError();
      SampleRateRange = SoapySdrHelper.MarshalRangeArray(ptr, length);

      // bandwidth range
      ptr = SoapySDRDevice_getBandwidthRange(Device, Direction.Rx, 0, out length);
      SoapySdr.CheckError();
      BandwidthRange = SoapySdrHelper.MarshalRangeArray(ptr, length);

      // initial values
      SampleRate = SoapySDRDevice_getSampleRate(Device, Direction.Rx, 0);
      SoapySdr.CheckError();
      Bandwidth = SoapySDRDevice_getBandwidth(Device, Direction.Rx, 0);
      SoapySdr.CheckError();
      Frequency = SoapySDRDevice_getFrequency(Device, Direction.Rx, 0);
      SoapySdr.CheckError();
      Gain = SoapySDRDevice_getGain(Device, Direction.Rx, 0);
      SoapySdr.CheckError();
    }

    private void ReadProperties()
    {
      Properties.Clear();

      // common properties

      // antenna
      AddCommonProperty(
        SoapySDRArgInfoType.String,
        "Antenna",
        "Antenna Input Selection",
        GetAntenna(Device, Direction.Rx, 0),
        ListAntennas(Direction.Rx, 0));

      // DC offset
      if (SoapySDRDevice_hasDCOffsetMode(Device, Direction.Rx, 0))
        AddCommonProperty(
          SoapySDRArgInfoType.Bool,
          "DCOffsetMode",
          "Automatic frontend DC offset correction",
          SoapySDRDevice_getDCOffsetMode(Device, Direction.Rx, 0) ? "true" : "false");

      // IQ balance
      if (SoapySDRDevice_hasIQBalanceMode(Device, Direction.Rx, 0))
        AddCommonProperty(
          SoapySDRArgInfoType.Bool,
          "IQBalanceMode",
          "Automatic frontend IQ balance correction",
           SoapySDRDevice_getIQBalanceMode(Device, Direction.Rx, 0) ? "true" : "false");

      // AGC
      if (SoapySDRDevice_hasGainMode(Device, Direction.Rx, 0))
        AddCommonProperty(
          SoapySDRArgInfoType.Bool,
          "AGC",
          "Automatic gain control on the chain",
           SoapySDRDevice_getGainMode(Device, Direction.Rx, 0) ? "true" : "false");

      // PPM
      AddCommonProperty(
        SoapySDRArgInfoType.Float,
        "PPM",
        "SDR clock correction, Parts Per Million",
        "0");

      // Single Gain
      AddCommonProperty(
        SoapySDRArgInfoType.Bool,
        "Single Gain",
        "When true, gain is controlled by a slider, individual stage gains are ignored",
        "true");


      // model-specific properties

      var ptr = SoapySDRDevice_getSettingInfo(Device, out nint length);
      SoapySdr.CheckError();
      var argsInfo = SoapySdrHelper.MarshalArgsInfoArray(ptr, length);

      var properties = argsInfo.Select(s => new SdrProperty(s, ReadSetting(s.Key), "Model-specific")).ToList();
      foreach (SdrProperty p in properties) Properties.Add(p);

      // gains
      var gainNames = ListGains(Direction.Rx, 0);
      foreach (string gainName in gainNames)
      {
        AddCommonProperty(
          SoapySDRArgInfoType.Float,
          gainName,
          "",
           SoapySDRDevice_getGainElement(Device, Direction.Rx, 0, gainName).ToString(),
           null,
           SoapySDRDevice_getGainElementRange(Device, Direction.Rx, 0, gainName));
        Properties.Last().Category = "Stage Gains";
      }
    }

    private void AddCommonProperty(SoapySDRArgInfoType type, string name, string description, 
      string value, Dictionary<string, string>? options = null, SoapySDRRange range = new())
    {
      var argInfo = new SoapySDRArgInfo();
      argInfo.Type =type;
      argInfo.Name = name;
      argInfo.Description = description;
      argInfo.Options = options ?? new(); 
      argInfo.Range = range;

      var property = new SdrProperty(argInfo, value, "Common");
      Properties.Add(property);
    }

    private string ReadSetting(string key)
    {
      IntPtr ptr = SoapySDRDevice_readSetting(Device, key);
      SoapySdr.CheckError();
      return Marshal.PtrToStringAnsi(ptr);
    }

    private Dictionary<string, string>? ListAntennas(Direction rx, int v)
    {
      var ptr = SoapySDRDevice_listAntennas(Device, Direction.Rx, 0, out nint length);
      SoapySdr.CheckError();
      var values = SoapySdrHelper.MarshalStringArray(ptr, length);
      return values.ToDictionary(item => item);
    }

    private static string GetAntenna(IntPtr device, Direction direction, nint channel)
    {
      IntPtr ptr = SoapySDRDevice_getAntenna(device, direction, channel);
      SoapySdr.CheckError();
      return Marshal.PtrToStringAnsi(ptr);
    }

    private string[] ListGains(Direction rx, int v)
    {
      var ptr = SoapySDRDevice_listGains(Device, Direction.Rx, 0, out nint count);
      SoapySdr.CheckError();
      return SoapySdrHelper.MarshalStringArray(ptr, count);
    }

    private double GetPpm()
    {
      return double.Parse(Properties.Find(p => p.Name == "PPM")!.Value);
    }
  }
}
