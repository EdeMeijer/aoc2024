using Aoc2024.Lib;

namespace Aoc2024.Days;

public sealed class Day11 : IDay
{
    private const string TestInput =
        """
        125 17
        """;

    [Example(TestInput, 55312)]
    public object Part1(IInput input)
    {
        return Solve(input, 25);
    }

    public object Part2(IInput input)
    {
        return Solve(input, 75);
    }

    private static long Solve(IInput input, int iterations)
    {
        var state = input.Text.Split(' ').Select(long.Parse).GroupBy(v => v)
            .ToDictionary(g => g.Key, g => (long)g.Count());

        for (var b = 0; b < iterations; b++)
        {
            state = state
                .SelectMany(kvp => Transition(kvp.Key).Select(value => (value, count: kvp.Value)))
                .GroupBy(tup => tup.value, tup => tup.count)
                .ToDictionary(g => g.Key, g => g.Sum());
        }

        return state.Values.Sum();

        static IEnumerable<long> Transition(long value)
        {
            if (value == 0)
            {
                yield return 1;
            }
            else
            {
                var asString = value.ToString();
                var l = asString.Length;
                if (l % 2 == 0)
                {
                    var mid = l / 2;
                    yield return long.Parse(asString[..mid]);
                    yield return long.Parse(asString[mid..]);
                }
                else
                {
                    yield return value * 2024;
                }
            }
        }
    }
}