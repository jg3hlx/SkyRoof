using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrbiNom.UserControls;
using VE3NEA;

namespace OrbiNom
{
  public class Context
  {
    // resources
    internal readonly Font AwesomeFont8 = FontAwesomeFactory.Create(8);
    internal readonly Font AwesomeFont14 = FontAwesomeFactory.Create(14);
    internal readonly PaletteManager PaletteManager = new PaletteManager();

    // data
    public Settings Settings = new();
    public SatnogsDb SatnogsDb;
    public SatellitePasses GroupPasses;
    public SatellitePasses AllPasses;

    // panels
    public MainForm MainForm;
    public FrequencyControl DownlinkFrequencyControl;
    public SatelliteSelector SatelliteSelector;

    public GroupViewPanel? GroupViewPanel;
    public SatelliteDetailsPanel? SatelliteDetailsPanel;
    public PassesPanel? PassesPanel;
    public TimelinePanel? TimelinePanel;
    public SkyViewPanel? SkyViewPanel;
    public EarthViewPanel? EarthViewPanel;
    public TransmittersPanel? TransmittersPanel;
    public SoapySdrDevice? Sdr;
    public WaterfallPanel? WaterfallPanel;
  }
}
