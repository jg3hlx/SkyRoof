using VE3NEA;

namespace OrbiNom
{
  public class SdrSettings
  {
    public List<SoapySdrDeviceInfo> Devices = new();
    public string? SelectedDeviceName;
    public bool Enabled;
  }
}