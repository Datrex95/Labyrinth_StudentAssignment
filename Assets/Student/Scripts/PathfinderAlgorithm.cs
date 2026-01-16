using System.Collections.Generic;
using UnityEngine;

public static class PathfindingAlgorithm
{
    /* <summary>
     TODO: Implement pathfinding algorithm here
     Find the shortest path from start to goal position in the maze.
     
     Dijkstra's Algorithm Steps:
     1. Initialize distances to all nodes as infinity
     2. Set distance to start node as 0
     3. Add start node to priority queue
     4. While priority queue is not empty:
        a. Remove node with minimum distance
        b. If it's the goal, reconstruct path
        c. For each neighbor:
           - Calculate new distance through current node
           - If shorter, update distance and add to queue
     
     MAZE FEATURES TO HANDLE:
     - Basic movement cost: 1.0 between adjacent cells
     - Walls: Some have infinite cost (impassable), others have climbing cost
     - Vents (teleportation): Allow instant travel between distant cells with usage cost
     
     AVAILABLE DATA STRUCTURES:
     - Dictionary<Vector2Int, float> - for tracking distances
     - Dictionary<Vector2Int, Vector2Int> - for tracking previous nodes (path reconstruction)
     - SortedSet<T> or List<T> - for priority queue implementation
     - mapData provides methods to check walls, vents, and boundaries
     
     HINT: Start simple with BFS (ignore wall costs and vents), then extend to weighted Dijkstra
     </summary> */
    static float impassableWallValue = float.MaxValue;

    static Dictionary<Vector2Int, float> distances = new Dictionary<Vector2Int, float>();
    static Dictionary<Vector2Int, Vector2Int> edgeNodes = new Dictionary<Vector2Int, Vector2Int>();

    static DPQ<Vector2Int> dpq = new DPQ<Vector2Int>();

    static Graph graph;
    public static List<Vector2Int> FindShortestPath(Vector2Int start, Vector2Int goal, IMapData mapData)
    {
        // TODO: Implement your pathfinding algorithm here
        graph = new Graph();
        SetGraph(start, mapData);

        for (int y = 0; y < mapData.Height; y++)
        {
            for (int x = 0; x < mapData.Width; x++)
            {
                distances.Add(new Vector2Int(x, y), float.PositiveInfinity);
            }
        }
        distances[start] = 0;
        dpq.Queue(start, distances[start]);
        edgeNodes.Add(start, start);

        while (dpq.Count != 0)
        {
            Vector2Int vertex = dpq.DeQueue();
            //Debug.Log(vertex + "Picked position");
            foreach (Node adjacentItem in graph.AdjacentVerticies(vertex))
            {
                Relax(vertex, adjacentItem, mapData);
            }
        }

        List<Vector2Int> path = new List<Vector2Int>();
        for (Vector2Int i = goal; i != start; i = edgeNodes[i])
        {
            path.Add(i);
        }

        path.Add(start);
        path.Reverse();

        return path;
    }

    static void SetGraph(Vector2Int currentVertex, IMapData mapData)
    {
        Vector2Int[] adjacentArray = new Vector2Int[] {
           (currentVertex + Vector2Int.left),
           (currentVertex + Vector2Int.right),
            (currentVertex + Vector2Int.up),
            (currentVertex + Vector2Int.down)
        };

        if (mapData.HasVent(currentVertex.x, currentVertex.y))
        {
            foreach (Vector2Int othervent in mapData.GetOtherVentPositions(currentVertex))
                graph.AddEdge(currentVertex, new Node(othervent,
                    mapData.GetVentCost(currentVertex.x, currentVertex.y), true));
        }

        for (int i = 0; i < adjacentArray.Length; i++)
        {
            if (InRange(adjacentArray[i], mapData) && !IsMovementBlocked(currentVertex, adjacentArray[i], mapData))
            {
                graph.AddEdge(currentVertex, new Node(adjacentArray[i], AdjacentCost(currentVertex, adjacentArray[i], mapData), false));
                if (!graph.HasNode(adjacentArray[i]))
                    SetGraph(adjacentArray[i], mapData);
            }
        }
    }

    static bool InRange(Vector2Int currentVertex, IMapData mapData)
    {
        if (currentVertex.x < mapData.Width && currentVertex.x >= 0
           && currentVertex.y < mapData.Height && currentVertex.y >= 0)
        {
            return true;
        }
        return false;
    }
    static float AdjacentCost(Vector2Int from, Vector2Int to, IMapData mapData)
    {
        Vector2Int direction = to - from;

        float cost = 0;

        if (direction == Vector2Int.left)
        {
            cost = mapData.GetVerticalWallCost(from.x, from.y);
        }
        else if (direction == Vector2Int.right)
        {
            cost = mapData.GetVerticalWallCost(to.x, to.y);
        }
        else if (direction == Vector2Int.up)
        {
            cost = mapData.GetHorizontalWallCost(to.x, to.y);
        }
        else if (direction == Vector2Int.down)
        {
            cost = mapData.GetHorizontalWallCost(from.x, from.y);
        }

        return cost;
    }
    public static bool IsMovementBlocked(Vector2Int from, Vector2Int to, IMapData mapData)
    {
        Debug.Log($"From: {from} To: {to}");
        Vector2Int direction = to - from;

        float cost = AdjacentCost(from, to, mapData);

        if (cost == impassableWallValue)
        {
            //Debug.Log("Wall too high");
            return true;
        }

        //Debug.Log("Path not blocked");
        return false;
        // TODO: Implement movement blocking logic
        // For now, allow all movement so character can move while you work on pathfinding
    }

    static void Relax(Vector2Int from, Node to, IMapData mapData)
    {
        float test1 = distances[to.Vertex],
            test2 = distances[from];
        if (distances[to.Vertex] > distances[from] + to.EdgeWeight)
        {
            distances[to.Vertex] = distances[from] + to.EdgeWeight;
            edgeNodes[to.Vertex] = from;
            if (!dpq.ContainsKey(to.Vertex))
            {
                dpq.Queue(to.Vertex, distances[to.Vertex]);
            }
            else
            {
                dpq.Update(to.Vertex, distances[to.Vertex]);
            }
        }
    }
}