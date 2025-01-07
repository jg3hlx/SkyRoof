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
  public partial class TimelinePanel : DockContent
  {
    private Context ctx;

    public TimelinePanel()
    {
      InitializeComponent();
    }

    public TimelinePanel(Context ctx)
    {
      InitializeComponent();
      this.ctx = ctx;
      ctx.TimelinePanel = this;
      ctx.MainForm.TimelineMNU.Checked = true;
    }

    private void TimelinePanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.TimelinePanel = null;
      ctx.MainForm.TimelineMNU.Checked = false;
    }

    private void TimelinePanel_Paint(object sender, PaintEventArgs e)
    {
      e.Graphics.FillRectangle(Brushes.Aquamarine, e.ClipRectangle);
    }
  }
}
