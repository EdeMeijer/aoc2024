namespace Aoc2024.Lib;

public static class Dijkstra
{
    public static void Run<TNode>(
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