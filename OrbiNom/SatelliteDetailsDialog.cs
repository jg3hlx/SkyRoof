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
    public SatelliteDetailsDialog()
    {
      InitializeComponent();
    }

    internal static void ShowSatellite(SatnogsDbSatellite? satellite, Form parent)
    {
      var dlg = new SatelliteDetailsDialog();
      dlg.satelliteDetailsControl1.ShowSatellite(satellite);
      dlg.ShowDialog(parent);
    }
  }
}
