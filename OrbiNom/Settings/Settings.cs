using VE3NEA;
using Newtonsoft.Json;
using System.ComponentModel;

namespace OrbiNom
{
  public class Settings
  {
    public UiSettings Ui = new();
    public SatelliteSettings Satellites = new();



    [TypeConverter(typeof(ExpandableObjectConverter))]
    public UserSettings User { get; set; } = new();



    private static string GetFileName()
    {
      return Path.Combine(Utils.GetUserDataFolder(), "Settings.json");
    }

    public void LoadFromFile()
    {
      if (File.Exists(GetFileName()))
        JsonConvert.PopulateObject(File.ReadAllText(GetFileName()), this);
      SetDefaults();
    }

    public void SaveToFile()
    {
      File.WriteAllText(GetFileName(), JsonConvert.SerializeObject(this));
    }

    private void SetDefaults()
    {
      Satellites.Sanitize();
    }
  }
}