using System.Data;
using VE3NEA;
using WeifenLuo.WinFormsUI.Docking;

// todo: open properties dlg on Space
// todo: add Description field to tx tooltip

namespace OrbiNom
{
  public partial class GroupViewPanel : DockContentEx
  {
    public class ItemData
    {
      public SatnogsDbSatellite Sat;
      public SatellitePass? Pass;
      public ItemData(SatnogsDbSatellite sat) { Sat = sat; }
    }

    private int SortColumn = 2;
    public ListViewItem[] Items;



    public GroupViewPanel()
    {
      InitializeComponent();
    }

    public GroupViewPanel(Context ctx) : base(ctx)
    {
      InitializeComponent();


      this.ctx.GroupViewPanel = this;
      this.ctx.MainForm.GroupViewMNU.Checked = true;
      LoadGroup();
    }



    private void GroupViewPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.GroupViewPanel = null;
      ctx.MainForm.GroupViewMNU.Checked = false;
    }

    public void LoadGroup()
    {
      // select group or default
      var sett = ctx.Settings.Satellites;
      var group = sett.SatelliteGroups.First(g => g.Id == sett.SelectedGroup);
      Items = group.SatelliteIds.Select(id => ItemFromSat(ctx.SatnogsDb.GetSatellite(id))).ToArray();
      listView1.VirtualListSize = Items.Length;
      listView1.Invalidate();

      ShowSelectedSat();
      GroupNameLabel.Text = $"Group:   {group.Name}";
    }

    private ListViewItem ItemFromSat(SatnogsDbSatellite sat)
    {
      var item = new ListViewItem([
        sat.name,
        sat.norad_cat_id?.ToString() ?? "",
        "",
        "",
        ]);

      item.Tag = new ItemData(sat);
      item.ToolTipText = sat.GetTooltipText();

      if (sat.Flags.HasFlag(SatelliteFlags.Uhf)) item.BackColor = Color.LightCyan;
      else if (sat.Flags.HasFlag(SatelliteFlags.Vhf)) item.BackColor = Color.LightGoldenrodYellow;

      if (sat.Flags.HasFlag(SatelliteFlags.Ham)) item.Font = new(item.Font, FontStyle.Bold);
      if (!sat.status.StartsWith("alive")) item.ForeColor = Color.Silver;
      else if (sat.Tle == null) item.Font = new(item.Font, FontStyle.Strikeout);

      return item;
    }

    public void ShowSelectedSat()
    {
      var selectedSat = ctx.SatelliteSelector.SelectedSatellite;

      foreach (var item in Items)
        item.ImageIndex = ((ItemData)item.Tag!).Sat == selectedSat ? 0 : -1;

      listView1.SelectedIndices.Clear();
      listView1.Invalidate();
    }

    public void UpdatePassTimes()
    {
      var now = DateTime.UtcNow;
      bool changed = false;

      foreach (var item in Items)
      {
        var data = item.Tag as ItemData;
        if (data.Pass == null || data.Pass.EndTime < now)
        {
          data.Pass = ctx.AllPasses.GetNextPass(data.Sat);
          changed = true;
        }

        if (data.Pass == null)
        {
          item.SubItems[2].Text = "n/a";
          item.SubItems[3].Text = "";
        }
        else if (data.Pass.StartTime < now)
        {
          item.SubItems[2].Text = "Now";
          item.SubItems[3].Text = $"{Math.Round(data.Pass.MaxElevation)}°";
        }
        else
        {
          item.SubItems[2].Text = $"{Utils.TimespanToString(data.Pass.StartTime - now)}";
          item.SubItems[3].Text = $"{Math.Round(data.Pass.MaxElevation)}°";
        }
      }

      if (changed) SortItems();
      listView1.Invalidate();
    }

    private void SortItems()
    {
      switch (SortColumn)
      {
        case 0: Items = Items.OrderBy(item => item.Text).ToArray(); break;
        case 1: Items = Items.OrderBy(item => ((ItemData)item.Tag!).Sat.norad_cat_id).ToArray(); break;
        case 2: Items = Items.OrderBy(item => ((ItemData)item.Tag!).Pass?.StartTime ?? DateTime.MaxValue).ToArray(); break;
        case 3: Items = Items.OrderBy(item => ((ItemData)item.Tag!).Pass?.MaxElevation).ToArray(); break;
      }

      listView1.Invalidate();
    }




    //----------------------------------------------------------------------------------------------
    //                                        events
    //----------------------------------------------------------------------------------------------
    private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
    {
      e.Item = Items[e.ItemIndex];
    }

    private void listView1_DoubleClick(object sender, EventArgs e)
    {
      var sat = ((ItemData)Items[listView1.SelectedIndices[0]].Tag!).Sat;
      ctx.SatelliteSelector.SetSelectedSatellite(sat);
      ShowSelectedSat();
    }

    private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      SortColumn = e.Column;
      SortItems();
    }

    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (MouseButtons == MouseButtons.Right) return;

      if (listView1.SelectedIndices.Count == 0) return;

      var data = (ItemData)Items[listView1.SelectedIndices[0]].Tag!;

      ctx.SatelliteSelector.SetClickedSatellite(data.Sat);
      ctx.PassesPanel?.ShowPasses();
      if (data.Pass != null) ctx.SatelliteSelector.SetSelectedPass(data.Pass);
    }

    private void listView1_MouseDown(object sender, MouseEventArgs e)
    {
      var item = listView1.GetItemAt(e.X, e.Y);
      ClickedSat = item == null ? null : ((ItemData)item.Tag!).Sat;
    }
  }
}