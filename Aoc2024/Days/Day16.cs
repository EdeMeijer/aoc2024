using Aoc2024.Lib;
using Aoc2024.Lib.Matrix;

namespace Aoc2024.Days;

public sealed class Day16 : IDay
{
    private const string TestInput1 =
        """
        ###############
        #.......#....E#
        #.#.###.#.###.#
        #.....#.#...#.#
        #.###.#####.#.#
        #.#.#.......#.#
        #.#.#####.###.#
        #...........#.#
        ###.#.#####.#.#
        #...#.....#.#.#
        #.#.#.###.#.#.#
        #.....#...#.#.#
        #.###.#.#.#.#.#
        #S..#.....#...#
        ###############
        """;

    private const string TestInput2 =
        """
        #################
        #...#...#...#..E#
        #.#.#.#.#.#.#.#.#
        #.#.#.#...#...#.#
        #.#.#.#.###.#.#.#
        #...#.#.#.....#.#
        #.#.#.#.#.#####.#
        #.#...#.#.#.....#
        #.#.#####.#.###.#
        #.#.#.......#...#
        #.#.###.#####.###
        #.#.#...#.....#.#
        #.#.#.#####.###.#
        #.#.#.........#.#
        #.#.#.#########.#
        #S#.............#
        #################
        """;

    private readonly record struct Vertex(Vec2D Position, Vec2D Orientation);

    private static readonly Vec2D[] Orientations = [new(0, 1), new(1, 0), new(0, -1), new(-1, 0)];

    [Example(TestInput1, 7036)]
    [Example(TestInput2, 11048)]
    public object Part1(IInput input)
    {
        ComputeShortestPaths(input, out var endPos, out var costs, out _);
        return Orientations.Min(o => costs[new Vertex(endPos, o)]);
    }

    [Example(TestInput1, 45)]
    [Example(TestInput2, 64)]
    public object Part2(IInput input)
    {
        ComputeShortestPaths(input, out var endPos, out var costs, out var prevs);

        var endVertices = Orientations.Select(o => new Vertex(endPos, o)).ToList();
        var minCost = endVertices.Min(v => costs[v]);
        var winVertices = endVertices.Where(v => costs[v] == minCost).ToList();

        var bestPathSet = new HashSet<Vertex>();
        var backTodo = new Queue<Vertex>(winVertices);

        while (backTodo.Any())
        {
            var next = backTodo.Dequeue();
            if (bestPathSet.Add(next) && prevs.TryGetValue(next, out var prev))
            {
                foreach (var p in prev)
                {
                    backTodo.Enqueue(p);
                }
            }
        }

        return bestPathSet.Select(v => v.Position).ToHashSet().Count;
    }

    private static void ComputeShortestPaths(
        IInput input,
        out Vec2D endPos,
        out Dictionary<Vertex, int> costs,
        out Dictionary<Vertex, HashSet<Vertex>> prevs
    )
    {
        var map = Matrix.Parse(input.Text);

        var startPos = map.Coords.First(c => map[c] == 'S');
        endPos = map.Coords.First(c => map[c] == 'E');
        map[startPos] = '.';
        map[endPos] = '.';

        var start = new Vertex(startPos, new Vec2D(0, 1));

        Dijkstra.Run(start, GetNeighbors, GetCost, out costs, out prevs);
        return;

        int GetCost(Vertex a, Vertex b)
        {
            return a.Orientation != b.Orientation ? 1000 : 1;
        }

        IEnumerable<Vertex> GetNeighbors(Vertex v)
        {
            yield return v with { Orientation = v.Orientation.RotateCw() };
            yield return v with { Orientation = v.Orientation.RotateCcw() };

            var next = v with { Position = v.Position + v.Orientation };
            if (map[next.Position] == '.')
            {
                yield return next;
            }
        }
    }
}