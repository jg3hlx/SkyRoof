using System.ComponentModel;

namespace Sixer
{
  public class UserSettings
  {
    [DisplayName("Callsign")]
    [Description("Your callsign")]
    public string Call { get; set; }

    [DisplayName("Grid Square")]
    [Description("Your grid square")]
    public string Square { get; set; } = "JJ00jj";

    public override string ToString() { return ""; }
  }
}