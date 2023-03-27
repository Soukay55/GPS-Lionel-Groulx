using System;
using System.Collections.Generic;
using UnityEngine;
using BlueRaja;

public class DungeonPathfinder2D
{
    public class Node
    {
        public Vector2Int Position { get; private set; }
        public Node Previous { get; set; }
        public float Cost { get; set; }

        public Node(Vector2Int position)
        {
            Position = position;
        }
    }

    public struct PathCost
    {
        public bool traversable;
        public float cost;
    }

    private static readonly Vector2Int[] neighbors =
    {
        new(1, 0),
        new(-1, 0),
        new(0, 1),
        new(0, -1)
    };

    private Grid2D<Node> grid;
    private SimplePriorityQueue<Node, float> queue;
    private HashSet<Node> closed;
    private Stack<Vector2Int> stack;

    public DungeonPathfinder2D(Vector2Int size)
    {
        grid = new Grid2D<Node>(size, Vector2Int.zero);

        queue = new SimplePriorityQueue<Node, float>();
        closed = new HashSet<Node>();
        stack = new Stack<Vector2Int>();

        for (var x = 0; x < size.x; x++)
        for (var y = 0; y < size.y; y++)
            grid[x, y] = new Node(new Vector2Int(x, y));
    }

    private void ResetNodes()
    {
        var size = grid.Size;

        for (var x = 0; x < size.x; x++)
        for (var y = 0; y < size.y; y++)
        {
            var node = grid[x, y];
            node.Previous = null;
            node.Cost = float.PositiveInfinity;
        }
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, Func<Node, Node, PathCost> costFunction)
    {
        ResetNodes();
        queue.Clear();
        closed.Clear();

        queue = new SimplePriorityQueue<Node, float>();
        closed = new HashSet<Node>();

        grid[start].Cost = 0;
        queue.Enqueue(grid[start], 0);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            closed.Add(node);

            if (node.Position == end) return ReconstructPath(node);

            foreach (var offset in neighbors)
            {
                if (!grid.InBounds(node.Position + offset)) continue;
                var neighbor = grid[node.Position + offset];
                if (closed.Contains(neighbor)) continue;

                var pathCost = costFunction(node, neighbor);
                if (!pathCost.traversable) continue;

                var newCost = node.Cost + pathCost.cost;

                if (newCost < neighbor.Cost)
                {
                    neighbor.Previous = node;
                    neighbor.Cost = newCost;

                    if (queue.TryGetPriority(node, out var existingPriority))
                        queue.UpdatePriority(node, newCost);
                    else
                        queue.Enqueue(neighbor, neighbor.Cost);
                }
            }
        }

        return null;
    }

    private List<Vector2Int> ReconstructPath(Node node)
    {
        var result = new List<Vector2Int>();

        while (node != null)
        {
            stack.Push(node.Position);
            node = node.Previous;
        }

        while (stack.Count > 0) result.Add(stack.Pop());

        return result;
    }
}