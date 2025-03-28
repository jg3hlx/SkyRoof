using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VE3NEA;

namespace OrbiNom
{
  public class AnnouncerSettings
  {
    [DisplayName("Voice")]
    [Description("The voice to use for announcments")]
    [TypeConverter(typeof(VoiceNameConverter))]
    public string? Voice { get; set; }

    public int Volume { get; set; } = 50;

    [DisplayName("Announcement #1")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public Announcement Announcement1 { get; set; } = new(2, "Satellite {name} is rising in {minutes} minutes");

    [DisplayName("Announcement #2")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public Announcement Announcement2 { get; set; } = new(0, "Satellite {name} is rising now");

    public override string ToString() { return ""; }
  }

  public class Announcement
  {
    public bool Enabled { get; set; } = true;

    [DisplayName("Minutes before AOS")]
    public int Minutes { get; set; }

    public string Message { get; set; }
    
    
    public override string ToString() { return ""; }

    public Announcement(int minutes, string message)
    {
      Minutes = minutes;
      Message = message;
    }
  }
}
