using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VE3NEA;

namespace OrbiNom
{
  public class CatSettings
  {
    [DefaultValue(false)]
    public bool Enabled { get; set; }

    [TypeConverter(typeof(RadioModelConverter))]
    public string RadioModel { get; set; } = "IC-9700";

    [DefaultValue("127.0.0.1")]
    public string Host { get; set; } = "127.0.0.1";

    [DisplayName("TCP Port")]
    [DefaultValue((ushort)4532)]
    public ushort Port { get; set; } = 4532;

    [DefaultValue(false)]
    public bool ShowCorrectedFrequency { get; set; }

    [DefaultValue(true)]
    public bool IgnoreDialKnob { get; set; } = true;


    public override string ToString() { return string.Empty; }
  }
}
