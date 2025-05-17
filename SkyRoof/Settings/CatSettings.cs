using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VE3NEA;

namespace SkyRoof
{
  public class ControlEngineSettings
  {
    [DefaultValue(100)]
    [Description("Delay between the command cycles, ms")]
    public int Delay { get; set; } = 100;

    [DisplayName("Log Traffic")]
    [Description("Log command traffic for debugging")]
    [DefaultValue(false)]
    public bool LogTraffic { get; set; }
  
    public override string ToString() { return string.Empty; }
  }

  public class CatSettings : ControlEngineSettings
  {
    [DefaultValue(false)]
    [DisplayName("Ignore Dial Knob")]
    [Description("Tune only from the software")]
    public bool IgnoreDialKnob { get; set; } = false;

    [DisplayName("RX CAT")]
    [Description("RX CAT Control via rigctld.exe")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public CatRadioSettings RxCat { get; set; } = new();

    [DisplayName("TX CAT")]
    [Description("TX CAT Control via rigctld.exe")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public CatRadioSettings TxCat { get; set; } = new();
  }

  public class CatRadioSettings
  {
    [DefaultValue("127.0.0.1")]
    [Description("rigctld host")]
    public string Host { get; set; } = "127.0.0.1";

    [DisplayName("TCP Port")]
    [Description("rigctld port")]
    [DefaultValue((ushort)4532)]
    public ushort Port { get; set; } = 4532;

    [DefaultValue(false)]
    public bool Enabled { get; set; }

    [TypeConverter(typeof(RadioModelConverter))]
    [DisplayName("Radio Model")]
    [Description("Defines the capabilities of the radio")]
    public string RadioModel { get; set; } = "IC-9700";

    [DisplayName("Show Corrected Frequency")]
    [Description("Show the frequency with all corrections (True) or the nominal frequency (False)")]
    [DefaultValue(false)]
    public bool ShowCorrectedFrequency { get; set; }

    public override string ToString() { return string.Empty; }
  }
}
