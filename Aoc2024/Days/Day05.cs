using Aoc2024.Lib;

namespace Aoc2024.Days;

public class Day05 : IDay
{
    private const string TestInput =
        """
        47|53
        97|13
        97|61
        97|47
        75|29
        61|13
        75|53
        29|13
        97|29
        53|29
        61|53
        97|53
        61|29
        47|13
        75|47
        97|75
        47|61
        75|61
        47|29
        75|13
        53|13

        75,47,61,53,29
        97,61,53,29,13
        75,29,13
        75,97,47,61,53
        61,13,29
        97,13,75,29,47
        """;

    [Example(TestInput, 143)]
    public long Part1(IInput input)
    {
        return LoadAndSortUpdates(input.Text)
            .Where(tup => tup.update.SequenceEqual(tup.sorted))
            .Sum(tup => tup.sorted[tup.sorted.Count / 2]);
    }

    [Example(TestInput, 123)]
    public long Part2(IInput input)
    {
        return LoadAndSortUpdates(input.Text)
            .Where(tup => !tup.update.SequenceEqual(tup.sorted))
            .Sum(tup => tup.sorted[tup.sorted.Count / 2]);
    }

    private List<(List<int> update, List<int> sorted)> LoadAndSortUpdates(string input)
    {
        var parts = input.Split("\n\n");
        var rules = parts[0].Split("\n").Select(r => r.Split('|').Select(int.Parse).ToArray()).ToList();
        var updates = parts[1].Split("\n").Select(r => r.Split(',').Select(int.Parse).ToList()).ToList();

        return updates
            .Select(update => (update, SortUpdate(update.ToHashSet())))
            .ToList();

        List<int> SortUpdate(HashSet<int> pages)
        {
            var result = new List<int>(pages.Count);

            // Select relevant rules
            var updateRules = rules.Where(pages.IsSupersetOf).ToList();
            var todo = pages.ToHashSet();
            while (todo.Any())
            {
                // Add the next page that does not appear on the right side of any rule
                var next = todo.First(p => updateRules.All(r => r[1] != p));
                result.Add(next);
                todo.Remove(next);
                updateRules = updateRules.Where(r => !r.Contains(next)).ToList();
            }

            return result;
        }
    }
}