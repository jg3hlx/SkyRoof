using System.Reflection.Emit;
using Serilog;
using VE3NEA;
using WeifenLuo.WinFormsUI.Docking;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SkyRoof
{
  public partial class QsoSchedulerPanel : DockContent
  {
    private readonly Context ctx = null!;
    private bool changing;

    public QsoSchedulerPanel()
    {
      InitializeComponent();
    }

    public QsoSchedulerPanel(Context ctx)
    {
      Log.Information("Creating QsoScheduler");
      this.ctx = ctx;
      InitializeComponent();

      ctx.QsoSchedulerPanel = this;
      ctx.MainForm.QsoSchedulerMNU.Checked = true;

      SetSatelliteList();
    }

    private void QsoScheduler_FormClosing(object sender, FormClosingEventArgs e)
    {
      Log.Information("Closing QsoScheduler");
      ctx.QsoSchedulerPanel = null;
      ctx.MainForm.QsoSchedulerMNU.Checked = false;
    }

    private void DxSquareEdit_TextChanged(object sender, EventArgs e)
    {
      bool ok = GridSquare.IsValid(DxSquareEdit.Text.Trim());
      DxSquareEdit.BackColor = ok ? SystemColors.Window : Color.FromArgb(0xFF, 0xDD, 0xDD);

      UpdatePredictions();
    }

    private void SatelliteComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      UpdatePredictions();
    }

    public void SetSatelliteList()
    {
      SatelliteComboBox.Items.Clear();
      SatelliteComboBox.Items.AddRange(ctx.SatelliteSelector.GroupSatellites);
      UpdatePredictions();
    }

    public void UpdatePredictions()
    {
      PredictionList.Clear();

      bool ok = GridSquare.IsValid(DxSquareEdit.Text.Trim()) && SatelliteComboBox.SelectedIndex >= 0;

      if (ok) ComputePredictions();
    }

    private void ComputePredictions()
    {
      Console.Beep();
    }
  }
}
