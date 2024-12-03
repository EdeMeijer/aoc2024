using System.Text.RegularExpressions;
using Aoc2024.Lib;

namespace Aoc2024.Days;

public sealed class Day03 : IDay
{
    private const string TestInput1 = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
    private const string TestInput2 = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";

    [Example(TestInput1, 161)]
    public int Part1(IInput input)
    {
        var pattern = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");
        var result = 0;
        foreach (Match match in pattern.Matches(input.Text))
        {
            result += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
        }

        return result;
    }

    [Example(TestInput2, 48)]
    public int Part2(IInput input)
    {
        var pattern = new Regex(@"(do(?:n't)?\(\))|(?:mul\((\d{1,3}),(\d{1,3})\))");
        var result = 0;
        var enabled = true;
        foreach (Match match in pattern.Matches(input.Text))
        {
            if (match.Groups[1].Success)
            {
                enabled = match.Groups[1].Value == "do()";
            }
            else if (enabled)
            {
                result += int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value);
            }
        }

        return result;
    }
}