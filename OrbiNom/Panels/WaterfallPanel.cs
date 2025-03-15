using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics;
using SharpGL;
using SharpGL.SceneGraph.Lighting;
using WeifenLuo.WinFormsUI.Docking;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OrbiNom
{
  public partial class WaterfallPanel : DockContent
  {
    public Context ctx;
    private double SdrCenterFrequency => ctx.Sdr?.Info?.Frequency ?? SdrConst.UHF_CENTER_FREQUENCY;
    private double MaxBandwidth => ctx.Sdr?.Info?.MaxBandwidth ?? SdrConst.MAX_BANDWIDTH;
    private double SamplingRate => ctx.Sdr?.Info?.SampleRate ?? SdrConst.MAX_BANDWIDTH;

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

      SplitContainer.SplitterDistance = ctx.Settings.Waterfall.SplitterDistance;
      ApplySettings();
      ctx.MainForm.ConfigureWaterfall();

      ScaleControl.ctx = ctx;
      ScaleControl.BuildLabels();
      ScaleControl.MouseWheel += WaterfallControl_MouseWheel;
      ScaleControl.MouseMove += ScaleControl_MouseMove;
      ScaleControl.MouseDown += ScaleControl_MouseDown;

      WaterfallControl.OpenglControl.MouseDown += WaterfallControl_MouseDown;
      WaterfallControl.OpenglControl.MouseMove += WaterfallControl_MouseMove;
      WaterfallControl.OpenglControl.MouseUp += WaterfallControl_MouseUp;
      WaterfallControl.OpenglControl.MouseWheel += WaterfallControl_MouseWheel;
    }

    private void WaterfallPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.WaterfallPanel = null;
      ctx.MainForm.WaterfallMNU.Checked = false;

      ctx.Settings.Waterfall.SplitterDistance = SplitContainer.SplitterDistance;
    }

    public void ApplySettings()
    {
      var sett = ctx.Settings.Waterfall;
      WaterfallControl.Brightness = sett.Brightness;
      WaterfallControl.Contrast = sett.Contrast;
      int paletteIndex = Math.Min(ctx.PaletteManager.Palettes.Count() - 1, sett.PaletteIndex);
      WaterfallControl.SetPalette(ctx.PaletteManager.Palettes[paletteIndex]);
      WaterfallControl.Refresh();

      ctx.MainForm.SetWaterfallSpeed();
    }

    internal void SetPassband()
    {
      ScaleControl.CenterFrequency = ctx.Sdr.Info.Frequency;
      ScaleControl.VisibleBandwidth = ctx.Sdr.Info.MaxBandwidth;

      WaterfallControl.Zoom = ctx.Sdr.Info.SampleRate / ctx.Sdr.Info.MaxBandwidth;
      WaterfallControl.Pan = 0;

      //bool maxBandwidthChanged = MaxBandwidth != maxBandwidth;
      //if (maxBandwidthChanged) WaterfallControl.Clear();
    }

    public void SetCenterFrequency(double frequency)
    {

      ScaleControl.CenterFrequency = frequency;
      ValidateWaterfallViewport();
      ScaleControl.Refresh();

      WaterfallControl.Pan = (ctx.Sdr.Info.Frequency - ScaleControl.CenterFrequency) / ScaleControl.VisibleBandwidth * 2;
      WaterfallControl.OpenglControl.Refresh();
    }

    private void SlidersBtn_Click(object sender, EventArgs e)
    {
      var dlg = new WaterfallSildersDlg(ctx);
      dlg.Location = WaterfallControl.PointToScreen(new Point(2, 2));
      dlg.Show();
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
      WaterfallControl.VisibleBandwidth = visibleBandwidth;
    }

    private void WaterfallControl_Resize(object sender, EventArgs e)
    {
      ScaleControl.HistoryRowCount = WaterfallControl.Height;
    }






    //----------------------------------------------------------------------------------------------
    //                                   waterfall mouse 
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

      double frequency = MouseDownFrequency - dx * ScaleControl.VisibleBandwidth / ScaleControl.width;
      SetCenterFrequency(frequency);
    }

    private void WaterfallControl_MouseUp(object? sender, MouseEventArgs e)
    {
      WaterfallControl.Cursor = Cursors.Cross;
    }

    private void WaterfallControl_MouseWheel(object? sender, MouseEventArgs e)
    {
      double dx = e.X - ScaleControl.width / 2;
      double freq = ScaleControl.CenterFrequency + dx * ScaleControl.VisibleBandwidth / ScaleControl.width;

      ScaleControl.VisibleBandwidth = ScaleControl.VisibleBandwidth * Math.Pow(1.2, -e.Delta / 120);
      ValidateWaterfallViewport();

      ScaleControl.CenterFrequency = freq - dx * ScaleControl.VisibleBandwidth / ScaleControl.width;

      ValidateWaterfallViewport();

      WaterfallControl.Zoom = SamplingRate / ScaleControl.VisibleBandwidth;
      label1.Text = $"{ScaleControl.VisibleBandwidth / ScaleControl.Size.Width:F3} Hz/pix";
      WaterfallControl.Pan = (SdrCenterFrequency - ScaleControl.CenterFrequency) / ScaleControl.VisibleBandwidth * 2;

      ScaleControl.Refresh();
      WaterfallControl.OpenglControl.Refresh();
    }




    //----------------------------------------------------------------------------------------------
    //                                    scale mouse 
    //----------------------------------------------------------------------------------------------
    private void ScaleControl_MouseMove(object? sender, MouseEventArgs e)
    {
      TransmitterLabel? labelUnderCursor = ScaleControl.GetLabelUnderCursor(e.Location);
      if (labelUnderCursor == null)
      {
        toolTip1.Hide(ScaleControl);
        toolTip1.ToolTipTitle = null;
        ScaleControl.Cursor = Cursors.Cross;
      }
      else if (toolTip1.ToolTipTitle != labelUnderCursor.Pass.Satellite.name)
      {
        var parts = labelUnderCursor.Pass.GetTooltipText(true);
        string tooltip = $"{parts[0]}  ({parts[2]})\n{parts[4]}\n{parts[5]}\n{labelUnderCursor.Tooltip}";

        if (tooltip != toolTip1.GetToolTip(this))
        {
          Point location = new((int)labelUnderCursor.Rect.Right + 1, (int)labelUnderCursor.Rect.Top);
          toolTip1.ToolTipTitle = labelUnderCursor.Pass.Satellite.name;
          toolTip1.Show(tooltip, ScaleControl, location);
          ScaleControl.Cursor = Cursors.Hand;
        }
      }
    }

    private void ScaleControl_MouseDown(object? sender, MouseEventArgs e)
    {
      // select transmitter 
      var label = ScaleControl.GetLabelUnderCursor(e.Location);
      if (label != null)
      {
        ctx.SatelliteSelector.SetSelectedSatellite(label.Pass.Satellite);
        ctx.SatelliteSelector.SetSelectedTransmitter(label.Transmitters.First());
        ctx.SatelliteSelector.SetSelectedPass(label.Pass);
        return;
      }

      // tune to offset in transponder passband
      label = ScaleControl.GetTransponderUnderCursor(e.Location);
      if (label != null)
      {
        double offset = ScaleControl.PixelToNominalFreq(label.Pass, DateTime.UtcNow, e.X) - (double)label.Transponder!.downlink_low!;
        ctx.FrequencyControl.SetTransponderOffset(label.Transponder, offset);
        return;
      }

      // tune to terrestrial frequency
      ctx.FrequencyControl.SetFrequency(ScaleControl.PixelToFreq(e.X));
    }

    internal void BringInView(double value)
    {
      if (!ScaleControl.IsFrequencyVisible(value)) SetCenterFrequency(value);
    }
  }
}
