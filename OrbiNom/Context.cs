using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbiNom
{
  public class Context
  {
    public Settings Settings = new();
    public SatnogsDb SatnogsDb;

    internal MainForm MainForm;
    internal SatelliteSelector SatelliteSelector;
    internal GroupViewPanel? GroupViewPanel;
    public SatelliteDetailsPanel? SatelliteDetailsPanel;
  }
}
