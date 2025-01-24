using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.CompilerServices;
using SGPdotNET.Observation;
using SGPdotNET.Util;
using static System.Net.Mime.MediaTypeNames;
using VE3NEA;

namespace OrbiNom
{
  public class SatellitePass
  {
    public class TrackPoint
    {
      public DateTime Utc;
      public TopocentricObservation Observation;
    }

    private const double TwoPi = 2 * Math.PI;
    private readonly TimeSpan TrackStep = TimeSpan.FromSeconds(10);

    private GroundStation GroundStation;
    private PointF[] hump;
    public SatnogsDbSatellite Satellite;
    public Satellite SatTracker;
    public DateTime StartTime, CulminationTime, EndTime;
    public double MaxElevation;
    public int OrbitNumber;
    
    public List<TrackPoint> Track { get => track ?? MakeTrack(); }


    public PointF[] GetHump(float x0, float y0, float scaleX, float scaleY)
    {
      for (int i = 0; i < Track.Count; i++)
        hump[i] = new(
          x0 + (float)(Track[i].Utc - StartTime).TotalMinutes * scaleX, 
          y0 - (float)Track[i].Observation.Elevation.Degrees * scaleY);

      return hump;
    }

    private List<TrackPoint> MakeTrack()
    {
      int setpCount = (int)Math.Max(2, (EndTime - StartTime) / TrackStep);
      TimeSpan step = (EndTime - StartTime) / setpCount;

      track = new();
      
      for (int i=0; i <= setpCount; i++)
        {
          var trackPoint = new TrackPoint();
          trackPoint.Utc = StartTime + i * step;
          trackPoint.Observation = GroundStation.Observe(SatTracker, trackPoint.Utc);
          track.Add(trackPoint);
        }

      hump = new PointF[track.Count];

      return track;
    }

    private Satellite tracker;
    private SatelliteVisibilityPeriod SatelliteVisibilityPeriod;
    public PointF[]? MiniPath;
    private List<TrackPoint> track;

    public SatellitePass(GroundStation groundStation, SatnogsDbSatellite satellite, Satellite tracker, SatelliteVisibilityPeriod visibilityPeriod)
    {
      GroundStation = groundStation;
      Satellite = satellite;
      SatTracker = tracker;
      SatelliteVisibilityPeriod = visibilityPeriod;
      StartTime = visibilityPeriod.Start;
      CulminationTime = visibilityPeriod.MaxElevationTime;
      EndTime = visibilityPeriod.End;
      MaxElevation = visibilityPeriod.MaxElevation.Degrees;
      OrbitNumber = ComputeOrbitNumber();
    }

    private int ComputeOrbitNumber()
    {
      var utc = SatelliteVisibilityPeriod.MaxElevationTime;
      uint revNum = SatTracker.Tle.OrbitNumber;
      var timeSinceOrbit = (utc - SatTracker.Tle.Epoch).TotalDays;
      var revPerDay = SatTracker.Tle.MeanMotionRevPerDay;
      var drag = SatTracker.Tle.BStarDragTerm;
      var meanAnomaly = SatTracker.Tle.MeanAnomaly.Radians;
      var argumentPerigee = SatTracker.Tle.ArgumentPerigee.Radians;

      return (int)(
        revNum +
        Math.Floor((meanAnomaly + argumentPerigee) / TwoPi)
        + (revPerDay + drag * timeSinceOrbit) * timeSinceOrbit);

      // (int)((revPerDay + drag * timeSinceOrbit) * timeSinceOrbit + (meanAnomaly + argumentPerigee) / TwoPi)
      // - (int)((meanAnomaly + argumentPerigee) / TwoPi)
      // + (int)revNum;
    }

    const double HalfPi = Math.PI / 2;
    private const int StepCount = 10;
    public void MakeMiniPath()
    {
      if (MiniPath != null) return;

      var duration = EndTime - StartTime;
      var step = duration / StepCount;
      MiniPath = new PointF[StepCount + 1];

      for (int i = 0; i <= StepCount; i++)
      {
        var utc = StartTime + step * i;
        var observation = GroundStation.Observe(SatTracker, utc);

        double ro = 1 - observation.Elevation.Radians / HalfPi;
        double phi = HalfPi - observation.Azimuth.Radians;

        MiniPath[i] = new PointF((float)(ro * Math.Cos(phi)), (float)(ro * Math.Sin(phi)));
      }
    }

    public TrackPoint GetTrackPointAt(DateTime utc)
    {
      if (utc < Track.First().Utc) return Track.First();

      for (int i = 1; i < Track.Count; i++)
        if (Track[i - 1].Utc <= utc && Track[i].Utc > utc)
        {
          var r = (utc - Track[i - 1].Utc).TotalSeconds / (Track[i].Utc - Track[i - 1].Utc).TotalSeconds;
          var rangeRate = Track[i - 1].Observation.RangeRate * (1 - r) + Track[i].Observation.RangeRate * r;
          var elevation = Track[i - 1].Observation.Elevation.Radians * (1 - r) + Track[i].Observation.Elevation.Radians * r;

          var point = new TrackPoint();
          point.Utc = utc;
          point.Observation = new(0, Angle.FromRadians(elevation), 0, rangeRate);
          return point;
        }
      return Track.Last();
    }

    internal string GetTooltipText()
    {
      string tooltip = "";

      if (StartTime < DateTime.UtcNow) tooltip += "Started\n";
      else if (EndTime < DateTime.UtcNow) tooltip += "Ended\n";
      else tooltip += $"in {Utils.TimespanToString(StartTime - DateTime.UtcNow)}\n";
       
      tooltip += $"{StartTime.ToLocalTime():yyyy-MM-dd\nHH:mm:ss}  to  {EndTime.ToLocalTime():HH:mm:ss}\n";
      tooltip += $"Duration: {Utils.TimespanToString(EndTime - StartTime, false)}\n";
      tooltip += $"Max Elevation: {MaxElevation:F0}°\n";
      tooltip += $"Orbit: #{OrbitNumber}\n";

      return tooltip;
    }
  }
}
