using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace OrbiNom
{
  public partial class PassesPanel : DockContent
  {
    private readonly Context ctx;

    public PassesPanel()
    {
      InitializeComponent();
    }
    public PassesPanel(Context ctx)
    {
      InitializeComponent();

      this.ctx = ctx;
      ctx.PassesPanel = this;
      ctx.MainForm.SatellitePassesMNU.Checked = true;

      var controls = Enumerable.Range(0, 20).Select(i => MakePassControl(i)).ToArray();
      panel1.Controls.AddRange(controls);
    }

    private PassControl MakePassControl(object i)
    {
      var passCtrl = new PassControl();
      passCtrl.Parent = panel1;
      passCtrl.Dock = DockStyle.Top;
      passCtrl.SatNameLabel.Text += $"-{i}";
      passCtrl.SatNameLabel.BackColor = Color.LightCyan;
      return passCtrl;
    }

    private void PassesPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.PassesPanel = null;
      ctx.MainForm.SatellitePassesMNU.Checked = false;
    }
  }
}
