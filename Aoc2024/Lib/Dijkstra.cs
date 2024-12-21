namespace Aoc2024.Lib;

public static class Dijkstra
{
    public static void Run<TNode>(
        TNode start,
        Func<TNode, IEnumerable<TNode>> getNeighbors,
        Func<TNode, TNode, int> getDistance,
        out Dictionary<TNode, int> distances,
        out Dictionary<TNode, HashSet<TNode>> prevs
    )
    {
        Run(start, default, getNeighbors, getDistance, out distances, out prevs);
    }
    
    public static void Run<TNode>(
        TNode start,
        TNode? end,
        Func<TNode, IEnumerable<TNode>> getNeighbors,
        Func<TNode, TNode, int> getDistance,
        out Dictionary<TNode, int> distances,
        out Dictionary<TNode, HashSet<TNode>> prevs
    )
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

            if (end != null && distances.ContainsKey(end) && distances[end] < nextCost)
            {
                // Found the solution to get to the target node
                break;
            }

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