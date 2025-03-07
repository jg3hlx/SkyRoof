using System.ComponentModel;
using VE3NEA;

namespace OrbiNom
{
  public class AudioSettings
  {
    // non-browsable
    public int SoundcardVolume = -25;
    public bool SpeakerEnabled = true;
    public bool VacEnabled;

    [DisplayName("Speaker Audio Device")]
    [Description("Soundcard for audio output")]
    [TypeConverter(typeof(OutputSoundcardNameConverter))]
    public string? SpeakerSoundcard { get; set; } = Soundcard.GetDefaultSoundcardId();

    [DisplayName("VAC Device")]
    [Description("Virtual Audio Cable to feed audio to other software")]
    [TypeConverter(typeof(OutputSoundcardNameConverter))]
    public string? Vac { get; set; } = Soundcard.GetFirstVacId();

    [DisplayName("VAC Gain")]
    [Description("Virtual Audio Cable Gain, dB")]
    [DefaultValue(-40)]
    public int VacVolume { get; set; } = -40;


    public override string ToString() { return string.Empty; }
  }
}
