using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VE3NEA
{
  public static class Trig
  {
    public const double HalfPi = Math.PI / 2;
    public const double TwoPi = 2 * Math.PI;

    public static double NormalizeTwoPi(double angle)
    {
      double result = angle % TwoPi;
      if (result < 0) result += TwoPi;
      return result;
    }
  }
}
