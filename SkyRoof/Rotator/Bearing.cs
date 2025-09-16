using System;
using System.Drawing;
using VE3NEA;

namespace SkyRoof
{
  public class Bearing
  {
    // Stored in radians
    public double Az;
    public double El;

    // Degree properties for display
    public double AzDeg => Az * 180.0 / Math.PI;
    public double ElDeg => El * 180.0 / Math.PI;
    
    // Constructor for radians only
    public Bearing(double azRad, double elRad) 
    { 
      Az = azRad; 
      El = elRad; 
    }

    public double RotationTime(Bearing other)
    {
      // Assuming that elevation changes take twice as long as azimuth changes
      var cost1 = Math.Abs(Az - other.Az);
      var cost2 = 2 * Math.Abs(El - other.El);

      // Rotation time includes small penalty for changing both azimuth and elevation
      var cost3 = cost1 + 0.01 * cost2;
      var cost4 = cost2 + 0.01 * cost1;

      return Math.Max(cost3, cost4);
    }

    public double Angle(Bearing other, bool azOnly = false)
    {
      if (other == null) throw new ArgumentNullException(nameof(other));

      // rotator is not elevation capable, check only azimuth
      if (azOnly) 
      {
        double az1 = Trig.NormalizeTwoPi(Az);
        double az2 = Trig.NormalizeTwoPi(other.Az);
        double azDiff = Math.Abs(az1 - az2);
        if (azDiff > Math.PI) azDiff = Geo.TwoPi - azDiff;
        return azDiff;
      }

      double cosAngle = Math.Sin(El) * Math.Sin(other.El) +
        Math.Cos(El) * Math.Cos(other.El) * Math.Cos(Az - other.Az);

      cosAngle = Math.Clamp(cosAngle, -1.0, 1.0);
      return Math.Acos(cosAngle);
    }

    public Bearing Clamp(RectangleF rect)
    {
      return new Bearing(
          Math.Clamp(Az, rect.Left, rect.Right),
          Math.Clamp(El, rect.Top, rect.Bottom)
      );
    }

    //public double AngleBetween(Bearing b1, Bearing b2)
    //{
    //  if (b1 == null || b2 == null) throw new ArgumentNullException();

    //  // Haversine formula
    //  double se = Math.Sin((b2.El - b1.El) / 2);
    //  double ce = Math.Cos(b1.El) * Math.Cos(b2.El);
    //  double sa = Math.Sin((b2.Az - b1.Az) / 2);
    //  double a = se * se + ce * sa * sa;
    //  return Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)) * 2;
    //}
    //public double AzimuthDifference(Bearing other)
    //{
    //  // Use NormalizeTwoPi to handle wraparound correctly
    //  double az1 = Trig.NormalizeTwoPi(Az);
    //  double az2 = Trig.NormalizeTwoPi(other.Az);
    //  double azDiff = Math.Abs(az1 - az2);
    //  if (azDiff > Math.PI) azDiff = Trig.TwoPi - azDiff;
    //  return azDiff;
    //}

    // Equality operators
    public static bool operator ==(Bearing? b1, Bearing? b2)
    {
      if (ReferenceEquals(b1, b2)) return true;
      if (b1 is null || b2 is null) return false;
      return b1.Az == b2.Az && b1.El == b2.El;
    }

    public static bool operator !=(Bearing? b1, Bearing? b2)
    {
      return !(b1 == b2);
    }

    public override string ToString()
    {
      return $"Az: {AzDeg:F1}°, El: {ElDeg:F1}°";
    }
  }
}