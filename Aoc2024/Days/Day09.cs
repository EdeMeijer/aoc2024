using Aoc2024.Lib;

namespace Aoc2024.Days;

public sealed class Day09 : IDay
{
    private const string TestInput =
        """
        2333133121414131402
        """;

    [Example(TestInput, 1928)]
    public long Part1(IInput input)
    {
        var sizes = input.Text.Trim().Select(c => int.Parse(c.ToString())).ToList();
        var initialTotalSize = sizes.Sum();
        var blocks = new List<int>(initialTotalSize);
        for (var i = 0; i < sizes.Count; i++)
        {
            var size = sizes[i];
            var id = i % 2 == 0 ? i / 2 : -1;
            for (var j = 0; j < size; j++)
            {
                blocks.Add(id);
            }
        }

        // compaction
        for (var i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] == -1)
            {
                var last = blocks[^1];
                blocks.RemoveAt(blocks.Count - 1);
                blocks[i] = last;
                while (blocks[^1] == -1)
                {
                    blocks.RemoveAt(blocks.Count - 1);
                }
            }
        }

        return blocks.Select((id, i) => (long)(id == -1 ? 0 : id) * i).Sum();
    }

    [Example(TestInput, 2858)]
    public long Part2(IInput input)
    {
        var sizes = input.Text.Trim().Select(c => int.Parse(c.ToString())).ToList();

        var initialTotalSize = sizes.Sum();
        var blocks = new List<int>(initialTotalSize);
        var sourceInfo = new Dictionary<int, (int start, int length)>();
        for (var i = 0; i < sizes.Count; i++)
        {
            var size = sizes[i];
            var id = i % 2 == 0 ? i / 2 : -1;

            if (id >= 0)
            {
                sourceInfo.Add(id, (blocks.Count, size));
            }

            for (var j = 0; j < size; j++)
            {
                blocks.Add(id);
            }
        }

        // compaction
        var maxId = sizes.Count / 2;
        for (var id = maxId; id >= 0; id--)
        {
            var (start, length) = sourceInfo[id];

            // Find new spot
            var moved = false;
            var freeStart = -1;
            for (var i = 0; i < start; i++)
            {
                var free = blocks[i] == -1;
                if (free)
                {
                    if (freeStart == -1)
                    {
                        freeStart = i;
                    }
                }
                else
                {
                    freeStart = -1;
                }

                if (freeStart > -1)
                {
                    var freeLength = (i - freeStart) + 1;
                    if (freeLength >= length)
                    {
                        // Copy
                        for (var j = 0; j < length; j++)
                        {
                            blocks[j + freeStart] = id;
                        }

                        moved = true;
                        break;
                    }
                }
            }

            if (moved)
            {
                // Wipe source position
                for (var i = 0; i < length; i++)
                {
                    blocks[start + i] = -1;
                }

                while (blocks[^1] == -1)
                {
                    blocks.RemoveAt(blocks.Count - 1);
                }
            }
        }

        return blocks.Select((id, i) => (long)(id == -1 ? 0 : id) * i).Sum();
    }
}