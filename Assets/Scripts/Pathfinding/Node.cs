using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Node> neighbours;
    public int x;
    public int z;

    public bool isTaken;
    public Node()
    {
        neighbours = new List<Node>();
    }
    public float DistanceTo(Node n)
    {
        return Vector2.Distance(new Vector2(x, z), new Vector2(n.x, n.z));
    }
}
