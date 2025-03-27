using System.Drawing.Printing;
using SGPdotNET.Observation;
using SGPdotNET.Util;
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
    public SatnogsDbSatellite Satellite;

    public DateTime StartTime, CulminationTime, EndTime;
    public double MaxElevation;
    public int OrbitNumber;
    public bool Geostationary { get; private set; }
    private SatelliteVisibilityPeriod SatelliteVisibilityPeriod;

    public PointF[]? MiniPath;
    private PointF[] hump;
    private List<TrackPoint> track;
    public List<TrackPoint> Track { get => track ?? MakeTrack(); }



    public SatellitePass(GroundStation groundStation, SatnogsDbSatellite satellite, Satellite tracker, SatelliteVisibilityPeriod visibilityPeriod)
    {
      GroundStation = groundStation;
      Satellite = satellite;
      SatelliteVisibilityPeriod = visibilityPeriod;
      StartTime = visibilityPeriod.Start;
      CulminationTime = visibilityPeriod.MaxElevationTime;
      EndTime = visibilityPeriod.End;
      MaxElevation = visibilityPeriod.MaxElevation.Degrees;
      OrbitNumber = ComputeOrbitNumber();
      Geostationary = IsGeoStationary(tracker);
    }






    //----------------------------------------------------------------------------------------------
    //                                         get
    //----------------------------------------------------------------------------------------------
    public static bool IsGeoStationary(Satellite tracker)
    {
      return Math.Abs(tracker.Tle.MeanMotionRevPerDay - 1) < 0.1f;
    }

    public TrackPoint GetTrackPointAt(DateTime utc)
    {
      if (utc >= Track.First().Utc && utc <= Track.Last().Utc)
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

      var trackPoint = new TrackPoint();
      trackPoint.Utc = utc;
      trackPoint.Observation = GroundStation.Observe(Satellite.Tracker, trackPoint.Utc);
      return trackPoint;
    }

    internal TopocentricObservation GetObservationAt(DateTime utc)
    {
      return GroundStation.Observe(Satellite.Tracker, utc);
    }

    internal bool IsActive()
    {
      return StartTime < DateTime.UtcNow && EndTime > DateTime.UtcNow;
    }

    internal string[] GetTooltipText(bool showSeconds = true)
    {
      if (Geostationary) return [$"Geostationary   orbit #{OrbitNumber}", "", "", "", "", ""];

      var elevation = GetTrackPointAt(DateTime.UtcNow).Observation.Elevation.Degrees;
      var nextElevation = GetTrackPointAt(DateTime.UtcNow.AddMinutes(1)).Observation.Elevation.Degrees;
      string upDpwn = nextElevation > elevation ? "↑" : "↓";
  
      string[] tooltip = new string[6];

      if (EndTime < DateTime.UtcNow) tooltip[0] = "Ended.";
      else if (StartTime < DateTime.UtcNow) tooltip[0] = $"LOS ↓  in {Utils.TimespanToString(EndTime - DateTime.UtcNow, showSeconds)}";
      else tooltip[0] = $"AOS ↑  in {Utils.TimespanToString(StartTime - DateTime.UtcNow, showSeconds)}";



      tooltip[1] = $"{StartTime.ToLocalTime():yyyy-MM-dd}";
      tooltip[2] = $"{StartTime.ToLocalTime():HH:mm:ss} to {EndTime.ToLocalTime():HH:mm:ss}";
      tooltip[3] = $"Duration: {Utils.TimespanToString(EndTime - StartTime, false)}";
      tooltip[4] = $"Elevation: {elevation:F0}º {upDpwn}  (Max {MaxElevation:F0}º)";
      tooltip[5] = $"Orbit: #{OrbitNumber}";

      return tooltip;
    }




    //----------------------------------------------------------------------------------------------
    //                                        compute
    //----------------------------------------------------------------------------------------------
    private int ComputeOrbitNumber()
    {
      var utc = SatelliteVisibilityPeriod.MaxElevationTime;
      uint revNum = Satellite.Tracker.Tle.OrbitNumber;
      var timeSinceOrbit = (utc - Satellite.Tracker.Tle.Epoch).TotalDays;
      var revPerDay = Satellite.Tracker.Tle.MeanMotionRevPerDay;
      var drag = Satellite.Tracker.Tle.BStarDragTerm;
      var meanAnomaly = Satellite.Tracker.Tle.MeanAnomaly.Radians;
      var argumentPerigee = Satellite.Tracker.Tle.ArgumentPerigee.Radians;

      return (int)(
        revNum +
        Math.Floor((meanAnomaly + argumentPerigee) / TwoPi)
        + (revPerDay + drag * timeSinceOrbit) * timeSinceOrbit);

      // (int)((revPerDay + drag * timeSinceOrbit) * timeSinceOrbit + (meanAnomaly + argumentPerigee) / TwoPi)
      // - (int)((meanAnomaly + argumentPerigee) / TwoPi)
      // + (int)revNum;
    }

    public PointF[] ComputeHump(float x0, float y0, float scaleX, float scaleY)
    {
      for (int i = 0; i < Track.Count; i++)
        hump[i] = new(
          x0 + (float)(Track[i].Utc - StartTime).TotalMinutes * scaleX, 
          y0 - (float)Track[i].Observation.Elevation.Degrees * scaleY);

      return hump;
    }

    private List<TrackPoint> MakeTrack()
    {
      int stepCount = 63;
      if (Geostationary) stepCount = 3;
      TimeSpan step = (EndTime - StartTime) / stepCount;
      if (step < TimeSpan.FromSeconds(10)) step = TimeSpan.FromSeconds(10);
      stepCount = (int)Math.Round((EndTime - StartTime) / step);

      track = new();
      
      for (int i=0; i <= stepCount; i++)
        {
          var trackPoint = new TrackPoint();
          trackPoint.Utc = StartTime + i * step;
          trackPoint.Observation = GroundStation.Observe(Satellite.Tracker, trackPoint.Utc);
          track.Add(trackPoint);
        }

      hump = new PointF[track.Count];

      return track;
    }


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
        var observation = GroundStation.Observe(Satellite.Tracker, utc);

        double ro = 1 - observation.Elevation.Radians / Utils.HalfPi;
        double phi = Utils.HalfPi - observation.Azimuth.Radians;

        MiniPath[i] = new PointF((float)(ro * Math.Cos(phi)), (float)(ro * Math.Sin(phi)));
      }
    }

    // currently not used
    public List<GeoPath> GetCoveragePolygon()
    {
      // only for LEO
      if (EndTime - StartTime > TimeSpan.FromHours(2)) return new();

      var leftPoints = new List<GeoPoint>();
      var rightPoints = new List<GeoPoint>();

      var predictions = Track.Select(t => Satellite.Tracker.Predict(t.Utc).ToGeodetic()).ToArray();

      for (int i = 0; i < predictions.Length; i++)
      {
        var prevP = predictions[Math.Max(0, i-1)];
        var P = predictions[i];
        var nextP = predictions[Math.Min(predictions.Length-1, i+1)];

        var prevG = new GeoPoint(prevP.Latitude.Degrees, prevP.Longitude.Degrees);
        var G = new GeoPoint(P.Latitude.Degrees, P.Longitude.Degrees);
        var nextG = new GeoPoint(nextP.Latitude.Degrees, nextP.Longitude.Degrees);

        var azim = (nextG - prevG).Azimuth;
        var radius = P.GetFootprint();

        var pointL = G + new GeoPath(azim - 90, radius);
        var pointR = G + new GeoPath(azim + 90, radius);

        if (i == 0)
          for (double a = 1; a < 180; a+=1)
            leftPoints.Add(G + new GeoPath(azim + 90 + a, radius));

        leftPoints.Add(pointL);
        rightPoints.Add(pointR);

        if (i == predictions.Length-1)
          for (double a = 1; a < 180; a+=1)
            leftPoints.Add(G + new GeoPath(azim - 90 + a, radius));
      }

      rightPoints.Reverse();
      leftPoints.AddRange(rightPoints);
      
      // close the loop
      leftPoints.Add(leftPoints[0]);

      // lat/lon to az/dist
      var homeP = GroundStation.Location.ToGeodetic();
      var homeG = new GeoPoint(homeP.Latitude.Degrees, homeP.Longitude.Degrees);
      return leftPoints.Select(g => g - homeG).ToList();
    }
  }
}
