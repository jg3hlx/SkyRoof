using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;
using SharpGL.SceneGraph.Lighting;
using WeifenLuo.WinFormsUI.Docking;

namespace OrbiNom
{
  public partial class WaterfallPanel : DockContent
  {
    public Context ctx;
    double SdrCenterFrequency, SamplingRate, MaxBandwidth;


    public WaterfallPanel()
    {
      InitializeComponent();
    }

    public WaterfallPanel(Context ctx)
    {
      InitializeComponent();
      this.ctx = ctx;

      ctx.WaterfallPanel = this;
      ctx.MainForm.WaterfallMNU.Checked = true;

      ScaleControl.ctx = ctx;
      ScaleControl.BuildLabels();
      ScaleControl.MouseWheel += WaterfallControl_MouseWheel;

      ctx.MainForm.ConfigureWaterfall();

      WaterfallControl.OpenglControl.MouseDown += WaterfallControl_MouseDown;
      WaterfallControl.OpenglControl.MouseMove += WaterfallControl_MouseMove;
      WaterfallControl.OpenglControl.MouseUp += WaterfallControl_MouseUp;
      WaterfallControl.OpenglControl.MouseWheel += WaterfallControl_MouseWheel;
    }

    private void WaterfallPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.WaterfallPanel = null;
      ctx.MainForm.WaterfallMNU.Checked = false;
    }
    internal void SetPassband(double frequency, double samplingRate, double maxBandwidth)
    {
      bool maxBandwidthChanged = MaxBandwidth != maxBandwidth;

      SdrCenterFrequency = frequency;
      SamplingRate = samplingRate;
      MaxBandwidth = maxBandwidth;

      ScaleControl.CenterFrequency = frequency;
      ScaleControl.VisibleBandwidth = maxBandwidth;

      WaterfallControl.Zoom = samplingRate / maxBandwidth;
      WaterfallControl.Pan = 0;
      if (maxBandwidthChanged) WaterfallControl.Clear();
    }






    //----------------------------------------------------------------------------------------------
    //                                 mouse over waterfall
    //----------------------------------------------------------------------------------------------
    int MouseDownX, MouseMoveX;
    double MouseDownFrequency;

    private void WaterfallControl_MouseDown(object? sender, MouseEventArgs e)
    {
      MouseDownX = MouseMoveX = e.X;
      MouseDownFrequency = ScaleControl.CenterFrequency;
      WaterfallControl.Cursor = Cursors.NoMoveHoriz;
    }

    private void WaterfallControl_MouseMove(object? sender, MouseEventArgs e)
    {
      if (e.X == MouseMoveX) return;
      MouseMoveX = e.X;

      if (e.Button != MouseButtons.Left) return;

      var dx = MouseMoveX - MouseDownX;
      ScaleControl.CenterFrequency = MouseDownFrequency - dx * ScaleControl.VisibleBandwidth / ScaleControl.width;
      ValidateWaterfallViewport();
      ScaleControl.Refresh();

      WaterfallControl.Pan = (SdrCenterFrequency - ScaleControl.CenterFrequency) / ScaleControl.VisibleBandwidth * 2;
      WaterfallControl.OpenglControl.Refresh();
    }

    private void WaterfallControl_MouseUp(object? sender, MouseEventArgs e)
    {
      WaterfallControl.Cursor = Cursors.Cross;
    }

    private void WaterfallControl_MouseWheel(object? sender, MouseEventArgs e)
    {
      double freq = PixelToFreq(e.X);
      double dx = e.X - ScaleControl.width / 2;

      ScaleControl.VisibleBandwidth = ScaleControl.VisibleBandwidth * Math.Pow(1.2, -e.Delta / 120);
      ValidateWaterfallViewport();

      ScaleControl.CenterFrequency = freq - dx / ScaleControl.width * ScaleControl.VisibleBandwidth;
      ValidateWaterfallViewport();

      WaterfallControl.Zoom = SamplingRate / ScaleControl.VisibleBandwidth;
      WaterfallControl.Pan = (SdrCenterFrequency - ScaleControl.CenterFrequency) / ScaleControl.VisibleBandwidth * 2;

      ScaleControl.Refresh();
      WaterfallControl.OpenglControl.Refresh();
    }

    public double PixelToFreq(float x)
    {
      double dx = x - ScaleControl.width / 2d;
      return ScaleControl.CenterFrequency + dx * ScaleControl.VisibleBandwidth / ScaleControl.width;
    }


    private const double MinHzPerPixel = 20;
    private void ValidateWaterfallViewport()
    {
      double minBandwidth = ScaleControl.width * MinHzPerPixel;
      double visibleBandwidth = Math.Min(MaxBandwidth, Math.Max(minBandwidth, ScaleControl.VisibleBandwidth));

      double minFreq = SdrCenterFrequency - MaxBandwidth / 2 + visibleBandwidth / 2;
      double maxFreq = SdrCenterFrequency + MaxBandwidth / 2 - visibleBandwidth / 2;
      double centerFrequency = Math.Max(minFreq, Math.Min(maxFreq, ScaleControl.CenterFrequency));

      ScaleControl.VisibleBandwidth = visibleBandwidth;
      ScaleControl.CenterFrequency = centerFrequency;
    }

    private void SlidersBtn_Click(object sender, EventArgs e)
    {
      var dlg = new WaterfallSildersDlg(ctx);
      dlg.Location = WaterfallControl.PointToScreen(new Point(2, 2));
      dlg.Show();
    }
  }
}
