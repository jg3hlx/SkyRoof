using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbiNom
{
  public partial class SatelliteSelector : UserControl
  {
    private bool changing;

    public Context ctx;

    public event EventHandler? SelectedGroupChanged;
    public event EventHandler? SelectedSatelliteChanged;
    public event EventHandler? SelectedTransmitterChanged;

    public SatelliteSelector()
    {
      InitializeComponent();
    }

    public void SetSatelliteGroups()
    {
      changing = true;
      var sett = ctx.Settings.Satellites;

      GroupComboBox.Items.Clear();
      GroupComboBox.Items.AddRange(sett.SatelliteGroups.ToArray());
      var group = sett.SatelliteGroups.First(g => g.Id == sett.SelectedGroup);
      GroupComboBox.SelectedItem = group;

      SetSatellites();

      changing = false;
    }

    private void SetSatellites()
    {
      var sett = ctx.Settings.Satellites;
      var group = (SatelliteGroup)GroupComboBox.SelectedItem!;

      changing = true;
      SatelliteComboBox.Items.Clear();
      SatelliteComboBox.Items.AddRange(group.SatelliteIds.Select(id => ctx.SatnogsDb.GetSatellite(id)!).ToArray());
      SetSelectedSatellite();
    }

    internal void SetSelectedSatellite()
    {
      var sett = ctx.Settings.Satellites;
      var group = (SatelliteGroup)GroupComboBox.SelectedItem!;
      var sat = ctx.SatnogsDb.GetSatellite(group.SelectedSatId);

      changing = true;
      SatelliteComboBox.SelectedItem = sat;
      toolTip1.SetToolTip(SatelliteComboBox, sat.GetTooltipText());
      changing = false;

      SetTransmitters();
    }

    private void SetTransmitters()
    {
      var sett = ctx.Settings.Satellites;
      var sat = (SatnogsDbSatellite)SatelliteComboBox.SelectedItem;

      changing = true;
      TransmitterComboBox.Items.Clear();
      TransmitterComboBox.Items.AddRange(sat.Transmitters.ToArray());
      changing = false;

      SetSelectedTransmitter();
    }

    private void SetSelectedTransmitter()
    {
      var sett = ctx.Settings.Satellites;
      var sat = (SatnogsDbSatellite)SatelliteComboBox.SelectedItem;

      sett.SatelliteCustomizations.TryAdd(sat.sat_id, new SatelliteCustomization { sat_id = sat.sat_id });
      var cust = sett.SatelliteCustomizations[sat.sat_id];
      var tx = sat.Transmitters.FirstOrDefault(t => t.uuid == cust.SelectedTransmitter);

      if (tx == null)
      {
        tx = sat.Transmitters[0];
        cust.SelectedTransmitter = tx.uuid;
      }

      changing = true;
      TransmitterComboBox.SelectedItem = tx;
      toolTip1.SetToolTip(TransmitterComboBox, tx.GetTooltipText());
      changing = false;

      OnSelectedTransmitterChanged();
    }





    //----------------------------------------------------------------------------------------------
    //                                   change events
    //----------------------------------------------------------------------------------------------
    private void GroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (changing) return;

      var group = (SatelliteGroup)GroupComboBox.SelectedItem!;
      ctx.Settings.Satellites.SelectedGroup = group.Id;
      SetSatellites();

      OnSelectedGroupChanged();
    }


    private void SatelliteComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (changing) return;

      var group = (SatelliteGroup)GroupComboBox.SelectedItem!;
      var sat = (SatnogsDbSatellite)SatelliteComboBox.SelectedItem!;
      group.SelectedSatId = sat.sat_id;
      toolTip1.SetToolTip(SatelliteComboBox, sat.GetTooltipText());
      SetTransmitters();

      OnSelectedSatelliteChanged();
    }

    private void TransmitterComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      var sett = ctx.Settings.Satellites;
      var sat = (SatnogsDbSatellite)SatelliteComboBox.SelectedItem;

      sett.SatelliteCustomizations.TryAdd(sat.sat_id, new SatelliteCustomization { sat_id = sat.sat_id });
      var cust = sett.SatelliteCustomizations[sat.sat_id];
      var tx = ((SatnogsDbTransmitter)TransmitterComboBox.SelectedItem!);

      cust.SelectedTransmitter = tx.uuid;
      toolTip1.SetToolTip(TransmitterComboBox, tx.GetTooltipText());
    }

    public void OnSelectedGroupChanged()
    {
      SelectedGroupChanged?.Invoke(this, EventArgs.Empty);
      SelectedSatelliteChanged?.Invoke(this, EventArgs.Empty);
      SelectedTransmitterChanged?.Invoke(this, EventArgs.Empty);
    }

    public void OnSelectedSatelliteChanged()
    {
      SelectedSatelliteChanged?.Invoke(this, EventArgs.Empty);
      SelectedTransmitterChanged?.Invoke(this, EventArgs.Empty);
    }

    public void OnSelectedTransmitterChanged()
    {
      SelectedTransmitterChanged?.Invoke(this, EventArgs.Empty);
    }




    //----------------------------------------------------------------------------------------------
    //                                   draw events
    //----------------------------------------------------------------------------------------------
    private void SatelliteComboBox_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index < 0) { e.DrawBackground(); return; }

      Color backColor = Color.White;
      Color foreColor = e.ForeColor;
      Font font = e.Font;

      var sat = (SatnogsDbSatellite)SatelliteComboBox.Items[e.Index]!;

      if (sat.Flags.HasFlag(SatelliteFlags.Uhf)) backColor = Color.LightCyan;
      else if (sat.Flags.HasFlag(SatelliteFlags.Vhf)) backColor = Color.LightGoldenrodYellow;

      if (sat.Flags.HasFlag(SatelliteFlags.Ham)) font = new(font, FontStyle.Bold);
      if (!sat.status.StartsWith("alive")) foreColor = Color.Silver;
      else if (sat.Tle == null) font = new(font, FontStyle.Strikeout);

      e.Graphics.FillRectangle(new SolidBrush(backColor), e.Bounds);
      e.Graphics.DrawString(sat.name, font, Brushes.Black, e.Bounds);
      e.DrawFocusRectangle();

      //  if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
      //    toolTip1.SetToolTip(SatelliteComboBox, sat.GetTooltipText());
    }

    private void TransmitterComboBox_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index < 0) { e.DrawBackground(); return; }

      Color backColor = Color.White;
      Color foreColor = e.ForeColor;
      Font font = e.Font;

      var tx = (SatnogsDbTransmitter)TransmitterComboBox.Items[e.Index];

      if (tx.downlink_low >= 144000000 && tx.downlink_low <= 148000000) backColor = Color.LightGoldenrodYellow;
      if (tx.downlink_low >= 430000000 && tx.downlink_low <= 440000000) backColor = Color.LightCyan;
      if (tx.service == "Amateur") font = new(font, FontStyle.Bold);
      if (!tx.alive || tx.status != "active") foreColor = Color.Silver;
      
      e.Graphics.FillRectangle(new SolidBrush(backColor), e.Bounds);
      e.Graphics.DrawString(tx.description, font, Brushes.Black, e.Bounds);
      e.DrawFocusRectangle();
    }
  }
}
