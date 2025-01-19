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

    private static readonly TimeSpan HistoryTimeSpan = TimeSpan.FromMinutes(-30);
    private static readonly TimeSpan PredictionTimeSpan = TimeSpan.FromDays(2);
    private static double TotalMinutes = PredictionTimeSpan.Subtract(HistoryTimeSpan).TotalMinutes;
    private static readonly Brush ShadowBrush = new SolidBrush(Color.FromArgb(25, Color.Black));

    private Context ctx;
    private double PixelsPerMinute;
    private int MouseDownX;
    private TimeSpan MouseDownLeftSpan;
    private int MouseMoveX;
    private bool Dragging;

    // Now minus time at leftmost pixel
    TimeSpan LeftSpan = TimeSpan.FromMinutes(5);
    double Zoom = 8;


    // for visual designer
    public TimelinePanel() { InitializeComponent(); }

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
      ValidateZoom();
      ValidateLeftSpan();
      var now = DateTime.Now;

      DrawBg(e.Graphics, now);
      DrawDateLabels(e.Graphics, now);
      DrawtimeLabels(e.Graphics, now);
      DrawPasses(e.Graphics, now);

      //g.DrawString($"x0 = {X0:F1}   zoom = {Zoom:F3} pix/min = {PixelsPerMinute:F3}", Font, Brushes.Black, 5, 5);
    }

    private void DrawBg(Graphics g, DateTime now)
    {
      // chart
      var rect = new RectangleF(0, 0, ClientSize.Width, ClientSize.Height - ScaleHeight);
      LinearGradientBrush lgb = new LinearGradientBrush(rect, Color.SkyBlue, Color.White, LinearGradientMode.Vertical);
      g.FillRectangle(lgb, rect);

      // time scale
      rect = new RectangleF(0, ClientSize.Height - ScaleHeight, ClientSize.Width, ScaleHeight);
      g.FillRectangle(Brushes.Silver, rect);

      // past time shadow
      float x = TimeToPixel(now, now);
      rect = new RectangleF(0, 0, x, ClientSize.Height - ScaleHeight);
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
        x2 = Math.Min(ClientSize.Width, TimeToPixel(date2, now));
        g.DrawLine(Pens.Black, x2, ClientSize.Height - ScaleHeight, x2, ClientSize.Height);

        string label = $"{date1:MMM dd}";
        var size = TextRenderer.MeasureText(label, Font);
        if (x2 - x1 > size.Width + 15)
          g.DrawString(label, Font, Brushes.Black, (x1 + x2 - size.Width) / 2, ClientSize.Height - size.Height - 5);

        date1 = date2;
        x1 = Math.Max(-1, x2);
      }
      while (x2 < ClientSize.Width);
    }


    double[] Steps = [1, 2, 5, 10, 20, 30, 60, 120, 240, 360, 720];
    double[] SmallSteps = [1 / 6d, 1 / 2d, 1, 2, 5, 5, 15, 30, 60, 60, 120];

    private void DrawtimeLabels(Graphics g, DateTime now)
    {
      // compute steps
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

      // draw ticks
      var time = TruncateTime(now.Add(HistoryTimeSpan), smallStep);
      var x = TimeToPixel(time, now);
      while (x < ClientSize.Width)
      {
        g.DrawLine(Pens.Black, x, ClientSize.Height - ScaleHeight, x, ClientSize.Height - ScaleHeight + 5);
        time = time.Add(smallStep);
        x = TimeToPixel(time, now);
      }

      // draw labels
      time = TruncateTime(now.Add(HistoryTimeSpan), step);
      x = TimeToPixel(time, now);
      while (x < ClientSize.Width)
      {
        string label = $"{time:HH:mm}";
        var size = TextRenderer.MeasureText(label, Font);
        g.DrawString(label, Font, Brushes.Black, x - size.Width / 2, ClientSize.Height - ScaleHeight + 7);

        g.DrawLine(Pens.Black, x, ClientSize.Height - ScaleHeight, x, ClientSize.Height - ScaleHeight + 10);
        time = time.Add(step);
        x = TimeToPixel(time, now);
      }
    }

    private void DrawPasses(Graphics g, DateTime now)
    {
      if (ctx.GroupPasses == null) return;
      var passes = ctx.GroupPasses.Passes;

      now = now.ToUniversalTime();
      g.SmoothingMode = SmoothingMode.AntiAlias;
      var pen = new Pen(Brushes.Blue, 2);

      foreach (var pass in passes)
      {
        var firstX = TimeToPixel(pass.Track.First().Utc, now);
        var lastX = TimeToPixel(pass.Track.Last().Utc, now);
        if (lastX < 0 || firstX > ClientSize.Width) continue;

        float y0 = ClientSize.Height - ScaleHeight - 1;
        float ys = (y0 - 10) / 90f;

        var x1 = firstX;
        var y1 = y0;
        foreach (var p in pass.Track)
        {
          var x2 = TimeToPixel(p.Utc, now);
          if (x2 == x1) continue;
          var y2 = y0 - (float)Math.Max(0, p.Observation.Elevation.Degrees) * ys;
          g.DrawLine(pen, x1, y1, x2, y2);
          x1 = x2;
          y1 = y2;
        }
      }
    }



    //----------------------------------------------------------------------------------------------
    //                                    viewport
    //----------------------------------------------------------------------------------------------
    private void ValidateZoom()
    {
      double minPixelsPerMinute = ClientRectangle.Width / TotalMinutes;
      double maxZoom = MaxPixelsPerMinute / minPixelsPerMinute;
      Zoom = Math.Max(1, Math.Min(maxZoom, Zoom));
      PixelsPerMinute = minPixelsPerMinute * Zoom;
    }
    private void ValidateLeftSpan()
      {
      var maxLeftOffset = TimeSpan.FromMinutes(TotalMinutes - ClientRectangle.Width / PixelsPerMinute);
      if (LeftSpan > -HistoryTimeSpan) LeftSpan = -HistoryTimeSpan;
      if (LeftSpan < -maxLeftOffset) LeftSpan = maxLeftOffset;
    }

    private float TimeToPixel(DateTime time, DateTime now)
    {
      return (float)((time - (now - LeftSpan)).TotalMinutes * PixelsPerMinute);
    }

    private DateTime PixelToTime(float x, DateTime now)
    {
      return now - LeftSpan + TimeSpan.FromMinutes(x / PixelsPerMinute);
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
      var now = DateTime.Now;
      var timeUnderCursor = PixelToTime(e.X, now);

      int dZoom = e.Delta / 120;// WHEEL_DELTA;
      Zoom *= Math.Pow(2, dZoom / 4d);
      ValidateZoom();

      LeftSpan -= timeUnderCursor - PixelToTime(e.X, now);

      Invalidate();
    }

    private void SatelliteTimelineControl_MouseDown(object sender, MouseEventArgs e)
    {
      MouseDownX = e.X;
      MouseDownLeftSpan = LeftSpan;
      Dragging = true;
      Cursor = Cursors.NoMoveHoriz;
    }

    private void SatelliteTimelineControl_MouseMove(object sender, MouseEventArgs e)
    {
      if (e.X == MouseMoveX) return;
      MouseMoveX = e.X;

      if (Dragging)
      {
        LeftSpan = MouseDownLeftSpan - TimeSpan.FromMinutes((MouseDownX - MouseMoveX) / PixelsPerMinute);
        Invalidate();
        Update();
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
