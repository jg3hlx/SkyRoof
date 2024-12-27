using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using static OrbiNom.SatnogsDbSatellite;

namespace OrbiNom
{
  public class SatnogsDbSatellite
  {
    public class Telemetry
    {
      public string decoder;
      public override string ToString() => decoder;
    }

    public class Telemetries : List<Telemetry>
    {
      public override string ToString() => string.Join(", ", this.Select(t => t.decoder));
    }



    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    public string sat_id { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    public int? norad_cat_id { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    public int? norad_follow_id { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    public string name { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    public string names { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    public string image { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    public string status { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    public DateTime? decayed { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    public DateTime? launched { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    public DateTime? deployed { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    public string website { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    public string @operator { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    public string countries { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    [TypeConverter(typeof(TelemetriesTypeConverter))]
    public Telemetries telemetries { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    public DateTime? updated { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    public string citation { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    public bool is_frequency_violator { get; set; }

    [ReadOnly(true)]
    [Category("SatNOGS Database")]
    [TypeConverter(typeof(CsvTypeConverter))]
    public List<string> associated_satellites { get; set; }



    // other sources

    [ReadOnly(true)]
    [Category("JE9PEL")]
    [DisplayName("Names")]
    [TypeConverter(typeof(CsvTypeConverter))]
    public List<string> JE9PEL_Names { get; set; } = new();

    [ReadOnly(true)]
    [Category("JE9PEL")]
    [DisplayName("Callsigns")]
    [TypeConverter(typeof(CsvTypeConverter))]
    public List<string> JE9PEL_Callsigns { get; set; } = new();

    [ReadOnly(true)]
    [Category("LoTW")]
    [DisplayName("Name")]
    public string LotwName { get; set; }



    // orbit

    [ReadOnly(true)]
    [Category("Orbit")]
    [DisplayName("Period, min")]
    public int? Period { get; set; }

    [ReadOnly(true)]
    [Category("Orbit")]
    [DisplayName("Inclination, deg.")]
    public int? Inclination { get; set; }

    [ReadOnly(true)]
    [Category("Orbit")]
    [DisplayName("Elevation, km")]
    public int? Elevation { get; set; }

    [ReadOnly(true)]
    [Category("Orbit")]
    [DisplayName("Footprint, km")]
    public int? Footprint { get; set; }



    // non-browsable

    [Browsable(false)]
    public List<SatnogsDbTransmitter> Transmitters = new();

    [Browsable(false)]
    public SatnogsDbTle Tle;

    [Browsable(false)]
    public string SearchText;

    [Browsable(false)]
    public List<string> AllNames = new();

    [Browsable(false)]
    public SatelliteFlags Flags;

    [Browsable(false)]
    public List<JE9PELtransmitter> JE9PELtransmitters { get; set; } = new();



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

    public string GetTooltipText()
    {
      string names = string.Join(", ", AllNames);
      string radio = "Transmitter";
      if (Flags.HasFlag(SatelliteFlags.Transponder)) radio = "Transponder";
      else if (Flags.HasFlag(SatelliteFlags.Transceiver)) radio = "Transceiver";
      SetPeriodAndInclination();

      string tooltipText = $"{names}\nstatus: {status}\ncountries: {countries}";
      if (Tle != null) tooltipText += $"\nTLE: available\nperiod: {Period} min.\ninclination: {Inclination}°";
      tooltipText += $"\nradio: {radio}";
      if (!string.IsNullOrEmpty(LotwName)) tooltipText += "\nAccepted by LoTW";
      tooltipText += $"\nupdated: {updated:yyyy-mm-dd}";

      return tooltipText;
    }

    internal void SetPeriodAndInclination()
    {
      if (Tle == null || Period != null) return;
      float v;
      string s = Tle.tle2.Substring(8, 8);
      if (float.TryParse(s, out v)) Inclination = (int)v;
      s = Tle.tle2.Substring(52, 10);
      if (float.TryParse(s, out v)) Period = (int)(1440f / v);
    }
  }



  public class SatnogsDbSatelliteList : List<SatnogsDbSatellite> { }

  public class CsvTypeConverter : StringConverter
  {
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      return string.Join(", ", (List<string>)value);
    }
  }

  public class TelemetriesTypeConverter : StringConverter
  {
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      return ((Telemetries)value).ToString();
    }
  }
}