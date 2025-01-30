using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbiNom
{
  public class Context
  {
    // data
    public Settings Settings = new();
    public SatnogsDb SatnogsDb;
    public SatellitePasses GroupPasses;
    public SatellitePasses AllPasses;

    // panels
    public MainForm MainForm;
    public SatelliteSelector SatelliteSelector;
    public GroupViewPanel? GroupViewPanel;
    public SatelliteDetailsPanel? SatelliteDetailsPanel;
    public PassesPanel? PassesPanel;
    public TimelinePanel? TimelinePanel;
    public SkyViewPanel? SkyViewPanel;
    public EarthViewPanel? EarthViewPanel;
  }
}
