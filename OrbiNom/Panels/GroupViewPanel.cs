using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SGPdotNET.Observation;
using SGPdotNET.Propagation;
using VE3NEA;
using WeifenLuo.WinFormsUI.Docking;

// todo: open properties dlg on Space
// todo: add Description field to tx tooltip

namespace OrbiNom
{
  public partial class GroupViewPanel : DockContent
  {
    public class ItemData
    {
      public SatnogsDbSatellite Sat;
      public SatellitePass? Pass;
      public ItemData(SatnogsDbSatellite sat) { Sat = sat; }
    }

    private Context ctx;
    private int SortColumn = 2;
    public ListViewItem[] Items;

    public GroupViewPanel()
    {
      InitializeComponent();
    }

    public GroupViewPanel(Context ctx)
    {
      InitializeComponent();

      this.ctx = ctx;
      ctx.GroupViewPanel = this;
      ctx.MainForm.GroupViewMNU.Checked = true;
      listView1.SelectedIndexChanged += ListView1_SelectedIndexChanged;
      LoadGroup();
    }

    private void ListView1_SelectedIndexChanged(object? sender, EventArgs e)
    {
      if (((ListView)sender).SelectedIndices.Count == 0) return;

      ctx.SatelliteDetailsPanel?.LoadSatelliteDetails();
      ctx.PassesPanel?.ShowPasses();
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
      GroupNameLabel.Text = group.Name;
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
      var sett = ctx.Settings.Satellites;
      var group = sett.SatelliteGroups.First(g => g.Id == sett.SelectedGroup);

      foreach (var item in Items)
      {
        var sat = ((ItemData)item.Tag!).Sat;
        item.ImageIndex = sat.sat_id == group.SelectedSatId ? 0 : -1;
      }

      listView1.SelectedIndices.Clear();
      listView1.Invalidate();
    }

    private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
    {
      e.Item = Items[e.ItemIndex];
    }

    private void listView1_DoubleClick(object sender, EventArgs e)
    {
      var sett = ctx.Settings.Satellites;
      var group = sett.SatelliteGroups.First(g => g.Id == sett.SelectedGroup);
      group.SelectedSatId = ((ItemData)Items[listView1.SelectedIndices[0]].Tag!).Sat.sat_id;
      ShowSelectedSat();

      ctx.SatelliteSelector.SetSelectedSatellite();
      ctx.SatelliteSelector.OnSelectedSatelliteChanged();
    }

    public void UpdatePassTimes()
    {
      if (ctx.Passes == null) return;

      var now = DateTime.UtcNow;
      bool changed = false;

      foreach (var item in Items)
      {
        var data = item.Tag as ItemData;
        if (data.Pass == null || data.Pass.EndTime < now)
        {
          data.Pass = ctx.Passes.ComputeFor(data.Sat, now, now.AddDays(1)).OrderBy(pass => pass.StartTime).FirstOrDefault();
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

    private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      SortColumn = e.Column;
      SortItems();
    }

    private void SortItems()
    {
      switch(SortColumn)
      {
        case 0: Items = Items.OrderBy(item => item.Text).ToArray(); break;
        case 1: Items = Items.OrderBy(item => ((ItemData)item.Tag!).Sat.norad_cat_id).ToArray(); break;
        case 2: Items = Items.OrderBy(item => ((ItemData)item.Tag!).Pass?.StartTime ?? DateTime.MaxValue).ToArray(); break;
        case 3: Items = Items.OrderBy(item => ((ItemData)item.Tag!).Pass?.MaxElevation).ToArray(); break;
      }
      
      listView1.Invalidate();
    }
  }
}