using System.Diagnostics;
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
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      LoadSatelliteList();

      // apply settings
      ctx.Settings.Ui.RestoreWindowPosition(this);
      if (!ctx.Settings.Ui.RestoreDockingLayout(this)) SetDefaultDockingLayout();
      Clock.UtcMode = ctx.Settings.Ui.ClockUtcMode;

      ctx.Settings.SatelliteSettings.DeleteInvalidData(ctx.SatnogsDb);
      SatelliteSelector.SetSatelliteGroups();
    }

    const int LIST_DOWNLOAD_DAYS = 7;
    const int TLE_DOWNLOAD_DAYS = 1;

    private void LoadSatelliteList()
    {
      // load from file
      ctx.SatnogsDb = new();
      ctx.SatnogsDb.LoadFromFile();
      ctx.SatnogsDb.Customize(ctx.Settings.SatelliteSettings.SatelliteCustomizations);

      // {!} fired before customization
      ctx.SatnogsDb.ListUpdated += SatnogsDb_ListUpdated;
      ctx.SatnogsDb.TleUpdated += SatnogsDb_TleUpdated;

      // download if needed
      if (!ctx.SatnogsDb.Loaded || DateTime.UtcNow > ctx.Settings.SatList.LastDownloadTime.AddDays(LIST_DOWNLOAD_DAYS))
        DownloadDialog.Download(this, ctx);

      if (DateTime.UtcNow > ctx.Settings.SatList.LastTleTime.AddDays(TLE_DOWNLOAD_DAYS))
        try
        {
          ctx.SatnogsDb.DownloadTle();
          ctx.Settings.SatList.LastTleTime = DateTime.UtcNow;
        }
        catch
        {
        }

      // no satellite data, cannot proceed
      if (!ctx.SatnogsDb.Loaded) Environment.Exit(1);

      // delete sats that are no longer in the db
      ctx.Settings.SatelliteSettings.DeleteInvalidData(ctx.SatnogsDb);
    }

    private void SatnogsDb_TleUpdated(object? sender, EventArgs e)
    {

    }

    private void SatnogsDb_ListUpdated(object? sender, EventArgs e)
    {

    }

    private void MainForm_FormClosing(object sender, EventArgs e)
    {
      // save settings
      ctx.Settings.Ui.StoreDockingLayout(DockHost);
      ctx.Settings.Ui.StoreWindowPosition(this);
      ctx.Settings.Ui.ClockUtcMode = Clock.UtcMode;
      ctx.Settings.SaveToFile();
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
      ctx.Settings.SatelliteSettings.DeleteInvalidData(ctx.SatnogsDb);
      ctx.SatelliteSelector.SetSatelliteGroups();
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
        default: return null;
      }
    }




    private void SatelliteSelector_SelectedGroupChanged(object sender, EventArgs e)
    {
      ctx.GroupViewPanel?.LoadGroup();
    }

    private void SatelliteSelector_SelectedSatelliteChanged(object sender, EventArgs e)
    {
      ctx.GroupViewPanel?.ShowSelectedSat();
    }
  }
}