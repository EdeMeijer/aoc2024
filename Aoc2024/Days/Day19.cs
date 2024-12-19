using Aoc2024.Lib;

namespace Aoc2024.Days;

public sealed class Day19 : IDay
{
    private const string TestInput1 =
        """
        r, wr, b, g, bwu, rb, gb, br

        brwrr
        bggr
        gbbr
        rrbgbr
        ubwu
        bwurrg
        brgr
        bbrgwb
        """;

    [Example(TestInput1, 6)]
    public object Part1(IInput input)
    {
        var parts = input.Text.Split("\n\n");

        var towels = parts[0].Split(", ");
        var designs = parts[1].Split("\n");

        bool IsPossible(string design)
        {
            return IsPossible2(design);
        }

        bool IsPossible2(ReadOnlySpan<char> design)
        {
            if (design.Length == 0)
            {
                return true;
            }

            foreach (var towel in towels)
            {
                if (towel.Length <= design.Length && design[..towel.Length].SequenceEqual(towel) &&
                    IsPossible2(design[towel.Length..]))
                {
                    return true;
                }
            }

            return false;
        }

        return designs.Count(IsPossible);
    }

    [Example(TestInput1, 16)]
    public object Part2(IInput input)
    {
        var parts = input.Text.Split("\n\n");

        var towels = parts[0].Split(", ");
        var designs = parts[1].Split("\n");
        var cache = new Dictionary<string, long>();

        long GetNumArrangements(string design)
        {
            return GetNumArrangementsCached(design);
        }

        long GetNumArrangementsCached(ReadOnlySpan<char> design)
        {
            var designStr = design.ToString();
            if (cache.TryGetValue(designStr, out var cached))
            {
                return cached;
            }
            
            var value = GetNumArrangements2(design);
            cache[designStr] = value;
            return value;
        }
        
        long GetNumArrangements2(ReadOnlySpan<char> design)
        {
            if (design.Length == 0)
            {
                return 1;
            }

            var total = 0L;
            foreach (var towel in towels)
            {
                if (towel.Length <= design.Length && design[..towel.Length].SequenceEqual(towel))
                {
                    total += GetNumArrangementsCached(design[towel.Length..]);
                }
            }

            return total;
        }

        return designs.Sum(GetNumArrangements);
    }
}