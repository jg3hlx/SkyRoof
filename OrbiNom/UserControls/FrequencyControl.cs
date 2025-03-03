using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
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
        SatelliteLabel.Text = $"{Satellite.name}  {Transmitter.description}";

        Observation = ctx.AllPasses.ObserveSatellite(Satellite, DateTime.UtcNow);


        var txFreq = Transmitter.invert ? Transmitter.uplink_high : Transmitter.uplink_low;
        UplinkLabel.Text = txFreq.HasValue ? "Uplink" : "No Uplink";
        UplinkFrequencyLabel.Text = txFreq.HasValue ? $"{txFreq:n0}" : "000,000,000";

        SetFieldColors(txFreq);
      }
    }

    private void SetFieldColors(long? upFreq)
    {
      if (Transmitter == null) return;

      bool isAboveHorizon = Observation.Elevation.Degrees > 0;

      if (Transmitter.IsUhf())
        DownlinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.Cyan : Color.Teal;
      else if (Transmitter.IsVhf())
        DownlinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.Yellow : Color.Olive;
      else DownlinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.White : Color.Gray;

      if (!upFreq.HasValue)
        UplinkFrequencyLabel.ForeColor = Color.Gray;
      else if (Transmitter.IsUhf(upFreq))
        UplinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.Cyan : Color.Teal;
      else if (Transmitter.IsVhf(upFreq))
        UplinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.Yellow : Color.Olive;
      else UplinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.White : Color.Gray;
    }

    private void GetPass()
    {
      //var now = DateTime.UtcNow;
      //if (Pass == null || Pass.EndTime < now)
      //  Pass = ctx.AllPasses
      //    .ComputePassesFor(Satellite, now, now.AddDays(1))
      //    .OrderBy(pass => pass.StartTime)
      //    .FirstOrDefault();
    }

    // 4-Hz timer tick
    internal void UpdateDoppler()
    {
      if (Satellite != null)
      {
        Observation = ctx.AllPasses.ObserveSatellite(Satellite, DateTime.UtcNow);

        double freq = (double)Frequency! * -Observation.RangeRate / 3e5;
        string sign = freq > 0 ? "+" : "";
        DownlinkDopplerLabel.Text = $"{sign}{freq:n0}";

        var txFreq = Transmitter.invert ? Transmitter.uplink_high : Transmitter.uplink_low;
        if (txFreq.HasValue)
        {
          freq = (double)txFreq * Observation.RangeRate / 3e5;
          sign = freq > 0 ? "+" : "";
          UplinkDopplerLabel.Text = $"{sign}{freq:n0}";
          SetFieldColors(txFreq);
        }
        else
        {
          UplinkDopplerLabel.Text = "0,000";
          SetFieldColors(txFreq);
        }
      }
      else
      {
        DownlinkDopplerLabel.Text = "0,000";
        SetFieldColors(null);
      }
    }
  }
}
