using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbiNom
{
  public partial class FrequencyScale : UserControl
  {
    private static readonly double[] TickMults = { 2, 2.5, 2 };
    
    public double CenterFrequency;
    internal double Bandwidth;
    internal int width;
    internal int height;

    public FrequencyScale()
    {
      InitializeComponent();
      DoubleBuffered = true;
    }

    private void FrequencyScale_Resize(object sender, EventArgs e)
    {
      width = ClientSize.Width;
      height = ClientSize.Height;
      Invalidate();
    }

    private void FrequencyScale_Paint(object sender, PaintEventArgs e)
    {
      var g = e.Graphics;
      using (var brush = new SolidBrush(BackColor)) g.FillRectangle(brush, ClientRectangle);
      DrawTicks(g);
    }

    private void DrawTicks(Graphics g)
    {
      double leftFreq = CenterFrequency - Bandwidth / 2;
      double pixPerHz = width / Bandwidth;

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

    private double FreqToPixel(double f)
    {
      double df = f - CenterFrequency;
      double dx = df * width / Bandwidth;
      return width / 2d + dx;
    }

  }
}
