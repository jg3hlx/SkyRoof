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

    public Slicer.Mode DownlinkMode => (Slicer.Mode)DownlinkModeCombobox.SelectedItem!;
    public Slicer.Mode UplinkMode => (Slicer.Mode)UplinkModeCombobox.SelectedItem!;


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




    //----------------------------------------------------------------------------------------------
    //                                        set
    //----------------------------------------------------------------------------------------------
    public void SetTransmitter()
    {
      SetTerrestrial(false);
      var tx = ctx.SatelliteSelector.SelectedTransmitter;

      Frequency = tx.downlink_low;

      if (IsTransponder())
        Frequency += ctx.Settings.Satellites.
          TransmitterCustomizations.GetOrCreate(tx.uuid).TranspnderOffset;

      SetMode();
      UpdateAllControls();
      UpdateFrequency();
      SetSlicerFrequency();
      ctx.WaterfallPanel?.BringInView(CorrectedDownlinkFrequency!.Value);
    }

    internal void SetTerrestrialFrequency(double frequency)
    {
      SetTerrestrial(true);
      CorrectedDownlinkFrequency = Frequency = frequency;
      SetSlicerFrequency();
      UpdateAllControls();
    }

    internal void SetTransponderOffset(SatnogsDbTransmitter transponder, double offset)
    {
      SetTerrestrial(false);

      var cust = ctx.Settings.Satellites.TransmitterCustomizations.GetOrCreate(transponder.uuid);
      cust.TranspnderOffset = offset;

      if (transponder != ctx.SatelliteSelector.SelectedTransmitter)
        ctx.SatelliteSelector.SetSelectedTransmitter(transponder);

      Frequency = transponder.downlink_low + offset;

      UpdateAllControls();
    }

    internal void IncrementFrequency(int delta)
    {
      var tx = ctx.SatelliteSelector.SelectedTransmitter;

      // terrestrial
      if (IsTerrestrial)
        SetTerrestrialFrequency(Frequency!.Value + delta);

      // transponder
      else if (IsTransponder())
      {
        var transponder = ctx.SatelliteSelector.SelectedTransmitter;
        var cust = ctx.Settings.Satellites.TransmitterCustomizations.GetOrCreate(transponder.uuid);
        cust.TranspnderOffset += delta;
        Frequency = transponder.downlink_low + cust.TranspnderOffset;
        UpdateAllControls();
      }

      // transmitter
      else
      {
        var value = DownlinkManualSpinner.Value + (decimal)(delta / 1000d);
        value = Math.Max(DownlinkManualSpinner.Minimum, Math.Min(DownlinkManualSpinner.Maximum, value));
        DownlinkManualSpinner.Value = value;
        UpdateAllControls();
      }
    }

    private bool IsTransponder()
    {
      var tx = ctx.SatelliteSelector.SelectedTransmitter;
      return tx.downlink_high.HasValue && tx.downlink_high != tx.downlink_low;
    }

    internal double GetDraggableFrequency()
    {
      if (IsTerrestrial) return Frequency!.Value;

      else if (IsTransponder())
      {
        var transponder = ctx.SatelliteSelector.SelectedTransmitter;
        var cust = ctx.Settings.Satellites.TransmitterCustomizations.GetOrCreate(transponder.uuid);
        return cust.TranspnderOffset;
      }
      else return (double)DownlinkManualSpinner.Value * 1000d;
    }

    internal void SetDraggableFrequency(double freq)
    {
      if (IsTerrestrial) Frequency = freq;
      else if (IsTransponder())
      {
        var transponder = ctx.SatelliteSelector.SelectedTransmitter;
        var cust = ctx.Settings.Satellites.TransmitterCustomizations.GetOrCreate(transponder.uuid);
        cust.TranspnderOffset = freq;
        Frequency = transponder.downlink_low + cust.TranspnderOffset;
      }
      else 
        DownlinkManualSpinner.Value = (decimal)(freq / 1000);

      UpdateAllControls();
    }





    //----------------------------------------------------------------------------------------------
    //                                   4-Hz timer tick
    //----------------------------------------------------------------------------------------------
    internal void UpdateFrequency()
    {
      if (IsTerrestrial)
      // todo: call this once
      {
        DownlinkDopplerLabel.Text = "0.000";

        Changing = true;
        DownlinkManualSpinner.Value = 0;
        Changing = false;

        CorrectedDownlinkFrequency = Frequency;

        SetFieldColors(null);
      }
      else
      {
        Observation = ctx.AllPasses.ObserveSatellite(ctx.SatelliteSelector.SelectedSatellite, DateTime.UtcNow);

        // downlink
        double doppler = (double)Frequency! * -Observation.RangeRate / 3e5;
        string sign = doppler > 0 ? "+" : "";
        DownlinkDopplerLabel.Text = $"{sign}{doppler:n0}";

        CorrectedDownlinkFrequency = Frequency;
        if (DownlinkDopplerCheckbox.Checked) CorrectedDownlinkFrequency += doppler;
        if (DownlinkManualCheckbox.Checked) CorrectedDownlinkFrequency += (double)DownlinkManualSpinner.Value * 1000;

        // uplink
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
          UplinkDopplerLabel.Text = "0.000";
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




    //----------------------------------------------------------------------------------------------
    //                                      UI events
    //----------------------------------------------------------------------------------------------
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




    //----------------------------------------------------------------------------------------------
    //                                      internal set
    //----------------------------------------------------------------------------------------------
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

    // todo: include transponder offset
    private bool BringToPassband(double frequency)
    {
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




    //----------------------------------------------------------------------------------------------
    //                                      settings to UI
    //----------------------------------------------------------------------------------------------
    private void UpdateAllControls()
    {
      DownlinkFrequencyLabel.Text = Frequency.HasValue ? $"{Frequency:n0}" : "000,000,000";

      if (IsTerrestrial)
      {
        SatelliteLabel.Text = "Terrestrial";
        SatelliteLabel.ForeColor = Color.Red;
        DownlinkFrequencyLabel.ForeColor = Color.Gray;

        UplinkLabel.Text = "No Uplink";
        UplinkFrequencyLabel.ForeColor = SystemColors.ControlText;
      }
      else
      {
        SatelliteLabel.Text = "Downlink";
        SatelliteLabel.ForeColor = Color.Black;

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

    private void SetTerrestrial(bool value)
    {
      IsTerrestrial = value;

      if (IsTerrestrial)
      {
        DownlinkDopplerCheckbox.Visible = false;
        DownlinkDopplerLabel.BackColor = SystemColors.Control;

        DownlinkManualCheckbox.Visible = false;
        DownlinkManualSpinner.BackColor = SystemColors.Control;
      }
      else
      {
        DownlinkDopplerCheckbox.Visible = true;
        DownlinkDopplerLabel.BackColor = SystemColors.Window;

        DownlinkManualCheckbox.Visible = true;
        DownlinkManualSpinner.BackColor = SystemColors.Window;
      }
    }
  }
}
