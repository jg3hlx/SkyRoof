using VE3NEA;
using Newtonsoft.Json;
using System.ComponentModel;
using JTSkimmer;

namespace OrbiNom
{
  public class Settings
  {
    public UiSettings Ui = new();
    public SatelliteSettings Satellites = new();
    public SdrSettings Sdr = new();
    public WaterfallSettings Waterfall = new();



    [TypeConverter(typeof(ExpandableObjectConverter))]
    public UserSettings User { get; set; } = new();


    [TypeConverter(typeof(ExpandableObjectConverter))]
    public AudioSettings Audio { get; set; } = new();

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public AnnouncerSettings Announcements { get; set; } = new();



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