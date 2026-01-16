using UnityEngine;

public class Node
{
    Vector2Int vertex;
    float edgeWeight;
    bool edgeVent;

    public Vector2Int Vertex { get { return vertex; } }

    public float EdgeWeight { get { return edgeWeight; } }

    public bool EdgeVent { get { return edgeVent; } }

    public Node(Vector2Int vertex, float edgeWeight, bool edgeVent)
    {
        this.vertex = vertex;
        this.edgeWeight = edgeWeight;
        this.edgeVent = edgeVent;
    }
}
