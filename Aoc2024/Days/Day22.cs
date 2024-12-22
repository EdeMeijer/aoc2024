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
        const int numPossibleDeltas = 19;
        const int bufferSize = numPossibleDeltas * numPossibleDeltas * numPossibleDeltas * numPossibleDeltas;
        
        var results = new int[bufferSize];
        var monkeyBitmap = new bool[bufferSize];

        foreach (var (line, m) in input.Lines.Select((line, i) => (line, i)))
        {
            Array.Clear(monkeyBitmap);
            var s = long.Parse(line);
            var prevBananas = (int)(s % 10);
            var q = new Queue<int>();

            var key = 0;

            for (var i = 0; i < 2000; i++)
            {
                s = DeriveSecret(s);

                var bananas = (int)(s % 10);
                var delta = bananas - prevBananas + 9;

                key *= numPossibleDeltas;
                key += delta;

                q.Enqueue(delta);
                if (q.Count == 5)
                {
                    key -= q.Dequeue() * bufferSize;
                }

                if (i >= 3)
                {
                    if (!monkeyBitmap[key])
                    {
                        monkeyBitmap[key] = true;
                        results[key] += bananas;
                    }
                }

                prevBananas = bananas;
            }
        }

        return results.Max();
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