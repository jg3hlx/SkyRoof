using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Serilog;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.Util;
using VE3NEA;

namespace OrbiNom
{
  public class SatellitePasses
  {
    private const double TwoPi = 2 * Math.PI;
    
    private List<SatnogsDbSatellite> Satellites = new();
    private GroundStation GroundStation;
    private readonly Context ctx;
    private readonly bool OnlyGroup;
    private readonly TimeSpan PredictionTimeSpan;
    private readonly TimeSpan HistoryTimeSpan = TimeSpan.FromMinutes(30);

    private DateTime LastPredictionTime = DateTime.MinValue;

    public List<SatellitePass> Passes = new();

    public SatellitePasses(Context ctx, bool onlyGroup)
    {
      this.ctx = ctx;
      OnlyGroup = onlyGroup;
      PredictionTimeSpan = onlyGroup ? TimeSpan.FromDays(2) : TimeSpan.FromHours(2);

      CreateGroundStation(ctx.Settings.User.Square, ctx.Settings.User.Altitude);
    }

    // call when satellite list or group changes
    // rebuilds list of valid satellites, recomputes all passes
    public void FullRebuild()
    {
      ListSatellites();

      var now = DateTime.UtcNow;
      Passes = ComputePasses(now, now + PredictionTimeSpan).ToList();

      if (OnlyGroup) ctx.Announcer.RebuildQueue();
    }

    // call when TLE changes
    // recomputes all active and future passes      
    public void Rebuild()
    {
      // keep historical passes
      var now = DateTime.UtcNow;
      var historyStartTime = now - HistoryTimeSpan;
      Passes = Passes.Where(p => p.EndTime > historyStartTime && p.EndTime < now).ToList();

      // recompute all current and future passes
      Passes.AddRange(ComputePasses(now, now + PredictionTimeSpan));

      if (OnlyGroup) ctx.Announcer.RebuildQueue();
    }

    // call every minute
    internal async Task PredictMorePassesAsync()
    {
      DeleteOld();

      if (LastPredictionTime == DateTime.MinValue) return;

      // add new
      var startTime = LastPredictionTime + PredictionTimeSpan;
      var endTime = DateTime.UtcNow + PredictionTimeSpan;

      var newPasses = await Task.Run(() => ComputePasses(startTime, endTime).Where(p => p.StartTime > startTime));

      Passes.AddRange(newPasses);
      if (OnlyGroup) ctx.Announcer.AddToQueue(newPasses);
    }

    // delete old passes but keep history
    private void DeleteOld()
    {
      var now = DateTime.UtcNow;
      var historyStartTime = now - HistoryTimeSpan;
      Passes = Passes.Where(p => p.EndTime > historyStartTime).ToList();
    }

    private IEnumerable<SatellitePass> ComputePasses(DateTime startTime, DateTime endTime)
    {
      LastPredictionTime = DateTime.UtcNow;

      return Satellites
        .SelectMany(sat => ComputePassesFor(sat, startTime, endTime))
        .OrderBy(p => p.StartTime);
    }

    public IEnumerable<SatellitePass> ComputePassesFor(SatnogsDbSatellite satellite, DateTime startTime, DateTime endTime)
    {
      IEnumerable<SatellitePass> result = new List<SatellitePass>();
      if (satellite.Tle == null) return result;

      var tracker = new Satellite(satellite.Tle.tle0, satellite.Tle.tle1, satellite.Tle.tle2);

      try
      {
        List<SatelliteVisibilityPeriod> passes;
        if (SatellitePass.IsGeoStationary(tracker)) passes = ComputeGeostationaryPasses(tracker);
        else passes = GroundStation.Observe(tracker, startTime, endTime, TimeSpan.FromSeconds(15), clipToStartTime: false);
        result = passes.Select(p => new SatellitePass(GroundStation, satellite, tracker, p));
      }
      catch (Exception e)
      {
        // no need to log all prediction exceptions
        //Log.Error(e, $"Pass computation failed for {satellite.name}: {e.Message}");
      }

      return result;
    }

    private List<SatelliteVisibilityPeriod> ComputeGeostationaryPasses(Satellite tracker)
    {
      var elevation = GroundStation.Observe(tracker, DateTime.UtcNow).Elevation;
      if (elevation < 0) return new();

      return new ()
      {
        new SatelliteVisibilityPeriod(
          tracker,
          DateTime.UtcNow,
          DateTime.UtcNow + TimeSpan.FromDays(1),
          elevation,
          DateTime.UtcNow + TimeSpan.FromHours(12)
          )
      };
    }


    private void ListSatellites()
    {
      IEnumerable<SatnogsDbSatellite> sats;

      if (OnlyGroup)
        sats = ctx.SatelliteSelector.GroupSatellites;
      else
        sats = ctx.SatnogsDb.Satellites.Where(
            s => s.Flags.HasFlag(SatelliteFlags.Vhf) || s.Flags.HasFlag(SatelliteFlags.Uhf));

      Satellites = sats.Where(sat => sat.Tle != null && sat.status == "alive").ToList();
    }

    private void CreateGroundStation(string gridSquare, double altitude)
    {
      var pos = GridSquare.ToGeoPoint(ctx.Settings.User.Square);
      var myLocation = new GeodeticCoordinate(Angle.FromRadians(pos.LatitudeRad), Angle.FromRadians(pos.LongitudeRad), altitude / 1000d);
      GroundStation = new GroundStation(myLocation);
    }

    internal TopocentricObservation ObserveSatellite(SatnogsDbSatellite satellite, DateTime utcNow)
    {
      return GroundStation.Observe(satellite.Tracker, utcNow);
    }

    internal SatellitePass? GetNextPass(SatnogsDbSatellite satellite)
    {
      var now = DateTime.UtcNow;
      return ComputePassesFor(satellite, now, now.AddDays(1)).OrderBy(pass => pass.StartTime).FirstOrDefault();
    }
  }
}
