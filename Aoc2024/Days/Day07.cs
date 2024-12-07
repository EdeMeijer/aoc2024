using Aoc2024.Lib;

namespace Aoc2024.Days;

public sealed class Day07 : IDay
{
    private const string TestInput =
        """
        190: 10 19
        3267: 81 40 27
        83: 17 5
        156: 15 6
        7290: 6 8 6 15
        161011: 16 10 13
        192: 17 8 14
        21037: 9 7 18 13
        292: 11 6 16 20
        """;

    [Example(TestInput, 3749)]
    public long Part1(IInput input)
    {
        return Solve(input, [
            (a, b) => a + b,
            (a, b) => a * b
        ]);
    }

    [Example(TestInput, 11387)]
    public long Part2(IInput input)
    {
        return Solve(input, [
            (a, b) => a + b,
            (a, b) => a * b,
            (a, b) => long.Parse($"{a}{b}")
        ]);
    }

    private long Solve(IInput input, Func<long, long, long>[] operators)
    {
        var result = 0L;

        foreach (var line in input.Lines)
        {
            var parts = line.Split(':');
            var expected = long.Parse(parts[0]);
            var operands = parts[1].Trim().Split(' ').Select(long.Parse).ToList();

            var solution = new int[operands.Count - 1];
            var failed = false;

            while (!failed)
            {
                var test = operands[0];
                for (var i = 1; i < operands.Count; i++)
                {
                    test = operators[solution[i - 1]](test, operands[i]);
                }

                if (test == expected)
                {
                    result += expected;
                    break;
                }

                for (var i = 0; i < solution.Length; i++)
                {
                    solution[i]++;
                    if (solution[i] < operators.Length)
                    {
                        break;
                    }

                    solution[i] = 0;
                    if (i == solution.Length - 1)
                    {
                        failed = true;
                    }
                }
            }
        }

        return result;
    }
}