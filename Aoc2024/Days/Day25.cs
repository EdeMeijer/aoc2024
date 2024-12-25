using Aoc2024.Lib;
using Aoc2024.Lib.Matrix;

namespace Aoc2024.Days;

public sealed class Day25 : IDay
{
    private const string TestInput1 =
        """
        #####
        .####
        .####
        .####
        .#.#.
        .#...
        .....
        
        #####
        ##.##
        .#.##
        ...##
        ...#.
        ...#.
        .....
        
        .....
        #....
        #....
        #...#
        #.#.#
        #.###
        #####
        
        .....
        .....
        #.#..
        ###..
        ###.#
        ###.#
        #####
        
        .....
        .....
        .....
        #....
        #.#..
        #.#.#
        #####
        """;
    
    [Example(TestInput1, 3)]
    public object Part1(IInput input)
    {
        var blocks = input.Text.Split("\n\n").Select(Matrix.Parse);
        var locks = new List<List<int>>();
        var keys = new List<List<int>>();
        foreach (var block in blocks)
        {
            var heights = Enumerable.Range(0, 5).Select(c => block.Col(c).Skip(1).SkipLast(1).Count(v => v == '#'))
                .ToList();
            var collection = block[0, 0] == '#' ? locks : keys;
            collection.Add(heights);
        }

        var result = 0;
        foreach (var lok in locks)
        {
            foreach (var key in keys)
            {
                var fits = lok.Zip(key, (l, k) => l + k).All(s => s <= 5);
                if (fits)
                {
                    result++;
                }
            }
        }

        return result;
    }

    public object Part2(IInput input)
    {
        throw new NotImplementedException();
    }
}