using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using VE3NEA;

namespace OrbiNom
{
  public partial class PassesPanel : DockContent
  {
    public class ItemData
    {
      public SatellitePass Pass;
      public PointF[] path;
      public ItemData(SatellitePass pass, PointF[] path) { Pass = pass; this.path = path; }
    }



    private readonly Context ctx;
    private List<SatnogsDbSatellite> Sats = new();
    private List<ListViewItem> Items = new();
    private TimeSpan PredictionTimeSpan = TimeSpan.FromDays(2);
    private readonly Font BoldFont;

    public PassesPanel()
    {
      InitializeComponent();
    }

    public PassesPanel(Context ctx)
    {
      InitializeComponent();

      this.ctx = ctx;
      ctx.PassesPanel = this;
      ctx.MainForm.SatellitePassesMNU.Checked = true;

      BoldFont = new Font(listViewEx1.Font, FontStyle.Bold);
      listViewEx1.SetRowHeight(45);
    }

    private void PassesPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.PassesPanel = null;
      ctx.MainForm.SatellitePassesMNU.Checked = false;
    }

    private void listViewEx1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
    {
      e.Item = Items[e.ItemIndex];
    }

    private void listViewEx1_Resize(object sender, EventArgs e)
    {
      listViewEx1.Columns[0].Width = listViewEx1.ClientSize.Width;
    }

    private void radioButton3_CheckedChanged(object sender, EventArgs e)
    {
      var radioBtn = (RadioButton)sender;
      if (!radioBtn.Checked) return;

      if (CurrentSatBtn.Checked) PredictionTimeSpan = TimeSpan.FromDays(2);
      else if (GroupBtn.Checked) PredictionTimeSpan = TimeSpan.FromDays(1);
      else PredictionTimeSpan = TimeSpan.FromMinutes(30);

      ShowPasses();
    }

    private IEnumerable<SatnogsDbSatellite> ListSats()
    {
      IEnumerable<SatnogsDbSatellite> sats;
      var group = ctx.Settings.Satellites.SatelliteGroups.FirstOrDefault(g => g.Id == ctx.Settings.Satellites.SelectedGroup);

      if (AllBtn.Checked) sats = ctx.SatnogsDb.Satellites.Where(sat =>
        sat.Flags.HasFlag(SatelliteFlags.Vhf) || sat.Flags.HasFlag(SatelliteFlags.Uhf));

      else if (GroupBtn.Checked) sats = group.SatelliteIds.Select(ctx.SatnogsDb.GetSatellite);

      else if (ctx.GroupViewPanel != null && ctx.GroupViewPanel.listView1.SelectedIndices.Count > 0)
      {
        int idx = ctx.GroupViewPanel.listView1.SelectedIndices[0];
        sats = [((GroupViewPanel.ItemData)ctx.GroupViewPanel.Items[idx].Tag).Sat];
      }

      else sats = [ctx.SatnogsDb.GetSatellite(group.SelectedSatId)];

      return sats.Where(sat => sat.Tle != null && sat.status == "alive");
    }


    internal void ShowPasses()
    {
      var sats = ListSats();
      if (Sats.SequenceEqual(sats)) return;
      Sats = sats.ToList();
      var now = DateTime.UtcNow;

      Items = Sats
        .SelectMany(sat => ctx.Passes.ComputeFor(sat, now, now + PredictionTimeSpan))
        .OrderBy(p => p.StartTime)
        .Select(ItemForPass)
        .ToList();

      listViewEx1.VirtualListSize = Items.Count;
      listViewEx1.Invalidate();
    }

    private ListViewItem ItemForPass(SatellitePass pass)
    {
      var item = new ListViewItem();
      item.Tag = new ItemData(pass, MakePath(pass));
      return item;
    }

    private PointF[] MakePath(SatellitePass pass)
    {
      return [];
    }

    internal void UpdatePassTimes()
    {
      listViewEx1.Invalidate();
    }


    private void listViewEx1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
    {
      e.DrawBackground();

      var pass = ((ItemData)e.Item.Tag).Pass;
      int h = e.Bounds.Height - 1;
      int w = e.Bounds.Width - h;
      bool now = pass.StartTime < DateTime.UtcNow;

      // sat name
      string text = pass.Satellite.name;
      e.Graphics.DrawString(text, BoldFont, Brushes.Black, e.Bounds);

      // start/end time
      text = $"{pass.StartTime:yyyy-MM-dd  hh:mm:ss}  to  {pass.EndTime:hhhh:mm:ss}";
      var size = e.Graphics.MeasureString(text, listViewEx1.Font);
      var rect = new RectangleF(e.Bounds.X, e.Bounds.Y + e.Bounds.Height - size.Height, size.Width, size.Height);
      e.Graphics.DrawString(text, listViewEx1.Font, Brushes.Black, rect);

      // duration / elevation
      text = $"{Utils.TimespanToString(pass.EndTime - pass.StartTime, false)}   {pass.MaxElevation:F0}°";
      size = e.Graphics.MeasureString(text, listViewEx1.Font);
      rect = new RectangleF(e.Bounds.X + w - size.Width, e.Bounds.Y + e.Bounds.Height - size.Height, size.Width, size.Height);
      e.Graphics.DrawString(text, listViewEx1.Font, Brushes.Black, rect);


      // wait time
      text = now ? "Now" : $"in {Utils.TimespanToString(pass.StartTime - DateTime.UtcNow)}";
      var brush = now ? Brushes.Green : Brushes.Black;
      size = e.Graphics.MeasureString(text, BoldFont);
      rect = new RectangleF(e.Bounds.X + w - size.Width, e.Bounds.Y, size.Width, size.Height);
      e.Graphics.DrawString(text, BoldFont, brush, rect);

      // mini sky view
      rect = new RectangleF(e.Bounds.X + w, e.Bounds.Y, h, h);
      e.Graphics.FillRectangle(Brushes.Azure, rect);
      var path = ((ItemData)e.Item.Tag).path;
      if (path.Length > 1) e.Graphics.DrawLines(Pens.Blue, path);


      // item separator
      rect = new RectangleF(e.Bounds.X, e.Bounds.Y + h, e.Bounds.Width, 1);
      e.Graphics.FillRectangle(Brushes.Silver, rect);
    }
  }
}
