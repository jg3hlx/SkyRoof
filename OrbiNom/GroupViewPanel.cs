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

namespace OrbiNom
{
  public partial class GroupViewPanel : DockContent
  {
    private Context ctx;
    private ListViewItem[] Items;

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
      var sett = ctx.Settings.SatelliteSettings;

      var group = sett.SatelliteGroups.First(g => g.Id == sett.SelectedGroup);
      Items = group.SatelliteIds.Select(id => ItemFromSat(ctx.SatnogsDb.GetSatellite(id))).ToArray();
      listView1.VirtualListSize = Items.Length;

      ShowSelectedSat();
      GroupNameLabel.Text = group.Name;
    }

    private ListViewItem ItemFromSat(SatnogsDbSatellite sat)
    {
      var item = new ListViewItem([
        sat.name,
        sat.norad_cat_id?.ToString() ?? "",
        "",
        ]);

      item.Tag = sat;
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
      var sett = ctx.Settings.SatelliteSettings;
      var group = sett.SatelliteGroups.First(g => g.Id == sett.SelectedGroup);

      foreach (var item in Items)
      {
        var sat = (SatnogsDbSatellite)item.Tag!;
        item.ImageIndex = sat.sat_id == group.SelectedSatId ? 1 : -1;
      }

      listView1.Invalidate();
    }

    private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
    {
      e.Item = Items[e.ItemIndex];
    }

    private void listView1_DoubleClick(object sender, EventArgs e)
    {
      var sett = ctx.Settings.SatelliteSettings;
      var group = sett.SatelliteGroups.First(g => g.Id == sett.SelectedGroup);
      group.SelectedSatId = ((SatnogsDbSatellite)Items[listView1.SelectedIndices[0]].Tag!).sat_id;
      ShowSelectedSat();

      ctx.SatelliteSelector.SetSelectedSatellite();
      ctx.SatelliteSelector.OnSelectedSatelliteChanged();
    }
  }
}