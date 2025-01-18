using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace OrbiNom
{
  public partial class TimelinePanel : DockContent
  {
    private const double MaxPixelsPerMinute = 60; // at max zoom, 1 pixel = 1 second
    private const int ScaleHeight = 40;

    private readonly TimeSpan MinTimeOffset = TimeSpan.FromMinutes(-30);
    private readonly TimeSpan MaxTimeOffset = TimeSpan.FromDays(2);
    private readonly Brush ShadowBrush = new SolidBrush(Color.FromArgb(25, Color.Black));

    private Context ctx;
    double Zoom = 1;
    private double PixelsPerMinute;
    double X0 = int.MaxValue;
    int width, height;
    private int MouseDownX;
    private double MouseDownX0;
    private int MouseMoveX;
    private bool Dragging;


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

      MouseWheel += SatelliteTimelineControl_MouseWheel;
    }

    private void TimelinePanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.TimelinePanel = null;
      ctx.MainForm.TimelineMNU.Checked = false;
    }




    //----------------------------------------------------------------------------------------------
    //                                          draw
    //----------------------------------------------------------------------------------------------
    private void TimelinePanel_Paint(object sender, PaintEventArgs e)
    {
      var g = e.Graphics;
      ValidateViewport();
      DrawBg(g);

      var now = DateTime.Now;
      DrawDateLabels(g, now);
      DrawtimeLabels(g, now);
      g.DrawString($"x0 = {X0:F1}   zoom = {Zoom:F3} pix/min = {PixelsPerMinute:F3}", Font, Brushes.Black, 5, 5);

      //DrawPasses(g, now);
    }

    private void DrawBg(Graphics g)
    {
      // chart
      var rect = new RectangleF(0, 0, width, height - ScaleHeight);
      LinearGradientBrush lgb = new LinearGradientBrush(rect, Color.SkyBlue, Color.White, LinearGradientMode.Vertical);
      g.FillRectangle(lgb, rect);

      // time scale
      rect = new RectangleF(0, height - ScaleHeight, width, ScaleHeight);
      g.FillRectangle(Brushes.Silver, rect);

      // past time shadow
      rect = new RectangleF(0, 0, (int)X0, height - ScaleHeight);
      g.FillRectangle(ShadowBrush, rect);

    }

    private void DrawDateLabels(Graphics g, DateTime now)
    {
      var date1 = now.Date;
      float x1 = -1;
      float x2;

      do
      {
        var date2 = date1.AddDays(1);
        x2 = Math.Min(width, TimeToPixel(date2, now));
        g.DrawLine(Pens.Black, x2, height - ScaleHeight, x2, height);

        string label = $"{date1:MMM dd}";
        var size = TextRenderer.MeasureText(label, Font);
        if (x2 - x1 > size.Width + 15)
          g.DrawString(label, Font, Brushes.Black, (x1 + x2 - size.Width) / 2, height - size.Height - 5);

        date1 = date2;
        x1 = Math.Max(-1, x2);
      }
      while (x2 < width);
    }


    double[] Steps = [1, 2, 5, 10, 20, 30, 60, 120, 240, 360, 720];
    double[] SmallSteps = [1 / 6d, 1 / 2d, 1, 2, 5, 5, 15, 30, 60, 60, 120];

    private void DrawtimeLabels(Graphics g, DateTime now)
    {
      TimeSpan step = TimeSpan.Zero;
      TimeSpan smallStep = TimeSpan.Zero;

      for (int i = 0; i < Steps.Count(); i++)
        if (Steps[i] * PixelsPerMinute > 59)
        {
          step = TimeSpan.FromMinutes(Steps[i]);
          smallStep = TimeSpan.FromMinutes(SmallSteps[i]);
          break;
        }
      if (step == TimeSpan.Zero) return;


      // ticks
      var time = TruncateTime(now.Add(MinTimeOffset), smallStep);
      var x = TimeToPixel(time, now);
      while (x < width)
      {
        g.DrawLine(Pens.Black, x, height - ScaleHeight, x, height - ScaleHeight + 5);
        time = time.Add(smallStep);
        x = TimeToPixel(time, now);
      }


      // labels
      time = TruncateTime(now.Add(MinTimeOffset), step);
      x = TimeToPixel(time, now);

      while (x < width)
      {
        string label = $"{time:HH:mm}";
        var size = TextRenderer.MeasureText(label, Font);
        g.DrawString(label, Font, Brushes.Black, x - size.Width / 2, height - ScaleHeight + 7);

        g.DrawLine(Pens.Black, x, height - ScaleHeight, x, height - ScaleHeight + 10);
        time = time.Add(step);
        x = TimeToPixel(time, now);
      }
    }




    //----------------------------------------------------------------------------------------------
    //                                    viewport
    //----------------------------------------------------------------------------------------------
    private void ValidateViewport()
    {
      width = ClientRectangle.Width;
      height = ClientRectangle.Height;
      var now = DateTime.UtcNow;

      // validate zoom
      double totalMinutes = MaxTimeOffset.Subtract(MinTimeOffset).TotalMinutes;
      double minPixelsPerMinute = width / totalMinutes;
      double maxZoom = MaxPixelsPerMinute / minPixelsPerMinute;
      Zoom = Math.Max(1, Math.Min(maxZoom, Zoom));
      PixelsPerMinute = minPixelsPerMinute * Zoom;

      // validate offset
      double x0Min = -MaxTimeOffset.TotalMinutes * PixelsPerMinute + width;
      double x0Max = -MinTimeOffset.TotalMinutes * PixelsPerMinute;
      X0 = Math.Max(x0Min, Math.Min(x0Max, X0));
    }

    private float TimeToPixel(DateTime time, DateTime now)
    {
      return (float)Math.Round(X0 + (time - now).TotalMinutes * PixelsPerMinute);
    }

    DateTime TruncateTime(DateTime time, TimeSpan span)
    {
      return new DateTime((time.Ticks / span.Ticks) * span.Ticks, time.Kind);
    }




    //----------------------------------------------------------------------------------------------
    //                                        mouse
    //----------------------------------------------------------------------------------------------
    private void SatelliteTimelineControl_MouseWheel(object? sender, MouseEventArgs e)
    {
      int dZoom = e.Delta / 120;// WHEEL_DELTA;

      Zoom *= Math.Pow(2, dZoom / 4d);
      Invalidate();
    }

    private void SatelliteTimelineControl_MouseDown(object sender, MouseEventArgs e)
    {
      MouseDownX = e.X;
      MouseDownX0 = X0;
      Dragging = true;
      Cursor = Cursors.NoMoveHoriz;
    }

    private void SatelliteTimelineControl_MouseMove(object sender, MouseEventArgs e)
    {
      if (e.X == MouseMoveX) return;
      MouseMoveX = e.X;

      if (Dragging)
      {
        X0 = MouseDownX0 + MouseMoveX - MouseDownX;
        Invalidate();
      }
    }

    private void SatelliteTimelineControl_MouseUp(object sender, MouseEventArgs e)
    {
      Dragging = false;
      Cursor = Cursors.Default;
    }

    private void TimelinePanel_Resize(object sender, EventArgs e)
    {
      Invalidate();
    }
  }
}
