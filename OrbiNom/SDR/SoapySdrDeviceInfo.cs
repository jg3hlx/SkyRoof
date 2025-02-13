using System.Runtime.InteropServices;
using static VE3NEA.NativeSoapySdr;

namespace VE3NEA
{
  public class SoapySdrDeviceInfo
  {
    private readonly SoapySDRKwargs KwArgs;
    public SoapySDRRange[] FrequencyRange, SampleRateRange, BandwidthRange;
    private IntPtr Device;
    public double SampleRate;
    public double Bandwidth;
    public double Frequency;
    public double Gain;

    public SdrProperties Properties = new();

    public string Name { get => KwArgs["label"]; }
    public SoapySDRRange GainRange;

    public SoapySdrDeviceInfo(SoapySDRKwargs kwArgs)
    {
      KwArgs = kwArgs;

      Device = CreateDevice();

      ReadCapabilities();
      ReadProperties();

      ReleaseDevice();
    }

    public IntPtr CreateDevice()
    {
      IntPtr nativeKwargs = KwArgs.ToNative();
      var device = SoapySDRDevice_make(nativeKwargs);
      Marshal.FreeHGlobal(nativeKwargs);
      SoapySdr.CheckError();
      return device;
    }

    public void ReleaseDevice()
    {
      if (Device != IntPtr.Zero) SoapySDRDevice_unmake(Device);
    }

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

      // scalar params
      Gain = SoapySDRDevice_getGain(Device, Direction.Rx, 0);
      SoapySdr.CheckError();
      Frequency = SoapySDRDevice_getFrequency(Device, Direction.Rx, 0);
      SoapySdr.CheckError();
      SampleRate = SoapySDRDevice_getSampleRate(Device, Direction.Rx, 0);
      SoapySdr.CheckError();
      Bandwidth = SoapySDRDevice_getBandwidth(Device, Direction.Rx, 0);
      SoapySdr.CheckError();
    }

    private void ReadProperties()
    {
      Properties.Clear();

      // model-specific properties
      var ptr = SoapySDRDevice_getSettingInfo(Device, out nint length);
      SoapySdr.CheckError();
      var argsInfo = SoapySdrHelper.MarshalArgsInfoArray(ptr, length);

      var properties = argsInfo.Select(s => new SdrProperty(s, ReadSetting(s.Key), false)).ToList();
      foreach (SdrProperty p in properties) Properties.Add(p);

      SoapySDRArgInfo argInfo;
      string value;
      SdrProperty property;

      // antenna
      argInfo = new SoapySDRArgInfo();
      argInfo.Type = SoapySDRArgInfoType.String;
      argInfo.Name = "Antenna";
      argInfo.Description = "Antenna Input Selection";
      argInfo.Options = ListAntennas(Direction.Rx, 0);
      value = GetAntenna(Device, Direction.Rx, 0);
      property = new SdrProperty(argInfo, value);
      Properties.Add(property);

      // DC offset
      if (SoapySDRDevice_hasDCOffsetMode(Device, Direction.Rx, 0))
      {
        argInfo = new SoapySDRArgInfo();
        argInfo.Type = SoapySDRArgInfoType.Bool;
        argInfo.Name = "DCOffsetMode";
        argInfo.Description = "Enable or disable automatic frontend DC offset correction";
        value = SoapySDRDevice_getDCOffsetMode(Device, Direction.Rx, 0) ? "True" : "False";
        property = new SdrProperty(argInfo, value);
        Properties.Add(property);
      }

      // IQ balance
      if (SoapySDRDevice_hasIQBalanceMode(Device, Direction.Rx, 0))
      {
        argInfo = new SoapySDRArgInfo();
        argInfo.Type = SoapySDRArgInfoType.Bool;
        argInfo.Name = "IQBalanceMode";
        argInfo.Description = "Enable or disable automatic frontend IQ balance correction";
        value = SoapySDRDevice_getIQBalanceMode(Device, Direction.Rx, 0) ? "True" : "False";
        property = new SdrProperty(argInfo, value);
        Properties.Add(property);
      }

      // AGC
      if (SoapySDRDevice_hasGainMode(Device, Direction.Rx, 0))
      {
        argInfo = new SoapySDRArgInfo();
        argInfo.Type = SoapySDRArgInfoType.Bool;
        argInfo.Name = "GainMode";
        argInfo.Description = "Enable or disable automatic gain control on the chain";
        value = SoapySDRDevice_getGainMode(Device, Direction.Rx, 0) ? "True" : "False";
        property = new SdrProperty(argInfo, value);
        Properties.Add(property);
      }

      // Gains
      var gainNames = ListGains(Direction.Rx, 0);
      foreach (string gainName in gainNames)
      {
        argInfo = new SoapySDRArgInfo();
        argInfo.Type = SoapySDRArgInfoType.Float;
        argInfo.Name = gainName + " Gain";
        argInfo.Key = gainName;
        argInfo.Range = SoapySDRDevice_getGainElementRange(Device, Direction.Rx, 0, gainName);
        SoapySdr.CheckError();
        value = SoapySDRDevice_getGainElement(Device, Direction.Rx, 0, gainName).ToString();
        SoapySdr.CheckError();
        property = new SdrProperty(argInfo, value, false);
        Properties.Add(property);
      }
    }

    private Dictionary<string, string>? ListAntennas(Direction rx, int v)
    {
      var ptr = SoapySDRDevice_listAntennas(Device, Direction.Rx, 0, out nint length);
      var values = SoapySdrHelper.MarshalStringArray(ptr, length);
      return values.ToDictionary(item => item);
    }

    public static string GetAntenna(IntPtr device, Direction direction, nint channel)
    {
      IntPtr ptr = SoapySDRDevice_getAntenna(device, direction, channel);
      return Marshal.PtrToStringAnsi(ptr);
    }

    private string ReadSetting(string key)
    {
      IntPtr ptr = SoapySDRDevice_readSetting(Device, key);
      return Marshal.PtrToStringAnsi(ptr);
    }

    private string[] ListGains(Direction rx, int v)
    {
      var ptr = SoapySDRDevice_listGains(Device, Direction.Rx, 0, out nint count);
      SoapySdr.CheckError();
      return SoapySdrHelper.MarshalStringArray(ptr, count);
    }
  }
}
