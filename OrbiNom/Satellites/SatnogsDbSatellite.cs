using System.Text.RegularExpressions;

namespace SatNOGS
{
  public class SatnogsDbSatellite
  {
    public class Telemetry { public string decoder; }


    public string sat_id { get; set; }
    public int? norad_cat_id { get; set; }
    public int? norad_follow_id { get; set; }
    public string name { get; set; }
    public string names { get; set; }
    public string image { get; set; }
    public string status { get; set; }
    public DateTime? decayed { get; set; }
    public DateTime? launched { get; set; }
    public DateTime? deployed { get; set; }
    public string website { get; set; }
    public string @operator { get; set; }
    public string countries { get; set; }
    public List<Telemetry> telemetries { get; set; }
    public DateTime? updated { get; set; }
    public string citation { get; set; }
    public bool is_frequency_violator { get; set; }
    public List<string> associated_satellites { get; set; }

    public List<SatnogsDbTransmitter> Transmitters = new();
    public SatnogsDbTle Tle;

    public List<string> JE9PEL_Names = new();
    public List<string> JE9PEL_Callsigns = new();
    public string LotwName;

    public string SearchText;
    public List<string> AllNames = new();


    internal void BuildAllNames()
    {
      AllNames.Clear();
      AllNames.Add(name);
      AllNames.AddRange(names.Replace("\r\n", ",").Split(",").Select(n => n.Trim()));
      if (Tle != null) AllNames.Add(Tle.tle0.Substring(2));
      AllNames.AddRange(JE9PEL_Names);
      AllNames.AddRange(JE9PEL_Callsigns);

      AllNames = AllNames.Distinct().Where(n => n != "").ToList();
      SearchText = $"{MakeSearchText(string.Join("|", AllNames))}|{norad_cat_id}";

      SearchText = $"{string.Join("|", AllNames.Select(MakeSearchText))}|{norad_cat_id}";
    }
    public static string MakeSearchText(string s) { return Regex.Replace(s, "[^a-zA-Z0-9|]", "").ToLower(); }
  }


  public class SatnogsDbSatelliteList : List<SatnogsDbSatellite> { }
}
