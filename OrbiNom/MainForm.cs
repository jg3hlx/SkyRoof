using System.Diagnostics;
using System.Net.NetworkInformation;
using VE3NEA;
using WeifenLuo.WinFormsUI.Docking;

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
      SatelliteSelector.ctx = ctx;
      ctx.Settings.LoadFromFile();

      EnsureUserDetails();

      ctx.GroupPasses = new(ctx, true);
      ctx.AllPasses = new(ctx, false);

      timer.Interval = 1000 / TICKS_PER_SECOND;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      LoadSatelliteList();

      // apply settings
      ctx.Settings.Ui.RestoreWindowPosition(this);
      if (!ctx.Settings.Ui.RestoreDockingLayout(this)) SetDefaultDockingLayout();
      Clock.UtcMode = ctx.Settings.Ui.ClockUtcMode;
    }

    const int LIST_DOWNLOAD_DAYS = 7;
    const int TLE_DOWNLOAD_DAYS = 1;

    private void LoadSatelliteList()
    {
      // load from file
      ctx.SatnogsDb = new();
      ctx.SatnogsDb.ListUpdated += SatnogsDb_ListUpdated;
      ctx.SatnogsDb.TleUpdated += SatnogsDb_TleUpdated;
      ctx.SatnogsDb.LoadFromFile();


      // download if needed
      var nextDownloadTime = ctx.Settings.Satellites.LastDownloadTime.AddDays(LIST_DOWNLOAD_DAYS);
      if (ctx.SatnogsDb.Loaded && DateTime.UtcNow < nextDownloadTime)
        SatnogsDb_ListUpdated(null, null);
      else
        DownloadDialog.Download(this, ctx);

      if (DateTime.UtcNow > ctx.Settings.Satellites.LastTleTime.AddDays(TLE_DOWNLOAD_DAYS))
        try
        {
          ctx.SatnogsDb.DownloadTle();
          ctx.Settings.Satellites.LastTleTime = DateTime.UtcNow;
        }
        catch { }

      // no satellite data, cannot proceed
      if (!ctx.SatnogsDb.Loaded) Environment.Exit(1);

      ShowSatDataStatus();
    }

    private void MainForm_FormClosing(object sender, EventArgs e)
    {
      // save settings
      ctx.Settings.Ui.StoreDockingLayout(DockHost);
      ctx.Settings.Ui.StoreWindowPosition(this);
      ctx.Settings.Ui.ClockUtcMode = Clock.UtcMode;

      if (ctx.SatelliteDetailsPanel != null)
        ctx.Settings.Ui.SatelliteDetailsPanel.SplitterDistance = ctx.SatelliteDetailsPanel.satelliteDetailsControl1.splitContainer1.SplitterDistance;

      ctx.Settings.SaveToFile();
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
    //                                      menu
    //----------------------------------------------------------------------------------------------
    private void ExitMNU_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void UpdateSatelliteListMNU_Click(object sender, EventArgs e)
    {
      DownloadDialog.Download(this, ctx);
    }

    private void DataFolderMNU_Click(object sender, EventArgs e)
    {
      Process.Start("explorer.exe", Utils.GetUserDataFolder());
    }

    private void EditGroupsMNU_Click(object sender, EventArgs e)
    {
      var dlg = new SatelliteGroupsForm();
      dlg.SetList(ctx);
      dlg.ShowDialog(this);
      ctx.Settings.Satellites.DeleteInvalidData(ctx.SatnogsDb);
      SatelliteSelector.SetSatelliteGroups();
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
        new SatelliteDetailsPanel(ctx).Show(DockHost, DockState.Float);
      else
        ctx.SatelliteDetailsPanel.Close();
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
        new TimelinePanel(ctx).Show(DockHost, DockState.DockRight);
      else
        ctx.TimelinePanel.Close();
    }

    private void SkyViewMNU_Click(object sender, EventArgs e)
    {
      if (ctx.SkyViewPanel == null)
        new SkyViewPanel(ctx).Show(DockHost, DockState.DockRight);
      else
        ctx.SkyViewPanel.Close();
    }

    private void EarthViewMNU_Click(object sender, EventArgs e)
    {
      if (ctx.EarthViewPanel == null)
        new EarthViewPanel(ctx).Show(DockHost, DockState.DockRight);
      else
        ctx.EarthViewPanel.Close();
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




    //----------------------------------------------------------------------------------------------
    //                                     statusbar
    //----------------------------------------------------------------------------------------------
    public void ShowSatDataStatus()
    {
      var sett = ctx.Settings.Satellites;

      if (sett.LastDownloadTime < DateTime.UtcNow.AddDays(-LIST_DOWNLOAD_DAYS) &&
        sett.LastTleTime < DateTime.UtcNow.AddDays(-TLE_DOWNLOAD_DAYS))
        SatDataLedLabel.ForeColor = Color.Red;
      else
        SatDataLedLabel.ForeColor = Color.Lime;

      SatDataLedLabel.ToolTipText = SatDataStatusLabel.ToolTipText =
        $"Satellite List:  {sett.LastDownloadTime.ToLocalTime():yyyy-MM-dd HH:mm}\n" +
        $"TLE:                 {sett.LastTleTime.ToLocalTime():yyyy-MM-dd HH:mm}";
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



    //----------------------------------------------------------------------------------------------
    //                                     docking
    //----------------------------------------------------------------------------------------------
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
        case "OrbiNom.PassesPanel": return new PassesPanel(ctx);
        case "OrbiNom.TimelinePanel": return new TimelinePanel(ctx);
        case "OrbiNom.SkyViewPanel": return new SkyViewPanel(ctx);
        case "OrbiNom.EarthViewPanel": return new EarthViewPanel(ctx);
        default: return null;
      }
    }




    //----------------------------------------------------------------------------------------------
    //                                       timer
    //----------------------------------------------------------------------------------------------
    private const int TICKS_PER_SECOND = 8;
    long TickCount;

    private void timer_Tick(object sender, EventArgs e)
    {
      ctx.EarthViewPanel?.Advance();

      // 1/8 s ticks
      TickCount += 1;
      if (TickCount % (TICKS_PER_SECOND / 4) == 0) FourHertzTick();
      if (TickCount % TICKS_PER_SECOND == 0) OneSecondTick();
      if (TickCount % (TICKS_PER_SECOND * 60) == 0) OneMinuteTick();
    }

    private void FourHertzTick()
    {
      Clock.ShowTime();
      ctx.SkyViewPanel?.Advance();
    }

    private void OneSecondTick()
    {
      ctx.GroupViewPanel?.UpdatePassTimes();
      ctx.PassesPanel?.UpdatePassTimes();
      ctx.TimelinePanel?.Advance();

      ShowCpuUsage();      
    }

    private void OneMinuteTick()
    {
      ctx.GroupPasses.PredictMorePasses();
      ctx.AllPasses.PredictMorePasses();
      ctx.PassesPanel?.ShowPasses();
    }




    //----------------------------------------------------------------------------------------------
    //                                 change
    //----------------------------------------------------------------------------------------------
    private void SatnogsDb_ListUpdated(object? sender, EventArgs e)
    {
      ctx.SatnogsDb.Customize(ctx.Settings.Satellites.SatelliteCustomizations);
      ctx.Settings.Satellites.DeleteInvalidData(ctx.SatnogsDb);
      SatelliteSelector.SetSatelliteGroups();
      ctx.AllPasses.FullRebuild();
      ctx.GroupPasses.FullRebuild();

      ctx.PassesPanel?.ShowPasses();
      ctx.SkyViewPanel?.ClearPass();
    }

    private void SatelliteSelector_SelectedGroupChanged(object sender, EventArgs e)
    {
      ctx.GroupViewPanel?.LoadGroup();
      ctx.GroupPasses.FullRebuild();
      ctx.PassesPanel?.ShowPasses();
    }

    private void SatelliteSelector_SelectedSatelliteChanged(object sender, EventArgs e)
    {
      ctx.GroupViewPanel?.ShowSelectedSat();
      ctx.EarthViewPanel?.SetSatellite();
      ctx.SatelliteDetailsPanel?.SetSatellite();
    }

    private void SatnogsDb_TleUpdated(object? sender, EventArgs e)
    {
      ctx.AllPasses.Rebuild();
      ctx.GroupPasses.Rebuild();

      ctx.SatelliteSelector.SetSelectedPass(null);
      ctx.GroupViewPanel?.LoadGroup(); // replace passes attached to sats
      ctx.PassesPanel?.ShowPasses();
      ctx.SkyViewPanel?.ClearPass();
    }

    private void SatelliteSelector_SelectedPassChanged(object sender, EventArgs e)
    {
      SatellitePass? pass = ctx.SatelliteSelector.SelectedPass;
      ctx.SkyViewPanel?.SetPass(pass);
    }

    internal void SetGridSquare()
    {
      // update data
      ctx.GroupPasses = new(ctx, true);
      ctx.GroupPasses.FullRebuild();
      ctx.AllPasses = new(ctx, false);
      ctx.AllPasses.FullRebuild();

      ctx.SatelliteSelector.SetSelectedPass(null);
      ctx.GroupViewPanel?.LoadGroup();
      ctx.PassesPanel?.ShowPasses();
      ctx.EarthViewPanel?.SetGridSquare();
      ctx.SkyViewPanel?.ClearPass();
    }
  }
}