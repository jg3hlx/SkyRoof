using SGPdotNET.Observation;
using VE3NEA;
using System.ComponentModel;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;

namespace OrbiNom
{
  // todo: click on the frequency display opens direct input box
  // todo: toggle freq/corrected-freq display

  public partial class FrequencyControl : UserControl
  {
    public Context ctx;
    private TopocentricObservation? Observation;
    private bool Changing;
    public double DownlinkFrequency, CorrectedDownlinkFrequency;
    public double UplinkFrequency, CorrectedUplinkFrequency;

    public Slicer.Mode DownlinkMode => (Slicer.Mode)DownlinkModeCombobox.SelectedItem!;
    public Slicer.Mode UplinkMode => (Slicer.Mode)UplinkModeCombobox.SelectedItem!;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal bool RitEnabled { get => DownlinkRitCheckbox.Checked; set => DownlinkRitCheckbox.Checked = value; }
    public double RitOffset { get => (double)DownlinkRitSpinner.Value * 1000; }


    // shortcuts
    private SatnogsDbSatellite Sat => ctx.SatelliteSelector.SelectedSatellite;
    private SatnogsDbTransmitter Tx => ctx.SatelliteSelector.SelectedTransmitter;
    private SatelliteCustomization SatCust => ctx.Settings.Satellites.SatelliteCustomizations.GetOrCreate(Sat.sat_id);
    private TransmitterCustomization TxCust => ctx.Settings.Satellites.TransmitterCustomizations.GetOrCreate(Tx.uuid);
    private bool IsTransponder => Tx.downlink_high.HasValue && Tx.downlink_high != Tx.downlink_low;
    private bool HasUplink => UplinkFrequency > 0;

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




    //----------------------------------------------------------------------------------------------
    //                                 set from outside
    //----------------------------------------------------------------------------------------------
    public void SetTransmitter()
    {
      IsTerrestrial = false;
      SetModes();
      SetFrequencies();
      SettingsToUi();
    }

    internal void SetTerrestrialFrequency(double frequency)
    {
      IsTerrestrial = true;
      DownlinkFrequency = frequency;
      SetFrequencies();
      SettingsToUi();
    }

    internal void SetTransponderOffset(SatnogsDbTransmitter transponder, double offset)
    {
      // set the offset first
      var transponderCust = ctx.Settings.Satellites.TransmitterCustomizations.GetOrCreate(transponder.uuid);
      Debug.Assert(offset >= 0 && offset <= transponder.uplink_high - transponder.uplink_low);
      transponderCust.TranspnderOffset = offset;

      // if same TX, just force its settings in case we were in terrestrial mode and changed them
      if (transponder == Tx) SetTransmitter();

      // different TX, select it for all panels in the app
      else ctx.SatelliteSelector.SetSelectedTransmitter(transponder);
    }

    internal void IncrementFrequency(int delta)
    {
      //Ctrl-mousewheel-spin enables RIT
      DownlinkRitCheckbox.Checked = ModifierKeys.HasFlag(Keys.Control);

      // RIT
      if (RitEnabled)
        DownlinkRitSpinner.Value += (decimal)(delta / 1000d);

      // terrestrial
      else if (IsTerrestrial)
        DownlinkFrequency += delta;

      // transponder
      else if (IsTransponder)
        TxCust.TranspnderOffset += delta;

      // transmitter
      else
        IncrementSpinnerValue(DownlinkManualSpinner, delta);

      SetFrequencies();
    }

    internal double GetDraggableFrequency()
    {
      if (IsTerrestrial) return DownlinkFrequency;
      else if (IsTransponder) return TxCust.TranspnderOffset;
      else return SatCust.DownlinkManualCorrection;
    }

    internal void SetDraggableFrequency(double freq)
    {
      if (IsTerrestrial) DownlinkFrequency = freq;
      else if (IsTransponder)
      {
        Debug.Assert(freq >= 0 && freq <= Tx.uplink_high - Tx.uplink_low);
        TxCust.TranspnderOffset = freq;
      }
      else SetSpinnerValue(DownlinkManualSpinner, (decimal)(freq / 1000));

      SetFrequencies();
    }

    internal void ClockTick()
    {
      if (!IsTerrestrial) SetFrequencies();
    }




    //----------------------------------------------------------------------------------------------
    //                                      internal set
    //----------------------------------------------------------------------------------------------
    private void SetModes()
    {
      if (!IsTerrestrial)
      {
        // set in comboboxes
        DownlinkModeCombobox.SelectedItem = TxCust.DownlinkMode;
        UplinkModeCombobox.SelectedItem = TxCust.UplinkMode;
      }

      // set in slicer
      if (ctx.Slicer != null) ctx.Slicer.CurrentMode = DownlinkMode;

      // set in external radio
      ctx.CatControl.Rx?.SetRxMode(TxCust.DownlinkMode);
      ctx.CatControl.Tx?.SetTxMode(TxCust.UplinkMode);
    }

    private void SetSlicerFrequency()
    {
      if (ctx.Sdr?.Enabled != true) return;

      double bandwidth = ctx.Sdr.Info.MaxBandwidth;
      double low = ctx.Sdr.Frequency - bandwidth / 2;
      double high = ctx.Sdr.Frequency + bandwidth / 2;

      double targetFrequency = (double)CorrectedDownlinkFrequency!;

      if (targetFrequency < low || targetFrequency > high)
        if (ctx.Sdr.IsFrequencySupported(targetFrequency))
        {
          BringToPassband(targetFrequency);
          ctx.WaterfallPanel?.SetCenterFrequency(ctx.Sdr.Info.Frequency);
        }
        else
          return;

      if (targetFrequency >= low && targetFrequency <= high)
        if (ctx.Slicer?.Enabled == true)
          ctx.Slicer.SetOffset(targetFrequency - ctx.Sdr.Frequency);
    }

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
    //                                      UI events
    //----------------------------------------------------------------------------------------------
    private void DownlinkManualSpinner_ValueChanged(object sender, EventArgs e)
    {
      if (!Changing && !IsTerrestrial)
      SatCust.DownlinkManualCorrection = (int)(DownlinkManualSpinner.Value * 1000);
    }

    private void DownlinkManualCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      if (!Changing && !IsTerrestrial)
      SatCust.DownlinkManualCorrectionEnabled = DownlinkManualCheckbox.Checked;
    }

    private void DownlinkDopplerCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      if (!Changing && !IsTerrestrial)
      SatCust.DownlinkDopplerCorrectionEnabled = DownlinkDopplerCheckbox.Checked;
    }

    private void UplinkManualSpinner_ValueChanged(object sender, EventArgs e)
    {
      if (!Changing && !IsTerrestrial && HasUplink)
      SatCust.UplinkManualCorrection = (int)(UplinkManualSpinner.Value * 1000);
    }

    private void UplinkManualCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      if (!Changing && !IsTerrestrial)
      SatCust.UplinkManualCorrectionEnabled = UplinkManualCheckbox.Checked;
    }

    private void UplinkDopplerCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      if (!Changing && !IsTerrestrial)
      SatCust.UplinkDopplerCorrectionEnabled = UplinkDopplerCheckbox.Checked;
    }

    private void ModeCombobox_SelectedValueChanged(object sender, EventArgs e)
    {
      if (Changing) return;

      // mode from comboboxes to settings
      if (!IsTerrestrial)
      {
        TxCust.DownlinkMode = DownlinkMode;
        TxCust.UplinkMode = UplinkMode;
      }

      SetModes();
    }

    private void label3_Click(object sender, EventArgs e)
    {
      DownlinkDopplerCheckbox.Checked = !DownlinkDopplerCheckbox.Checked;
    }

    private void label4_Click(object sender, EventArgs e)
    {
      DownlinkManualCheckbox.Checked = !DownlinkManualCheckbox.Checked;
    }

    private void label7_Click(object sender, EventArgs e)
    {
      DownlinkRitCheckbox.Checked = !DownlinkRitCheckbox.Checked;
    }

    private void Spinner_DoubleClick(object sender, EventArgs e)
    {
      var spinner = (NumericUpDown)sender;
      spinner.Value = 0;
    }






    //----------------------------------------------------------------------------------------------
    //                                    frequencies
    //----------------------------------------------------------------------------------------------
    private void SetFrequencies()
    {
      bool bright = true;
      double dopplerFactor = 0;

      if (IsTerrestrial)
      {
        CorrectedDownlinkFrequency = DownlinkFrequency;
        if (DownlinkRitCheckbox.Checked) CorrectedDownlinkFrequency += RitOffset;
        CorrectedUplinkFrequency = UplinkFrequency = 0;
      }

      else
      {
        // doppler
        var observation = ctx.HamPasses.ObserveSatellite(Sat, DateTime.UtcNow);
        dopplerFactor = observation.RangeRate / 3e5;
        bright = observation.Elevation > 0;

        // downlink
        DownlinkFrequency = Tx.DownlinkLow;
        if (IsTransponder) DownlinkFrequency += TxCust.TranspnderOffset;

        CorrectedDownlinkFrequency = DownlinkFrequency;
        if (DownlinkRitCheckbox.Checked) CorrectedDownlinkFrequency += RitOffset;
        if (DownlinkDopplerCheckbox.Checked) CorrectedDownlinkFrequency *= 1 - dopplerFactor;
        if (DownlinkManualCheckbox.Checked) CorrectedDownlinkFrequency += SatCust.DownlinkManualCorrection;

        // uplink
        if (IsTransponder)
          if (Tx.invert) UplinkFrequency = (double)Tx.uplink_high! - TxCust.TranspnderOffset;
          else UplinkFrequency = (double)Tx.uplink_low! + TxCust.TranspnderOffset;
        else if (Tx.uplink_low.HasValue) UplinkFrequency = (double)Tx.uplink_low;
        else UplinkFrequency = 0;

        CorrectedUplinkFrequency = UplinkFrequency;
        if (UplinkDopplerCheckbox.Checked) CorrectedUplinkFrequency *= 1 + dopplerFactor;
        if (UplinkManualCheckbox.Checked) CorrectedUplinkFrequency += SatCust.UplinkManualCorrection;
      }

      // send to slicer and CAT
      SetSlicerFrequency();
      ctx.CatControl.Rx?.SetRxFrequency(CorrectedDownlinkFrequency);
      if (CorrectedUplinkFrequency != 0) ctx.CatControl.Tx?.SetTxFrequency(CorrectedUplinkFrequency);

      // show
      FrequenciesToUi(dopplerFactor);
      SetFrequencyColors(bright);
    }




    //----------------------------------------------------------------------------------------------
    //                                      settings to UI
    //----------------------------------------------------------------------------------------------
    private void FrequenciesToUi(double dopplerFactor)
    {
      DownlinkFrequencyLabel.Text = $"{DownlinkFrequency:n0}";
      UplinkFrequencyLabel.Text = UplinkFrequency == 0 ? "000,000,000" : $"{UplinkFrequency:n0}";

      string sign = dopplerFactor >= 0 ? "" : "+";
      double offset = -DownlinkFrequency * dopplerFactor;
      DownlinkDopplerLabel.Text = offset == 0 ? "0,000" : $"{sign}{offset:n0}";

      sign = dopplerFactor > 0 ? "+" : "";
      offset = UplinkFrequency * dopplerFactor;
      UplinkDopplerLabel.Text = offset == 0 ? "0,000" : $"{sign}{offset:n0}";
    }

    private void SetFrequencyColors(bool bright)
    {
      // downlink
      if (Tx.IsUhf())
        DownlinkFrequencyLabel.ForeColor = bright ? Color.Cyan : Color.Teal;
      else if (Tx.IsVhf())
        DownlinkFrequencyLabel.ForeColor = bright ? Color.Yellow : Color.Olive;
      else
        DownlinkFrequencyLabel.ForeColor = bright ? Color.White : Color.Gray;

      // uplink
      if (!HasUplink)
        UplinkFrequencyLabel.ForeColor = Color.Gray;
      else if (Tx.IsUhf((long)UplinkFrequency))
        UplinkFrequencyLabel.ForeColor = bright ? Color.Cyan : Color.Teal;
      else if (Tx.IsVhf((long)UplinkFrequency))
        UplinkFrequencyLabel.ForeColor = bright ? Color.Yellow : Color.Olive;
      else
        UplinkFrequencyLabel.ForeColor = bright ? Color.White : Color.Gray;
    }

    private void SettingsToUi()
    {
      Changing = true;

      DownlinkRitCheckbox.Checked = false;
      DownlinkRitSpinner.Value = 0;

      // downlink
      if (IsTerrestrial)
      {
        DownlinkManualSpinner.Value = 0;

        DownlinkLabel.Text = "Terrestrial";
        DownlinkLabel.ForeColor = Color.Red;
        DownlinkLabel.Font = new(DownlinkLabel.Font, FontStyle.Bold);

        DownlinkDopplerCheckbox.Visible = false;
        DownlinkDopplerLabel.BackColor = SystemColors.Control;

        DownlinkManualCheckbox.Visible = false;
        DownlinkManualSpinner.BackColor = SystemColors.Control;

        DownlinkManualCheckbox.Enabled = DownlinkManualSpinner.Enabled = DownlinkModeCombobox.Enabled = true;
        label3.Enabled = label4.Enabled = false;
      }
      else
      {
        DownlinkDopplerCheckbox.Checked = SatCust.DownlinkDopplerCorrectionEnabled;
        DownlinkManualCheckbox.Checked = SatCust.DownlinkManualCorrectionEnabled;
        DownlinkManualSpinner.Value = (decimal)(SatCust.DownlinkManualCorrection / 1000d);

        DownlinkLabel.Text = "Downlink";
        DownlinkLabel.ForeColor = SystemColors.ControlText;
        DownlinkLabel.Font = new(DownlinkLabel.Font, FontStyle.Regular);

        DownlinkDopplerCheckbox.Visible = true;
        DownlinkDopplerLabel.BackColor = SystemColors.Window;

        DownlinkManualCheckbox.Visible = true;
        DownlinkManualSpinner.BackColor = SystemColors.Window;


        DownlinkManualCheckbox.Enabled = DownlinkManualSpinner.Enabled = DownlinkModeCombobox.Enabled = true;
        label3.Enabled = label4.Enabled = true;
      }

      // uplink
      if (IsTerrestrial || !HasUplink)
      {
        UplinkManualSpinner.Value = 0;

        UplinkLabel.Text = "No Uplink";

        UplinkDopplerCheckbox.Visible = false;
        UplinkDopplerLabel.BackColor = SystemColors.Control;

        UplinkManualCheckbox.Visible = false;
        UplinkManualSpinner.BackColor = SystemColors.Control;

        UplinkManualCheckbox.Enabled = UplinkManualSpinner.Enabled = UplinkModeCombobox.Enabled = false;
        label5.Enabled = label6.Enabled = false;
      }
      else 
      {
        UplinkDopplerCheckbox.Checked = SatCust.UplinkDopplerCorrectionEnabled;
        UplinkManualCheckbox.Checked = SatCust.UplinkManualCorrectionEnabled;
        UplinkManualSpinner.Value = (decimal)(SatCust.UplinkManualCorrection / 1000d);

        UplinkLabel.Text = "Uplink";

        UplinkDopplerCheckbox.Visible = true;
        UplinkDopplerLabel.BackColor = SystemColors.Window;

        UplinkManualCheckbox.Visible = true;
        UplinkManualSpinner.BackColor = SystemColors.Window;

        UplinkManualCheckbox.Enabled = UplinkManualSpinner.Enabled = UplinkModeCombobox.Enabled = true;
        label5.Enabled = label6.Enabled = true;
      }

      Changing = false;
    }

    private void IncrementSpinnerValue(NumericUpDown spinner, int deltaHz)
    {
      var value = spinner.Value + (decimal)(deltaHz / 1000d);
      SetSpinnerValue(spinner, value);
    }

    private void SetSpinnerValue(NumericUpDown spinner, decimal valueKHz)
    {
      spinner.Value = Math.Max(spinner.Minimum, Math.Min(spinner.Maximum, valueKHz));
    }
  }
}
