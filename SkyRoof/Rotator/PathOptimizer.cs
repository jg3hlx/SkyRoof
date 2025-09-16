using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SkyRoof
{
  public class PathOptimizer
  {
    private readonly RectangleF feasibleRect;
    private readonly float maxErrorDeg;

    private const double COST_TOLERANCE = 1e-3;
    private const double ANGLE_TOLERANCE = 0.1 * Math.PI / 180;
    private const int MAX_PATHS_PER_NODE = 2;

    // Store the DP table as an instance variable so it can be accessed by other classes
    internal List<Dictionary<Bearing, List<(double cost, Bearing prev)>>> DP { get; private set; }
    
    // Store the trellis as an instance variable
    internal Trellis Trellis { get; private set; }

    public PathOptimizer(RectangleF feasibleRect, float maxErrorDeg)
    {
      this.feasibleRect = feasibleRect;
      this.maxErrorDeg = maxErrorDeg;
      this.DP = new List<Dictionary<Bearing, List<(double cost, Bearing prev)>>>();
    }

    public List<Bearing> OptimizePath(List<Bearing> rawPath, Bearing? currentDirection = null)
    {
      if (rawPath == null || rawPath.Count == 0) return new List<Bearing>();

      var current = currentDirection ?? new Bearing(0, 0);

      // Build and enhance trellis using the Trellis class
      Trellis = new Trellis(rawPath, feasibleRect, maxErrorDeg);

      // Build DP table
      DP = BuildDPTable(Trellis);

      // Find and compare best paths
      return FindBestPath(DP, Trellis, current);
    }

    private List<Dictionary<Bearing, List<(double cost, Bearing prev)>>> BuildDPTable(Trellis trellis)
    {
      int n = trellis.Count;

      // Store two paths for each node: best and second best
      var dp = new List<Dictionary<Bearing, List<(double cost, Bearing prev)>>>();
      for (int i = 0; i < n; i++) dp.Add(new Dictionary<Bearing, List<(double cost, Bearing prev)>>());

      // Initialize first layer
      foreach (var node in trellis[0])
        dp[0][node] = new List<(double cost, Bearing prev)> { (0, null) };

      // Fill DP table
      for (int i = 1; i < n; i++)
      {
        foreach (var node in trellis[i])
        {
          var paths = new List<(double cost, Bearing prev)>();

          foreach (var prev in trellis[i - 1])
          {
            if (!dp[i - 1].ContainsKey(prev)) continue;

            foreach (var (prevCost, _) in dp[i - 1][prev])
            {
              double cost = prevCost + prev.RotationTime(node);
              paths.Add((cost, prev));
            }
          }

          if (paths.Count > 0)
          {
            // Keep at most MAX_PATHS_PER_NODE best paths for each node
            dp[i][node] = paths.OrderBy(p => p.cost).Take(MAX_PATHS_PER_NODE).ToList();
          }
        }
      }

      return dp;
    }

    private List<Bearing> FindBestPath(List<Dictionary<Bearing, List<(double cost, Bearing prev)>>> dp,
                                     Trellis trellis, Bearing current)
    {
      int n = trellis.States.Count;

      // Find the two best overall paths
      var allPaths = new List<(double cost, Bearing endNode, int pathIndex)>();

      foreach (var kvp in dp[n - 1])
      {
        for (int i = 0; i < kvp.Value.Count; i++)
        {
          allPaths.Add((kvp.Value[i].cost, kvp.Key, i));
        }
      }

      var bestTwo = allPaths.OrderBy(p => p.cost).Take(MAX_PATHS_PER_NODE).ToList();

      if (bestTwo.Count == 0)
        return new List<Bearing>(); // No valid paths found

      var path1 = ReconstructPath(bestTwo[0].endNode, bestTwo[0].pathIndex, dp);
      var path2 = bestTwo.Count > 1 ? ReconstructPath(bestTwo[1].endNode, bestTwo[1].pathIndex, dp) : null;

      // If only one path exists, return it
      if (path2 == null)
        return path1;

      // Compare costs with small tolerance
      double cost1 = bestTwo[0].cost;
      double cost2 = bestTwo[1].cost;

      if (Math.Abs(cost1 - cost2) <= COST_TOLERANCE)
      {
        // Costs are effectively equal, compare initial rotation time
        var rt1 = current.RotationTime(path1[0]);
        var rt2 = current.RotationTime(path2[0]);
        return rt1 <= rt2 ? path1 : path2;
      }
      else
      {
        // Return the path with lower cost
        return cost1 < cost2 ? path1 : path2;
      }
    }

    private List<Bearing> ReconstructPath(Bearing endNode, int pathIndex, List<Dictionary<Bearing, List<(double cost, Bearing prev)>>> dp)
    {
      var path = new List<Bearing>();
      Bearing current = endNode;
      int currentPathIdx = pathIndex;

      for (int i = dp.Count - 1; i >= 0; i--)
      {
        path.Add(current);

        if (i > 0 && dp[i].ContainsKey(current) && currentPathIdx < dp[i][current].Count)
        {
          var prev = dp[i][current][currentPathIdx].prev;

          // Find the index of the path from prev to current
          if (prev != null && dp[i - 1].ContainsKey(prev))
          {
            // Use the first (best) path from the previous node
            currentPathIdx = 0;
            current = prev;
          }
          else break;
        }
        else break;
      }

      path.Reverse();
      return path;
    }
    
    // Helper method to get the cost for a specific node in a specific state
    internal double? GetNodeCost(int stateIndex, Bearing node)
    {
      if (stateIndex < 0 || stateIndex >= DP.Count || !DP[stateIndex].ContainsKey(node))
        return null;
        
      // Return the best cost for this node
      return DP[stateIndex][node].FirstOrDefault().cost;
    }
  }
}