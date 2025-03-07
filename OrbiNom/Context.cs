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

    // satellite data
    public Settings Settings = new();
    public SatnogsDb SatnogsDb;
    public SatellitePasses GroupPasses;
    public SatellitePasses AllPasses;

    // main form
    public MainForm MainForm;
    public FrequencyControl FrequencyControl;
    public SatelliteSelector SatelliteSelector;

    // panels
    public GroupViewPanel? GroupViewPanel;
    public SatelliteDetailsPanel? SatelliteDetailsPanel;
    public PassesPanel? PassesPanel;
    public TimelinePanel? TimelinePanel;
    public SkyViewPanel? SkyViewPanel;
    public EarthViewPanel? EarthViewPanel;
    public TransmittersPanel? TransmittersPanel;
    public SoapySdrDevice? Sdr;
    public WaterfallPanel? WaterfallPanel;

    // soundcards
    public readonly Soundcard SpeakerSoundcard = new();
    public readonly Soundcard VacSoundcard = new();
    public Slicer? Slicer;
  }
}
