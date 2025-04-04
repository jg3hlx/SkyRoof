using System.Diagnostics;
using MathNet.Numerics;
using Serilog;
using VE3NEA;
using WeifenLuo.WinFormsUI.Docking;
using CSCore.CoreAudioAPI;
using System.Speech.Synthesis;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Reflection;

namespace OrbiNom
{
  public partial class MainForm : Form
  {
    Context ctx = new();

    public MainForm()
    {
      InitializeComponent();

      Text = Utils.GetVersionString();
      ctx.MainForm = this;

      ctx.SatelliteSelector = SatelliteSelector;
      ctx.FrequencyControl = FrequencyControl;
      SatelliteSelector.ctx = ctx;
      FrequencyControl.ctx = ctx;
      GainControl.ctx = ctx;
      ctx.Announcer.ctx = ctx;
      ctx.CatControl.ctx = ctx;

      ctx.Settings.LoadFromFile();

      EnsureUserDetails();

      ctx.GroupPasses = new(ctx);
      ctx.HamPasses = new(ctx);
      ctx.SdrPasses = new(ctx);

      timer.Interval = 1000 / TICKS_PER_SECOND;

      ctx.SpeakerSoundcard.StateChanged += Soundcard_StateChanged;
      ctx.AudioVacSoundcard.StateChanged += Soundcard_StateChanged;
      ctx.IqVacSoundcard.StateChanged += Soundcard_StateChanged;

      ApplyAudioSettings();
      ctx.CatControl.ApplySettings();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      ReadOrDownloadSatelliteData();

      // apply settings
      ctx.Settings.Ui.RestoreWindowPosition(this);
      if (!ctx.Settings.Ui.RestoreDockingLayout(this)) SetDefaultDockingLayout();
      Clock.UtcMode = ctx.Settings.Ui.ClockUtcMode;

      SetupDsp();
      StartSdrIfEnabled();
    }

    private void MainForm_FormClosing(object sender, EventArgs e)
    {
      // save settings
      ctx.Settings.Ui.StoreDockingLayout(DockHost);
      ctx.Settings.Ui.StoreWindowPosition(this);
      ctx.Settings.Ui.ClockUtcMode = Clock.UtcMode;
      if (ctx.WaterfallPanel != null)
        ctx.Settings.Waterfall.SplitterDistance = ctx.WaterfallPanel.SplitContainer.SplitterDistance;
      ctx.Settings.SaveToFile();

      // dispose sdr and dsp
      ctx.Sdr?.Dispose();
      ctx.Slicer?.Dispose();
      ctx.SpeakerSoundcard.Dispose();
      ctx.AudioVacSoundcard?.Dispose();
      ctx.IqVacSoundcard?.Dispose();
      Fft<float>.SaveWisdom();
    }

    private void EnsureUserDetails()
    {
      if (UserDetailsDialog.UserDetailsAvailable(ctx)) return;

      var dialog = new UserDetailsDialog(ctx);
      var rc = dialog.ShowDialog();
      bool ok = rc == DialogResult.OK;

      if (ok)
        ctx.Settings.SaveToFile();
      else
        Environment.Exit(1); // unable to proceed without user details, terminate app
    }




    //----------------------------------------------------------------------------------------------
    //                                        sdr
    //----------------------------------------------------------------------------------------------
    internal WidebandSpectrumAnalyzer SpectrumAnalyzer;

    private void SetupDsp()
    {
      Fft<float>.LoadWisdom(Path.Combine(Utils.GetUserDataFolder(), "wsjtx_wisdom.dat"));

      SpectrumAnalyzer = new(WaterfallControl.SPECTRA_WIDTH, 6_000_000);
      SpectrumAnalyzer.SpectrumAvailable += Spect_SpectrumAvailable;


    }

    private void Spect_SpectrumAvailable(object? sender, DataEventArgs<float> e)
    {
      ctx.WaterfallPanel?.WaterfallControl?.AppendSpectrum(e.Data);
    }

    private void StartSdrIfEnabled()
    {
      var sdrProperties = ctx.Settings.Sdr.Devices.FirstOrDefault(d => d.Name == ctx.Settings.Sdr.SelectedDeviceName);
      ctx.Sdr = sdrProperties == null ? null : new(sdrProperties);

      if (ctx.Sdr != null)
      {
        ctx.Sdr.StateChanged += Sdr_StateChanged;
        ctx.Sdr.DataAvailable += Sdr_DataAvailable;

        GainControl.ApplyRfGain();
        ConfigureWaterfall();
        ConfigureSlicer();

        if (ctx.Settings.Sdr.Enabled) ctx.Sdr.Enabled = true;
      }

      UpdateSdrLabel();
    }

    private void StopSdr()
    {
      ctx.Sdr?.Dispose();
      ctx.Sdr = null;
      UpdateSdrLabel();
    }

    private void Sdr_StateChanged(object? sender, EventArgs e)
    {
      BeginInvoke(UpdateSdrLabel);
    }

    private void ToggleSdrEnabled()
    {
      StopSdr();
      ctx.Settings.Sdr.Enabled = !ctx.Settings.Sdr.Enabled;
      StartSdrIfEnabled();
    }

    private void Sdr_DataAvailable(object? sender, DataEventArgs<Complex32> e)
    {
      SpectrumAnalyzer?.StartProcessing(e);
      ctx.Slicer?.StartProcessing(e);
    }

    internal void ConfigureWaterfall()
    {
      if (ctx.WaterfallPanel == null || ctx.Sdr?.Info == null || SpectrumAnalyzer == null) 
        return;

      SetWaterfallSpeed();
      ctx.WaterfallPanel?.SetPassband();
    }

    private void ConfigureSlicer()
    {
      if (ctx.Slicer != null) ctx.Slicer?.Dispose();

      var rate = ctx.Sdr.Info.SampleRate;
      var mode = ctx.FrequencyControl.DownlinkMode;
      ctx.Slicer = new Slicer(rate, 0, mode);
      ctx.Slicer.AudioDataAvailable += Slicer_AudioDataAvailable;
      ctx.Slicer.IqDataAvailable += Slicer_IqDataAvailable;
    }

    private void Slicer_IqDataAvailable(object? sender, DataEventArgs<Complex32> e)
    {
      if (ctx.Settings.Audio.VacDataFormat == VacDataFormat.IQ)
        ctx.IqVacSoundcard.AddSamples(e.Data);
    }

    private void Slicer_AudioDataAvailable(object? sender, DataEventArgs<float> e)
    {
      ctx.SpeakerSoundcard.AddSamples(e.Data);

      if (ctx.Settings.Audio.VacDataFormat == VacDataFormat.Audio)
        ctx.AudioVacSoundcard.AddSamples(e.Data);
    }

    private void UpdateSdrLabel()
    {
      Color color;
      string tooltip;

      if (ctx.Sdr == null || !ctx.Sdr.Enabled)
      {
        color = Color.Gray;
        if (string.IsNullOrEmpty(ctx.Settings.Sdr.SelectedDeviceName))
          tooltip = "Not configured. Click to configure";
        else
          tooltip = $"{ctx.Settings.Sdr.SelectedDeviceName}   Disabled\nClick to enable";
      }
      else if (ctx.Sdr.IsRunning())
      {
        color = Color.Lime;
        tooltip = $"{ctx.Sdr.Info.Name}   Running\nClick to disable";
      }
      else
      {
        color = Color.Red;
        tooltip = $"{ctx.Settings.Sdr.SelectedDeviceName}   FAILED\nClick to disable";
      }

      SdrLedLabel.ForeColor = color;
      SdrLedLabel.ToolTipText = SdrStatusLabel.ToolTipText = tooltip;
    }

    private void EditSdrDevices()
    {
      StopSdr();

      var dlg = new SdrDevicesDialog(ctx);
      var rc = dlg.ShowDialog();

      if (rc == DialogResult.OK)
        ctx.WaterfallPanel?.WaterfallControl?.Invalidate();

      StartSdrIfEnabled();
    }


    internal void SetWaterfallSpeed()
    {
      bool change = ctx.Settings.Waterfall.Speed != ctx.WaterfallPanel.WaterfallControl.ScrollSpeed;

      if (ctx.Sdr != null)
        SpectrumAnalyzer.Spectrum.Step = ctx.Sdr.Info.SampleRate / ctx.Settings.Waterfall.Speed;

      if (ctx.WaterfallPanel?.WaterfallControl != null)
      {
        ctx.WaterfallPanel.WaterfallControl.ScrollSpeed = ctx.Settings.Waterfall.Speed;
        if (change) ctx.WaterfallPanel.ClearWaterfall();
      }
    }

    internal void SuggestSdrSettings(SoapySdrDeviceInfo info)
    {
      info.Frequency = 436_500_000;
      info.Gain = info.GainRange.maximum;


      if (info.Name.ToLower().Contains("airspy"))
      {
        info.SampleRate = 6_000_000;
        info.MaxBandwidth = 3_100_000;
      }
      else if (info.Name.ToLower().Contains("rtl"))
      {
        info.SampleRate = 2_500_000;
        info.MaxBandwidth = 1_900_000;
      }
      else if (info.Name.ToLower().Contains("sdrplay"))
      {
        //info.SampleRate = 8_000_000;
        //info.HardwareBandwidth = 5_000_000;
        //info.MaxBandwidth = 3_100_000;

        info.SampleRate = 4_000_000;
        info.HardwareBandwidth = 5_000_000;
        info.MaxBandwidth = 3_100_000;
      }
      else
      {
        info.SampleRate = 4_000_000;
        info.MaxBandwidth = 3_100_000;
      }

      info.ValidateRateAndBandwidth();
    }





    //----------------------------------------------------------------------------------------------
    //                                     soundcards
    //----------------------------------------------------------------------------------------------
    internal void ApplyAudioSettings()
    {
      var sett = ctx.Settings.Audio;

      ctx.SpeakerSoundcard.SetDeviceId(sett.SpeakerSoundcard);
      GainControl.ApplyAfGain();

      ctx.AudioVacSoundcard.SetDeviceId(sett.Vac);
      ctx.AudioVacSoundcard.Volume = Dsp.FromDb2(sett.VacVolume - 30); // -30 dB from speaker level

      ctx.IqVacSoundcard.SetDeviceId(sett.Vac);
      ctx.IqVacSoundcard.Volume = Dsp.FromDb2(sett.VacVolume);

      ctx.SpeakerSoundcard.Enabled = sett.SpeakerEnabled;
      EnableDisableVac();

      //ctx.Slicer.Enabled = ctx.Settings.Audio.SpeakerEnabled || ctx.Settings.Audio.VacEnabled;
    }

    private void EnableDisableVac()
    {
      var sett = ctx.Settings.Audio;
      ctx.AudioVacSoundcard.Enabled = false;
      ctx.IqVacSoundcard.Enabled = sett.VacEnabled && sett.VacDataFormat == VacDataFormat.IQ;
      ctx.AudioVacSoundcard.Enabled = sett.VacEnabled && sett.VacDataFormat == VacDataFormat.Audio;
    }

    private void Soundcard_StateChanged(object? sender, EventArgs e)
    {
      ShowSoundcardLabels();
    }

    public void ShowSoundcardLabels()
    {
      if (!ctx.SpeakerSoundcard.Enabled)
        SoundcardLedLabel.ForeColor = Color.Gray;
      else if (!ctx.SpeakerSoundcard.IsPlaying())
        SoundcardLedLabel.ForeColor = Color.Red;
      else
        SoundcardLedLabel.ForeColor = Color.Lime;

      if (!ctx.AudioVacSoundcard.Enabled && !ctx.IqVacSoundcard.Enabled)
        VacLedLabel.ForeColor = Color.Gray;
      else if (!ctx.AudioVacSoundcard.IsPlaying() && !ctx.IqVacSoundcard.IsPlaying())
        VacLedLabel.ForeColor = Color.Red;
      else
        VacLedLabel.ForeColor = Color.Lime;

      SoundcardStatusLabel.ToolTipText = ctx.SpeakerSoundcard.GetDisplayName();
      VacStatusLabel.ToolTipText = ctx.AudioVacSoundcard.GetDisplayName();
    }




    //----------------------------------------------------------------------------------------------
    //                                      sat data
    //----------------------------------------------------------------------------------------------
    const int LIST_DOWNLOAD_DAYS = 7;
    const int TLE_DOWNLOAD_DAYS = 1;

    bool DownloadOk = true;

    private void ReadOrDownloadSatelliteData()
    {
      LoadSatelliteData();
      CheckDownloadSatelliteList();
      if (!ctx.SatnogsDb.Loaded) Environment.Exit(1);

      CheckDownloadTle();
    }

    private void LoadSatelliteData()
    {
      ctx.SatnogsDb = new();
      ctx.SatnogsDb.ListUpdated += SatnogsDb_ListUpdated;
      ctx.SatnogsDb.TleUpdated += SatnogsDb_TleUpdated;

      ctx.SatnogsDb.LoadFromFile();
      SatnogsDb_ListUpdated(null, null);
    }

    private void CheckDownloadSatelliteList()
    {
      var nextDownloadTime = ctx.Settings.Satellites.LastDownloadTime.AddDays(LIST_DOWNLOAD_DAYS);

      if (!ctx.SatnogsDb.Loaded || DateTime.UtcNow > nextDownloadTime)
        DownloadSatList();
    }

    private void DownloadSatList()
    {
      DownloadOk = DownloadDialog.Download(this, ctx);
      ShowSatDataStatus();
    }

    public void CheckDownloadTle()
    {
      if (DateTime.UtcNow < ctx.Settings.Satellites.LastTleTime.AddDays(TLE_DOWNLOAD_DAYS)) return;
      DownloadTle().DoNotAwait();
    }

    private async Task DownloadTle()
    {
      try
      {
        SatDataLedLabel.ForeColor = Color.Gray;
        SatDataStatusLabel.ToolTipText = SatDataLedLabel.ToolTipText = "Downloading TLE...";


        await ctx.SatnogsDb.DownloadTle();
        DownloadOk = true;
        Log.Information("TLE downloaded");
      }
      catch (Exception ex)
      {
        DownloadOk = false;
        Log.Error(ex, "TLE download failed");
        ShowSatDataStatus();
      }
    }





    //----------------------------------------------------------------------------------------------
    //                                      menu
    //----------------------------------------------------------------------------------------------
    private void ExitMNU_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void DownloadSatDataMNU_Click(object sender, EventArgs e)
    {
      DownloadSatList();
    }

    private void DownloadTleMNU_Click(object sender, EventArgs e)
    {
      DownloadTle().DoNotAwait();
    }

    private void DataFolderMNU_Click(object sender, EventArgs e)
    {
      Process.Start("explorer.exe", Utils.GetUserDataFolder());
    }

    private void EditGroupsMNU_Click(object sender, EventArgs e)
    {
      var dlg = new SatelliteGroupsForm();
      dlg.SetList(ctx);
      var rc = dlg.ShowDialog(this);

      if (rc != DialogResult.OK) return;
      ctx.Settings.Satellites.DeleteInvalidData(ctx.SatnogsDb);
      SatelliteSelector.LoadSatelliteGroups();
    }

    private void GroupViewMNU_Click(object sender, EventArgs e)
    {
      if (ctx.GroupViewPanel == null)
        new GroupViewPanel(ctx).Show(DockHost, DockState.DockLeft);
      else
        ctx.GroupViewPanel.Close();
    }

    private void SatelliteDetailsMNU_Click(object sender, EventArgs e)
    {
      if (ctx.SatelliteDetailsPanel == null)
        ShowFloatingPanel(new SatelliteDetailsPanel(ctx));
      else
        ctx.SatelliteDetailsPanel.Close();
    }

    private void TransmittersMNU_Click(object sender, EventArgs e)
    {
      if (ctx.TransmittersPanel == null)
        ShowFloatingPanel(new TransmittersPanel(ctx));
      else
        ctx.TransmittersPanel.Close();
    }

    private void SatellitePassesMNU_Click(object sender, EventArgs e)
    {
      if (ctx.PassesPanel == null)
        new PassesPanel(ctx).Show(DockHost, DockState.DockRight);
      else
        ctx.PassesPanel.Close();
    }

    private void TimelineMNU_Click(object sender, EventArgs e)
    {
      if (ctx.TimelinePanel == null)
        new TimelinePanel(ctx).Show(DockHost, DockState.DockBottom);
      else
        ctx.TimelinePanel.Close();
    }

    private void SkyViewMNU_Click(object sender, EventArgs e)
    {
      if (ctx.SkyViewPanel == null)
        ShowFloatingPanel(new SkyViewPanel(ctx));
      else
        ctx.SkyViewPanel.Close();
    }

    private void EarthViewMNU_Click(object sender, EventArgs e)
    {
      if (ctx.EarthViewPanel == null)
        ShowFloatingPanel(new EarthViewPanel(ctx));
      else
        ctx.EarthViewPanel.Close();
    }

    private void WaterfallMNU_Click(object sender, EventArgs e)
    {
      if (ctx.WaterfallPanel == null)
        ShowFloatingPanel(new WaterfallPanel(ctx));
      else
        ctx.WaterfallPanel.Close();
    }

    private void SettingsMNU_Click(object sender, EventArgs e)
    {
      new SettingsDialog(ctx).ShowDialog();
    }

    private void WebsiteMNU_Click(object sender, EventArgs e)
    {
      Process.Start(new ProcessStartInfo("https://ve3nea.github.io/OrbiNom") { UseShellExecute = true });
    }

    private void EmailTheAuthorMNU_Click(object sender, EventArgs e)
    {
      Process.Start(new ProcessStartInfo("mailto:ve3nea@dxatlas.com") { UseShellExecute = true });
    }

    private void AboutMNU_Click(object sender, EventArgs e)
    {
      new AboutBox().ShowDialog();
    }

    private void SdrDevicesMNU_Click(object sender, EventArgs e)
    {
      EditSdrDevices();
    }





    //----------------------------------------------------------------------------------------------
    //                                     statusbar
    //----------------------------------------------------------------------------------------------
    public void ShowSatDataStatus()
    {
      var sett = ctx.Settings.Satellites;
      bool upToDate =
        sett.LastDownloadTime < DateTime.UtcNow.AddDays(-LIST_DOWNLOAD_DAYS) &&
        sett.LastTleTime < DateTime.UtcNow.AddDays(-TLE_DOWNLOAD_DAYS);

      if (upToDate) SatDataLedLabel.ForeColor = Color.Red;
      else if (!DownloadOk) SatDataLedLabel.ForeColor = Color.Gold;
      else SatDataLedLabel.ForeColor = Color.Lime;

      string tooltip =
        $"Satellite List:  {sett.LastDownloadTime.ToLocalTime():yyyy-MM-dd HH:mm}\n" +
        $"TLE:                 {sett.LastTleTime.ToLocalTime():yyyy-MM-dd HH:mm}" +
        (DownloadOk ? "" : "\nDownload failed");

      SatDataStatusLabel.ToolTipText = SatDataLedLabel.ToolTipText = tooltip;
    }

    DateTime StartTime;
    double StartSeconds;
    private void ShowCpuUsage()
    {
      var time = DateTime.UtcNow;
      var usedSeconds = AppDomain.CurrentDomain.MonitoringTotalProcessorTime.TotalSeconds;
      var usage = (usedSeconds - StartSeconds) / (time - StartTime).TotalSeconds * 100d / Environment.ProcessorCount;
      StartTime = time;
      StartSeconds = usedSeconds;

      CpuLoadlabel.Text = $"    CPU Load: {usage:F1}%";
    }

    private void SdrStatus_Click(object sender, EventArgs e)
    {
      if (ModifierKeys.HasFlag(Keys.Control) || string.IsNullOrEmpty(ctx.Settings.Sdr.SelectedDeviceName))
        EditSdrDevices();
      else
        ToggleSdrEnabled();
    }

    private void StatusLabel_MouseEnter(object sender, EventArgs e)
    {
      Cursor = Cursors.Hand;
    }

    private void StatusLabel_MouseLeave(object sender, EventArgs e)
    {
      Cursor = Cursors.Default;
    }

    private void SoundcardLabel_Click(object sender, EventArgs e)
    {
      var sett = ctx.Settings.Audio;
      sett.SpeakerEnabled = !sett.SpeakerEnabled;
      ctx.SpeakerSoundcard.Enabled = sett.SpeakerEnabled;
      ShowSoundcardLabels();
    }

    private void VacLabel_Click(object sender, EventArgs e)
    {
      ctx.Settings.Audio.VacEnabled = !ctx.Settings.Audio.VacEnabled;
      EnableDisableVac();
      ShowSoundcardLabels();
    }

    private void SoundcardDropdownBtn_DropDownOpening(object sender, EventArgs e)
    {
      SoundcardDropdownBtn.DropDownItems.Clear();
      foreach (var dev in Soundcard<float>.ListDevices(DataFlow.Render))
      {
        var item = new ToolStripMenuItem(dev.Name);
        item.Checked = dev.Id == ctx.Settings.Audio.SpeakerSoundcard;

        item.Click += (s, e) =>
        {
          ctx.Settings.Audio.SpeakerSoundcard = dev.Id;
          ApplyAudioSettings();
        };
        SoundcardDropdownBtn.DropDownItems.Add(item);
      }
    }

    private void RxCatLabel_Click(object sender, EventArgs e)
    {
      ctx.Settings.RxCat.Enabled = !ctx.Settings.RxCat.Enabled;
      ctx.CatControl.ApplySettings();
      ShowCatStatus();
    }

    private void TxCatLabel_Click(object sender, EventArgs e)
    {
      ctx.Settings.TxCat.Enabled = !ctx.Settings.TxCat.Enabled;
      ctx.CatControl.ApplySettings();
      ShowCatStatus();
    }

    public void ShowCatStatus()
    {
      if (ctx.CatControl.Rx == null) RxCatLedLabel.ForeColor = Color.Gray;
      else if (!ctx.CatControl.Rx!.IsRunning()) RxCatLedLabel.ForeColor = Color.Red;
      else RxCatLedLabel.ForeColor = Color.Lime;

      if (ctx.CatControl.Tx == null) TxCatLedLabel.ForeColor = Color.Gray;
      else if (!ctx.CatControl.Tx!.IsRunning()) TxCatLedLabel.ForeColor = Color.Red;
      else TxCatLedLabel.ForeColor = Color.Lime;

      RxCatStatusLabel.ToolTipText = ctx.CatControl.Rx?.GetStatusString() ?? "Disabled";
      TxCatStatusLabel.ToolTipText = ctx.CatControl.Tx?.GetStatusString() ?? "Disabled";
    }





    //----------------------------------------------------------------------------------------------
    //                                     docking
    //----------------------------------------------------------------------------------------------
    private void ShowFloatingPanel(DockContent panel)
    {
      var rect = panel.Bounds;
      rect.Offset(
        ctx.MainForm.Location.X + (ctx.MainForm.Width - panel.Width) / 2,
        ctx.MainForm.Location.Y + (ctx.MainForm.Size.Height - panel.Height) / 2
        );

      panel.Show(DockHost, rect);
    }

    private void SetDefaultDockingLayout()
    {
      GroupViewMNU_Click(null, null);
    }

    public IDockContent? MakeDockContentFromPersistString(string persistString)
    {
      switch (persistString)
      {
        case "OrbiNom.GroupViewPanel": return new GroupViewPanel(ctx);
        case "OrbiNom.SatelliteDetailsPanel": return new SatelliteDetailsPanel(ctx);
        case "OrbiNom.TransmittersPanel": return new TransmittersPanel(ctx);
        case "OrbiNom.PassesPanel": return new PassesPanel(ctx);
        case "OrbiNom.TimelinePanel": return new TimelinePanel(ctx);
        case "OrbiNom.SkyViewPanel": return new SkyViewPanel(ctx);
        case "OrbiNom.EarthViewPanel": return new EarthViewPanel(ctx);
        case "OrbiNom.WaterfallPanel": return new WaterfallPanel(ctx);
        default: return null;
      }
    }




    //----------------------------------------------------------------------------------------------
    //                                       timer
    //----------------------------------------------------------------------------------------------
    private const int TICKS_PER_SECOND = 8;
    nuint TickCount;

    private void timer_Tick(object sender, EventArgs e)
    {
      // 8 Hz (125 ms) ticks
      TickCount += 1;

      EightHzTick();
      if (TickCount % (TICKS_PER_SECOND / 4) == 0) FourHertzTick();
      if (TickCount % TICKS_PER_SECOND == 0) OneSecondTick();
      if (TickCount % (TICKS_PER_SECOND * 60) == 0) OneMinuteTick().DoNotAwait();
      if (TickCount % (TICKS_PER_SECOND * 3600) == 0) OneHourTick();
    }

    private void EightHzTick()
    {
      ctx.EarthViewPanel?.Advance();
    }

    private void FourHertzTick()
    {
      Clock.ShowTime();
      ctx.SkyViewPanel?.Advance();
      ctx.WaterfallPanel?.ScaleControl?.Invalidate();
      FrequencyControl?.ClockTick();
    }

    private void OneSecondTick()
    {
      ctx.GroupViewPanel?.UpdatePassTimes();
      ctx.PassesPanel?.UpdatePassTimes();
      ctx.TimelinePanel?.Advance();
      ctx.Sdr?.Retry();
      ctx.Announcer.AnnouncePasses();

      ShowCpuUsage();
    }
    private async Task OneMinuteTick()
    {
      ctx.GroupPasses.PredictMorePasses();
      ctx.HamPasses.PredictMorePasses();
      ctx.SdrPasses.PredictMorePasses();
      ctx.PassesPanel?.ShowPasses();
      ctx.WaterfallPanel?.ScaleControl?.BuildLabels();
    }

    private void OneHourTick()
    {
      CheckDownloadSatelliteList();
      CheckDownloadTle();
    }




    //----------------------------------------------------------------------------------------------
    //                                 change
    //----------------------------------------------------------------------------------------------
    private void SatnogsDb_ListUpdated(object? sender, EventArgs e)
    {
      if (sender != null)
      {
        ctx.Settings.Satellites.LastDownloadTime = DateTime.UtcNow;
        ctx.Settings.Satellites.LastTleTime = ctx.Settings.Satellites.LastDownloadTime;
      }

      ctx.SatnogsDb.Customize(ctx.Settings.Satellites.SatelliteCustomizations);
      ctx.Settings.Satellites.DeleteInvalidData(ctx.SatnogsDb);

      SatelliteSelector.LoadSatelliteGroups();
      ctx.HamPasses.FullRebuild();
      ctx.GroupPasses.FullRebuild();
      ctx.SdrPasses.FullRebuild();

      ctx.PassesPanel?.ShowPasses();
      ctx.SkyViewPanel?.ClearPass();
      ctx.WaterfallPanel?.ScaleControl?.BuildLabels();

      ShowSatDataStatus();
    }

    internal void SetLocation()
    {
      // update data
      ctx.GroupPasses = new(ctx);
      ctx.GroupPasses.FullRebuild();

      ctx.HamPasses = new(ctx);
      ctx.HamPasses.FullRebuild();

      ctx.SdrPasses = new(ctx);
      ctx.SdrPasses.FullRebuild();

      ctx.SatelliteSelector.SetSelectedPass(null);
      ctx.WaterfallPanel?.ScaleControl?.BuildLabels();
      ctx.GroupViewPanel?.LoadGroup();
      ctx.PassesPanel?.ShowPasses();
      ctx.EarthViewPanel?.SetGridSquare();
      ctx.SkyViewPanel?.ClearPass();
    }

    private void SatnogsDb_TleUpdated(object? sender, EventArgs e)
    {
      ctx.Settings.Satellites.LastTleTime = DateTime.UtcNow;
      ShowSatDataStatus();

      ctx.HamPasses.Rebuild();
      ctx.GroupPasses.Rebuild();
      ctx.SdrPasses.Rebuild();

      ctx.SatelliteSelector.SetSelectedPass(null);
      ctx.GroupViewPanel?.LoadGroup(); // replace passes attached to sats
      ctx.PassesPanel?.ShowPasses();
      ctx.SkyViewPanel?.ClearPass();
      ctx.WaterfallPanel?.ScaleControl?.BuildLabels();
    }

    private void SatelliteSelector_SelectedGroupChanged(object sender, EventArgs e)
    {
      ctx.GroupViewPanel?.LoadGroup();
      ctx.GroupPasses.FullRebuild();
      ctx.PassesPanel?.ShowPasses();
      ctx.SkyViewPanel?.ClearPass();
    }

    private void SatelliteSelector_SelectedSatelliteChanged(object sender, EventArgs e)
    {
      ctx.GroupViewPanel?.ShowSelectedSat();
      ctx.EarthViewPanel?.SetSatellite();
      ctx.SatelliteDetailsPanel?.SetSatellite();
      ctx.TransmittersPanel?.SetSatellite();
      ctx.PassesPanel?.ShowPasses();
    }

    private void SatelliteSelector_SelectedTransmitterChanged(object sender, EventArgs e)
    {
      FrequencyControl.SetTransmitter();
      ctx.TransmittersPanel?.ShowSelectedTransmitter();
      ctx.WaterfallPanel?.BringInView(ctx.FrequencyControl.CorrectedDownlinkFrequency);
      ctx.SdrPasses.UpdateFrequencyRange();
      ctx.WaterfallPanel?.ScaleControl?.BuildLabels();
    }

    private void SatelliteSelector_SelectedPassChanged(object sender, EventArgs e)
    {
      SatellitePass? pass = ctx.SatelliteSelector.SelectedPass;
      ctx.SkyViewPanel?.SetPass(pass);
    }
  }
}