using SkyRoof;
using VE3NEA;

namespace SkyRoof
{

  public partial class RotatorWidget : UserControl
  {
    public Context ctx;
    private RotatorControlEngine? engine;
    private AzElEntryDialog Dialog = new();
    private SatnogsDbSatellite? Satellite;
    private Bearing SatBearing;
    private bool WasAboveHorizon = false;
    public Bearing? AntBearing { get => engine?.LastReadBearing; }

    public RotatorWidget()
    {
      InitializeComponent();
    }




    //----------------------------------------------------------------------------------------------
    //                               public interface
    //----------------------------------------------------------------------------------------------
    public void ApplySettings(bool restoreTracking = false)
    {
      if (engine != null) StopRotation();

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

    // TODO: use path optimizer
    public void SetSatellite(SatnogsDbSatellite? sat)
    {
      if (sat == Satellite) return;

      Satellite = sat;
      engine?.StopRotation();

      ResetUi();
      Advance();

      // show black LED if no satellite
      ctx.MainForm.ShowRotatorStatus();
    }

    public void Retry()
    {
      engine?.Retry();
    }

    public bool IsRunning()
    {
      return engine != null && engine.IsRunning;
    }

    // TODO: use path optimizer
    internal void Advance()
    {
      if (Satellite == null) return;

      var obs = ctx.SdrPasses.ObserveSatellite(Satellite, DateTime.UtcNow);
      if (obs == null || obs?.Azimuth == null || obs?.Elevation == null)
      {
        ResetUi();
        return;
      }

      // Convert observation degrees to radians for Bearing
      double azRad = obs.Azimuth.Degrees * Geo.RinD;
      double elRad = obs.Elevation.Degrees * Geo.RinD;
      SatBearing = new Bearing(azRad, elRad);

      WasAboveHorizon = WasAboveHorizon || SatBearing.ElDeg > 0;
      if (WasAboveHorizon && SatBearing.ElDeg < -3) StopRotation();

      if (engine != null && TrackCheckbox.Checked)
      {
        var bearing = Sanitize(SatBearing);
        var diff = AngleBetween(bearing, engine.RequestedBearing!);
        // Convert step size from degrees to radians for comparison
        if (diff >= ctx.Settings.Rotator.StepSize * Geo.RinD) RotateTo(SatBearing);
      }

      BearingToUi();
      ctx.Announcer.AnnouncePosition(SatBearing);
    }

    public void RotateTo(Bearing bearing)
    {
      if (engine == null) return;

      var sanitizedBearing = Sanitize(bearing);
      engine.RotateTo(sanitizedBearing);
    }

    public void StopRotation()
    {
      TrackCheckbox.Checked = false;
      WasAboveHorizon = false;
      engine?.StopRotation();
    }

    public void ToggleTracking()
    {
      if (!TrackCheckbox.Enabled) return;
      TrackCheckbox.Checked = !TrackCheckbox.Checked;
      TrackCheckbox_CheckedChanged(StopBtn, EventArgs.Empty);
    }

    public string? GetStatusString()
    {
      if (!ctx.Settings.Rotator.Enabled) return "Rotator control disabled";
      else if (!IsRunning()) return "No connection";
      else if (!TrackCheckbox.Checked) return "Connected, tracking disabled";
      else return "Connected and tracking";
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

      ctx.MainForm.ShowRotatorStatus();
    }

    private void StopBtn_Click(object sender, EventArgs e)
    {
      StopRotation();
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

    private void BearingToUi()
    {
      if (SatBearing == null) { ResetUi(); return; }

      Color satColor = TrackCheckbox.Checked ? Color.Aqua : Color.Teal;

      bool trackError = TrackCheckbox.Checked && (!IsRunning() ||
        AntBearing == null ||
        AngleBetween(SatBearing, AntBearing!) > 1.5 * ctx.Settings.Rotator.StepSize * Geo.RinD);

      Color antColor = trackError ? Color.LightCoral : Color.Transparent;

      SatelliteAzimuthLabel.ForeColor = satColor;
      SatelliteElevationLabel.ForeColor = satColor;
      SatelliteAzimuthLabel.Text = $"{SatBearing.AzDeg:F0}°";
      SatelliteElevationLabel.Text = $"{SatBearing.ElDeg:F0}°";

      AntennaAzimuthLabel.BackColor = antColor;
      AntennaElevationLabel.BackColor = antColor;

      if (IsRunning() && AntBearing != null)
      {
        AntennaAzimuthLabel.Text = $"{AntBearing.AzDeg:F1}°";
        AntennaElevationLabel.Text = $"{AntBearing.ElDeg:F1}°";
      }
      else
      {
        AntennaAzimuthLabel.Text = "---";
        AntennaElevationLabel.Text = "---";
      }
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




    //----------------------------------------------------------------------------------------------
    //                                   helper functions
    //----------------------------------------------------------------------------------------------
    private Bearing Sanitize(Bearing bearing)
    {
      var sett = ctx.Settings.Rotator;

      var sanitizedBearing = new Bearing(bearing.Az, bearing.El);
      sanitizedBearing.Az += sett.AzimuthOffset * Trig.RinD;
      sanitizedBearing.El += sett.ElevationOffset * Trig.RinD;

      var bounds = new RectangleF(
        sett.MinAzimuth * Trig.RinD,
        sett.MinElevation * Trig.RinD,
        (sett.MaxAzimuth - sett.MinAzimuth) * Trig.RinD,
        (sett.MaxElevation - sett.MinElevation) * Trig.RinD
      );
      sanitizedBearing = sanitizedBearing.Clamp(bounds);

      return sanitizedBearing;
    }

    private double AngleBetween(Bearing bearing1, Bearing bearing2)
    {
      bool azOnly = ctx.Settings.Rotator.MinElevation == ctx.Settings.Rotator.MaxElevation;
      return bearing1.Angle(bearing2, azOnly);
    }
  }
}