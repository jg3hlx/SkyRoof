using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VE3NEA;
using Newtonsoft.Json;
using System.ComponentModel;

namespace OrbiNom
{
  public class Settings
  {
    public UiSettings Ui = new();
    public SatListSettings SatList = new();
    public Customization Customization = new();





    //    [TypeConverter(typeof(ExpandableObjectConverter))]


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
    }
  }
}