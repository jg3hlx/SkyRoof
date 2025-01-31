using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VE3NEA
{
  public class TransparentPanel : Panel
  {
    private const int WS_EX_TRANSPARENT = 0x20;

    protected override CreateParams CreateParams
    {
      get
      {
        var cp = base.CreateParams;
        //cp.ExStyle |= WS_EX_TRANSPARENT;
        return cp;
      }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      using (var b = new SolidBrush(Color.FromArgb(50 * 255 / 100, BackColor)))
      {
        e.Graphics.FillRectangle(b, ClientRectangle);
      }

      base.OnPaint(e);
    }
  }
}
