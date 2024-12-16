namespace OrbiNom
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
  }

  public class ItuNotification
  {
    public List<string> urls { get; set; } = new();
  }

  public class SatnogsDbTransmitterList : List<SatnogsDbTransmitter> { }
}
