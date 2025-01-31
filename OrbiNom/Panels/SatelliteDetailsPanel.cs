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

// todo: sort transmitters
// todo: split in 2 panels
// todo: select transmitter and show selection

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
      //satelliteDetailsControl1.splitContainer1.SplitterMoved += SplitContainer1_SplitterMoved;

      SetSatellite();
    }

    private void SatelliteDetailsPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.SatelliteDetailsPanel = null;
      ctx.MainForm.SatelliteDetailsMNU.Checked = false;
      ctx.Settings.Ui.SatelliteDetailsPanel.SplitterDistance = satelliteDetailsControl1.splitContainer1.SplitterDistance;
    }

    public void SetSatellite(SatnogsDbSatellite? sat = null)
    {
      sat ??= ctx.SatelliteSelector.SelectedSatellite; ;
      sat.SetElevationAndFootPrint();
      satelliteDetailsControl1.ShowSatellite(sat);
    }

    private void SatelliteDetailsPanel_Shown(object sender, EventArgs e)
    {
      if (ctx.Settings.Ui.SatelliteDetailsPanel.SplitterDistance > -1)
        satelliteDetailsControl1.splitContainer1.SplitterDistance = ctx.Settings.Ui.SatelliteDetailsPanel.SplitterDistance;
    }
  }
}
