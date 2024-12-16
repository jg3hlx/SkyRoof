using System.ComponentModel;
using System.Text.RegularExpressions;

namespace OrbiNom
{
  public class SatnogsDbSatellite
  {
    public class Telemetry { public string decoder; }

    [ReadOnly(true)]
    public string sat_id { get; set; }

    [ReadOnly(true)]
    public int? norad_cat_id { get; set; }

    [ReadOnly(true)]
    public int? norad_follow_id { get; set; }

    [ReadOnly(true)]
    public string name { get; set; }

    [ReadOnly(true)]
    public string names { get; set; }

    [ReadOnly(true)]
    public string image { get; set; }

    [ReadOnly(true)]
    public string status { get; set; }

    [ReadOnly(true)]
    public DateTime? decayed { get; set; }

    [ReadOnly(true)]
    public DateTime? launched { get; set; }

    [ReadOnly(true)]
    public DateTime? deployed { get; set; }

    [ReadOnly(true)]
    public string website { get; set; }

    [ReadOnly(true)]
    public string @operator { get; set; }

    [ReadOnly(true)]
    public string countries { get; set; }

    [ReadOnly(true)]
    public List<Telemetry> telemetries { get; set; }

    [ReadOnly(true)]
    public DateTime? updated { get; set; }

    [ReadOnly(true)]
    public string citation { get; set; }

    [ReadOnly(true)]
    public bool is_frequency_violator { get; set; }

    [ReadOnly(true)]
    public List<string> associated_satellites { get; set; }

    [Browsable(false)]
    public List<SatnogsDbTransmitter> Transmitters = new();

    [Browsable(false)]
    public SatnogsDbTle Tle;

    [ReadOnly(true)]
    public List<string> JE9PEL_Names { get; set; } = new();

    [ReadOnly(true)]
    public List<string> JE9PEL_Callsigns { get; set; } = new ();

    [ReadOnly(true)]
    public string LotwName { get; set; }

    [Browsable(false)]
    public string SearchText;

    [Browsable(false)]
    public List<string> AllNames = new();

    [Browsable(false)]
    public SatelliteFlags Flags;

    internal void BuildAllNames()
    {
      AllNames.Clear();
      AllNames.Add(name);
      AllNames.AddRange(names.Replace("\r\n", ",").Split(",").Select(n => n.Trim()));

      if (Tle != null)
      {
        string tleName = Tle.tle0.StartsWith("0 ") ? Tle.tle0.Substring(2) : Tle.tle0;
        AllNames.Add(tleName);
      }

      AllNames.AddRange(JE9PEL_Names);
      AllNames.AddRange(JE9PEL_Callsigns);

      AllNames = AllNames.Distinct().Where(n => n != "").ToList();
      SearchText = $"{MakeSearchText(string.Join("|", AllNames))}|{norad_cat_id}";

      SearchText = $"{string.Join("|", AllNames.Select(MakeSearchText))}|{norad_cat_id}";
    }

    internal void SetFlags()
    {
      // status
      if (status == "alive") Flags = SatelliteFlags.Alive;
      else if (status == "future") Flags = SatelliteFlags.Future;
      else Flags = SatelliteFlags.ReEntered;

      // ham
      if (Transmitters.Any(t => t.service == "Amateur")) Flags |= SatelliteFlags.Ham;
      else Flags |= SatelliteFlags.NonHam;

      // band
      if (Transmitters.Any(t => t.downlink_low >= 144000000 && t.downlink_low <= 148000000))
        Flags |= SatelliteFlags.Vhf;
      if (Transmitters.Any(t => t.downlink_low >= 430000000 && t.downlink_low <= 440000000))
        Flags |= SatelliteFlags.Uhf;
      if ((Flags & (SatelliteFlags.Vhf | SatelliteFlags.Uhf)) == SatelliteFlags.None)
        Flags |= SatelliteFlags.OtherBands;

      // tx
      if (Transmitters.Any(t => t.type == "Transponder")) Flags |= SatelliteFlags.Transponder;
      if (Transmitters.Any(t => t.type == "Transceiver")) Flags |= SatelliteFlags.Transceiver;
      if ((Flags & (SatelliteFlags.Transponder | SatelliteFlags.Transceiver)) == SatelliteFlags.None)
        Flags |= SatelliteFlags.Transmitter;
    }

    public static string MakeSearchText(string s) { return Regex.Replace(s, "[^a-zA-Z0-9|]", "").ToLower(); }
  }


  public class SatnogsDbSatelliteList : List<SatnogsDbSatellite> { }
}
