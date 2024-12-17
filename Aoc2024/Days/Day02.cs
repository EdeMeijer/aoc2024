using Aoc2024.Lib;

namespace Aoc2024.Days;

public sealed class Day02 : IDay
{
    private const string ExampleInput =
        """
        7 6 4 2 1
        1 2 7 8 9
        9 7 6 2 1
        1 3 2 4 5
        8 6 4 4 1
        1 3 6 7 9
        """;

    [Example(ExampleInput, 2)]
    public object Part1(IInput input)
    {
        return input.Lines.Count(line => IsSafe(line.Split(" ").Select(int.Parse).ToList()));
    }

    [Example(ExampleInput, 4)]
    public object Part2(IInput input)
    {
        return input.Lines.Count(line => IsSafeDampened(line.Split(" ").Select(int.Parse).ToList()));
    }

    private static bool IsSafeDampened(List<int> levels)
    {
        if (IsSafe(levels))
        {
            return true;
        }

        for (var i = 0; i < levels.Count; i++)
        {
            var withoutLevel = levels.ToList();
            withoutLevel.RemoveAt(i);
            if (IsSafe(withoutLevel))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsSafe(List<int> levels)
    {
        var prevSign = 0;
        for (var i = 1; i < levels.Count; i++)
        {
            var sign = levels[i].CompareTo(levels[i - 1]);
            if (sign == 0)
            {
                return false;
            }

            if (prevSign != 0 && prevSign != sign)
            {
                return false;
            }

            if (Math.Abs(levels[i] - levels[i - 1]) is < 1 or > 3)
            {
                return false;
            }

            prevSign = sign;
        }

        return true;
    }
}