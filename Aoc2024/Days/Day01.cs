using Aoc2024.Lib;

namespace Aoc2024.Days;

public sealed class Day01 : IDay
{
    private const string ExampleInput =
        """
        3   4
        4   3
        2   5
        1   3
        3   9
        3   3
        """;

    [Example(ExampleInput, 11)]
    public int Part1(IInput input)
    {
        var (left, right) = ParseInput(input);
        left.Sort();
        right.Sort();

        var result = 0;
        foreach (var tup in left.Zip(right))
        {
            result += Math.Abs(tup.First - tup.Second);
        }

        return result;
    }

    [Example(ExampleInput, 31)]
    public int Part2(IInput input)
    {
        var (left, right) = ParseInput(input);
        var leftCounts = CountValues(left);
        var rightCounts = CountValues(right);
        return leftCounts.Sum(kvp => kvp.Key * kvp.Value * rightCounts.GetValueOrDefault(kvp.Key, 0));
    }

    private static (List<int>, List<int>) ParseInput(IInput input)
    {
        var left = new List<int>();
        var right = new List<int>();
        foreach (var line in input.Lines)
        {
            var parts = line.Split("   ");
            left.Add(int.Parse(parts[0]));
            right.Add(int.Parse(parts[1]));
        }

        return (left, right);
    }

    private static Dictionary<int, int> CountValues(List<int> values)
    {
        return values.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());
    }
}