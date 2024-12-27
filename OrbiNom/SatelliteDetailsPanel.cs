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
using WeifenLuo.WinFormsUI.Docking;

namespace OrbiNom
{
  public partial class SatelliteDetailsPanel : DockContent
  {
    private readonly Context ctx;

    public SatelliteDetailsPanel()
    {
      InitializeComponent();
    }
    public SatelliteDetailsPanel(Context ctx)
    {
      InitializeComponent();

      this.ctx = ctx;
      ctx.SatelliteDetailsPanel = this;
      ctx.MainForm.SatelliteDetailsMNU.Checked = true;
      satelliteDetailsControl1.splitContainer1.SplitterMoved += SplitContainer1_SplitterMoved;

      LoadSatelliteDetails();
    }

    private void SatelliteDetailsPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.SatelliteDetailsPanel = null;
      ctx.MainForm.SatelliteDetailsMNU.Checked = false;
    }

    private void SplitContainer1_SplitterMoved(object? sender, SplitterEventArgs e)
    {
      Console.Beep();
      ctx.Settings.Ui.SatelliteDetailsPanel.SplitterDistance = satelliteDetailsControl1.splitContainer1.SplitterDistance;
    }

    public void LoadSatelliteDetails()
    {
      var sett = ctx.Settings.SatelliteSettings;
      var group = sett.SatelliteGroups.First(g => g.Id == sett.SelectedGroup);
      var sat = ctx.SatnogsDb.GetSatellite(group.SelectedSatId);

      sat.SetElevationAndFootPrint();

      satelliteDetailsControl1.ShowSatellite(sat);
    }

    private void SatelliteDetailsPanel_Layout(object sender, LayoutEventArgs e)
    {
      if (ctx.Settings.Ui.SatelliteDetailsPanel.SplitterDistance > 0)
        satelliteDetailsControl1.splitContainer1.SplitterDistance = ctx.Settings.Ui.SatelliteDetailsPanel.SplitterDistance;
    }
  }
}
