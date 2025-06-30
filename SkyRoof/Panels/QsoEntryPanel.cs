using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Serilog;
using WeifenLuo.WinFormsUI.Docking;

namespace SkyRoof
{
  public partial class QsoEntryPanel : DockContent
  {
    private const string States = "AL,AK,AZ,AR,CA,CO,CT,DE,FL,GA,HI,ID,IL,IN,IA,KS,KY,LA,ME,MD,MA,MI,MN,MS,MO,MT,NE,NV,NH,NJ,NM,NY,NC,ND,OH,OK,OR,PA,RI,SC,SD,TN,TX,UT,VT,VA,WA,WV,WI,WY";
    private Context ctx;
    private SatelliteNames SatelliteNames = new();

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

      BandComboBox.DataSource = new string[] { "2M", "70CM" };
      ModeComboBox.DataSource = new string[] { "CW", "SSB", "FM", "FT4" };
      StateComboBox.DataSource = States.Split(',');
      SatComboBox.DataSource = SatelliteNames.Lotw.Values.ToArray();

      ClearFields();
    }

    private void ClearFields()
    {
      // utc
      UtcPicker.Value = DateTime.UtcNow;

      // band
      var freq = ctx.FrequencyControl.RadioLink.CorrectedUplinkFrequency;
      if (SatnogsDbTransmitter.IsUhfFrequency(freq))
        BandComboBox.Text = "70CM";
      else if (SatnogsDbTransmitter.IsVhfFrequency(freq))
        BandComboBox.Text = "2M";
      else
        BandComboBox.SelectedIndex = -1;

      // mode
      var mode = ctx.FrequencyControl.RadioLink.UplinkMode;
      if (mode == Slicer.Mode.USB || mode == Slicer.Mode.LSB)
        ModeComboBox.Text = "SSB";
      else if (mode == Slicer.Mode.CW)
        ModeComboBox.Text = "CW";
      else if (mode == Slicer.Mode.FM)
        ModeComboBox.Text = "FM";
      else if (mode == Slicer.Mode.USB_D || mode == Slicer.Mode.LSB_D)
        ModeComboBox.Text = "FT4";
      else
        ModeComboBox.SelectedIndex = -1;

      // sat
      if (ctx.SatelliteSelector.SelectedSatellite != null)
        SatComboBox.Text = ctx.SatelliteSelector.SelectedSatellite?.AmsatEntries.FirstOrDefault();
      else
        SatComboBox.SelectedIndex = -1;

      // report
      if (ModeComboBox.Text == "CW")
        SentEdit.Text = RecvEdit.Text = "599";
      else if (ModeComboBox.Text == "SSB")
        SentEdit.Text = RecvEdit.Text = "59";
      else if (ModeComboBox.Text == "FM")
        SentEdit.Text = RecvEdit.Text = "59";
      else
        SentEdit.Text = RecvEdit.Text = string.Empty;

      // other
      CallEdit.Text = GridEdit.Text = NameEdit.Text = string.Empty;
      StateComboBox.SelectedIndex = -1;
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
  }
}

