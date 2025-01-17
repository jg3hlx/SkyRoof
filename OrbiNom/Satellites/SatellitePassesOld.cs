using System.Diagnostics;
using System.Text;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.Util;
using VE3NEA;

namespace OrbiNom
{
  public class SatellitePassesOld
  {
    private readonly IEnumerable<SatnogsDbSatellite> Satellites;
    private GroundStation GroundStation;
    private DateTime EndTime;
    private System.Windows.Forms.Timer Timer = new();

    public List<SatellitePass> Passes { get; private set; } = new();
    public List<SatellitePass> ActivePasses { get => Passes.Where(p => p.StartTime <= DateTime.UtcNow.AddMinutes(0) && p.EndTime > DateTime.UtcNow).ToList(); }
    public event EventHandler Changed;


    public SatellitePassesOld(GeoPoint observerLocation, IEnumerable<SatnogsDbSatellite> satellites)
    {
      Satellites = satellites;
      CreateGroundStation(observerLocation);
      
      //{!} ComputePasses(true);

      Timer.Interval = 60_000;
      Timer.Tick += Timer_Tick;
      //{!} Timer.Start();
    }

    public void Rebuild()
    {
      // keep the passes that already ended
      var minTime = DateTime.UtcNow;
      Passes = Passes.Where(p => p.EndTime < minTime).ToList();

      // recompute active and future passes
      ComputePasses(true);
    }

    public void Update()
    {
      // delete old
      int countBefore = Passes.Count;
      var minTime = DateTime.UtcNow.AddHours(-1);
      Passes = Passes.Where(p => p.EndTime > minTime).ToList();
      bool changed = Passes.Count != countBefore;

      // add new
      countBefore = Passes.Count;
      ComputePasses(false);
      changed = changed || Passes.Count != countBefore;

      // notify
      if (changed) Changed?.Invoke(this, EventArgs.Empty);
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
      Update();
    }

    private void CreateGroundStation(GeoPoint p)
    {
      var myLocation = new GeodeticCoordinate(Angle.FromRadians(p.LatitudeRad), Angle.FromRadians(p.LongitudeRad), 0);
      GroundStation = new GroundStation(myLocation);
    }

    private void ComputePasses(bool rebuild)
    {
      var delta = TimeSpan.FromSeconds(15);
      DateTime startTime = rebuild ? DateTime.UtcNow : EndTime;
      EndTime = DateTime.UtcNow.AddMinutes(65);

      var satellites = Satellites.Where(s => s.status == "alive" && s.Tle != null && HamTransmitters(s.Transmitters).Any());

      foreach (SatnogsDbSatellite sat in satellites)
        try
        {
          var satTracker = new Satellite(sat.Tle.tle0, sat.Tle.tle1, sat.Tle.tle2);
          var passes = GroundStation.Observe(satTracker, startTime, EndTime, delta, clipToStartTime: false);

          foreach (var p in passes)
          {
            var orbitNumber = ComputeOrbitNumber(satTracker, p.MaxElevationTime);
            if (Passes.Any(p => p.Satellite == sat && p.OrbitNumber == orbitNumber)) continue;

            if (rebuild || p.Start > startTime)
            {
              var pass = new SatellitePass(GroundStation, sat, satTracker, p);
              ComputeTrack(pass);
              Passes.Add(pass);
            }
          }
        }
        catch (Exception e)
        {
          Debug.WriteLine(sat.name);
        }
    }

    readonly TimeSpan Margin = TimeSpan.FromSeconds(300);

    private void ComputeTrack(SatellitePass pass)
    {
      if (pass.Track != null) return;

      pass.Track = new();


      for (DateTime t = pass.StartTime - Margin; t < pass.EndTime + Margin; t = t.AddSeconds(5))
      {
        var point = new SatellitePass.TrackPoint();
        point.Utc = t;
        point.Observation = GroundStation.Observe(pass.SatTracker, t);

        pass.Track.Add(point);
      }
    }

    private const double TwoPi = 2 * Math.PI;


    private int ComputeOrbitNumber(Satellite satellite, DateTime utc)
    {
      uint revNum = satellite.Tle.OrbitNumber;
      var timeSinceOrbit = (utc - satellite.Tle.Epoch).TotalDays;
      var revPerDay = satellite.Tle.MeanMotionRevPerDay;
      var drag = satellite.Tle.BStarDragTerm;
      var meanAnomaly = satellite.Tle.MeanAnomaly.Radians;
      var argumentPerigee = satellite.Tle.ArgumentPerigee.Radians;


      //return (int)(orbit + timeSinceOrbit.TotalDays * orbitPeriod);

      return (int)(
        revNum +
        Math.Floor((meanAnomaly + argumentPerigee) / TwoPi)
        + (revPerDay + drag * timeSinceOrbit) * timeSinceOrbit);

      return 
        (int)((revPerDay + drag * timeSinceOrbit) * timeSinceOrbit + (meanAnomaly + argumentPerigee) / TwoPi)
        - (int)((meanAnomaly + argumentPerigee) / TwoPi)
        + (int)revNum;
    }

    public static IEnumerable<SatnogsDbTransmitter> HamTransmitters(IEnumerable<SatnogsDbTransmitter> transmitters)
    {
      return transmitters.Where(t => t.IsHamBand());
    }

    public string FormatAsString()
    {
      var passes = Passes.OrderBy(p => p.StartTime);
      var t1 = DateTime.UtcNow;
      var t2 = t1.AddMinutes(5);

      var builder = new StringBuilder();

      foreach (var pass in passes.Where(p => p.EndTime < t1))
        foreach (var link in pass.Satellite.TransmittersHint)
          builder.AppendLine($"{pass.Satellite.name,-20}  {pass.OrbitNumber,6}   {pass.StartTime.ToLocalTime():HH:mm:ss}   {pass.CulminationTime.ToLocalTime():HH:mm:ss}   {pass.EndTime.ToLocalTime():HH:mm:ss}   {pass.MaxElevation,2:F0}    {link}");

      builder.AppendLine("------------");

      foreach (var pass in passes.Where(p => p.StartTime < t1 && p.EndTime > t1))
        foreach (var link in pass.Satellite.TransmittersHint)
          builder.AppendLine($"{pass.Satellite.name,-20}  {pass.OrbitNumber,6}   {pass.StartTime.ToLocalTime():HH:mm:ss}   {pass.CulminationTime.ToLocalTime():HH:mm:ss}   {pass.EndTime.ToLocalTime():HH:mm:ss}   {pass.MaxElevation,2:F0}    {link}");

      builder.AppendLine("------------");

      foreach (var pass in passes.Where(p => p.StartTime > t1))
        foreach (var link in pass.Satellite.TransmittersHint)
          builder.AppendLine($"{pass.Satellite.name,-20}  {pass.OrbitNumber,6}   {pass.StartTime.ToLocalTime():HH:mm:ss}   {pass.CulminationTime.ToLocalTime():HH:mm:ss}   {pass.EndTime.ToLocalTime():HH:mm:ss}   {pass.MaxElevation,2:F0}    {link}");

      return builder.ToString();
    }

    private IEnumerable<string> FormatTransmitters(IEnumerable<SatnogsDbTransmitter> transmitters)
    {
      return HamTransmitters(transmitters)
        .Where(s => s.alive)
        .GroupBy(t => t.downlink_low).Select(d => $"{d.Key / 1e6:F3}   {string.Join(", ", d.Select(r => r.mode).Distinct())}").Order();
    }

    internal IEnumerable<SatellitePass> ComputeFor(SatnogsDbSatellite satellite, DateTime startTime, DateTime endTime)
    {
      var result = new List<SatellitePass>();
      if (satellite.Tle == null) return result;

      var sat = new Satellite(satellite.Tle.tle0, satellite.Tle.tle1, satellite.Tle.tle2);

      try
      {
        var passes = GroundStation.Observe(sat, startTime, endTime, TimeSpan.FromSeconds(15), clipToStartTime: false);

        var delta = TimeSpan.FromSeconds(15);

        foreach (var p in passes)
        {
          //var pass = new SatellitePass();
          //pass.Satellite = satellite;
          //pass.SatTracker = sat;
          //pass.StartTime = p.Start;
          //pass.CulminationTime = p.MaxElevationTime;
          //pass.EndTime = p.End;
          //pass.MaxElevation = p.MaxElevation.Degrees;
          //pass.TransmittersHint = FormatTransmitters(satellite.Transmitters).ToList();
          //pass.OrbitNumber = ComputeOrbitNumber(sat, p.MaxElevationTime);
          //ComputeTrack(pass);
          //result.Add(pass);
        }
      }
      catch (Exception e)
      {
        Debug.WriteLine(satellite.name);
      }

      return result;
    }
  }
}
