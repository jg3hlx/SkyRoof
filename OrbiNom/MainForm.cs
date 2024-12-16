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
      ctx.Settings.LoadFromFile();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      // apply settings
      ctx.Settings.Ui.RestoreWindowPosition(this);
      if (!ctx.Settings.Ui.RestoreDockingLayout(this)) SetDefaultDockingLayout();
      Clock.UtcMode = ctx.Settings.Ui.ClockUtcMode;

      LoadSatelliteList();
    }

    const int LIST_DOWNLOAD_DAYS = 7;
    const int TLE_DOWNLOAD_DAYS = 1;

    private void LoadSatelliteList()
    {
      // load from file
      ctx.SatnogsDb = new();
      ctx.SatnogsDb.LoadFromFile();
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
        catch { }

      // no satellite data, cannot proceed
      if (!ctx.SatnogsDb.Loaded) Environment.Exit(1);
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




    //----------------------------------------------------------------------------------------------
    //                                     docking
    //----------------------------------------------------------------------------------------------
    private void SetDefaultDockingLayout()
    {
      //ViewReceiversMNU_Click(null, null);
      //ViewBandViewMNU_Click(null, null);
      //ViewMessagesMNU_Click(null, null);
    }

    public IDockContent? MakeDockContentFromPersistString(string persistString)
    {
      switch (persistString)
      {
        //   case "JTSkimmer.ReceiversPanel": return new ReceiversPanel(ctx);
        //   case "JTSkimmer.BandViewPanel": return new BandViewPanel(ctx);
        //   case "JTSkimmer.MessagesPanel": return new MessagesPanel(ctx);
        default: return null;
      }
    }

    private void SatelliteGroupsMNU_Click(object sender, EventArgs e)
    {
      var dlg = new SatGroupsForm();
      dlg.SatellitesListView.SetList(ctx.SatnogsDb.Satellites, ctx.Settings.SatList.LastDownloadTime);
      dlg.ShowDialog(this);
    }
  }
}