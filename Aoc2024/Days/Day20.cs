using Aoc2024.Lib;
using Aoc2024.Lib.Matrix;

namespace Aoc2024.Days;

public sealed class Day20 : IDay
{
    private const string TestInput1 =
        """
        ###############
        #...#...#.....#
        #.#.#.#.#.###.#
        #S#...#.#.#...#
        #######.#.#.###
        #######.#.#...#
        #######.#.###.#
        ###..E#...#...#
        ###.#######.###
        #...###...#...#
        #.#####.#.###.#
        #.#...#.#.#...#
        #.#.#.#.#.#.###
        #...#...#...###
        ###############
        """;

    [Example(TestInput1, 10)]
    public object Part1(IInput input)
    {
        var threshold = input is ExampleInput ? 10 : 100;
        return Solve(input, 2, threshold);
    }

    [Example(TestInput1, 285)]
    public object Part2(IInput input)
    {
        var threshold = input is ExampleInput ? 50 : 100;
        return Solve(input, 20, threshold);
    }

    public object Solve(IInput input, int maxCheatTime, int minSaved)
    {
        var map = Matrix.Parse(input.Text);
        var startPos = map.Coords.First(c => map[c] == 'S');
        var endPos = map.Coords.First(c => map[c] == 'E');
        map[startPos] = '.';
        map[endPos] = '.';

        IEnumerable<Vec2D> GetNeighbors(Vec2D pos)
        {
            var dir = new Vec2D(0, 1);
            for (var i = 0; i < 4; i++)
            {
                var next = pos + dir;
                if (map[next] == '.')
                {
                    yield return next;
                }

                dir = dir.RotateCw();
            }
        }

        ComputeShortestPaths(startPos, GetNeighbors, (_, _) => 1, out var distances, out var prevs);

        int Cheat(Vec2D start, Vec2D end)
        {
            var oldDistance = distances[end];
            var newDistance = distances[start] + Math.Abs(end.Y - start.Y) + Math.Abs(end.X - start.X);
            return oldDistance - newDistance;
        }

        IEnumerable<(Vec2D Start, Vec2D End)> GeneratePotentialCheats()
        {
            foreach (var start in map.Coords)
            {
                if (map[start] != '.')
                {
                    continue;
                }

                for (var dy = -maxCheatTime; dy <= maxCheatTime; dy++)
                {
                    for (var dx = -maxCheatTime; dx <= maxCheatTime; dx++)
                    {
                        if (Math.Abs(dy) + Math.Abs(dx) <= maxCheatTime)
                        {
                            var end = start + new Vec2D(dy, dx);
                            if (map.Contains(end) && map[end] == '.')
                            {
                                yield return (start, end);
                            }
                        }
                    }
                }
            }
        }

        return GeneratePotentialCheats()
            .Count(cheat => Cheat(cheat.Start, cheat.End) >= minSaved);
    }

    private void ComputeShortestPaths<TNode>(
        TNode start,
        Func<TNode, IEnumerable<TNode>> getNeighbors,
        Func<TNode, TNode, int> getDistance,
        out Dictionary<TNode, int> distances,
        out Dictionary<TNode, HashSet<TNode>> prevs
    ) where TNode : notnull
    {
        distances = new Dictionary<TNode, int>
        {
            [start] = 0
        };
        prevs = new Dictionary<TNode, HashSet<TNode>>();

        var todo = new PriorityQueue<TNode, int>();
        todo.Enqueue(start, 0);
        while (todo.Count > 0)
        {
            var next = todo.Dequeue();
            var nextCost = distances[next];

            foreach (var neighbor in getNeighbors(next))
            {
                var cost = getDistance(next, neighbor);
                var alt = nextCost + cost;
                if (!distances.ContainsKey(neighbor) || alt < distances[neighbor])
                {
                    distances[neighbor] = alt;
                    todo.Enqueue(neighbor, alt);
                    prevs[neighbor] = [next];
                }
                else if (alt == distances[neighbor])
                {
                    prevs[neighbor].Add(next);
                }
            }
        }
    }
}