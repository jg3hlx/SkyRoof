using SGPdotNET.Observation;
using VE3NEA;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OrbiNom
{
  public partial class FrequencyControl : UserControl
  {
    public Context ctx;
    public double? Frequency;
    private TopocentricObservation Observation;
    private bool Changing;
    public double? CorrectedDownlinkFrequency;
    private bool IsTerrestrial = true;

    public FrequencyControl()
    {
      InitializeComponent();

      Changing = true;
      DownlinkModeCombobox.DataSource = Enum.GetValues(typeof(Slicer.Mode));
      UplinkModeCombobox.DataSource = Enum.GetValues(typeof(Slicer.Mode));
      DownlinkModeCombobox.SelectedIndex = 0;
      UplinkModeCombobox.SelectedIndex = 0;
      Changing = false;
    }

    public void SetTransmitter()
    {
      IsTerrestrial = false;
      Frequency = ctx.SatelliteSelector.SelectedTransmitter.downlink_low;
      SetMode();
      UpdateAllControls();
      UpdateDoppler();
      SetSlicerFrequency();
      ctx.WaterfallPanel?.BringInView(CorrectedDownlinkFrequency!.Value);
    }

    internal void SetFrequency(double frequency)
    {
      IsTerrestrial = true;
      CorrectedDownlinkFrequency = Frequency = frequency;
      SetSlicerFrequency();
      UpdateAllControls();
    }

    private void UpdateAllControls()
    {
      DownlinkFrequencyLabel.Text = Frequency.HasValue ? $"{Frequency:n0}" : "000,000,000";

      if (IsTerrestrial)
      {
        SatelliteLabel.Text = "Terrestrial";
        DownlinkFrequencyLabel.ForeColor = Color.Gray;

        UplinkLabel.Text = "No Uplink";
        UplinkFrequencyLabel.ForeColor = Color.Gray;
      }
      else
      {
        SatelliteLabel.Text = "Downlink";

        Observation = ctx.AllPasses.ObserveSatellite(ctx.SatelliteSelector.SelectedSatellite, DateTime.UtcNow);

        var tx = ctx.SatelliteSelector.SelectedTransmitter;
        var txFreq = tx.invert ? tx.uplink_high : tx.uplink_low;
        UplinkLabel.Text = txFreq.HasValue ? "Uplink" : "No Uplink";
        UplinkFrequencyLabel.Text = txFreq.HasValue ? $"{txFreq:n0}" : "000,000,000";

        SetFieldColors(txFreq);
      }
    }

    private void SetFieldColors(long? upFreq)
    {
      if (IsTerrestrial) return;

      bool isAboveHorizon = Observation.Elevation.Degrees > 0;

      var tx = ctx.SatelliteSelector.SelectedTransmitter;

      if (tx.IsUhf())
        DownlinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.Cyan : Color.Teal;
      else if (tx.IsVhf())
        DownlinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.Yellow : Color.Olive;
      else DownlinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.White : Color.Gray;

      if (!upFreq.HasValue)
        UplinkFrequencyLabel.ForeColor = Color.Gray;
      else if (tx.IsUhf(upFreq))
        UplinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.Cyan : Color.Teal;
      else if (tx.IsVhf(upFreq))
        UplinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.Yellow : Color.Olive;
      else UplinkFrequencyLabel.ForeColor = isAboveHorizon ? Color.White : Color.Gray;
    }

    // 4-Hz timer tick
    internal void UpdateDoppler()
    {
      if (IsTerrestrial)
      // todo: call this once
      {
        DownlinkDopplerLabel.Text = "N/A";

        Changing = true;
        DownlinkManualSpinner.Value = 0;
        Changing = false;

        CorrectedDownlinkFrequency = Frequency;

        SetFieldColors(null);
      }
      else
      {
        Observation = ctx.AllPasses.ObserveSatellite(ctx.SatelliteSelector.SelectedSatellite, DateTime.UtcNow);

        double doppler = (double)Frequency! * -Observation.RangeRate / 3e5;
        string sign = doppler > 0 ? "+" : "";
        DownlinkDopplerLabel.Text = $"{sign}{doppler:n0}";

        CorrectedDownlinkFrequency = Frequency + doppler + (double)DownlinkManualSpinner.Value * 1000;

        var tx = ctx.SatelliteSelector.SelectedTransmitter;
        var txFreq = tx.invert ? tx.uplink_high : tx.uplink_low;
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

        var cust = ctx.Settings.Satellites.SatelliteCustomizations.GetOrCreate(ctx.SatelliteSelector.SelectedSatellite.sat_id);
        Changing = true;
        DownlinkDopplerCheckbox.Checked = cust.DownlinkDopplerCorrectionEnabled;
        DownlinkManualCheckbox.Checked = cust.DownlinkManualCorrectionEnabled;
        DownlinkManualSpinner.Value = (decimal)(cust.DownlinkManualCorrection / 1000f);
        Changing = false;

        SetSlicerFrequency();
      }
    }

    private void SetSlicerFrequency()
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
      if (Changing || IsTerrestrial) return;
      var cust = ctx.Settings.Satellites.SatelliteCustomizations.GetOrCreate(ctx.SatelliteSelector.SelectedSatellite.sat_id);
      cust.DownlinkManualCorrection = (int)(DownlinkManualSpinner.Value * 1000);
    }

    private void DownlinkManualCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      if (Changing || IsTerrestrial) return;
      var cust = ctx.Settings.Satellites.SatelliteCustomizations.GetOrCreate(ctx.SatelliteSelector.SelectedSatellite.sat_id);
      cust.DownlinkManualCorrectionEnabled = DownlinkManualCheckbox.Checked;
    }

    private void DownlinkDopplerCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      if (Changing || IsTerrestrial) return;
      var cust = ctx.Settings.Satellites.SatelliteCustomizations.GetOrCreate(ctx.SatelliteSelector.SelectedSatellite.sat_id);
      cust.DownlinkDopplerCorrectionEnabled = DownlinkDopplerCheckbox.Checked;
    }

    private void ModeCombobox_SelectedValueChanged(object sender, EventArgs e)
    {
      if (Changing) return;

      // mode from comboboxes to settings
      if (!IsTerrestrial)
      {
        var tx = ctx.SatelliteSelector.SelectedTransmitter;
        var cust = ctx.Settings.Satellites.TransmitterCustomizations.GetOrCreate(tx.uuid);
        cust.DownlinkMode = (Slicer.Mode)DownlinkModeCombobox.SelectedItem!;
        cust.UplinkMode = (Slicer.Mode)UplinkModeCombobox.SelectedItem!;
      }

      SetMode();
    }

    private void SetMode()
    {
      if (!IsTerrestrial)
      {
        // read settings
        var tx = ctx.SatelliteSelector.SelectedTransmitter;
        var cust = ctx.Settings.Satellites.TransmitterCustomizations.GetOrCreate(tx.uuid);
        
        // set in comboboxes
        Changing = true;
        DownlinkModeCombobox.SelectedItem = cust.DownlinkMode;
        UplinkModeCombobox.SelectedItem = cust.UplinkMode;
        Changing = false;
      }

      // set in slicer
      if (ctx.Slicer != null)
        ctx.Slicer.CurrentMode = (Slicer.Mode)DownlinkModeCombobox.SelectedItem!;
    }
  }
}
