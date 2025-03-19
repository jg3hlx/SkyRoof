using Newtonsoft.Json;
using OrbiNom.Properties;
using VE3NEA;

namespace OrbiNom
{
  public class AmsatData
  {
    public readonly Dictionary<string, string[]> SatelliteNames;

    public AmsatData()
    {
      string dir = Utils.GetReferenceDataFolder();
      Directory.CreateDirectory(dir);
      string path = Path.Combine(dir, "amsat_sat_names.json");
      if (!File.Exists(path)) File.WriteAllBytes(path, Resources.amsat_sat_names);

      SatelliteNames = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(File.ReadAllText(path));
    }
  }
}