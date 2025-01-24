using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace OrbiNom
{
  public partial class SkyViewPanel : DockContent
  {
    private readonly Context ctx;
    private SatellitePass? Pass;

    public SkyViewPanel() { InitializeComponent(); }

    public SkyViewPanel(Context ctx)
    {
      InitializeComponent();
      this.ctx = ctx;
      ctx.SkyViewPanel = this;
      ctx.MainForm.GroupViewMNU.Checked = true;
      ctx.SatelliteSelector.SelectedPassChanged += SatelliteSelector_SelectedPassChanged;

      //https://stackoverflow.com/questions/818415
      typeof(Panel).InvokeMember("DoubleBuffered",
        BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
        null, panel, [true]);
    }

    private void SkyViewPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.SatelliteSelector.SelectedPassChanged -= SatelliteSelector_SelectedPassChanged;
      ctx.SkyViewPanel = null;
      ctx.MainForm.SkyViewMNU.Checked = false;
    }

    private void SatelliteSelector_SelectedPassChanged(object? sender, EventArgs e)
    {
      Pass = ctx.SatelliteSelector.SelectedPass;
      if (Pass == null)
      {
        PassRadioBtn.Text = "Orbit";
        PassRadioBtn.Enabled = false;
        RealTimeRadioBtn.Checked = true;
      }
      else
      {
        PassRadioBtn.Text = $"Orbit #{Pass.OrbitNumber} of {Pass.Satellite.name}";
        PassRadioBtn.Enabled = true;
        PassRadioBtn.Checked = true;
      }

      panel.Invalidate();
    }

    private void radioButton_CheckedChanged(object sender, EventArgs e)
    {
      var radioBtn = (RadioButton)sender;
      if (!radioBtn.Checked) return;
      panel.Invalidate();
    }

    private void panel_Resize(object sender, EventArgs e)
    {
      panel.Invalidate();
    }

    private void panel_Paint(object sender, PaintEventArgs e)
    {
      var g = e.Graphics;
      g.SmoothingMode = SmoothingMode.AntiAlias;
      DrawBackground(g);
    }

    Brush BgBrush = new SolidBrush(Color.FromArgb(0xda, 0xef, 0xf8));
    private void DrawBackground(Graphics g)
    {
      var sizeN = g.MeasureString("N", panel.Font);
      var sizeS = g.MeasureString("S", panel.Font);
      var sizeE = g.MeasureString("E", panel.Font);
      var sizeW = g.MeasureString("W", panel.Font);

      var center = new PointF(panel.Width / 2, panel.Height / 2);
      float radiusX = center.X - Math.Max(sizeE.Width, sizeW.Width) - 4;
      float radiusY = center.Y - Math.Max(sizeN.Width, sizeS.Width) - 4;
      float radius = Math.Min(radiusX, radiusY);

      g.FillRectangle(Brushes.White, panel.ClientRectangle);
      RectangleF bounds = new RectangleF(center.X - radius, center.Y - radius, 2*radius, 2*radius);
      g.FillEllipse(BgBrush, bounds);

      for (float r = radius / 3; r <= radius; r += radius / 3)
      {
        var rect = new RectangleF(center.X - r, center.Y - r, 2 * r, 2 * r);
        g.DrawEllipse(Pens.Teal, rect);
      }

      g.DrawLine(Pens.Teal, center.X - radius, center.Y, center.X + radius, center.Y);
      g.DrawLine(Pens.Teal, center.X, center.Y - radius, center.X, center.Y + radius);

      g.DrawString("N", panel.Font, Brushes.Teal, center.X - sizeN.Width / 2, center.Y - radius - sizeN.Height - 2);
      g.DrawString("S", panel.Font, Brushes.Teal, center.X - sizeS.Width / 2, center.Y + radius + 2);
      g.DrawString("E", panel.Font, Brushes.Teal, center.X + radius + 2, center.Y - sizeE.Height / 2);
      g.DrawString("W", panel.Font, Brushes.Teal, center.X - radius - sizeW.Width - 2, center.Y - sizeW.Height / 2);
    }


    // currently not used
    //https://stackoverflow.com/questions/3519835
    PathGradientBrush? GradientBrush;
    private PathGradientBrush GetRadialBrush(RectangleF bounds)
    {
      if (GradientBrush != null) return GradientBrush;

      using (var ellipsePath = new GraphicsPath())
      {
        ellipsePath.AddEllipse(bounds);
        GradientBrush = new PathGradientBrush(ellipsePath);
        GradientBrush.CenterPoint = new PointF(bounds.Width / 2f, bounds.Height / 2f);
        GradientBrush.CenterColor = Color.Black;
        GradientBrush.SurroundColors = [Color.SkyBlue];
        GradientBrush.FocusScales = new PointF(0, 0);
      }
      return GradientBrush;
    }
  }
}
