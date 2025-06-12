using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyRoof
{
  public class AmsatSettings
  {
    [DefaultValue(false)] 
    public bool Enabled { get; set; } = false;

    public override string ToString() { return ""; }
  }
}
