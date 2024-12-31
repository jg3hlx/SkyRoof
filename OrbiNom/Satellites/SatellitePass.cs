using SGPdotNET.Observation;
using SGPdotNET.Util;

namespace OrbiNom
{
  public class SatellitePass
  {
    public class TrackPoint
    {
      public DateTime Utc;
      public TopocentricObservation Observation;
    }

    public SatnogsDbSatellite Satellite;
    public Satellite SatTracker;
    public DateTime StartTime, CulminationTime, EndTime;
    public double MaxElevation;
    public int OrbitNumber;
    public List<string> DownLinks = new();
    public List<TrackPoint> Track;

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
  }
}
