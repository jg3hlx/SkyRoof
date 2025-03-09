using SGPdotNET.Observation;
using VE3NEA;

namespace OrbiNom
{
  public partial class FrequencyControl : UserControl
  {
    public Context ctx;
    public SatnogsDbTransmitter? Transmitter;
    public SatnogsDbSatellite? Satellite;
    public double? Frequency;
    private TopocentricObservation Observation;
    private bool Changing;
    public double? CorrectedDownlinkFrequency;

    public FrequencyControl()
    {
      InitializeComponent();
    }

    public void SetTransmitter()
    {
      SetTransmitter(
        ctx.SatelliteSelector.SelectedTransmitter,
        ctx.SatelliteSelector.SelectedSatellite);
    }

    public void SetTransmitter(SatnogsDbTransmitter transmitter, SatnogsDbSatellite satellite)
    {
      Transmitter = transmitter;
      Satellite = satellite;
      Frequency = Transmitter.downlink_low;
      UpdateAllControls();
      UpdateDoppler();
      SetReceivedFrequency();
      ctx.WaterfallPanel?.BringInView(CorrectedDownlinkFrequency!.Value);
    }

    internal void SetFrequency(double frequency)
    {
      Satellite = null;
      CorrectedDownlinkFrequency = Frequency = frequency;
      SetReceivedFrequency();
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
      //  Pass = ctx.AllPasses.GetNextPass(Satellite);
    }

    // 4-Hz timer tick
    internal void UpdateDoppler()
    {
      if (Satellite != null)
      {
        Observation = ctx.AllPasses.ObserveSatellite(Satellite, DateTime.UtcNow);

        double doppler = (double)Frequency! * -Observation.RangeRate / 3e5;
        string sign = doppler > 0 ? "+" : "";
        DownlinkDopplerLabel.Text = $"{sign}{doppler:n0}";

        CorrectedDownlinkFrequency = Frequency + doppler + (double)DownlinkManualSpinner.Value * 1000;

        var txFreq = Transmitter.invert ? Transmitter.uplink_high : Transmitter.uplink_low;
        if (txFreq.HasValue)
        {
          doppler = (double)txFreq * Observation.RangeRate / 3e5;
          sign = doppler > 0 ? "+" : "";
          UplinkDopplerLabel.Text = $"{sign}{doppler:n0}";
          SetFieldColors(txFreq);
        }
        else
        {
          UplinkDopplerLabel.Text = "N/A";
          SetFieldColors(txFreq);
        }

        var cust = ctx.Settings.Satellites.SatelliteCustomizations.GetOrCreate(Satellite.sat_id);
        Changing = true;
        DownlinkDopplerCheckbox.Checked = cust.DownlinkDopplerCorrectionEnabled;
        DownlinkManualCheckbox.Checked = cust.DownlinkManualCorrectionEnabled;
        DownlinkManualSpinner.Value = (decimal)(cust.DownlinkManualCorrection / 1000f);
        Changing = false;

        SetReceivedFrequency();
      }
      else
      // todo: call this once
      {
        DownlinkDopplerLabel.Text = "N/A";

        Changing = true;
        DownlinkManualSpinner.Value = 0;
        Changing = false;

        CorrectedDownlinkFrequency = Frequency;

        SetFieldColors(null);
      }
    }

    private void SetReceivedFrequency()
    {
      if (ctx.Sdr?.Enabled != true) return;

      double bandwidth = ctx.Sdr.Info.MaxBandwidth;
      double low = ctx.Sdr.Frequency - bandwidth / 2;
      double high = ctx.Sdr.Frequency + bandwidth / 2;

      if (CorrectedDownlinkFrequency < low || CorrectedDownlinkFrequency > high)
        if (ctx.Sdr.IsFrequencySupported(CorrectedDownlinkFrequency!.Value))
        {
          BringToPassband(CorrectedDownlinkFrequency!.Value);
          ctx.WaterfallPanel?.SetCenterFrequency(ctx.Sdr.Info.Frequency);
        }
        else      
          return;

      if (CorrectedDownlinkFrequency >= low && CorrectedDownlinkFrequency <= high)
        if (ctx.Slicer?.Enabled == true) 
          ctx.Slicer.SetOffset(CorrectedDownlinkFrequency!.Value - ctx.Sdr.Frequency);
    }


    private bool BringToPassband(double frequency)
    {
      // todo:
      // is frequency in UHF or VHF segment?
      // if so, adjust frequency so that sdr segment is fully within vhf/uhf segment
      // does sdr support frequency?
      // set sdr center frequency

      double bandWing = SdrConst.MAX_BANDWIDTH / 2;
      double sdrWing = ctx.Sdr!.Info.MaxBandwidth / 2;

      bool uhf = frequency >= SdrConst.UHF_CENTER_FREQUENCY - bandWing && 
        frequency <= SdrConst.UHF_CENTER_FREQUENCY + bandWing;

      if (uhf)
      {
        double minCenter = SdrConst.UHF_CENTER_FREQUENCY - bandWing + sdrWing;
        double maxCenter = SdrConst.UHF_CENTER_FREQUENCY + bandWing - sdrWing;
        frequency = Math.Max(minCenter, Math.Min(maxCenter, frequency));
      }

      bool vhf = frequency >= SdrConst.VHF_CENTER_FREQUENCY - bandWing &&
        frequency <= SdrConst.VHF_CENTER_FREQUENCY + bandWing;

      if (vhf)
      {
        double minCenter = SdrConst.VHF_CENTER_FREQUENCY - bandWing + sdrWing;
        double maxCenter = SdrConst.VHF_CENTER_FREQUENCY + bandWing - sdrWing;
        frequency = Math.Max(minCenter, Math.Min(maxCenter, frequency));
      }

      if (ctx.Sdr.IsFrequencySupported(frequency))
      {
        ctx.Sdr.Frequency = frequency;
        return true;
      }
      else return false;
    }

    private void DownlinkManualSpinner_ValueChanged(object sender, EventArgs e)
    {
      if (Changing || Satellite == null) return;
      var cust = ctx.Settings.Satellites.SatelliteCustomizations.GetOrCreate(Satellite.sat_id);
      cust.DownlinkManualCorrection = (int)(DownlinkManualSpinner.Value * 1000);
    }

    private void DownlinkManualCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      if (Changing || Satellite == null) return;
      var cust = ctx.Settings.Satellites.SatelliteCustomizations.GetOrCreate(Satellite.sat_id);
      cust.DownlinkManualCorrectionEnabled = DownlinkManualCheckbox.Checked;
    }

    private void DownlinkDopplerCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      if (Changing || Satellite == null) return;
      var cust = ctx.Settings.Satellites.SatelliteCustomizations.GetOrCreate(Satellite.sat_id);
      cust.DownlinkDopplerCorrectionEnabled = DownlinkDopplerCheckbox.Checked;
    }
  }
}
