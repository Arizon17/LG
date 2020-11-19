using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node
{
    public int f;
    public int h;
    public int g;
    public Node parent;
    public Vector3Int position;

    public Node(Vector3Int position)
    {
        this.position = position;
    }
}
