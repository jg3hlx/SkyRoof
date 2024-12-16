using System.Data;
using System.Data.Common;

namespace OrbiNom
{
  public partial class SatnogsSatListControl : UserControl
  {
    List<ListViewItem> AllItems = new();
    List<ListViewItem> FilteredItems;
    private string TextToSearch = "";
    private SatelliteFlags SearchFlags;
    private int SortColumn;

    public SatnogsSatListControl()
    {
      InitializeComponent();
    }

    public void SetList(IEnumerable<SatnogsDbSatellite> satelliteList, DateTime updateTime)
    {
      foreach (var sat in satelliteList) AllItems.Add(ItemFromSat(sat));
      ApplyFilter();
      UpdatedDateLabel.Text = $"Updated {updateTime:yyy-mm-dd}";
    }

    private void ApplyFilter()
    {
      FilteredItems = AllItems.Where(IsItemVisible).ToList();
      listView1.VirtualListSize = FilteredItems.Count;
      CountLabel.Text = $"{FilteredItems.Count} of {AllItems.Count}";
      SortSatellites(SortColumn);
    }

    private ListViewItem ItemFromSat(SatnogsDbSatellite sat)
    {
      var item = new ListViewItem(new string[] {
        sat.name,
        sat.norad_cat_id.ToString(),
        $"{sat.launched:yyyy-mm-dd}",
        string.Join(", ", sat.Transmitters.Select(t => t.service).Where(s=>s != "Unknown").Distinct().Order()),
        });

      item.Tag = sat;
      item.Checked = sat.Tle != null;

      if (!sat.status.StartsWith("alive")) item.ForeColor = Color.Silver;
      else if (!string.IsNullOrEmpty(sat.LotwName)) item.Font = new(item.Font, FontStyle.Bold);

      string names = string.Join(", ", sat.AllNames);
      item.ToolTipText = $"{names}\nstatus: {sat.status}";
      if (sat.Tle != null) item.ToolTipText += "\nTLE available";
      if (!string.IsNullOrEmpty(sat.LotwName)) item.ToolTipText += "\nAccepted by LoTW";

      return item;
    }

    private bool IsItemVisible(ListViewItem item)
    {
      var sat = (SatnogsDbSatellite)item.Tag;

      bool statusOk =
        (AliveCheckbox.Checked && sat.Flags.HasFlag(SatelliteFlags.Alive)) ||
        (FutureCheckbox.Checked && sat.Flags.HasFlag(SatelliteFlags.Future)) ||
        (ReEnteredCheckbox.Checked && sat.Flags.HasFlag(SatelliteFlags.ReEntered));

      bool hamOk =
      (HamCheckbox.Checked && sat.Flags.HasFlag(SatelliteFlags.Ham)) ||
      (NonHamCheckbox.Checked && sat.Flags.HasFlag(SatelliteFlags.NonHam));

      bool bandOk =
        (VhfCheckbox.Checked && sat.Flags.HasFlag(SatelliteFlags.Vhf)) ||
        (UhfCheckbox.Checked && sat.Flags.HasFlag(SatelliteFlags.Uhf)) ||
        (OtherBandsCheckbox.Checked && sat.Flags.HasFlag(SatelliteFlags.OtherBands));


      bool txOk =
      (TransponderCheckbox.Checked && sat.Flags.HasFlag(SatelliteFlags.Transponder)) ||
      (TransceiverCheckbox.Checked && sat.Flags.HasFlag(SatelliteFlags.Transceiver)) ||
      (TransmitterCheckbox.Checked && sat.Flags.HasFlag(SatelliteFlags.Transmitter));


      bool filterOk = (TextToSearch == "") || (sat.SearchText.Contains(TextToSearch));

      return statusOk && hamOk && bandOk && txOk && filterOk;
    }

    private void Filter_Changed(object sender, EventArgs e)
    {
      TextToSearch = SatnogsDbSatellite.MakeSearchText(FilterTextbox.Text);
      ApplyFilter();
    }

    private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
    {
      e.Item = FilteredItems[e.ItemIndex];
    }

    private void listView1_ColumnClick(object sender, ColumnClickEventArgs e) => SortSatellites(e.Column);


    private void SortSatellites(int column)
    {
      SortColumn = column;

      switch (column)
      {
        case 0:
          FilteredItems = FilteredItems.OrderBy(s => ((SatnogsDbSatellite)s.Tag).name).ToList();
          break;
        case 1:
          FilteredItems = FilteredItems.OrderBy(s => ((SatnogsDbSatellite)s.Tag).norad_cat_id).ToList();
          break;
        case 2:
          FilteredItems = FilteredItems.OrderBy(s => ((SatnogsDbSatellite)s.Tag).launched).ToList();
          break;
        case 3:
          FilteredItems = FilteredItems.OrderBy(s => s.SubItems[3].Text).ToList();
          break;
      }

      listView1.Invalidate();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      FilterTextbox.Text = "";
    }

    private void SatelliteListPopupMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {

    }

    private void PropertiesSatMNU_Click(object sender, EventArgs e)
    {
      var item = FilteredItems[listView1.SelectedIndices[0]];
      var satellite = (SatnogsDbSatellite)item.Tag;
      Point location = listView1.GetItemRect(listView1.SelectedIndices[0]).Location;
      location.Offset(listView1.Columns[0].Width, 0);
      SatelliteDetailsDialog.ShowSatellite(satellite, ParentForm, location);
    }

    private void RenameSatMNU_Click(object sender, EventArgs e)
    {
      var item = FilteredItems[listView1.SelectedIndices[0]];
      item.BeginEdit();
    }

    private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      string newName = e.Label.Trim();
      e.CancelEdit = newName == "";
      if (e.CancelEdit) return;

      var sat = (SatnogsDbSatellite)FilteredItems[listView1.SelectedIndices[0]].Tag;
      sat.name = newName;
      sat.AllNames.Add(newName);
      sat.AllNames = sat.AllNames.Distinct().ToList();  
    }
  }
}
