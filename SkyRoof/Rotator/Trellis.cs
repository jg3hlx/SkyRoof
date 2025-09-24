using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using VE3NEA;

namespace SkyRoof
{
  public class Trellis
  {
    private const double twoPi = 2 * Math.PI;
    private const double ANGLE_TOLERANCE = 0.1 * Geo.RinD;

    private readonly List<Bearing> rawPath;
    private readonly RectangleF feasibleRect;
    private readonly float maxErrorDeg;
    public readonly List<List<Bearing>> States;

    public int Count => States.Count;
    public List<Bearing> this[int index] => States[index];

    public Trellis(List<Bearing> rawPath, RectangleF feasibleRect, float maxErrorDeg)
    {
      this.rawPath = rawPath;
      this.feasibleRect = feasibleRect;
      this.maxErrorDeg = maxErrorDeg;
      States = rawPath.Select(BuildFeasibleNodes).ToList();
      InterpolateHighElevationSegments();
    }

    public List<Bearing> BuildFeasibleNodes(Bearing originalNode)
    {
      float expand = maxErrorDeg * (float)Geo.RinD;
      var toleranceRect = RectangleF.Inflate(feasibleRect, expand, expand);

      var allCandidates = new List<Bearing>();
      var candidates = new List<Bearing>();
      var clampedCandidates = new List<Bearing>();

      // generate alternative nodes by unwraping the azimuth and flipping the elevation
      List<Bearing> flippedCandidates = [originalNode, new Bearing(originalNode.Az + Math.PI, Math.PI - originalNode.El)];
      foreach (var node in flippedCandidates)
        foreach (var k in new[] { -2, -1, 0, 1, 2 })
        {
          double azu = node.Az + twoPi * k;
          var newNode = new Bearing(azu, node.El).Clamp(feasibleRect);
          allCandidates.Add(newNode);

          if (toleranceRect.Contains((float)azu, (float)node.El))
            candidates.Add(newNode);
          else
            clampedCandidates.Add(newNode);
        }

      var minAngle = allCandidates.Min(cand => originalNode.AngleFrom(cand));
      clampedCandidates = clampedCandidates.Where(n => originalNode.AngleFrom(n) - minAngle < ANGLE_TOLERANCE).ToList();

      candidates.AddRange(clampedCandidates);
      return candidates;
    }

    private void InterpolateHighElevationSegments()
    {
      double highElevationThreshold = (90 - 2 * maxErrorDeg) * Geo.RinD;
      double ninetyDegrees = 90 * Geo.RinD;
      double rotationTimeTolerance = 0.01; // Tolerance for considering rotation times as equal

      List<(int start, int end)> highElevationSegments = FindHighElevationSegments(highElevationThreshold);

      foreach (var (start, end) in highElevationSegments)
      {
        if (end <= start || end >= States.Count) continue;

        var firstState = States[start];
        var lastState = States[end];
        double minRotationTime = double.MaxValue;
        var bestPairs = new List<(Bearing firstNode, Bearing lastNode)>();

        // First pass: find the minimum rotation time
        foreach (var firstNode in firstState)
          foreach (var lastNode in lastState)
          {
            // Only consider pairs where one node has elevation < 90° and the other > 90°
            bool firstNodeLessThan90 = firstNode.El < ninetyDegrees;
            bool lastNodeLessThan90 = lastNode.El < ninetyDegrees;

            // Skip if both nodes are on the same side of 90°
            if (firstNodeLessThan90 == lastNodeLessThan90)
              continue;

            double rotationTime = firstNode.RotationTime(lastNode);
            if (rotationTime < minRotationTime)
            {
              minRotationTime = rotationTime;
              bestPairs.Clear();
              bestPairs.Add((firstNode, lastNode));
            }
            else if (Math.Abs(rotationTime - minRotationTime) <= rotationTimeTolerance)
            {
              bestPairs.Add((firstNode, lastNode));
            }
          }

        // Second pass: interpolate using all the best pairs within tolerance
        foreach (var (bestFirstNode, bestLastNode) in bestPairs)
        {
          for (int i = start + 1; i < end; i++)
          {
            double factor = (double)(i - start) / (end - start);
            double interpAz = bestFirstNode.Az + factor * (bestLastNode.Az - bestFirstNode.Az);
            double interpEl = bestFirstNode.El + factor * (bestLastNode.El - bestFirstNode.El);
            var interpNode = new Bearing(interpAz, interpEl);
            interpNode = interpNode.Clamp(feasibleRect);
            States[i].Add(interpNode);
          }
        }
      }
    }

    private List<(int start, int end)> FindHighElevationSegments(double threshold)
    {
      var segments = new List<(int start, int end)>();
      int? segmentStart = null;
      for (int i = 0; i < rawPath.Count; i++)
      {
        if (rawPath[i].El > threshold)
          segmentStart ??= i;
        else if (segmentStart.HasValue)
        {
          segments.Add((segmentStart.Value, i - 1));
          segmentStart = null;
        }
      }

      if (segmentStart.HasValue) segments.Add((segmentStart.Value, rawPath.Count - 1));
      return segments;
    }
  }
}