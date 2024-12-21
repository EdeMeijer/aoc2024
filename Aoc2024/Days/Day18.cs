using Aoc2024.Lib;
using Aoc2024.Lib.Matrix;

namespace Aoc2024.Days;

public sealed class Day18 : IDay
{
    private const string TestInput1 =
        """
        5,4
        4,2
        4,5
        3,0
        2,1
        6,3
        2,4
        1,5
        0,6
        3,3
        2,6
        5,1
        1,2
        5,5
        2,5
        6,5
        1,4
        0,4
        6,4
        1,1
        6,1
        1,0
        0,5
        1,6
        2,0
        """;

    [Example(TestInput1, 22)]
    public object Part1(IInput input)
    {
        var coords = input.Lines.Select(line =>
        {
            var parts = line.Split(',').Select(int.Parse).ToArray();
            return new Vec2D(parts[1], parts[0]);
        }).ToList();

        var size = input is ExampleInput ? 7 : 71;
        var map = new Matrix<bool>(size, size, true);

        var numBytes = input is ExampleInput ? 12 : 1024;

        foreach (var coord in coords.Take(numBytes))
        {
            map[coord] = false;
        }

        IEnumerable<Vec2D> GetNeighbors(Vec2D coord)
        {
            var offset = new Vec2D(0, 1);
            for (var i = 0; i < 4; i++)
            {
                var next = coord + offset;
                if (map.Contains(next) && map[next])
                {
                    yield return next;
                }

                offset = offset.RotateCw();
            }
        }

        Dijkstra.Run(
            new Vec2D(0, 0),
            GetNeighbors,
            (a, b) => 1,
            out var distances,
            out _
        );

        return distances[new Vec2D(size - 1, size - 1)];
    }

    [Example(TestInput1, "6,1")]
    public object Part2(IInput input)
    {
        var coords = input.Lines.Select(line =>
        {
            var parts = line.Split(',').Select(int.Parse).ToArray();
            return new Vec2D(parts[1], parts[0]);
        }).ToList();

        var size = input is ExampleInput ? 7 : 71;
        var map = new Matrix<bool>(size, size, true);
        
        IEnumerable<Vec2D> GetNeighbors(Vec2D coord)
        {
            var offset = new Vec2D(0, 1);
            for (var i = 0; i < 4; i++)
            {
                var next = coord + offset;
                if (map.Contains(next) && map[next])
                {
                    yield return next;
                }

                offset = offset.RotateCw();
            }
        }
        
        foreach (var coord in coords)
        {
            map[coord] = false;
            Dijkstra.Run(
                new Vec2D(0, 0),
                GetNeighbors,
                (a, b) => 1,
                out var distances,
                out _
            );

            if (!distances.ContainsKey(new Vec2D(size - 1, size - 1)))
            {
                return $"{coord.X},{coord.Y}";
            }
        }

        throw new ApplicationException();
    }
}