using System.Reflection;

namespace VE3NEA
{
  public static class SoapySdr
  {
    public static string ABIVersion => "0.8-3";

    static SoapySdr()
    {
      SetSoapySdrPluginFolder();
    }


    // Force SoapySDR to look for the SDR drivers in a sub-folder of the app folder
    public static void SetSoapySdrPluginFolder()
    {
      string? appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      if (appDir == null) throw new Exception("Cannot get assembly location");
      Environment.SetEnvironmentVariable("SOAPY_SDR_ROOT", appDir);
      string path = Environment.GetEnvironmentVariable("PATH") ?? "";
      path = $"{appDir}\\lib\\SoapySDR\\modules{ABIVersion};{path}";
      Environment.SetEnvironmentVariable("PATH", path);
    }

    public static SoapySdrDeviceInfo[] EnumerateDevices(string args = "")
    {
      IntPtr ptr = NativeSoapySdr.SoapySDRDevice_enumerateStrArgs(args, out IntPtr length);
      CheckError();

      if (ptr == IntPtr.Zero) return Array.Empty<SoapySdrDeviceInfo>();
      var kwargs = SoapySdrHelper.MarshalKwArgsArray(ptr, length);
      return kwargs.Select(args => new SoapySdrDeviceInfo(args)).ToArray();
    }

    public static void CheckError()
    {
      if (NativeSoapySdr.SoapySDRDevice_lastStatus() != 0)
        throw new Exception("SoapySDR error: " + NativeSoapySdr.SoapySDRDevice_lastError());
    }
  }
}