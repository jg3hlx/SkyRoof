using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VE3NEA;

namespace OrbiNom
{
  public enum EngineType { 
    OmniRig, 
    rigctld 
  }

  public class CatSettings
  {
    [Browsable(false)]
    public bool Enabled { get; set; }

    [Browsable(false)]
    public bool ShowCorrectedFrequency { get; set; }

    [DefaultValue(EngineType.OmniRig)]
    public EngineType EngineType { get; set; }

    [DisplayName("OmniRig settings")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public OmniRigSettings OmniRig { get; set; } = new();

    [DisplayName("rigctld settings")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public RigctldSettings Rigctld { get; set; } = new();



    public override string ToString() { return string.Empty; }
  }

  public class OmniRigSettings
  {
    [DisplayName("OmniRig Radio")]
    [Description("Radio selection in OmniRig")]
    [DefaultValue(OmniRigRig.Rig1)]
    public OmniRigRig RigNo { get; set; } = OmniRigRig.Rig1;

    public override string ToString() { return string.Empty; }
  }

  public class RigctldSettings
  {
    [DefaultValue("127.0.0.1")]
    public string Host { get; set; } = "127.0.0.1";

    [DisplayName("TCP Port")]
    [DefaultValue((ushort)4532)]
    public ushort Port { get; set; } = 4532;

    public override string ToString() { return string.Empty; }
  }
}
