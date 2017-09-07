using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {
    public bool walkable;
    public Vector3 worldPos;
    public int g_cost, h_cost, gridX, gridY;
    public int f_cost
    {
        get
        {
            return h_cost + g_cost;
        }
    }
    public Node parent;

    public Node(bool  _walkable, Vector3 _worldpos, int _x, int _y)
    {
        walkable = _walkable;
        worldPos = _worldpos;
        gridX = _x;
        gridY = _y;
    }

    protected int m_heapIndex;

    public int heapIndex
    {
        get
        {
            return m_heapIndex;
        }

        set
        {
            m_heapIndex = value;
        }
    }

    public int CompareTo(Node other)
    {
        int compare = f_cost.CompareTo(other.f_cost);
        if (compare == 0)
        {
            compare = h_cost.CompareTo(other.h_cost);
        }
        return -compare;
    }
}
