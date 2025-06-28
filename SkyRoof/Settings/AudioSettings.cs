using System.ComponentModel;
using VE3NEA;

namespace SkyRoof
{
  public enum DataStreamType {
    [Description("I/Q to VAC")] 
    IqToVac,
    [Description("Audio to VAC")] 
    AudioToVac,
    [Description("I/Q to UDP")]
    IqToUdp,
    [Description("Audio to UDP")]
    AudioToUdp,
  }

  public class AudioSettings
  {
    // non-browsable
    public int SoundcardVolume = -25;
    public bool SpeakerEnabled = true;
    public bool StreamEnabled;

    [DisplayName("Speaker Audio Device")]
    [Description("Soundcard for audio output")]
    [TypeConverter(typeof(OutputSoundcardNameConverter))]
    public string? SpeakerSoundcard { get; set; } = Soundcard<float>.GetDefaultSoundcardId();

    [DisplayName("Stream Data Format")]
    [DefaultValue(DataStreamType.IqToVac)]
    [TypeConverter(typeof(EnumDescriptionConverter))]
    public DataStreamType DataStreamType { get; set; } = DataStreamType.IqToVac;

    [DisplayName("Stream Gain")]
    [Description(" Amplify stream output data by Gain, dB")]
    [DefaultValue(0)]
    public int StreamGain { get; set; } = 0;

    [DisplayName("VAC Device")]
    [Description("Virtual Audio Cable to feed audio to other software")]
    [TypeConverter(typeof(OutputSoundcardNameConverter))]
    public string? Vac { get; set; } = Soundcard<float>.GetFirstVacId();

    [DisplayName("UDP Port")]
    [DefaultValue((ushort)7355)]
    public ushort UdpPort { get; set; } = 7355;


    public override string ToString() { return string.Empty; }
  }
}
