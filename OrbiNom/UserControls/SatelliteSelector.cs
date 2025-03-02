using System.ComponentModel;
using System.Data;
using VE3NEA;

namespace OrbiNom
{
  public partial class SatelliteSelector : UserControl
  {
    public Context ctx;
    private bool changing;
    private SatelliteGroup group;
    private SatnogsDbSatellite clickedSatellite;

    public event EventHandler? SelectedGroupChanged;
    public event EventHandler? SelectedSatelliteChanged;
    public event EventHandler? ClickedSatelliteChanged;
    public event EventHandler? SelectedTransmitterChanged;
    public event EventHandler? SelectedPassChanged;


    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SatnogsDbSatellite[] GroupSatellites { get; private set; } = [];

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SatnogsDbSatellite SelectedSatellite { get; private set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SatnogsDbSatellite ClickedSatellite { get => clickedSatellite; set => SetClickedSatellite(value); }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SatellitePass? SelectedPass { get; private set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SatnogsDbTransmitter SelectedTransmitter { get; private set; }


    public SatelliteSelector()
    {
      InitializeComponent();
    }

    public void SetSatelliteGroups()
    {
      var sett = ctx.Settings.Satellites;

      changing = true;
      GroupComboBox.Items.Clear();
      GroupComboBox.Items.AddRange(sett.SatelliteGroups.ToArray());
      changing = false;

      SetSelectedGroup();
    }

    public void SetSelectedGroup()
    {
      var sett = ctx.Settings.Satellites;
      group = sett.SatelliteGroups.First(g => g.Id == sett.SelectedGroup);
      GroupSatellites = group.SatelliteIds.Select(id => ctx.SatnogsDb.GetSatellite(id)!).ToArray();

      changing = true;
      GroupComboBox.SelectedItem = group;
      changing = false;

      SetSatellites();

      OnSelectedGroupChanged();
    }

    private void SetSatellites()
    {
      changing = true;
      SatelliteComboBox.Items.Clear();
      SatelliteComboBox.Items.AddRange(GroupSatellites);
      changing = false;

      SetSelectedSatellite();
    }

    public void SetSelectedSatellite(SatnogsDbSatellite satellite)
    {
      if (SelectedSatellite == satellite) return;

      if (group.SatelliteIds.Contains(satellite.sat_id))
      {
        group.SelectedSatId = satellite.sat_id;
        SetSelectedSatellite();
        OnSelectedSatelliteChanged();
      }
      else
        Console.Beep();
    }

    private void SetSelectedSatellite()
    {
      var sett = ctx.Settings.Satellites;
      SelectedSatellite = ctx.SatnogsDb.GetSatellite(group.SelectedSatId);

      changing = true;
      SatelliteComboBox.SelectedItem = SelectedSatellite;
      toolTip1.SetToolTip(SatelliteComboBox, SelectedSatellite.GetTooltipText());
      changing = false;

      SetTransmitters();
      //SetSelectedPass(null);
    }

    public void SetClickedSatellite(SatnogsDbSatellite value)
    {
      clickedSatellite = value;
      ClickedSatelliteChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetTransmitters()
    {
      var sett = ctx.Settings.Satellites;

      changing = true;
      TransmitterComboBox.Items.Clear();
      TransmitterComboBox.Items.AddRange(SelectedSatellite.Transmitters.ToArray());
      changing = false;

      SetSelectedTransmitter();
    }

    private void SetSelectedTransmitter()
    {
      var sett = ctx.Settings.Satellites;

      var cust = ctx.Settings.Satellites.SatelliteCustomizations.GetOrCreate(SelectedSatellite.sat_id);
      SelectedTransmitter = SelectedSatellite.Transmitters.FirstOrDefault(t => t.uuid == cust.SelectedTransmitter);

      if (SelectedTransmitter == null)
      {
        SelectedTransmitter = SelectedSatellite.Transmitters[0];
        cust.SelectedTransmitter = SelectedTransmitter.uuid;
      }


      changing = true;
      TransmitterComboBox.SelectedItem = SelectedTransmitter;
      toolTip1.SetToolTip(TransmitterComboBox, SelectedTransmitter.GetTooltipText());
      changing = false;
    }

    public void SetSelectedTransmitter(SatnogsDbTransmitter? tx)
    {
      if (tx == null) return;

      if (SelectedSatellite.Transmitters.Contains(tx))
      {
        SelectedTransmitter = tx;
        var cust = ctx.Settings.Satellites.SatelliteCustomizations.GetOrCreate(SelectedSatellite.sat_id);
        cust.SelectedTransmitter = tx.uuid;

        SetSelectedTransmitter();
        OnSelectedTransmitterChanged();
      }
      else
        Console.Beep();
    }

    public void SetSelectedPass(SatellitePass? pass)
    {
      SelectedPass = pass;
      SelectedPassChanged?.Invoke(this, EventArgs.Empty);
    }





    //----------------------------------------------------------------------------------------------
    //                   events fired when dropdown selection is manually changed
    //----------------------------------------------------------------------------------------------
    private void GroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (changing) return;

      var group = (SatelliteGroup)GroupComboBox.SelectedItem!;
      ctx.Settings.Satellites.SelectedGroup = group.Id;

      SetSelectedGroup();
    }

    private void SatelliteComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (changing) return;

      SelectedSatellite = (SatnogsDbSatellite)SatelliteComboBox.SelectedItem!;
      group.SelectedSatId = SelectedSatellite.sat_id;
      toolTip1.SetToolTip(SatelliteComboBox, SelectedSatellite.GetTooltipText());

      SetTransmitters();
      //SetSelectedPass(null);

      OnSelectedSatelliteChanged();
    }

    private void TransmitterComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (changing) return;

      SetSelectedTransmitter((SatnogsDbTransmitter)TransmitterComboBox.SelectedItem!);
    }




    //----------------------------------------------------------------------------------------------
    //                          notify other objects of changes
    //----------------------------------------------------------------------------------------------
    public void OnSelectedGroupChanged()
    {
      SelectedGroupChanged?.Invoke(this, EventArgs.Empty);
      SelectedSatelliteChanged?.Invoke(this, EventArgs.Empty);
      SelectedTransmitterChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OnSelectedSatelliteChanged()
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

      Brush bacBrush = Brushes.White;
      Brush foreBrush = Brushes.Black;

      Font font = e.Font;

      var tx = (SatnogsDbTransmitter)TransmitterComboBox.Items[e.Index];

      if (tx.IsUhf()) bacBrush = Brushes.LightCyan;
      else if (tx.IsVhf()) bacBrush = Brushes.LightGoldenrodYellow;

      if (tx.service == "Amateur") font = new(font, FontStyle.Bold);
      if (!tx.alive || tx.status != "active") foreBrush = Brushes.Silver;

      e.Graphics.FillRectangle(bacBrush, e.Bounds);
      e.Graphics.DrawString(tx.description, font, foreBrush, e.Bounds);
      e.DrawFocusRectangle();
    }

    private void GainSlider_ValueChanged(object sender, EventArgs e)
    {
      toolTip1.SetToolTip(GainSlider, $"RF Gain: {GainSlider.Value}");
    }

    private void TuneToBtn_Click(object sender, EventArgs e)
    {
      ctx.DownlinkFrequencyControl.SetTransmitter(SelectedTransmitter, SelectedSatellite);
    }
  }
}
