namespace OrbiNom
{
  public partial class TimelineControl : UserControl
  {
    private const double MaxPixelsPerMinute = 60; // at max zoom, 1 pixel = 1 second
    private const int ScaleHeight = 50;

    private readonly TimeSpan MinTimeOffset = TimeSpan.FromMinutes(-30);
    private readonly TimeSpan MaxTimeOffset = TimeSpan.FromDays(2);
    private readonly Brush ShadowBrush = new SolidBrush(Color.FromArgb(25, Color.Black));

    List<SatellitePass> Passes = new();

    double Zoom = 1;
    private double PixelsPerMinute;
    double X0 = int.MaxValue;
    int width, height;
    private int MouseDownX;
    private double MouseDownX0;
    private int MouseMoveX;
    private bool Dragging;

    public TimelineControl()
    {
      InitializeComponent();
      MouseWheel += SatelliteTimelineControl_MouseWheel;
    }

    internal void SetList(List<SatellitePass> passes)
    {
      Passes = passes;
      Invalidate();
    }

    private void SatelliteTimelineControl_Resize(object sender, EventArgs e)
    {
      Invalidate();
    }




    //----------------------------------------------------------------------------------------------
    //                                          draw
    //----------------------------------------------------------------------------------------------
    private void SatelliteTimelineControl_Paint(object sender, PaintEventArgs e)
    {
      var g = e.Graphics;
      ValidateViewport();
      DrawBg(g);

      var now = DateTime.Now;
      DrawDateLabels(g, now);
      DrawtimeLabels(g, now);
      g.DrawString($"x0 = {X0:F1}   zoom = {Zoom:F3} pix/min = {PixelsPerMinute:F3}", Font, Brushes.Black, 5, 5);

      DrawPasses(g, now);
    }

    private void DrawBg(Graphics g)
    {
      using (var brush = new SolidBrush(BackColor)) g.FillRectangle(brush, ClientRectangle);
      var rect = new RectangleF(0, height - ScaleHeight, width, ScaleHeight);
      g.FillRectangle(Brushes.Silver, rect);
      rect = new RectangleF(0, 0, (int)X0, height - ScaleHeight);
      g.FillRectangle(ShadowBrush, rect);
    }

    private void DrawDateLabels(Graphics g, DateTime now)
    {
      var date1 = now;
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

    int[] Steps = [1, 2, 5, 10, 20, 30, 60, 120, 240, 360, 720];

    private void DrawtimeLabels(Graphics g, DateTime now)
    {
      TimeSpan step = TimeSpan.Zero;
      for (int i = 0; i < Steps.Count(); i++)
        if (Steps[i] * PixelsPerMinute > 59)
        {
          step = TimeSpan.FromMinutes(Steps[i]);
          break;
        }
      if (step == TimeSpan.Zero) return;

      var time = TruncateTime(now.Add(MinTimeOffset), step);
      var x = TimeToPixel(time, now);

      while (x < width)
      {
        g.DrawLine(Pens.Black, x, height - ScaleHeight, x, height - ScaleHeight + 10);
        string label = $"{time:HH:mm}";
        var size = TextRenderer.MeasureText(label, Font);
        g.DrawString(label, Font, Brushes.Black, x - size.Width / 2, height - ScaleHeight + 12);
        time = time.Add(step);
        x = TimeToPixel(time, now);
      }
    }

    private void DrawPasses(Graphics g, DateTime now)
    {
      now = now.ToUniversalTime();

      foreach (var pass in Passes)
      {
        var firstX = TimeToPixel(pass.Track.First().Utc, now);
        var lastX = TimeToPixel(pass.Track.Last().Utc, now);
        if (lastX < 0 || firstX > width) continue;

        float y0 = height - ScaleHeight - 1;
        float ys = (y0 - 10) / 90f;

        var x1 = firstX;
        var y1 = y0;
        foreach (var p in pass.Track)
        {
          var x2 = TimeToPixel(p.Utc, now);
          if (x2 == x1) continue;
          var y2 = y0 - (float)Math.Max(0, p.Observation.Elevation.Degrees) * ys;
          g.DrawLine(Pens.Black, x1, y1, x2, y2);
          x1 = x2;
          y1 = y2;
        }
      }
    }

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
      return (float)(X0 + (time - now).TotalMinutes * PixelsPerMinute);
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

    DateTime TruncateTime(DateTime dt, TimeSpan d)
    {
      return new DateTime(dt.Ticks / d.Ticks * d.Ticks, dt.Kind);
    }

  }
}
