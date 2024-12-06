using Aoc2024.Lib;

namespace Aoc2024.Days;

public sealed class Day06 : IDay
{
    private const string TestInput =
        """
        ....#.....
        .........#
        ..........
        ..#.......
        .......#..
        ..........
        .#..^.....
        ........#.
        #.........
        ......#...
        """;

    private readonly record struct Vec2D(int Y, int X)
    {
        public static Vec2D operator +(Vec2D a, Vec2D b) => new(a.Y + b.Y, a.X + b.X);

        public Vec2D RotateCw() => new(X, -Y);
    }

    private sealed record Scenario(
        int Height,
        int Width,
        ISet<Vec2D> Obstacles,
        Vec2D Guard,
        Vec2D GuardDir
    );

    [Example(TestInput, 41)]
    public int Part1(IInput input)
    {
        Simulate(LoadInitialScenario(input), out var visited, out _);
        return visited.Count;
    }

    [Example(TestInput, 6)]
    public int Part2(IInput input)
    {
        var scenario = LoadInitialScenario(input);
        // Obtain default path to get a list of candidate positions for additional obstruction
        Simulate(scenario, out var candidates, out _);

        return candidates
            .Where(c => c != scenario.Guard)
            .Count(c =>
            {
                scenario.Obstacles.Add(c);
                Simulate(scenario, out _, out var isLoop);
                scenario.Obstacles.Remove(c);
                return isLoop;
            });
    }

    private static void Simulate(Scenario scenario, out ISet<Vec2D> visited, out bool isLoop)
    {
        visited = new HashSet<Vec2D>();
        var visitedWithDirs = new HashSet<(Vec2D, Vec2D)>();
        isLoop = false;

        var guard = scenario.Guard;
        var guardDir = scenario.GuardDir;

        while (guard.X >= 0 && guard.X < scenario.Width && guard.Y >= 0 && guard.Y < scenario.Height)
        {
            visited.Add(guard);
            if (!visitedWithDirs.Add((guard, guardDir)))
            {
                isLoop = true;
                break;
            }

            while (scenario.Obstacles.Contains(guard + guardDir))
            {
                guardDir = guardDir.RotateCw();
            }

            guard += guardDir;
        }
    }

    private static Scenario LoadInitialScenario(IInput input)
    {
        var guard = new Vec2D(0, 0);
        var guardDir = new Vec2D(-1, 0); // Up
        var obstacles = new HashSet<Vec2D>();

        var lines = input.Lines;
        var height = lines.Count;
        var width = lines[0].Length;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var c = lines[y][x];

                switch (c)
                {
                    case '.':
                        break;
                    case '#':
                        obstacles.Add(new Vec2D(y, x));
                        break;
                    case '^':
                        guard = new Vec2D(y, x);
                        break;
                    default:
                        throw new Exception($"Invalid value {c}");
                }
            }
        }

        return new Scenario(height, width, obstacles, guard, guardDir);
    }
}