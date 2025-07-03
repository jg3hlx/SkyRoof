using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Serilog;
using VE3NEA;
using WeifenLuo.WinFormsUI.Docking;

namespace SkyRoof
{
  public partial class QsoEntryPanel : DockContent
  {
    private const string States = "AL,AK,AZ,AR,CA,CO,CT,DE,FL,GA,HI,ID,IL,IN,IA,KS,KY,LA,ME,MD,MA,MI,MN,MS,MO,MT,NE,NV,NH,NJ,NM,NY,NC,ND,OH,OK,OR,PA,RI,SC,SD,TN,TX,UT,VT,VA,WA,WV,WI,WY";
    private Context ctx;
    private LoggerInterface LoggerInterface = new();
    private bool Changing;

    public Slicer.Mode? LastSetMode = null;

    public QsoEntryPanel()
    {
      InitializeComponent();
    }

    public QsoEntryPanel(Context ctx)
    {
      this.ctx = ctx;
      Log.Information("Creating QsoEntryPanel");
      InitializeComponent();

      ApplySettings();

      ctx.QsoEntryPanel = this;
      ctx.MainForm.QsoEntryMNU.Checked = true;

      Changing = true;
      BandComboBox.DataSource = new string[] { "2M", "70CM" };
      ModeComboBox.DataSource = new string[] { "CW", "SSB", "FM", "FT4" };
      StateComboBox.DataSource = States.Split(',');
      SatComboBox.DataSource = new SatelliteNames().Lotw.Values.ToArray();

      BandComboBox.SelectedIndex = -1;
      ModeComboBox.SelectedIndex = -1;
      StateComboBox.SelectedIndex = -1;
      SatComboBox.SelectedIndex = -1;

      Changing = false;

      ClearFields();
    }

    private void ClearFields()
    {
      ClearFrames();

      SetUtc();

      Changing = true;

      SetSatellite();
      SetBand();
      SetMode();

      CallEdit.Text = GridEdit.Text = NameEdit.Text = string.Empty;
      CallEdit.BackColor = SystemColors.Window;
      CallEdit.ForeColor = SystemColors.WindowText;
      StateComboBox.SelectedIndex = -1;

      Changing = false;
    }

    private void ClearFrames()
    {
      UtcFrame.BackColor = Color.LightSkyBlue;
      BandFrame.BackColor = Color.LightSkyBlue;
      ModeFrame.BackColor = Color.LightSkyBlue;
      SatFrame.BackColor = Color.LightSkyBlue;
      CallFrame.BackColor = Color.LightSkyBlue;
      GridFrame.BackColor = Color.LightSkyBlue;
      StateFrame.BackColor = Color.LightSkyBlue;
      SentFrame.BackColor = Color.LightSkyBlue;
      RecvFrame.BackColor = Color.LightSkyBlue;
      NameFrame.BackColor = Color.LightSkyBlue;
    }

    private void QsoEntryPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      Log.Information("Closing QsoEntryPanel");
      ctx.QsoEntryPanel = null;
      ctx.MainForm.QsoEntryMNU.Checked = false;
    }

    internal void ApplySettings()
    {
      ShowHideFields();
    }

    private void ShowHideFields()
    {
      var visibleFields = ctx.Settings.QsoEntry.Fields;

      foreach (var control in flowLayoutPanel1.Controls)
      {
        if (control is Panel panel)
          panel.Visible = panel.Name == "ButtonsPanel" || visibleFields.HasFlag((QsoFields)(1 << panel.TabIndex));
      }
    }

    internal void SetUtc()
    {
      if (UtcFrame.BackColor == Color.LightSkyBlue)
      {
        Changing = true;
        UtcPicker.Value = DateTime.UtcNow;
        Changing = false;
      }
    }

    internal void SetSatellite()
    {
      if (ctx.SatelliteSelector.SelectedSatellite != null)
      {
        string? sat = ctx.SatelliteSelector.SelectedSatellite?.LotwName;
        if (string.IsNullOrEmpty(sat)) SatComboBox.SelectedIndex = -1;
        else SatComboBox.SelectedIndex = SatComboBox.FindStringExact(sat);
      }
      else
        SatComboBox.SelectedIndex = -1;

      if (SatComboBox.SelectedIndex == -1) SatComboBox.Text = string.Empty;

      SatFrame.BackColor = Color.LightSkyBlue;

    }

    internal void SetBand()
    {
      var freq = ctx.FrequencyControl.RadioLink.CorrectedUplinkFrequency;
      if (SatnogsDbTransmitter.IsUhfFrequency(freq))
        BandComboBox.Text = "70CM";
      else if (SatnogsDbTransmitter.IsVhfFrequency(freq))
        BandComboBox.Text = "2M";
      else
      {
        BandComboBox.SelectedIndex = -1;
        BandComboBox.Text = string.Empty;
      }

      BandFrame.BackColor = Color.LightSkyBlue;
    }

    internal void SetMode()
    {
      Slicer.Mode? mode = ctx.FrequencyControl.RadioLink.HasUplink ? ctx.FrequencyControl.RadioLink.UplinkMode : null;
      if (mode == LastSetMode) return;
      LastSetMode = mode;

      string newMode;

      if (mode == Slicer.Mode.USB || mode == Slicer.Mode.LSB)
        newMode = "SSB";
      else if (mode == Slicer.Mode.CW)
        newMode = "CW";
      else if (mode == Slicer.Mode.FM)
        newMode = "FM";
      else if (mode == Slicer.Mode.USB_D || mode == Slicer.Mode.LSB_D)
        newMode = "FT4";
      else
        newMode = string.Empty;


      ModeComboBox.Text = newMode;
      if (string.IsNullOrEmpty(newMode)) ModeComboBox.SelectedIndex = -1;

      ModeFrame.BackColor = Color.LightSkyBlue;
      SetReport();
    }

    private void SetReport()
    {
      string defaultReport;

      if (ModeComboBox.Text == "CW") defaultReport = RecvEdit.Text = "599";
      else if (ModeComboBox.Text == "SSB") defaultReport = RecvEdit.Text = "59";
      else if (ModeComboBox.Text == "FM") defaultReport = RecvEdit.Text = "59";
      else defaultReport = RecvEdit.Text = string.Empty;

      SentEdit.Text = RecvEdit.Text = defaultReport;
      SentFrame.BackColor = RecvFrame.BackColor = Color.LightSkyBlue;
    }

    private void ClearBtn_Click(object sender, EventArgs e)
    {
      ClearFields();
    }

    private void Field_Changed(object sender, EventArgs e)
    {
      if (sender == ModeComboBox) SetReport();

      if (Changing) return;

      var control = (Control)sender;
      control.Parent!.BackColor = control.Text == "" ? Color.LightSkyBlue : Color.Blue;

      // todo: exclude sent, recv and name
      var qso = FieldsToQsoInfo();
      qso = LoggerInterface.Augment(qso);
      QsoInfoToFields(qso);
    }

    private void LogBtn_Click(object sender, EventArgs e)
    {
      var qso = FieldsToQsoInfo();

      if (!Utils.CallsignRegex.IsMatch(qso.Call)) { ErrBox("Invalid callsign"); return; }
      if (!Utils.GridSquare4Regex.IsMatch(qso.Grid)) { ErrBox("Invalid grid square"); return; }
      if (qso.Band == string.Empty) { ErrBox("Invalid band"); return; }
      if (qso.Mode == string.Empty) { ErrBox("Invalid mode"); return; }

      if (qso.Sat == string.Empty && !Ask("Satellite not specified")) return;
      if (qso.Sent == string.Empty && !Ask("Sent report not specified")) return;
      if (qso.Recv == string.Empty && !Ask("Received report not specified")) return;

      LoggerInterface.SaveQso(qso);
      ClearFields();
    }


    private QsoInfo FieldsToQsoInfo()
    {
      QsoInfo info = new();

      info.Utc = UtcPicker.Value;
      info.Band = BandComboBox.Text.Trim().ToUpperInvariant();
      info.Mode = ModeComboBox.Text.Trim().ToUpperInvariant();
      info.Sat = SatComboBox.Text.Trim();
      info.Call = CallEdit.Text.Trim().ToUpperInvariant();
      info.Grid = GridEdit.Text.Trim().ToUpperInvariant();
      info.State = StateComboBox.Text.Trim().ToUpperInvariant();
      info.Sent = SentEdit.Text.Trim().ToUpperInvariant();
      info.Recv = RecvEdit.Text.Trim().ToUpperInvariant();
      info.Name = NameEdit.Text.Trim();

      return info;
    }

    public void QsoInfoToFields(QsoInfo qso)
    {
      Changing = true;

      if (GridFrame.BackColor == Color.LightSkyBlue) GridEdit.Text = qso.Grid;
      if (NameFrame.BackColor == Color.LightSkyBlue) NameEdit.Text = qso.Name;

      if (StateFrame.BackColor == Color.LightSkyBlue)
      {
        if (qso.State == "") StateComboBox.SelectedIndex = -1;
        StateComboBox.SelectedItem = qso.State;
      }

      Changing = false;

      CallEdit.BackColor = ColorTranslator.FromHtml(qso.BackColor);
      CallEdit.ForeColor = ColorTranslator.FromHtml(qso.ForeColor);
      toolTip1.SetToolTip(CallEdit, qso.StatusString);
    }

    private void ErrBox(string message)
    {
      MessageBox.Show(message, "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private bool Ask(string question)
    {
      return MessageBox.Show(question + ". Save anyway?", "Invalid Data",
        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    }

    private void UtcPicker_MouseDown(object sender, MouseEventArgs e)
    {
      UtcFrame.BackColor = UtcFrame.BackColor == Color.LightSkyBlue ? Color.Blue : Color.LightSkyBlue;
    }
  }
}

