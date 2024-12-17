using Aoc2024.Lib;
using Aoc2024.Lib.Matrix;

namespace Aoc2024.Days;

public sealed class Day12 : IDay
{
    private const string TestInput1 =
        """
        AAAA
        BBCD
        BBCC
        EEEC
        """;

    private const string TestInput2 =
        """
        RRRRIICCFF
        RRRRIICCCF
        VVRRRCCFFF
        VVRCCCJFFF
        VVVVCJJCFE
        VVIVCCJJEE
        VVIIICJJEE
        MIIIIIJJEE
        MIIISIJEEE
        MMMISSJEEE
        """;

    private const string TestInput3 =
        """
        OOOOO
        OXOXO
        OOOOO
        OXOXO
        OOOOO
        """;

    private const string TestInput4 =
        """
        EEEEE
        EXXXX
        EEEEE
        EXXXX
        EEEEE
        """;

    private const string TestInput5 =
        """
        AAAAAA
        AAABBA
        AAABBA
        ABBAAA
        ABBAAA
        AAAAAA
        """;

    [Example(TestInput1, 140)]
    [Example(TestInput2, 1930)]
    [Example(TestInput3, 772)]
    public object Part1(IInput input)
    {
        return Solve(input, segments => segments.Count);
    }

    [Example(TestInput1, 80)]
    [Example(TestInput2, 1206)]
    [Example(TestInput3, 436)]
    [Example(TestInput4, 236)]
    [Example(TestInput5, 368)]
    public object Part2(IInput input)
    {
        return Solve(input, segments =>
        {
            var sides = 0L;
            while (segments.Count > 0)
            {
                var next = segments.First();
                segments.Remove(next);
                sides++;

                var scanDir = next.inner.X == next.outer.X ? new Vec2D(0, 1) : new Vec2D(1, 0);
                foreach (var sign in new[] { 1, -1 })
                {
                    var cur = next;
                    for (;;)
                    {
                        cur = (cur.inner + scanDir * sign, cur.outer + scanDir * sign);
                        if (!segments.Contains(cur))
                        {
                            break;
                        }

                        segments.Remove(cur);
                    }
                }
            }

            return sides;
        });
    }

    private long Solve(IInput input, Func<HashSet<(Vec2D inner, Vec2D outer)>, long> handleFenceSegments)
    {
        var map = Matrix.Parse(input.Text);
        var handled = map.Map(_ => false);
        var result = 0L;

        foreach (var coord in map.Coords)
        {
            if (!handled[coord])
            {
                result += HandlePlot(coord);
            }
        }

        return result;

        long HandlePlot(Vec2D loc)
        {
            var area = 0L;
            var segments = new HashSet<(Vec2D inner, Vec2D outer)>();

            var type = map[loc];

            var todo = new Queue<Vec2D>();
            todo.Enqueue(loc);

            while (todo.Count > 0)
            {
                var next = todo.Dequeue();
                if (handled[next])
                {
                    continue;
                }

                area++;
                var offset = new Vec2D(1, 0);
                for (var dir = 0; dir < 4; dir++)
                {
                    var neighbor = next + offset;

                    if (map.Contains(neighbor) && map[neighbor] == type)
                    {
                        // Same plot
                        if (!handled[neighbor])
                        {
                            todo.Enqueue(neighbor);
                        }
                    }
                    else
                    {
                        segments.Add((next, neighbor));
                    }

                    offset = offset.RotateCw();
                }

                handled[next] = true;
            }

            return area * handleFenceSegments(segments);
        }
    }
}