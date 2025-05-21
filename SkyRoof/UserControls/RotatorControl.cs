using SkyRoof;
using VE3NEA;

namespace SkyRoof
{

  public partial class RotatorControl : UserControl
  {
    public Context ctx;
    private RotatorControlEngine? engine;
    private AzElEntryDialog Dialog = new();
    private SatnogsDbSatellite? Satellite;
    private Bearing SatBearing, LastWrittenBearing;
    public Bearing AntBearing { get => engine!.LastReadBearing; }

    public RotatorControl()
    {
      InitializeComponent();
    }




    //----------------------------------------------------------------------------------------------
    //                                        engine
    //----------------------------------------------------------------------------------------------
    public void ApplySettings(bool restoreTracking = false)
    {
      engine?.Dispose();
      engine = null;
      bool track = restoreTracking && TrackCheckbox.Checked;

      if (ctx.Settings.Rotator.Enabled)
      {
        engine = new RotatorControlEngine(ctx.Settings.Rotator);
        engine.StatusChanged += Engine_StatusChanged;
        engine.BearingChanged += Engine_BearingChanged;
      }

      ResetUi();
      TrackCheckbox.Checked = track;
      Advance();
      ctx.MainForm.ShowRotatorStatus();
    }

    internal void Retry()
    {
      engine?.Retry();
    }

    public bool IsRunning()
    {
      return engine != null && engine.IsRunning;
    }

    private void Engine_StatusChanged(object? sender, EventArgs e)
    {
      // ant bearing color
      BearingToUi();

      ctx.MainForm.ShowRotatorStatus();
    }

    private void Engine_BearingChanged(object? sender, EventArgs e)
    {
      BearingToUi();
      ctx.SkyViewPanel?.Refresh();
    }

    public void RotateTo(Bearing bearing)
    {
      if (engine == null) return;

      bearing = Sanitize(bearing);
      engine.RotateTo(bearing);
      LastWrittenBearing = bearing;
    }

    public void StopRotation()
    {
      TrackCheckbox.Checked = false;
      engine?.StopRotation();
    }

    private Bearing Sanitize(Bearing bearing)
    {
      var sett = ctx.Settings.Rotator;

      bearing.Azimuth += sett.AzimuthOffset;
      bearing.Elevation += sett.ElevationOffset;

      bearing.Azimuth = Math.Max(sett.MinAzimuth, Math.Min(bearing.Azimuth, sett.MaxAzimuth));
      bearing.Elevation = Math.Max(sett.MinElevation, Math.Min(bearing.Elevation, sett.MaxElevation));
      
      return bearing;
    }

    public void SetSatellite(SatnogsDbSatellite? sat)
    {
      Satellite = sat;
      engine?.StopRotation();

      ResetUi();
      Advance();

      // show black LED if no satellite
      ctx.MainForm.ShowRotatorStatus();
    }





    //----------------------------------------------------------------------------------------------
    //                                        UI
    //----------------------------------------------------------------------------------------------
    private void AzEl_Click(object sender, EventArgs e)
    {
      Dialog.Open(ctx);
    }

    private void TrackCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      if (TrackCheckbox.Checked)
        RotateTo(SatBearing);
      else
        StopRotation();

      // update color
      BearingToUi();
    }

    private void StopBtn_Click(object sender, EventArgs e)
    {
      StopRotation();
    }

    internal string? GetStatusString()
    {
      if (!ctx.Settings.Rotator.Enabled) 
        return "Rotator control disabled";
      else if (!IsRunning()) 
        return "No connection";
      else if (!TrackCheckbox.Checked)
        return "Connected, tracking disabled";
      else 
        return "Connected and tracking";      
    }

    private void ResetUi()
    {
      SatelliteAzimuthLabel.ForeColor = Color.Gray;
      SatelliteElevationLabel.ForeColor = Color.Gray;

      SatelliteAzimuthLabel.Text = "0°";
      SatelliteElevationLabel.Text = "0°";
      AntennaAzimuthLabel.Text = "---";
      AntennaElevationLabel.Text = "---";

      TrackCheckbox.Checked = false;
      TrackCheckbox.Enabled = ctx.Settings.Rotator.Enabled && Satellite != null;
    }

    internal void Advance()
    {
      if (!ctx.Settings.Rotator.Enabled || Satellite == null) return;

      var obs = ctx.SdrPasses.ObserveSatellite(Satellite, DateTime.UtcNow);
      SatBearing = new Bearing(obs.Azimuth.Degrees, obs.Elevation.Degrees);

      if (engine != null && TrackCheckbox.Checked)
      {
        var bearing = Sanitize(SatBearing);
        var diff = Bearing.AngleBetween(bearing, LastWrittenBearing);
        if (diff >= ctx.Settings.Rotator.StepSize) RotateTo(SatBearing);
      }

      BearingToUi();
    }


    private void BearingToUi()
    {
      Color satColor = TrackCheckbox.Checked ? Color.Aqua : Color.Teal;

      bool trackError = TrackCheckbox.Checked && (!IsRunning() || Bearing.AngleBetween(SatBearing, AntBearing) > 1.5 * ctx.Settings.Rotator.StepSize);
      Color antColor = trackError ? Color.Red : Color.Transparent;

      SatelliteAzimuthLabel.ForeColor = satColor;
      SatelliteElevationLabel.ForeColor = satColor;
      SatelliteAzimuthLabel.Text = $"{SatBearing.Azimuth:F0}°";
      SatelliteElevationLabel.Text = $"{SatBearing.Elevation:F0}°";

      AntennaAzimuthLabel.BackColor = antColor;
      AntennaElevationLabel.BackColor = antColor;

      if (IsRunning() && AntBearing != null)
      {
        AntennaAzimuthLabel.Text = $"{AntBearing.Azimuth:F1}°";
        AntennaElevationLabel.Text = $"{AntBearing.Elevation:F1}°";
      }
      else
      {
        AntennaAzimuthLabel.Text = "---";
        AntennaElevationLabel.Text = "---";
      }
    }
  }
}
