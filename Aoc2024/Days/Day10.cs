using System.Collections.Immutable;
using Aoc2024.Lib;
using Aoc2024.Lib.Matrix;

namespace Aoc2024.Days;

public sealed class Day10 : IDay
{
    private const string TestInput1 =
        """
        0123
        1234
        8765
        9876
        """;

    private const string TestInput2 =
        """
        89010123
        78121874
        87430965
        96549874
        45678903
        32019012
        01329801
        10456732
        """;

    [Example(TestInput1, 1)]
    [Example(TestInput2, 36)]
    public long Part1(IInput input)
    {
        var heightMap = Matrix.Parse(input.Text, c => int.Parse(c.ToString()));

        var trailHeads = heightMap.Coords.Where(c => heightMap[c] == 0).ToList();
        var tops = heightMap.Coords.Where(c => heightMap[c] == 9).ToList();
        var reachableTops = new Matrix<ImmutableHashSet<Vec2D>>(
            heightMap.Height, 
            heightMap.Width, 
            ImmutableHashSet<Vec2D>.Empty
        );

        foreach (var top in tops)
        {
            reachableTops[top] = reachableTops[top].Add(top);
        }
        var todo = tops.ToHashSet();

        while (todo.Any())
        {
            var loc = todo.First();
            todo.Remove(loc);
            var reachable = reachableTops[loc];
            var height = heightMap[loc];
            var offset = new Vec2D(1, 0);

            for (var dir = 0; dir < 4; dir++)
            {
                var newLoc = loc + offset;
                if (heightMap.Contains(newLoc) && heightMap[newLoc] == height - 1)
                {
                    var prevReachable = reachableTops[newLoc];
                    var newReachable = prevReachable.Union(reachable);
                    if (newReachable.Count > prevReachable.Count)
                    {
                        reachableTops[newLoc] = newReachable;
                        todo.Add(newLoc);
                    }
                }
                
                offset = offset.RotateCw();
            }
        }

        return trailHeads.Sum(loc => reachableTops[loc].Count);
    }

    [Example(TestInput2, 81)]
    public long Part2(IInput input)
    {
        var heightMap = Matrix.Parse(input.Text, c => int.Parse(c.ToString()));

        var trailHeads = heightMap.Coords.Where(c => heightMap[c] == 0).ToList();
        var tops = heightMap.Coords.Where(c => heightMap[c] == 9).ToList();
        var uniqueTrailCounts = new Matrix<int>(
            heightMap.Height, 
            heightMap.Width, 
           0
        );

        foreach (var top in tops)
        {
            uniqueTrailCounts[top] = 1;
        }
        var todo = tops.ToHashSet();

        while (todo.Any())
        {
            var loc = todo.OrderByDescending(loc => heightMap[loc]).First();
            todo.Remove(loc);
            var reachable = uniqueTrailCounts[loc];
            var height = heightMap[loc];
            var offset = new Vec2D(1, 0);

            for (var dir = 0; dir < 4; dir++)
            {
                var newLoc = loc + offset;
                if (heightMap.Contains(newLoc) && heightMap[newLoc] == height - 1)
                {
                    uniqueTrailCounts[newLoc] += reachable;
                    todo.Add(newLoc);
                }
                
                offset = offset.RotateCw();
            }
        }

        return trailHeads.Sum(loc => uniqueTrailCounts[loc]);
    }
}