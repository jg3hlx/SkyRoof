using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VE3NEA
{
    // https://stackoverflow.com/questions/2751686

    public class ListViewEx : ListView
    {
        private const int WM_ERASEBKGND = 0x14;

        public ListViewEx()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        protected override void OnNotifyMessage(Message m)
        {
            if (m.Msg != WM_ERASEBKGND) base.OnNotifyMessage(m);
        }
    }
}
