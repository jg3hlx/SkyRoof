using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SkyRoof
{
  public enum CatAction
  {
    setup,
    read_rx_frequency,
    read_tx_frequency,
    write_rx_frequency,
    write_tx_frequency,
    read_rx_mode,
    read_tx_mode,
    write_rx_mode,
    write_tx_mode,
    read_ptt,
    write_ptt_on,
    write_ptt_off
  }

  public class RadioCapabilitiesList
  {
    public int version { get; set; } = 0;
    public List<RadioCapabilities> radios { get; set; } = new();

    public static RadioCapabilitiesList Load(string path)
    {
      string json = File.ReadAllText(path);
      return JsonConvert.DeserializeObject<RadioCapabilitiesList>(json)!;
    }
  }

  public class RadioCapabilities
  {
    public string model { get; set; }
    public bool cross_band_split { get; set; }

    public AvailableCommands? simplex { get; set; }
    public AvailableCommands? split { get; set; }
    public AvailableCommands? duplex { get; set; }

    public bool CanSplitTune(bool crossband)
    {
      if (split == null) return false;
      if (crossband && !cross_band_split) return false;
      if (!split.when_receiving.Contains(CatAction.write_rx_frequency)) return false;
      if (!split.when_transmitting.Contains(CatAction.write_tx_frequency)) return false;

      return true;
    }
  }

  public class AvailableCommands
  {
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public CatAction[] when_receiving { get; set; } = Array.Empty<CatAction>();
    
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public CatAction[] when_transmitting { get; set; } = Array.Empty<CatAction>();
    
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public CatAction[] when_setting_up { get; set; } = Array.Empty<CatAction>();

    internal bool Can(CatAction action, bool ptt)
    {
      return ptt ? when_transmitting.Contains(action) : when_receiving.Contains(action);
    }

    internal bool CanSetup(CatAction action, bool ptt)
    {
      return when_setting_up.Contains(action);
    }
  }
}
