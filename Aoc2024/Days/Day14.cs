using System.Text.RegularExpressions;
using Aoc2024.Lib;
using Aoc2024.Lib.Matrix;

namespace Aoc2024.Days;

public sealed class Day14 : IDay
{
    private const string TestInput1 =
        """
        p=0,4 v=3,-3
        p=6,3 v=-1,-3
        p=10,3 v=-1,2
        p=2,0 v=2,-1
        p=0,0 v=1,3
        p=3,0 v=-2,-2
        p=7,6 v=-1,-3
        p=3,0 v=-1,-2
        p=9,3 v=2,3
        p=7,3 v=-1,2
        p=2,4 v=2,-3
        p=9,5 v=-3,-3
        """;

    private static readonly Regex Pattern = new(@"p=(\-?\d+),(\-?\d+) v=(\-?\d+),(\-?\d+)");

    private sealed record Robot(Vec2D P, Vec2D V);

    [Example(TestInput1, 12)]
    public object Part1(IInput input)
    {
        var area = input is ExampleInput ? new Vec2D(7, 11) : new Vec2D(103, 101);

        var robots = Pattern.Matches(input.Text)
            .Select(match => new Robot(
                new Vec2D(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[1].Value)),
                new Vec2D(int.Parse(match.Groups[4].Value), int.Parse(match.Groups[3].Value))
            ))
            .ToList();

        int WrapCoord(int c, int len)
        {
            c %= len;
            return c < 0 ? c + len : c;
        }

        var finalPositions = robots
            .Select(robot =>
            {
                var p = robot.P + robot.V * 100;
                return new Vec2D(WrapCoord(p.Y, area.Y), WrapCoord(p.X, area.X));
            })
            .ToList();

        var quadrantSums = new[] { 0, 0, 0, 0 };
        foreach (var p in finalPositions)
        {
            if (p.Y == area.Y / 2 || p.X == area.X / 2)
            {
                continue;
            }

            var q = (p.Y < area.Y / 2 ? 0 : 2) + (p.X < area.X / 2 ? 0 : 1);
            quadrantSums[q]++;
        }

        return quadrantSums.Aggregate((a, b) => a * b);
    }

    public object Part2(IInput input)
    {
        var area = new Vec2D(103, 101);

        var robots = Pattern.Matches(input.Text)
            .Select(match => new Robot(
                new Vec2D(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[1].Value)),
                new Vec2D(int.Parse(match.Groups[4].Value), int.Parse(match.Groups[3].Value))
            ))
            .ToList();

        int WrapCoord(int c, int len)
        {
            c %= len;
            return c < 0 ? c + len : c;
        }

        const int blockSize = 10;
        var blocksY = area.Y / blockSize;
        var blocksX = area.X / blockSize;

        double Measure(List<Robot> robots)
        {
            var sums = new int[blocksY * blocksX];
            foreach (var robot in robots)
            {
                var blockY = robot.P.Y / blockSize;
                var blockX = robot.P.X / blockSize;
                if (blockY >= blocksY || blockX >= blocksX)
                {
                    continue;
                }

                var q = blockY * blocksX + blockX;
                sums[q]++;
            }

            var mean = sums.Average();
            var stddev = Math.Sqrt(sums.Average(v => Math.Pow(v - mean, 2)));
            return stddev;
        }

        for (var s = 0;; s++)
        {
            var stddev = Measure(robots);
            if (stddev > 8)
            {
                var grid = new Matrix<char>(area.Y, area.X, '.');
                foreach (var robot in robots)
                {
                    grid[robot.P] = '#';
                }
                
                Console.WriteLine(grid.ToString());
                return s;
            }
            
            robots = robots.Select(robot =>
            {
                var p = robot.P + robot.V;
                p = new Vec2D(WrapCoord(p.Y, area.Y), WrapCoord(p.X, area.X));
                return robot with { P = p };
            }).ToList();
        }
    }
}