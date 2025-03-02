using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SGPdotNET.Observation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OrbiNom.UserControls
{
  public partial class FrequencyControl : UserControl
  {
    public Context ctx;
    public SatnogsDbTransmitter? Transmitter;
    public SatnogsDbSatellite? Satellite;
    private double? Frequency;
    private SatellitePass? Pass;
    TopocentricObservation Observation;

    public FrequencyControl()
    {
      InitializeComponent();
    }

    public void SetTransmitter(SatnogsDbTransmitter? transmitter, SatnogsDbSatellite satellite)
    {
      Transmitter = transmitter;
      Satellite = satellite;
      Frequency = Transmitter.downlink_low;
      UpdateAllControls();
    }

    internal void SetFrequency(double frequency)
    {
      Satellite = null;
      Frequency = frequency;
      UpdateAllControls();
    }

    private void UpdateAllControls()
    {
      DownlinkFrequencyLabel.Text = Frequency.HasValue ? $"{Frequency:n0}" : "000,000,000";

      if (Satellite == null)
      {
        SatelliteLabel.Text = "No Satellite";
        DownlinkFrequencyLabel.ForeColor = Color.Gray;

        UplinkLabel.Text = "No Uplink";
        UplinkFrequencyLabel.ForeColor = Color.Gray;
      }
      else
      {
        Observation = ctx.AllPasses.ObserveSatellite(Satellite, DateTime.UtcNow);
        bool isAboveHorizon = Observation.Elevation.Degrees > 0;

        SatelliteLabel.Text = $"{Satellite.name}  {Transmitter.description}";
        UplinkLabel.Text = "Uplink";

        if (Transmitter.IsUhf())
          DownlinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.Cyan : Color.Teal;
        else if (Transmitter.IsVhf())
          DownlinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.Yellow : Color.Olive;
        else DownlinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.White : Color.Gray;


        var txFreq = Transmitter.invert ? Transmitter.uplink_high : Transmitter.uplink_low;
        UplinkLabel.Text = txFreq.HasValue ? "Uplink" : "No Uplink";
        UplinkFrequencyLabel.Text = txFreq.HasValue ? $"{txFreq:n0}" : "000,000,000";

        if (!txFreq.HasValue)
          UplinkFrequencyLabel.ForeColor = Color.Gray;
        else if (Transmitter.IsUhf(txFreq))
          UplinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.Cyan : Color.Teal;
        else if (Transmitter.IsVhf(txFreq))
          UplinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.Yellow : Color.Olive;
        else UplinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.White : Color.Gray;
      }
    }

    private void GetPass()
    {
      var now = DateTime.UtcNow;
      if (Pass == null || Pass.EndTime < now)
        Pass = ctx.AllPasses
          .ComputePassesFor(Satellite, now, now.AddDays(1))
          .OrderBy(pass => pass.StartTime)
          .FirstOrDefault();
    }
  }
}
