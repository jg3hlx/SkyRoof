namespace SkyRoof
{
  public class SatnogsDbTransmitter
  {
    public string uuid { get; set; }
    public string description { get; set; }
    public bool alive { get; set; }
    public string type { get; set; }
    public long? uplink_low { get; set; }
    public long? uplink_high { get; set; }
    public long? uplink_drift { get; set; }
    public long? downlink_low { get; set; }
    public long? downlink_high { get; set; }
    public long? downlink_drift { get; set; }
    public string mode { get; set; }
    public int? mode_id { get; set; }
    public string uplink_mode { get; set; }
    public bool invert { get; set; }
    public float? baud { get; set; }
    public string sat_id { get; set; }
    public int? norad_cat_id { get; set; }
    public int? norad_follow_id { get; set; }
    public string status { get; set; }
    public DateTime updated { get; set; }
    public string citation { get; set; }
    public string service { get; set; }
    public string iaru_coordination { get; set; }
    public string iaru_coordination_url { get; set; }
    public ItuNotification itu_notification { get; set; }
    public bool frequency_violation { get; set; }
    public bool unconfirmed { get; set; }

    // canonical downlink mode name mapped from mode_id at import (see ModeMnemonic.ToModeName)
    public string DownlinkMode { get; set; }

    // Enrichment from the gr-satellites satyaml DB, matched by NORAD + nearest baud.
    public GrSatsInfo? gr_sats { get; set; }


    internal SatnogsDbSatellite Satellite;
    internal long DownlinkLow => (long)downlink_low!;


    public string GetTooltipText()
    {
      string mode = this.mode;
      if (baud != null && baud != 0) mode = $"{mode} {baud} Bd";
      string uplink = FormatFrequencyRange(uplink_low, uplink_high);
      string downlink = FormatFrequencyRange(downlink_low, downlink_high, invert);

      string result = $"{description}\nType: {type}\n";
      if (uplink_low != null) result += $"Uplink: {uplink}\n";
      result += $"Downlink: {downlink}\nMode: {mode}\n";
      if (uplink_low != null) result += $"Inverted: {invert}\n";
      result += $"Service: {service}\nUpdated: {updated:yyyy-MM-dd}";

      // gr-satellites satyaml enrichment (only the fields that are present)
      if (gr_sats != null)
      {
        if (gr_sats.modulation != null) result += $"\nModulation: {gr_sats.modulation}";
        if (gr_sats.baudrate != null) result += $"\nBaudrate: {gr_sats.baudrate} Bd";
        if (gr_sats.deviation != null) result += $"\nDeviation: {gr_sats.deviation} Hz";
        if (gr_sats.framing != null) result += $"\nFraming: {gr_sats.framing}";
        if (gr_sats.telemetry != null) result += $"\nTelemetry: {gr_sats.telemetry}";
      }

      return result;
    }

    public static string FormatFrequencyRange(long? low, long? high, bool inverted = false)
    {
      if (low == null) return "";
      if (high == null) return $"{low / 1000d:N1}";
      if (inverted) return $"{high / 1000d:N1} - {low / 1000d:N1}";
      return $"{low / 1000d:N1} - {high / 1000d:N1}";
    }

    public bool IsVhf(long? freq = null)
    {
      freq ??= DownlinkLow;
      return IsVhfFrequency((double)freq);
    }

    public bool IsUhf(long? freq = null)
    {
      freq ??= DownlinkLow;
      return IsUhfFrequency((double)freq);
    }

    public static bool IsVhfFrequency(double freq)
    {
      return freq >= 144000000 && freq <= 148000000;
    }

    public static bool IsUhfFrequency(double freq)
    {
      return freq >= 430000000 && freq <= 440000000;
    }

    public static bool IsHamFrequency(double freq)
    {
      return IsVhfFrequency(freq) || IsUhfFrequency(freq);
    }

    internal bool HasUplink()
    {
      return uplink_low != null;
    }
  }


  public class ItuNotification { public List<string> urls { get; set; } = new(); }

  // Fields sourced from the gr-satellites satyaml DB that the SatNOGS DB does not carry.
  // 'deviation' is the key one (needed for FSK/GFSK demod); 'telemetry' is a decoder-name
  // label only (decode logic lives in gr-satellites' Python construct modules, not here).
  public class GrSatsInfo
  {
    public string? modulation { get; set; }
    public double? baudrate { get; set; }
    public double? deviation { get; set; }
    public string? framing { get; set; }
    public string? telemetry { get; set; }   // real decoder name; null for ax25/csp/none
  }

  public class SatnogsDbTransmitterList : List<SatnogsDbTransmitter> { }


  // maps a SatNOGS mode to a canonical mode name (mode_id -> name) and a mode name to a Slicer.Mode
  public static class ModeMnemonic
  {
    // SatNOGS mode_id -> mode name, the official mapping from the SatNOGS modes API
    private static readonly Dictionary<int, string> ModeIdToName = new()
    {
      [49] = "AFSK", [61] = "AFSK S-Net", [62] = "AFSK SALSAT", [17] = "AHRPT", [19] = "AM",
      [44] = "APT", [50] = "BPSK", [71] = "BPSK PMT-A3", [59] = "CERTO", [6] = "CW",
      [72] = "DQPSK", [57] = "DSTAR", [58] = "DUV", [64] = "FFSK", [1] = "FM", [7] = "FMN",
      [65] = "FSK", [70] = "FSK AX.100 Mode 5", [73] = "FSK AX.100 Mode 6",
      [74] = "FSK AX.25 G3RUH", [85] = "GENESIS FSK", [66] = "GFSK", [84] = "GFSK Pkst",
      [75] = "GFSK Rktr", [82] = "GFSK/BPSK", [67] = "GMSK", [45] = "HRPT", [83] = "LoRa",
      [53] = "LRPT", [20] = "LSB", [76] = "MFSK", [63] = "MSK", [77] = "MSK AX.100 Mode 5",
      [78] = "MSK AX.100 Mode 6", [79] = "OFDM", [80] = "OQPSK", [68] = "PSK", [40] = "PSK31",
      [41] = "PSK63", [69] = "QPSK", [42] = "QPSK31", [43] = "QPSK63", [5] = "SSTV", [9] = "USB",
      [81] = "WSJT"
    };

    // SatNOGS mode_id -> Slicer.Mode. ids and names from the SatNOGS modes API
    private static readonly Dictionary<int, Slicer.Mode> ModeIdMap = new()
    {
      // CW
      [6] = Slicer.Mode.CW,
      // single-sideband voice
      [9] = Slicer.Mode.USB,
      [20] = Slicer.Mode.LSB,
      // FM / analog voice and image
      [1] = Slicer.Mode.FM, [7] = Slicer.Mode.FM, [19] = Slicer.Mode.FM,
      [5] = Slicer.Mode.FM, [44] = Slicer.Mode.FM, [57] = Slicer.Mode.FM, [58] = Slicer.Mode.FM,
      // narrowband SSB-data (USB_D, flipped to LSB_D when inverted)
      [49] = Slicer.Mode.USB_D, [61] = Slicer.Mode.USB_D, [62] = Slicer.Mode.USB_D,
      [40] = Slicer.Mode.USB_D, [81] = Slicer.Mode.USB_D,
      // all other digital -> FM_D
      [17] = Slicer.Mode.FM_D, [45] = Slicer.Mode.FM_D, [53] = Slicer.Mode.FM_D,
      [50] = Slicer.Mode.FM_D, [71] = Slicer.Mode.FM_D, [82] = Slicer.Mode.FM_D,
      [68] = Slicer.Mode.FM_D, [41] = Slicer.Mode.FM_D, [69] = Slicer.Mode.FM_D,
      [42] = Slicer.Mode.FM_D, [43] = Slicer.Mode.FM_D, [72] = Slicer.Mode.FM_D,
      [80] = Slicer.Mode.FM_D, [64] = Slicer.Mode.FM_D, [65] = Slicer.Mode.FM_D,
      [70] = Slicer.Mode.FM_D, [73] = Slicer.Mode.FM_D, [74] = Slicer.Mode.FM_D,
      [85] = Slicer.Mode.FM_D, [66] = Slicer.Mode.FM_D, [84] = Slicer.Mode.FM_D,
      [75] = Slicer.Mode.FM_D, [67] = Slicer.Mode.FM_D, [63] = Slicer.Mode.FM_D,
      [77] = Slicer.Mode.FM_D, [78] = Slicer.Mode.FM_D, [76] = Slicer.Mode.FM_D,
      [79] = Slicer.Mode.FM_D, [83] = Slicer.Mode.FM_D, [59] = Slicer.Mode.FM_D
    };

    // canonical mode name from mode_id; falls back to the raw mode string when the id is unknown
    public static string ToModeName(int? modeId, string? fallbackName)
    {
      if (modeId != null && ModeIdToName.TryGetValue(modeId.Value, out string? name)) return name;
      return fallbackName;
    }

    public static Slicer.Mode ToSlicerMode(int? modeId, string? modeName, bool invert)
    {
      Slicer.Mode mode;
      if (modeId == null || !ModeIdMap.TryGetValue(modeId.Value, out mode))
        mode = SlicerModeFromName(modeName);

      // an inverted SSB-data transmitter is received on the opposite sideband
      if (invert && mode == Slicer.Mode.USB_D) mode = Slicer.Mode.LSB_D;

      return mode;
    }

    // fallback used when mode_id is null or unmapped (e.g. uplink_mode, which has no id)
    private static Slicer.Mode SlicerModeFromName(string? name)
    {
      if (string.IsNullOrWhiteSpace(name)) return Slicer.Mode.FM_D;
      string n = name.Trim().ToUpperInvariant();

      if (n == "CW") return Slicer.Mode.CW;
      if (n == "USB" || n == "SSB") return Slicer.Mode.USB;
      if (n == "LSB") return Slicer.Mode.LSB;
      if (n is "FM" or "FMN" or "NFM" or "DSB" or "DSTAR" or "SSTV" or "APT" or "DUV")
        return Slicer.Mode.FM;
      if (n.StartsWith("AFSK") || n == "PSK31" || n == "WSJT" || n == "FT8")
        return Slicer.Mode.USB_D;

      return Slicer.Mode.FM_D;
    }
  }
}
