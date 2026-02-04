using UnityEngine;
using System.Collections.Generic;

public class Graph
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Dictionary<Vector2Int, List<Node>> adjacentSet = new Dictionary<Vector2Int, List<Node>>();

    public int Verticies { get { return adjacentSet.Count; } }
    public Graph()
    {

    }
    public void AddEdge(Vector2Int keyVertex, Node adjVertex)
    {
        if (adjacentSet.ContainsKey(keyVertex))
        {
            adjacentSet[keyVertex].Add(adjVertex);
        }
        else
        {
            adjacentSet.Add(keyVertex, new List<Node>() { adjVertex });
        }
    }

    public List<Node> AdjacentVerticies(Vector2Int vertex)
    {
        return adjacentSet[vertex];
    }

    public bool HasNode(Vector2Int vertex)
    {
        if (adjacentSet.ContainsKey(vertex))
            return true;

        return false;
    }
}
