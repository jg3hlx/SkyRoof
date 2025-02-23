using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL.SceneGraph.Lighting;
using WeifenLuo.WinFormsUI.Docking;

namespace OrbiNom
{
  public partial class WaterfallPanel : DockContent
  {
    private Context ctx;
    double SdrCenterFrequency, Bandwidth, SamplingRate;


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
    internal void SetPassband(double frequency, double samplingRate)
    {
      SdrCenterFrequency = frequency;
      SamplingRate = samplingRate;

      scaleControl1.CenterFrequency = frequency;
      scaleControl1.Bandwidth = samplingRate / WaterfallControl.Zoom;
    }






    //----------------------------------------------------------------------------------------------
    //                                 mouse over waterfall
    //----------------------------------------------------------------------------------------------
    int MouseDownX, MouseMoveX;
    double MouseDownFrequency;

    private void WaterfallControl_MouseDown(object? sender, MouseEventArgs e)
    {
      MouseDownX = MouseMoveX = e.X;
      MouseDownFrequency = scaleControl1.CenterFrequency;
      Cursor = Cursors.NoMoveHoriz;
    }

    private void WaterfallControl_MouseMove(object? sender, MouseEventArgs e)
    {
      if (e.X == MouseMoveX) return;
      MouseMoveX = e.X;
      //UpdateLabels(e);

      if (e.Button != MouseButtons.Left) return;

      var dx = MouseMoveX - MouseDownX;
      scaleControl1.CenterFrequency = MouseDownFrequency - dx * scaleControl1.Bandwidth / scaleControl1.width;
      ValidateWaterfallViewport();
      scaleControl1.Refresh();

      WaterfallControl.Pan = (SdrCenterFrequency - scaleControl1.CenterFrequency) / scaleControl1.Bandwidth * 2;
      WaterfallControl.OpenglControl.Refresh();
    }

    private void WaterfallControl_MouseUp(object? sender, MouseEventArgs e)
    {
      Cursor = Cursors.Cross;
    }

    private void WaterfallControl_MouseWheel(object? sender, MouseEventArgs e)
    {
      double freq = PixelToFreq(e.X);
      double dx = e.X - scaleControl1.width / 2;

      scaleControl1.Bandwidth = scaleControl1.Bandwidth * Math.Pow(1.2, -e.Delta / 120);
      ValidateWaterfallViewport();

      scaleControl1.CenterFrequency = freq - dx / scaleControl1.width * scaleControl1.Bandwidth;
      ValidateWaterfallViewport();

      WaterfallControl.Zoom = SamplingRate / scaleControl1.Bandwidth;
      WaterfallControl.Pan = (SdrCenterFrequency - scaleControl1.CenterFrequency) / scaleControl1.Bandwidth * 2;

      scaleControl1.Refresh();
      WaterfallControl.OpenglControl.Refresh();
    }

    public double PixelToFreq(float x)
    {
      double dx = x - scaleControl1.width / 2d;
      return scaleControl1.CenterFrequency + dx * scaleControl1.Bandwidth / scaleControl1.width;
    }


    // {!} todo: use max. bandwidth instead
    private const float MinZoom = 1.97f;
    
    private void ValidateWaterfallViewport()
    {
      double maxBW = SamplingRate / MinZoom;
      double minBW = scaleControl1.width / 0.05;
      double bandwidth = Math.Min(maxBW, Math.Max(minBW, scaleControl1.Bandwidth));

      double minFreq = SdrCenterFrequency - maxBW / 2 + bandwidth / 2;
      double maxFreq = SdrCenterFrequency + maxBW / 2 - bandwidth / 2;
      double centerFrequency = Math.Max(minFreq, Math.Min(maxFreq, scaleControl1.CenterFrequency));

      scaleControl1.Bandwidth = bandwidth;
      scaleControl1.CenterFrequency = centerFrequency;
    }
  }
}
