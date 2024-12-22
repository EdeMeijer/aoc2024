using Aoc2024.Lib;

namespace Aoc2024.Days;

public sealed class Day22 : IDay
{
    private const string TestInput1 =
        """
        1
        10
        100
        2024
        """;

    private const string TestInput2 =
        """
        1
        2
        3
        2024
        """;

    [Example(TestInput1, 37327623)]
    public object Part1(IInput input)
    {
        var result = 0L;

        foreach (var line in input.Lines)
        {
            var s = long.Parse(line);
            for (var i = 0; i < 2000; i++)
            {
                s = DeriveSecret(s);
            }

            result += s;
        }

        return result;
    }

    [Example(TestInput2, 23)]
    public object Part2(IInput input)
    {
        // Track the max number of bananas for each possible sequence, per monkey
        var tracker = new Dictionary<string, Dictionary<int, int>>();

        foreach (var (line, m) in input.Lines.Select((line, i) => (line, i)))
        {
            var s = long.Parse(line);
            var prevBananas = (int)(s % 10);
            var q = new Queue<int>();

            for (var i = 0; i < 2000; i++)
            {
                s = DeriveSecret(s);

                var bananas = (int)(s % 10);
                var delta = bananas - prevBananas;

                q.Enqueue(delta);
                if (q.Count == 5)
                {
                    q.Dequeue();
                }

                if (bananas > 0 && q.Count == 4)
                {
                    var key = string.Join(',', q);
                    if (!tracker.TryGetValue(key, out var firstPerMonkey))
                    {
                        firstPerMonkey = new Dictionary<int, int>();
                        tracker[key] = firstPerMonkey;
                    }

                    firstPerMonkey.TryAdd(m, bananas);
                }

                prevBananas = bananas;
            }
        }

        return tracker.Values.Max(d => d.Values.Sum());
    }

    private static long DeriveSecret(long s)
    {
        s = Prune(Mix(s, s * 64));
        s = Prune(Mix(s, s / 32));
        s = Prune(Mix(s, s * 2048));
        return s;
    }

    private static long Mix(long s, long r) => s ^ r;

    private static long Prune(long s) => s % 16777216;
}