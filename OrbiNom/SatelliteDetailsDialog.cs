using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbiNom
{
  public partial class SatelliteDetailsDialog : Form
  {
    private SatnogsDbSatellite? Satellite;

    public SatelliteDetailsDialog()
    {
      InitializeComponent();
    }

    internal static void ShowSatellite(SatnogsDbSatellite? satellite, Form parent, Point location)
    {
      var dlg = new SatelliteDetailsDialog();

      dlg.satelliteDetailsControl1.SatNameLabel.Text = satellite.name;
      dlg.satelliteDetailsControl1.SatAkaLabel.Visible = satellite.AllNames.Count > 1;
      dlg.satelliteDetailsControl1.SatAkaLabel.Text = $"a.k.a. {string.Join(", ", satellite.AllNames.Where(s=>s != satellite.name))}";
      dlg.Satellite = satellite;
      dlg.satelliteDetailsControl1.SatellitePropertyGrid.SelectedObject = satellite;

      //dlg.Location = location;
      dlg.ShowDialog(parent);
    }
  }
}
