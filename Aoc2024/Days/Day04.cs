using Aoc2024.Lib;

namespace Aoc2024.Days;

public sealed class Day04 : IDay
{
    private const string TestInput1 =
        """
        MMMSXXMASM
        MSAMXMSMSA
        AMXSXMAAMM
        MSAMASMSMX
        XMASAMXAMM
        XXAMMXXAMA
        SMSMSASXSS
        SAXAMASAAA
        MAMMMXMMMM
        MXMXAXMASX
        """;

    private const string TestInput2 =
        """
        .M.S......
        ..A..MSMS.
        .M.S.MAA..
        ..A.ASMSM.
        .M.S.M....
        ..........
        S.S.S.S.S.
        .A.A.A.A..
        M.M.M.M.M.
        ..........
        """;

    [Example(TestInput1, 18)]
    public long Part1(IInput input)
    {
        var m = Matrix.Parse(input.Text);

        int[] offsets = [-1, 0, 1];
        var directions = offsets
            .SelectMany(dy => offsets.Select(dx => (dy, dx))).Where(d => d != (0, 0))
            .ToArray();

        var result = 0;
        var query = "XMAS";
        foreach (var (y, x) in m.Coords)
        {
            foreach (var (dy, dx) in directions)
            {
                var found = true;
                for (var i = 0; i < query.Length; i++)
                {
                    var cy = y + i * dy;
                    var cx = x + i * dx;
                    if (!m.Contains(cy, cx) || m[cy, cx] != query[i])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    result++;
                }
            }
        }

        return result;
    }

    [Example(TestInput2, 9)]
    public long Part2(IInput input)
    {
        var m = Matrix.Parse(input.Text);
        var result = 0;

        foreach (var (y, x) in m.Coords)
        {
            if (m[y, x] != 'A' || !m.Contains(y - 1, x - 1) || !m.Contains(y + 1, x + 1))
            {
                continue;
            }

            if (
                new HashSet<char> { m[y - 1, x - 1], m[y + 1, x + 1] }.SetEquals(['M', 'S']) &&
                new HashSet<char> { m[y - 1, x + 1], m[y + 1, x - 1] }.SetEquals(['M', 'S'])
            )
            {
                result++;
            }
        }

        return result;
    }
}