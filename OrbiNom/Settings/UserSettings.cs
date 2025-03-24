using System.ComponentModel;

namespace OrbiNom
{
  public class UserSettings
  {
    [DisplayName("Callsign")]
    [Description("Your callsign")]
    public string Call { get; set; }

    [DisplayName("Grid Square")]
    [Description("Your grid square")]
    public string Square { get; set; } = "JJ00jj";

    [DisplayName("Altitude")]
    [Description("Your altitude above the sea level, in meters")]
    public int Altitude { get; set; } = 0;

    public override string ToString() { return ""; }
  }
}