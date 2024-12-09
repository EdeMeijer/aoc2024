using Aoc2024.Lib;
using Aoc2024.Lib.Matrix;

namespace Aoc2024.Days;

public sealed class Day08 : IDay
{
    private const string TestInput =
        """
        ............
        ........0...
        .....0......
        .......0....
        ....0.......
        ......A.....
        ............
        ............
        ........A...
        .........A..
        ............
        ............
        """;

    private const string TestInput2 =
        """
        T.........
        ...T......
        .T........
        ..........
        ..........
        ..........
        ..........
        ..........
        ..........
        ..........
        """;

    [Example(TestInput, 14)]
    public long Part1(IInput input)
    {
        return Solve(input, (a, b) =>
        {
            var delta = b - a;
            return [[a - delta], [b + delta]];
        });
    }

    [Example(TestInput, 34)]
    [Example(TestInput2, 9)]
    public long Part2(IInput input)
    {
        return Solve(input, (a, b) =>
        {
            var delta = b - a;
            return [RepeatNodes(a, -delta), RepeatNodes(b, delta)];
        });

        static IEnumerable<Vec2D> RepeatNodes(Vec2D start, Vec2D step)
        {
            for (;;)
            {
                yield return start;
                start += step;
            }
        }
    }

    public long Solve(IInput input, Func<Vec2D, Vec2D, IEnumerable<IEnumerable<Vec2D>>> nodeGen)
    {
        var inputMap = Matrix.Parse(input.Text);

        var antennaeGroups = inputMap.Coords
            .Select(coord => (c: inputMap[coord], coord))
            .Where(tup => tup.c != '.')
            .GroupBy(tup => tup.c, tup => tup.coord)
            .ToList();

        var antiNodes = new HashSet<Vec2D>();

        foreach (var group in antennaeGroups)
        {
            for (var i = 0; i < group.Count(); i++)
            {
                var a1 = group.ElementAt(i);
                for (var j = i + 1; j < group.Count(); j++)
                {
                    var a2 = group.ElementAt(j);
                    foreach (var gen in nodeGen(a1, a2))
                    {
                        foreach (var loc in gen)
                        {
                            if (inputMap.Contains(loc))
                            {
                                antiNodes.Add(loc);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        return antiNodes.Count;
    }
}