using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkyRoof.Forms
{
  public partial class AzElEntryDialog : Form
  {
    private Context ctx;

    public AzElEntryDialog()
    {
      InitializeComponent();
    }

    public void Open(Context ctx)
    {
      this.ctx = ctx;
      SetupSpinners();
      Location = Cursor.Position;
      ShowDialog();
    }

    private void SetupSpinners()
    {
      AzimuthSpinner.Minimum = ctx.Settings.Rotator.MinAzimuth - ctx.Settings.Rotator.AzimuthOffset;
      AzimuthSpinner.Maximum = ctx.Settings.Rotator.MaxAzimuth - ctx.Settings.Rotator.AzimuthOffset;
      ElevationSpinner.Minimum = ctx.Settings.Rotator.MinElevation - ctx.Settings.Rotator.ElevationOffset;
      ElevationSpinner.Maximum = ctx.Settings.Rotator.MaxElevation - ctx.Settings.Rotator.ElevationOffset;
    }

    private void OkBtn_Click(object sender, EventArgs e)
    {
      StartRotation();
      Close();
    }

    private void StartRotation()
    {
      ctx.RotatorControl.TrackCheckbox.Checked = false;
      ctx.RotatorControl.Go((int)AzimuthSpinner.Value, (int)ElevationSpinner.Value);
    }
  }
}
