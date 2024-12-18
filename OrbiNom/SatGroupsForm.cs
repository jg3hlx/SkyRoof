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
using SGPdotNET.TLE;

namespace OrbiNom
{
  public partial class SatGroupsForm : Form
  {
    List<ListViewItem> AllItems = new();
    List<ListViewItem> FilteredItems;
    private string TextToSearch = "";
    private SatelliteFlags SearchFlags;
    private int SortColumn;
    private Context ctx;

    public SatGroupsForm()
    {
      InitializeComponent();
    }

    public void SetList(Context context)
    {
      ctx = context;
      DateTime updateTime = ctx.Settings.SatList.LastDownloadTime;
    
      foreach (var sat in ctx.SatnogsDb.Satellites) AllItems.Add(ItemFromSat(sat));
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

    private ListViewItem ItemFromSat(SatnogsDbSatellite sat)
    {
      var item = new ListViewItem([
        sat.name,
        sat.norad_cat_id.ToString(),
        $"{sat.launched:yyyy-mm-dd}",
        string.Join(", ", sat.Transmitters.Select(t => t.service).Where(s=>s != "Unknown").Distinct().Order()),
        ]);

      item.Tag = sat;

      if (sat.Tle != null)
      {
        float v;
        string s = sat.Tle.tle2.Substring(8, 8);
        if (float.TryParse(s, out v)) sat.Inclination = (int)v;
        s = sat.Tle.tle2.Substring(52, 10);
        if (float.TryParse(s, out v)) sat.Period = (int)(1440f / v);
      }

      // highlighting

      if (sat.Flags.HasFlag(SatelliteFlags.Uhf)) item.BackColor = Color.LightCyan;
      else if (sat.Flags.HasFlag(SatelliteFlags.Vhf)) item.BackColor = Color.LightGoldenrodYellow;

      if (sat.Flags.HasFlag(SatelliteFlags.Ham)) item.Font = new(item.Font, FontStyle.Bold);
      if (!sat.status.StartsWith("alive")) item.ForeColor = Color.Silver;
      else if (sat.Tle == null) item.Font = new(item.Font, FontStyle.Strikeout);

      // tooltip

      string names = string.Join(", ", sat.AllNames);
      string radio = "Transmitter";
      if (sat.Flags.HasFlag(SatelliteFlags.Transponder)) radio = "Transponder";
      else if (sat.Flags.HasFlag(SatelliteFlags.Transceiver)) radio = "Transceiver";

      item.ToolTipText = $"{names}\nstatus: {sat.status}\ncountries: {sat.countries}";
      if (sat.Tle != null) item.ToolTipText += $"\nTLE: available\nperiod: {sat.Period} min.\ninclination: {sat.Inclination}°";
      item.ToolTipText += $"\nradio: {radio}";
      if (!string.IsNullOrEmpty(sat.LotwName)) item.ToolTipText += "\nAccepted by LoTW";
      item.ToolTipText += $"\nupdated: {sat.updated:yyyy-mm-dd}";

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

    private void FilterChanged(object sender, EventArgs e)
    {
      TextToSearch = SatnogsDbSatellite.MakeSearchText(FilterTextbox.Text);
      ApplyFilter();
    }

    private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
    {
      e.Item = FilteredItems[e.ItemIndex];
    }

    private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      SortSatellites(e.Column);
    }

    private void button3_Click(object sender, EventArgs e)
    {
      FilterTextbox.Text = "";
    }

    private void RenameSatMNU_Click(object sender, EventArgs e)
    {
      var item = FilteredItems[listView1.SelectedIndices[0]];
      item.BeginEdit();
    }

    private void PropertiesSatMNU_Click(object sender, EventArgs e)
    {
      var item = FilteredItems[listView1.SelectedIndices[0]];
      var sat = (SatnogsDbSatellite)item.Tag;

      // compute sat location only when needed for display
      if (sat.Tle != null)
        try
        {
          var satellite = new Satellite(sat.Tle.tle0, sat.Tle.tle1, sat.Tle.tle2);
          sat.Footprint = (int)satellite.Predict().ToGeodetic().GetFootprint();
          sat.Elevation = (int)satellite.Predict().ToGeodetic().Altitude;
        }
        catch { }


      SatelliteDetailsDialog.ShowSatellite(sat, ParentForm);
    }

    private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      // new name
      if (e.Label == null) return;
      string newName = e.Label.Trim();
      e.CancelEdit = newName == "";
      if (e.CancelEdit) return;

      // to sat object
      var sat = (SatnogsDbSatellite)FilteredItems[listView1.SelectedIndices[0]].Tag;
      sat.name = newName;
      sat.BuildAllNames();

      // to customization storage
      var casts = ctx.Settings.Customization.SatelliteCustomizations;
      casts.TryAdd(sat.sat_id, new());
      casts[sat.sat_id].sat_id = sat.sat_id;
      casts[sat.sat_id].Name = newName;
    }
  }
}
