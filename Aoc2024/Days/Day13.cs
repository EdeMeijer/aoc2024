using System.Text.RegularExpressions;
using Aoc2024.Lib;

namespace Aoc2024.Days;

public sealed class Day13 : IDay
{
    private const string TestInput1 =
        """
        Button A: X+94, Y+34
        Button B: X+22, Y+67
        Prize: X=8400, Y=5400

        Button A: X+26, Y+66
        Button B: X+67, Y+21
        Prize: X=12748, Y=12176

        Button A: X+17, Y+86
        Button B: X+84, Y+37
        Prize: X=7870, Y=6450

        Button A: X+69, Y+23
        Button B: X+27, Y+71
        Prize: X=18641, Y=10279
        """;

    private static readonly Regex Pattern =
        new(@".*?: X\+(\d+), Y\+(\d+)\n.*?: X\+(\d+), Y\+(\d+)\n.*?: X=(\d+), Y=(\d+)");

    private sealed record Scenario(Vec2DL ButtonA, Vec2DL ButtonB, Vec2DL Prize);

    [Example(TestInput1, 480)]
    public long Part1(IInput input)
    {
        return Solve(input, 0);
    }

    public long Part2(IInput input)
    {
        return Solve(input, 10000000000000L);
    }

    private long Solve(IInput input, long prizeOffset)
    {
        var scenarios = Pattern.Matches(input.Text)
            .Select(match => new Scenario(
                new Vec2DL(long.Parse(match.Groups[2].Value), long.Parse(match.Groups[1].Value)),
                new Vec2DL(long.Parse(match.Groups[4].Value), long.Parse(match.Groups[3].Value)),
                new Vec2DL(
                    long.Parse(match.Groups[6].Value) + prizeOffset,
                    long.Parse(match.Groups[5].Value) + prizeOffset
                )
            ))
            .ToList();

        var result = 0L;
        foreach (var scenario in scenarios)
        {
            // Find the intersection point of two lines
            // One line from the start using slope of button A
            // One line from the prize using slope of button B
            var slopeA = (double)scenario.ButtonA.Y / scenario.ButtonA.X;
            var slopeB = (double)scenario.ButtonB.Y / scenario.ButtonB.X;
            var lineBIntercept = scenario.Prize.Y - scenario.Prize.X * slopeB;
            var intersectX = (long)Math.Round(lineBIntercept / (slopeA - slopeB));

            // Check if the intersection point is a valid reachable point for both buttons
            var intersectToPrizeX = scenario.Prize.X - intersectX;
            if (intersectX % scenario.ButtonA.X == 0 && intersectToPrizeX % scenario.ButtonB.X == 0)
            {
                result += 3 * (intersectX / scenario.ButtonA.X) + (intersectToPrizeX / scenario.ButtonB.X);
            }
        }

        return result;
    }
}