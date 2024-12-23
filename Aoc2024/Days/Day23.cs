using System.Collections.Immutable;
using Aoc2024.Lib;

namespace Aoc2024.Days;

public sealed class Day23 : IDay
{
    private const string TestInput1 =
        """
        kh-tc
        qp-kh
        de-cg
        ka-co
        yn-aq
        qp-ub
        cg-tb
        vc-aq
        tb-ka
        wh-tc
        yn-cg
        kh-ub
        ta-co
        de-co
        tc-td
        tb-wq
        wh-td
        ta-ka
        td-qp
        aq-cg
        wq-ub
        ub-vc
        de-ta
        wq-aq
        wq-vc
        wh-yn
        ka-de
        kh-ta
        co-tc
        wh-qp
        tb-vc
        td-yn
        """;

    [Example(TestInput1, 7)]
    public object Part1(IInput input)
    {
        var map = new Dictionary<string, HashSet<string>>();

        void Register(string a, string b)
        {
            if (!map.TryGetValue(a, out var s))
            {
                s = new HashSet<string>();
                map[a] = s;
            }

            s.Add(b);
        }
        
        foreach (var line in input.Lines)
        {
            var computers = line.Split('-');
            Register(computers[0], computers[1]);
            Register(computers[1], computers[0]);
        }


        var result = FindInterconnectedGroups([], 3)
            .Where(g => g.Any(c => c.StartsWith('t')))
            .Select(g => string.Join(',', g.OrderBy(c => c)))
            .ToHashSet();

        return result.Count;

        IEnumerable<ImmutableHashSet<string>> FindInterconnectedGroups(ImmutableHashSet<string> cur, int groupSize)
        {
            if (cur.Count == groupSize)
            {
                yield return cur;
                yield break;
            }

            foreach (var kvp in map)
            {
                if (cur.Contains(kvp.Key))
                {
                    continue;
                }

                if (cur.Except(kvp.Value).Any())
                {
                    continue;
                }
                
                var newGroup = cur.Add(kvp.Key);
                foreach (var g in FindInterconnectedGroups(newGroup, groupSize))
                {
                    yield return g;
                }
            }
        }
    }

    [Example(TestInput1, "co,de,ka,ta")]
    public object Part2(IInput input)
    {
        var map = new Dictionary<string, HashSet<string>>();

        void Register(string a, string b)
        {
            if (!map.TryGetValue(a, out var s))
            {
                s = new HashSet<string>();
                map[a] = s;
            }

            s.Add(b);
        }
        
        foreach (var line in input.Lines)
        {
            var computers = line.Split('-');
            Register(computers[0], computers[1]);
            Register(computers[1], computers[0]);
        }

        var result = FindInterconnectedGroups([]);
        var party = result.OrderByDescending(g => g.Count).First();

        return string.Join(',', party.OrderBy(c => c));

        IEnumerable<ImmutableHashSet<string>> FindInterconnectedGroups(ImmutableHashSet<string> cur)
        {
            yield return cur;
            
            foreach (var kvp in map)
            {
                if (cur.Any(c => c.CompareTo(kvp.Key) >= 0))
                {
                    continue;
                }

                if (cur.Except(kvp.Value).Any())
                {
                    continue;
                }
                
                var newGroup = cur.Add(kvp.Key);
                foreach (var g in FindInterconnectedGroups(newGroup))
                {
                    yield return g;
                }
            }
        }
    }
}