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
using SGPdotNET.Observation;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Util;
using Newtonsoft.Json.Linq;

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
    private GroundStation GroundStation;
    const double HalfPi = Math.PI / 2;

    private DateTime LastPredictionTime = DateTime.MinValue;
    private TimeSpan PredictionTimeSpan = TimeSpan.FromDays(2);
    private readonly Font BoldFont;
    private readonly Pen PathPen = new Pen(Brushes.Teal, 2);

    
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
      listViewEx1.SetTooltipDelay(1500);
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

    private void radioButton_CheckedChanged(object sender, EventArgs e)
    {
      var radioBtn = (RadioButton)sender;
      if (!radioBtn.Checked) return;

      if (CurrentSatBtn.Checked) PredictionTimeSpan = TimeSpan.FromDays(2);
      else if (GroupBtn.Checked) PredictionTimeSpan = TimeSpan.FromDays(2);
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

      var startTime = DateTime.UtcNow;
      var endTime = startTime + PredictionTimeSpan;

      Items = CreatePassItems(startTime, endTime).ToList(); 
      listViewEx1.VirtualListSize = Items.Count;
      listViewEx1.Invalidate();
    }

    private ListViewItem ItemForPass(SatellitePass pass)
    {
      var item = new ListViewItem();
      item.Tag = new ItemData(pass, MakePath(pass));
      item.ToolTipText = pass.Satellite.GetTooltipText();
      return item;
    }

    internal void UpdatePassTimes()
    {
      // delete finished passes
      int oldCount = Items.Count;
      for (int i=Items.Count-1; i>=0; i--)
        if (((ItemData)Items[i].Tag).Pass.EndTime < DateTime.UtcNow) Items.RemoveAt(i);
      listViewEx1.VirtualListSize = Items.Count;

      // update wait times
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
      var size = e.Graphics.MeasureString(text, BoldFont);
      var rect = new RectangleF(e.Bounds.X, e.Bounds.Y, size.Width, size.Height);
      var brush = Brushes.White;
      if (pass.Satellite.Flags.HasFlag(SatelliteFlags.Uhf)) brush = Brushes.LightCyan;
      else if (pass.Satellite.Flags.HasFlag(SatelliteFlags.Vhf)) brush = Brushes.LightGoldenrodYellow;
      e.Graphics.FillRectangle(brush, rect);
      e.Graphics.DrawString(text, BoldFont, Brushes.Black, rect);

      // start/end time
      text = $"{pass.StartTime.ToLocalTime():yyyy-MM-dd  HH:mm:ss}  to  {pass.EndTime.ToLocalTime():HH:mm:ss}";
      size = e.Graphics.MeasureString(text, listViewEx1.Font);
      rect = new RectangleF(e.Bounds.X, e.Bounds.Y + e.Bounds.Height - size.Height - 2, size.Width, size.Height);
      e.Graphics.DrawString(text, listViewEx1.Font, Brushes.Black, rect);

      // duration / elevation
      text = $"{Utils.TimespanToString(pass.EndTime - pass.StartTime, false)}   {pass.MaxElevation:F0}°";
      size = e.Graphics.MeasureString(text, listViewEx1.Font);
      rect = new RectangleF(e.Bounds.X + w - size.Width, e.Bounds.Y + e.Bounds.Height - size.Height - 2, size.Width, size.Height);
      e.Graphics.DrawString(text, listViewEx1.Font, Brushes.Black, rect);

      // wait time
      text = now ? "Now" : $"in {Utils.TimespanToString(pass.StartTime - DateTime.UtcNow)}";
      brush = now ? Brushes.Green : Brushes.Black;
      size = e.Graphics.MeasureString(text, BoldFont);
      rect = new RectangleF(e.Bounds.X + w - size.Width, e.Bounds.Y, size.Width, size.Height);
      e.Graphics.DrawString(text, BoldFont, brush, rect);


      // mini sky view

      // background
      rect = new RectangleF(e.Bounds.X + w, e.Bounds.Y, h, h);
      //e.Graphics.FillRectangle(Brushes.Aquamarine, rect);      
      rect.Inflate(-5, -5);
      //e.Graphics.FillEllipse(Brushes.White, rect);

      var radius = rect.Width / 2;
      var center = new PointF(rect.Left + radius, rect.Top + radius);

      // grid
      e.Graphics.DrawEllipse(Pens.Silver, rect);
      e.Graphics.DrawLine(Pens.Silver, rect.Left, center.Y, rect.Right, center.Y);
      e.Graphics.DrawLine(Pens.Silver, center.X, rect.Top, center.X, rect.Bottom);
      rect.Inflate(-radius / 2, -radius / 2);
      e.Graphics.DrawEllipse(Pens.Silver, rect);

      var path = ((ItemData)e.Item.Tag).path;
      if (path.Length > 1)
      {
        // path
        var points = path.Select(p => 
          new PointF(center.X + p.X * radius, center.Y - p.Y * radius)).ToArray();
        e.Graphics.DrawLines(PathPen, points);

        // end points
        e.Graphics.FillEllipse(Brushes.Green, points.First().X - 3, points.First().Y - 3, 6, 6);
        e.Graphics.FillEllipse(Brushes.Red, points.Last().X - 3, points.Last().Y - 3, 6, 6);
      }

      // item separator
      rect = new RectangleF(e.Bounds.X, e.Bounds.Y + h, e.Bounds.Width, 1);
      e.Graphics.FillRectangle(Brushes.Gray, rect);
    }

    private const int StepCount = 10;
    private PointF[] MakePath(SatellitePass pass)
    {
      var duration = pass.EndTime - pass.StartTime;
      var step = duration / StepCount;
      var points = new PointF[StepCount + 1];

      for (int i = 0; i <= StepCount; i++)
      {
        var utc = pass.StartTime + step * i;
        var observation = GroundStation.Observe(pass.SatTracker, utc);

        double ro = 1 - observation.Elevation.Radians / HalfPi;
        double phi = HalfPi - observation.Azimuth.Radians;

        points[i] = new PointF((float)(ro * Math.Cos(phi)), (float)(ro * Math.Sin(phi)));
      }

        return points;
    }

    // every 5 minutes compute more passes
    internal void PredictMorePasses()
    {
      var startTime = LastPredictionTime + PredictionTimeSpan;
      var endTime = DateTime.UtcNow + PredictionTimeSpan;
      var items = CreatePassItems(startTime, endTime).Where(item => ((ItemData)item.Tag).Pass.StartTime > startTime);

      Items.AddRange(items);
      listViewEx1.VirtualListSize = Items.Count;
      listViewEx1.Invalidate();
    }

    public IEnumerable<ListViewItem> CreatePassItems(DateTime startTime, DateTime endTime)
    {
      // ground station for pass predictions
      var pos = GridSquare.ToGeoPoint(ctx.Settings.User.Square);
      var myLocation = new GeodeticCoordinate(Angle.FromRadians(pos.LatitudeRad), Angle.FromRadians(pos.LongitudeRad), 0);
      GroundStation = new GroundStation(myLocation);

      // predict passes and wrap them in listview items
      var items = Sats
        .SelectMany(sat => ctx.Passes.ComputeFor(sat, startTime, endTime))
        .OrderBy(p => p.StartTime)
        .Select(ItemForPass);        

      LastPredictionTime = DateTime.UtcNow;

      return items;
    }
  }
}
