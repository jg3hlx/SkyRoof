using WeifenLuo.WinFormsUI.Docking;

namespace OrbiNom
{
  public partial class WaterfallPanel : DockContent
  {
    public Context ctx;
    private double SdrCenterFrequency => ctx.Sdr?.Info?.Frequency ?? SdrConst.UHF_CENTER_FREQUENCY;
    private double MaxBandwidth => ctx.Sdr?.Info?.MaxBandwidth ?? SdrConst.MAX_BANDWIDTH;
    private double SamplingRate => ctx.Sdr?.Info?.SampleRate ?? SdrConst.MAX_BANDWIDTH;

    public WaterfallPanel()
    {
      InitializeComponent();
    }

    public WaterfallPanel(Context ctx)
    {
      InitializeComponent();
      this.ctx = ctx;

      ctx.WaterfallPanel = this;
      ctx.MainForm.WaterfallMNU.Checked = true;

      SplitContainer.SplitterDistance = ctx.Settings.Waterfall.SplitterDistance;
      ApplySettings();
      ctx.MainForm.ConfigureWaterfall();

      ScaleControl.ctx = ctx;
      ScaleControl.BuildLabels();
      ScaleControl.MouseMove += ScaleControl_MouseMove;
      ScaleControl.MouseDown += ScaleControl_MouseDown;
      ScaleControl.MouseUp += ScaleControl_MouseUp; ;
      ScaleControl.MouseLeave += ScaleControl_MouseLeave;
      ScaleControl.MouseWheel += ScaleControl_MouseWheel;

      WaterfallControl.OpenglControl.MouseMove += WaterfallControl_MouseMove;
      WaterfallControl.OpenglControl.MouseDown += WaterfallControl_MouseDown;
      WaterfallControl.OpenglControl.MouseUp += WaterfallControl_MouseUp;
      WaterfallControl.OpenglControl.MouseWheel += WaterfallControl_MouseWheel;
    }

    private void WaterfallPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.WaterfallPanel = null;
      ctx.MainForm.WaterfallMNU.Checked = false;

      ctx.Settings.Waterfall.SplitterDistance = SplitContainer.SplitterDistance;
    }

    public void ApplySettings()
    {
      var sett = ctx.Settings.Waterfall;
      WaterfallControl.Brightness = sett.Brightness;
      WaterfallControl.Contrast = sett.Contrast;
      int paletteIndex = Math.Min(ctx.PaletteManager.Palettes.Count() - 1, sett.PaletteIndex);
      WaterfallControl.SetPalette(ctx.PaletteManager.Palettes[paletteIndex]);
      WaterfallControl.Refresh();

      ctx.MainForm.SetWaterfallSpeed();
    }

    internal void SetPassband()
    {
      ScaleControl.CenterFrequency = ctx.Sdr.Info.Frequency;
      ScaleControl.VisibleBandwidth = ctx.Sdr.Info.MaxBandwidth;

      WaterfallControl.Zoom = ctx.Sdr.Info.SampleRate / ctx.Sdr.Info.MaxBandwidth;
      WaterfallControl.Pan = 0;

      //bool maxBandwidthChanged = MaxBandwidth != maxBandwidth;
      //if (maxBandwidthChanged) WaterfallControl.Clear();
    }

    public void SetCenterFrequency(double frequency)
    {

      ScaleControl.CenterFrequency = frequency;
      ValidateWaterfallViewport();
      ScaleControl.Refresh();

      WaterfallControl.Pan = (ctx.Sdr.Info.Frequency - ScaleControl.CenterFrequency) / ScaleControl.VisibleBandwidth * 2;
      WaterfallControl.OpenglControl.Refresh();
    }

    private void SlidersBtn_Click(object sender, EventArgs e)
    {
      var dlg = new WaterfallSildersDlg(ctx);
      dlg.Location = WaterfallControl.PointToScreen(new Point(2, 2));
      dlg.Show();
    }

    private const double MinHzPerPixel = 20;
    private void ValidateWaterfallViewport()
    {
      double minBandwidth = ScaleControl.width * MinHzPerPixel;
      double visibleBandwidth = Math.Min(MaxBandwidth, Math.Max(minBandwidth, ScaleControl.VisibleBandwidth));

      double minFreq = SdrCenterFrequency - MaxBandwidth / 2 + visibleBandwidth / 2;
      double maxFreq = SdrCenterFrequency + MaxBandwidth / 2 - visibleBandwidth / 2;
      double centerFrequency = Math.Max(minFreq, Math.Min(maxFreq, ScaleControl.CenterFrequency));

      ScaleControl.VisibleBandwidth = visibleBandwidth;
      ScaleControl.CenterFrequency = centerFrequency;
      WaterfallControl.VisibleBandwidth = visibleBandwidth;
    }

    private void WaterfallControl_Resize(object sender, EventArgs e)
    {
      ScaleControl.HistoryRowCount = WaterfallControl.Height;
    }






    //----------------------------------------------------------------------------------------------
    //                                   waterfall mouse 
    //----------------------------------------------------------------------------------------------
    Point MouseMovePos; 
    int MouseDownX;
    double MouseDownFrequency;
    bool Dragging;

    private void WaterfallControl_MouseDown(object? sender, MouseEventArgs e)
    {
      MouseDownX = MouseMovePos.X = e.X;
      MouseDownFrequency = ScaleControl.CenterFrequency;
    }

    private void WaterfallControl_MouseMove(object? sender, MouseEventArgs e)
    {
      if (e.Location == MouseMovePos) return;
      MouseMovePos = e.Location;

      // dragging
      if (e.Button == MouseButtons.Left)
      {
        var dx = MouseMovePos.X - MouseDownX;
        if (!Dragging && Math.Abs(dx) > 2)
        {
          Dragging = true;
          WaterfallControl.Cursor = Cursors.NoMoveHoriz;
        }
          
        if (Dragging)
        {
          double frequency = MouseDownFrequency - dx * ScaleControl.VisibleBandwidth / ScaleControl.width;
          SetCenterFrequency(frequency);
        }
      }
      // moving over transponder
      else
      {
        var label = ScaleControl.GetTransponderUnderCursor(e.X);
        if (label == null)
          WaterfallControl.Cursor = Cursors.Cross;
        else
          WaterfallControl.Cursor = Cursors.PanSouth;
      }
    }

    private void WaterfallControl_MouseUp(object? sender, MouseEventArgs e)
    {
      if (!Dragging)
        HandleFrequencyClick(e.X, int.MaxValue);

      Dragging = false;
      WaterfallControl.Cursor = Cursors.Cross;
    }

    private void WaterfallControl_MouseWheel(object? sender, MouseEventArgs e)
    {
      double dx = e.X - ScaleControl.width / 2;
      double freq = ScaleControl.CenterFrequency + dx * ScaleControl.VisibleBandwidth / ScaleControl.width;

      ScaleControl.VisibleBandwidth = ScaleControl.VisibleBandwidth * Math.Pow(1.2, -e.Delta / 120);
      ValidateWaterfallViewport();

      ScaleControl.CenterFrequency = freq - dx * ScaleControl.VisibleBandwidth / ScaleControl.width;

      ValidateWaterfallViewport();

      WaterfallControl.Zoom = SamplingRate / ScaleControl.VisibleBandwidth;
      WaterfallControl.Pan = (SdrCenterFrequency - ScaleControl.CenterFrequency) / ScaleControl.VisibleBandwidth * 2;

      ScaleControl.Refresh();
      WaterfallControl.OpenglControl.Refresh();
    }




    //----------------------------------------------------------------------------------------------
    //                                    scale mouse 
    //----------------------------------------------------------------------------------------------
     private void ScaleControl_MouseMove(object? sender, MouseEventArgs e)
    {
      if (e.Location == MouseMovePos) return;
      MouseMovePos = e.Location;

      // dragging
      if (e.Button == MouseButtons.Left)
      {
        var dx = e.X - MouseDownX;
        if (!Dragging && Math.Abs(dx) > 2 && ScaleControl.IsMouseInFilter(MouseDownX))
        {
          Dragging = true;
          MouseDownFrequency = ctx.FrequencyControl.GetDraggableFrequency();
          ScaleControl.Cursor = Cursors.NoMoveHoriz;
        }

        if (Dragging)
        {
          // adjust manual correction, or transponder offset, or RIT
          double freq = MouseDownFrequency + dx * ScaleControl.VisibleBandwidth / ScaleControl.width;
          ctx.FrequencyControl.SetDraggableFrequency(freq);
          ScaleControl.Refresh();
        }

        return;
      }

      // satellite label hover
      var label = ScaleControl.GetLabelUnderCursor(e.Location);
      if (label == null) toolTip1.ToolTipTitle = null;

      if (label != null)
      {
        if (toolTip1.ToolTipTitle != label.Pass.Satellite.name)
        {
          var parts = label.Pass.GetTooltipText(true);
          string tooltip = $"{parts[0]}  ({parts[2]})\n{parts[4]}\n{parts[5]}\n{label.Tooltip}";

          Point location = new((int)label.Rect.Right + 1, (int)label.Rect.Top);
          toolTip1.ToolTipTitle = label.Pass.Satellite.name;
          toolTip1.Show(tooltip, ScaleControl, location);
          ScaleControl.Cursor = Cursors.Hand;
        }
        return;
      }

      // transponder span hover
      label = ScaleControl.GetTransponderUnderCursor(e.X);
      if (label != null)
      {
        ScaleControl.Cursor = Cursors.PanSouth;
        toolTip1.Hide(ScaleControl);
        return;
      }

      // no hover
      toolTip1.Hide(ScaleControl);
      toolTip1.ToolTipTitle = null;
      ScaleControl.Cursor = Cursors.Cross;
    }

    private void ScaleControl_MouseDown(object? sender, MouseEventArgs e)
    {
      MouseDownX = MouseMovePos.X = e.X;
      MouseDownFrequency = ScaleControl.CenterFrequency;
    }

    private void ScaleControl_MouseUp(object? sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left && !Dragging)
        HandleFrequencyClick(e.X, e.Y);

      Dragging = false;
      ScaleControl.Cursor = Cursors.Cross;
    }


    private void ScaleControl_MouseLeave(object? sender, EventArgs e)
    {
      toolTip1.Hide(ScaleControl);
    }

    private void ScaleControl_MouseWheel(object? sender, MouseEventArgs e)
    {
      var freq = ctx.FrequencyControl.CorrectedDownlinkFrequency;
      if (freq == null) return;
      var x = ScaleControl.FreqToPixel((double)freq);
      if (Math.Abs(x - e.X) > 200) return;
      ctx.FrequencyControl.IncrementFrequency(e.Delta > 0 ? 20 : -20);
      Refresh();
    }

    private void HandleFrequencyClick(int x, int y)
    {
      var label = ScaleControl.GetLabelUnderCursor(new(x, y));

      if (label != null)
      {
        ctx.SatelliteSelector.SetSelectedTransmitter(label.Transmitters.First());
        ctx.SatelliteSelector.SetSelectedPass(label.Pass);
        return;
      }


      label = ScaleControl.GetTransponderUnderCursor(x);

      // tune to offset in transponder passband
      if (label != null)
      {
        double offset = ScaleControl.PixelToNominalFreq(label.Pass, DateTime.UtcNow, x) - (double)label.Transponder!.downlink_low!;
        ctx.FrequencyControl.SetTransponderOffset(label.Transponder, offset);
      }

      // tune to terrestrial frequency
      else
        ctx.FrequencyControl.SetTerrestrialFrequency(ScaleControl.PixelToFreq(x));
    }


    internal void BringInView(double value)
    {
      if (!ScaleControl.IsFrequencyVisible(value)) SetCenterFrequency(value);
    }




    //----------------------------------------------------------------------------------------------
    //                                    popup menu
    //----------------------------------------------------------------------------------------------
    private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
      var label = ScaleControl.GetLabelUnderCursor(MouseMovePos);
      e.Cancel = label == null;
      if (e.Cancel) return;

      var sat = label.Pass.Satellite;

      SelectTransmitterMNU.Enabled = label!.Transmitters.Count > 1;
      if (SelectTransmitterMNU.Enabled)
      {
        SelectTransmitterMNU.DropDownItems.Clear();
        foreach (var tx in label.Transmitters)
        {
          var item = new ToolStripMenuItem(tx.description);
          item.Click += (s, e) => ctx.SatelliteSelector.SetSelectedTransmitter(tx);
          SelectTransmitterMNU.DropDownItems.Add(item);
        }
      }

      AddToGroupMNU.DropDownItems.Clear();
      foreach (var group in ctx.Settings.Satellites.SatelliteGroups)
      {
        var item = new ToolStripMenuItem(group.Name);
        item.Click += (s, e) => ctx.SatelliteSelector.AddToGroup(sat, group);
        item.Enabled = !group.SatelliteIds.Contains(sat.sat_id);
        AddToGroupMNU.DropDownItems.Add(item);
      }

      ReportToAmsatMNU.Enabled = sat.AmsatEntries.Any();
      ReportToAmsatMNU.Tag = sat;
    }

    private void ReportToAmsatMNU_Click(object sender, EventArgs e)
    {
      var item = (ToolStripMenuItem)sender;
      var sat = (SatnogsDbSatellite)item.Tag!;
      AmsatReportDialog.SendReport(ctx, sat);
    }
  }
}
