namespace SkyRoof
{
  public class Capabilities
  {
    public string[] read_main_frequency { get; set; } = [];
    public string[] read_split_frequency { get; set; } = [];
    public string[] set_main_frequency { get; set; } = [];
    public string[] set_split_frequency { get; set; } = [];
    public string[] set_main_mode { get; set; } = [];
    public string[] set_split_mode { get; set; } = [];
  }

  public class Commands
  {
    public string[]? setup_duplex { get; set; }
    public string[]? setup_split { get; set; }
    public string[]? setup_simplex { get; set; }
    public string? read_main_frequency { get; set; }
    public string? read_split_frequency { get; set; }
    public string? set_main_frequency { get; set; }
    public string? set_split_frequency { get; set; }
    public string? set_main_mode { get; set; }
    public string? set_split_mode { get; set; }
    public string? read_ptt { get; set; }
    public string? set_ptt_on { get; set; }
    public string? set_ptt_off { get; set; }
  }
  
  public class RadioInfo
  {
    public string radio { get; set; } = "";
    public Capabilities capabilities { get; set; } = new();
    public Commands commands { get; set; } = new();
  }

  public class RadioInfoList : List<RadioInfo>;
}
