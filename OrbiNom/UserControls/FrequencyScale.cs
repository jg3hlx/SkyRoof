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
using SharpGL.SceneGraph.Assets;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace OrbiNom
{
  public partial class FrequencyScale : UserControl
  {
    private static readonly double[] TickMults = { 2, 2.5, 2 };

    public Context ctx;
    private List<TransmitterLabel> Labels = new();
    private readonly List<float> LastXPositions = new();
    private Brush BlueSpanBrush = new SolidBrush(Color.FromArgb(20, Color.Blue));
    private Brush GraySpanBrush = new SolidBrush(Color.FromArgb(20, Color.Gray));
    private Brush BgBrush;
    private readonly Font BoldFont;

    public double CenterFrequency = SdrConst.UHF_CENTER_FREQUENCY;
    internal double VisibleBandwidth = SdrConst.MAX_BANDWIDTH;
    internal int width;
    internal int height;

    public FrequencyScale()
    {
      InitializeComponent();
      DoubleBuffered = true;

      BgBrush = new SolidBrush(BackColor);
      BoldFont = new Font(Font, FontStyle.Bold);
    }

    private void FrequencyScale_Resize(object sender, EventArgs e)
    {
      width = ClientSize.Width;
      height = ClientSize.Height;
      Invalidate();
    }

    private double FreqToPixel(double f)
    {
      double df = f - CenterFrequency;
      double dx = df * width / VisibleBandwidth;
      return width / 2d + dx;
    }

    private double DopplerFreqToPixel(SatellitePass pass, DateTime time, long freq)
    {
      var point = pass.GetTrackPointAt(time);
      var f = freq * (1 - point.Observation.RangeRate / 3e5);
      return FreqToPixel(f);
    }




    //----------------------------------------------------------------------------------------------
    //                                        paint
    //----------------------------------------------------------------------------------------------
    private void FrequencyScale_Paint(object sender, PaintEventArgs e)
    {
      var g = e.Graphics;
      g.FillRectangle(BgBrush, ClientRectangle);
      DrawTicks(g);
      DrawTransmitters(g);
    }

    private void DrawTicks(Graphics g)
    {
      double leftFreq = CenterFrequency - VisibleBandwidth / 2;
      double pixPerHz = width / VisibleBandwidth;

      //select tick step
      double TickStep = 200;
      double LabelStep = 1000;
      for (int i = 0; i <= 24; ++i)
      {
        if (LabelStep * pixPerHz > 60) break;
        LabelStep *= TickMults[i % 3];
        TickStep *= TickMults[(i + 1) % 3];
      }

      //first label's frequency
      double leftmostLabelFrequency = Math.Round(Math.Truncate(leftFreq / LabelStep) * LabelStep);
      double freq = leftmostLabelFrequency;

      //draw lagre ticks and labels
      while (true)
      {
        float x = (float)FreqToPixel(freq);
        if (x > width) break;
        float y = height - 30;
        g.DrawLine(Pens.Black, x, height - 12, x, height);

        string freqText = (freq * 1e-6).ToString("F3");
        x -= g.MeasureString(freqText, Font).Width / 2;
        g.DrawString(freqText, Font, Brushes.Black, x, y);
        freq += LabelStep;
      }

      //draw small ticks 
      freq = leftmostLabelFrequency;
      while (true)
      {
        float x = (float)FreqToPixel(freq);
        if (x > width) break;
        g.DrawLine(Pens.Black, x, height - 6, x, height);
        freq += TickStep;
      }
    }

    private void DrawTransmitters(Graphics g)
    {
      if (Labels.Count == 0) return;
      var now = DateTime.UtcNow;

      // recompute label's X
      foreach (var label in Labels)
        label.x = (float)DopplerFreqToPixel(label.Pass, now, label.Frequency);

      foreach (var label in Labels.Where(lb => lb.Span != null))
        DrawSpan(label, g);

      // draw labels
      Labels = Labels.OrderByDescending(l => l.x).ToList();
      LastXPositions.Clear();
      foreach (var label in Labels) DrawLabel(g, label, now);

      foreach (var label in Labels)
        if (label.Pass.EndTime < now && label.Pass.EndTime > now.AddMinutes(-30))
          DrawTriangle(label, g, Brushes.Silver);

      // future
      foreach (var label in Labels)
        if (label.Pass.StartTime > now && label.Pass.StartTime < now.AddMinutes(5))
          DrawTriangle(label, g, Brushes.White);

      // current
      foreach (var label in Labels)
        if (label.Pass.StartTime <= now && label.Pass.EndTime >= now)
          DrawTriangle(label, g, Brushes.Lime);
    }

    private void DrawLabel(Graphics g, TransmitterLabel label, DateTime now)
    {
      if (label.x < 0 || label.x > width) return;

      bool inGroup = ctx.SatelliteSelector.GroupSatellites.Contains(label.Pass.Satellite);
      var font = inGroup ? BoldFont : Font;
      var size = TextRenderer.MeasureText(label.Pass.Satellite.name, font, Size, TextFormatFlags.NoPadding);

      // find the lowest row with enough space for the label
      int row = 0;
      while (true)
      {
        if (LastXPositions.Count <= row) LastXPositions.Add(int.MaxValue);
        if ((label.x + size.Width) < LastXPositions[row]) break;
        row++;
      }
      LastXPositions[row] = label.x;

      // rect from x, row and size
      float LastY = height - 27 - row * (size.Height - 3);

      label.Rect = new RectangleF(label.x, LastY - size.Height, size.Width + 3, size.Height);

      // line
      g.DrawLine(Pens.Blue, label.x, height, label.x, LastY);

      // selected sat BG
      if (label.Pass.Satellite == ctx.SatelliteSelector.SelectedSatellite)
        g.FillRectangle(Brushes.Aqua, label.Rect);

      // sat name
      var brush = label.Pass.StartTime <= now && label.Pass.EndTime >= now ? Brushes.Blue : Brushes.Gray;
      g.DrawString(label.Pass.Satellite.name, font, brush, label.Rect.Location);
    }

    private void DrawSpan(TransmitterLabel label, Graphics g)
    {
      var x2 = (float)DopplerFreqToPixel(label.Pass, DateTime.UtcNow, label.Frequency + (long)label.Span!);
      if (label.x > width || x2 < 0) return;

      RectangleF r = new(label.x, height - 15, x2 - label.x, 14);

      bool isNow = label.Pass.StartTime <= DateTime.UtcNow && label.Pass.EndTime >= DateTime.UtcNow;
      g.FillRectangle(isNow ? BlueSpanBrush : GraySpanBrush, r);
      g.DrawRectangle(isNow ? Pens.Blue : Pens.Gray, r);
    }

    private void DrawTriangle(TransmitterLabel label, Graphics g, Brush brush)
    {
      if (label.Span != null) return;
      if (label.x < 0 || label.x > width) return;

      int y = height;

      PointF[] points = {
        new PointF(label.x, y),
        new PointF(label.x - 5, y-9),
        new PointF(label.x + 5, y-9),
      };

      g.FillPolygon(brush, points);
      g.DrawPolygon(Pens.Black, points);
    }




    //----------------------------------------------------------------------------------------------
    //                                        labels
    //----------------------------------------------------------------------------------------------
    public void BuildLabels()
    {
      var now = DateTime.UtcNow;
      Labels.Clear();

      foreach (var pass in ctx.AllPasses.Passes)
        if (pass.StartTime < now.AddMinutes(5) && pass.EndTime > now.AddMinutes(-25))
        {
          var transmitters = pass.Satellite.Transmitters.Where(tx => tx.alive && tx.downlink_low != null);
          var freqs = transmitters.Where(tx => tx.downlink_low.HasValue).Select(tx => (long)tx.downlink_low!).Distinct();

          foreach (var freq in freqs)
          {
            var label = new TransmitterLabel(pass, freq);
            Labels.Add(label);
          }
        }
    }

    internal TransmitterLabel? GetLabelUnderCursor(Point location)
    {
      return Labels.FirstOrDefault(label => label.Rect.Contains(location));
    }
  }
}
