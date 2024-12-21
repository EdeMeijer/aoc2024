using System.Text;
using Aoc2024.Lib;

namespace Aoc2024.Days;

public sealed class Day21 : IDay
{
    private const string TestInput1 =
        """
        029A
        980A
        179A
        456A
        379A
        """;

    [Example(TestInput1, 126384)]
    public object Part1(IInput input)
    {
        return Solve(input, 3);
    }

    public object Part2(IInput input)
    {
        return Solve(input, 26);
    }

    private static object Solve(IInput input, int numRobots)
    {
        var result = 0L;
        foreach (var code in input.Lines)
        {
            result += SolveCode(code, numRobots) * long.Parse(code[..^1].TrimStart('0'));
        }

        return result;
    }

    private static readonly Dictionary<Vec2D, char> Numpad = new()
    {
        [new Vec2D(0, 0)] = '7',
        [new Vec2D(0, 1)] = '8',
        [new Vec2D(0, 2)] = '9',
        [new Vec2D(1, 0)] = '4',
        [new Vec2D(1, 1)] = '5',
        [new Vec2D(1, 2)] = '6',
        [new Vec2D(2, 0)] = '1',
        [new Vec2D(2, 1)] = '2',
        [new Vec2D(2, 2)] = '3',
        [new Vec2D(3, 1)] = '0',
        [new Vec2D(3, 2)] = 'A',
    };

    private static readonly Dictionary<char, Vec2D> InvNumpad = Numpad.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    private static readonly Dictionary<Vec2D, char> DirectionalPad = new()
    {
        [new Vec2D(0, 1)] = '^',
        [new Vec2D(0, 2)] = 'A',
        [new Vec2D(1, 0)] = '<',
        [new Vec2D(1, 1)] = 'v',
        [new Vec2D(1, 2)] = '>',
    };

    private static readonly Dictionary<char, Vec2D> InvDirectionalPad =
        DirectionalPad.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    private static readonly Dictionary<(string, int), long> Cache = new();

    private static long SolveCode(string code, int numRobots, bool topLevel = true)
    {
        var cacheKey = (code, numIntermediateRobots: numRobots);
        if (Cache.TryGetValue(cacheKey, out var result))
        {
            return result;
        }

        // We can solve the code by solving the segments between presses separately
        var cur = 'A';
        result = 0;
        foreach (var c in code)
        {
            result += SolveSegment(cur, c, numRobots, topLevel);
            cur = c;
        }

        Cache[cacheKey] = result;

        return result;
    }

    private static long SolveSegment(char start, char target, int numRobots, bool topLevel = true)
    {
        if (numRobots == 0)
        {
            // Just press the button directly
            return 1;
        }

        // There might be any number of ways to get from the start to the target. However, we can safely assume that
        // minimizing turns is the best approach, and that will leave at most 2 paths for the transition; first vertical
        // and then horizontal, or vice versa.
        var pad = topLevel ? Numpad : DirectionalPad;
        var invPad = topLevel ? InvNumpad : InvDirectionalPad;

        var startPos = invPad[start];
        var targetPos = invPad[target];

        bool TryMakeSubCode(Vec2D[] dirs, out string code)
        {
            var path = new StringBuilder();

            var pos = startPos;
            var distance = (targetPos - pos).ManhattanNorm;
            foreach (var dir in dirs)
            {
                var dirKey = dir switch
                {
                    (1, 0) => 'v',
                    (-1, 0) => '^',
                    (0, 1) => '>',
                    (0, -1) => '<',
                };
                for (;;)
                {
                    var nextPos = pos + dir;
                    var nextDistance = (targetPos - nextPos).ManhattanNorm;
                    if (nextDistance > distance)
                    {
                        break;
                    }

                    pos = nextPos;
                    distance = nextDistance;

                    if (!pad.ContainsKey(nextPos))
                    {
                        // Invalid path
                        code = string.Empty;
                        return false;
                    }

                    path.Append(dirKey);
                }
            }

            path.Append('A');
            code = path.ToString();
            return true;
        }

        var subCodes = new List<string>();
        var vDir = new Vec2D(targetPos.Y > startPos.Y ? 1 : -1, 0);
        var hDir = new Vec2D(0, targetPos.X > startPos.X ? 1 : -1);
        if (TryMakeSubCode([vDir, hDir], out var subCode))
        {
            subCodes.Add(subCode);
        }

        if (TryMakeSubCode([hDir, vDir], out subCode))
        {
            subCodes.Add(subCode);
        }

        return subCodes.Min(code => SolveCode(code, numRobots - 1, false));
    }
}