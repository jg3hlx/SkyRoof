using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbiNom
{
  public partial class SatelliteDetailsControl : UserControl
  {
    private SatnogsDbSatellite? Satellite;

    public SatelliteDetailsControl()
    {
      InitializeComponent();
    }

    internal void ShowSatellite(SatnogsDbSatellite? satellite)
    {
      Satellite = satellite;

      SatNameLabel.Text = satellite.name;

      SatAkaLabel.Visible = satellite.AllNames.Count > 1;
      SatAkaLabel.Text = $"a.k.a. {string.Join(", ", satellite.AllNames.Where(s => s != satellite.name))}";

      ImageLabel.Visible = !string.IsNullOrEmpty(satellite.image);
      WebsiteLabel.Visible = !string.IsNullOrEmpty(satellite.website);

      SatellitePropertyGrid.SelectedObject = satellite;

      CreateTransmitterItems();
    }

    private void CreateTransmitterItems()
    {
      listView1.BeginUpdate();
      listView1.Groups.Add(new ListViewGroup("SatNOGS"));
      listView1.Groups.Add(new ListViewGroup("JE9PEL"));


      // satnogs transmitters
      foreach (var t in Satellite.Transmitters)
      {
        // columns
        var item = new ListViewItem([
          t.description,
          FormatRange(t.uplink_low, t.uplink_high),
          FormatRange(t.downlink_low, t.downlink_high),
        ]);
        item.Group = listView1.Groups[0];

        // highlighting
        if (t.downlink_low >= 144000000 && t.downlink_low <= 148000000) item.BackColor = Color.LightGoldenrodYellow;
        if (t.downlink_low >= 430000000 && t.downlink_low <= 440000000) item.BackColor = Color.LightCyan;
        if (t.service == "Amateur") item.Font = new(item.Font, FontStyle.Bold);
        if (!t.alive || t.status != "active") item.ForeColor = Color.Silver; //item.Font = new(item.Font, FontStyle.Strikeout);
        
        // tooltip
        string mode = t.mode;
        if (t.baud != null && t.baud != 0) mode = $"{mode} {t.baud} Bd";
        item.ToolTipText = $"type: {t.type}\nmode: {mode}\nservice: {t.service}\nupdated: {t.updated:yyyy-mm-dd}";

        listView1.Items.Add(item);
      }

      // JE9PEL transmitters
      foreach (var t in Satellite.JE9PELtransmitters)
      {
        var item = new ListViewItem([t.Mode, t.Uplink, t.Downlink]);
        item.Group = listView1.Groups[1];
        if (t.Status != "active") item.ForeColor = Color.Silver;
        item.ToolTipText = $"Beacon: {t.Beacon}\nCall: {t.Call}\nstatus: {t.Status}";

        // band color
        var match = Regex.Match(t.Downlink, "^[0-9.]+");
        if (match.Success && float.TryParse(match.Groups[0].Value, out float freq))
          if (freq >= 144&& freq <= 148) item.BackColor = Color.LightGoldenrodYellow;
          else if (freq >= 430 && freq <= 440) item.BackColor = Color.LightCyan;

        listView1.Items.Add(item);
      }

      listView1.EndUpdate();
    }

    private string FormatRange(long? low, long? high)
    {
      if (low == null) return "";
      if (high == null) return $"{low / 1000:N0}";
      return $"{low / 1000:N0} - {high / 1000:N0}";
    }

    private void ImageLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      ImageLabel.LinkVisited = true;
      Process.Start(new ProcessStartInfo($"https://db-satnogs.freetls.fastly.net/media/{Satellite.image}") { UseShellExecute = true });
    }

    private void WebsiteLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      WebsiteLabel.LinkVisited = true;
      Process.Start(new ProcessStartInfo(Satellite.website) { UseShellExecute = true });
    }

    private void SatnogsLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      SatnogsLabel.LinkVisited = true;
      Process.Start(new ProcessStartInfo($"https://db.satnogs.org/satellite/{Satellite.sat_id}") { UseShellExecute = true });
    }

    private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      listView1.Sort();
    }
  }
}
