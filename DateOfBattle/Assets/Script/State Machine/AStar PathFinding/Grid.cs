using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    Node[,] grid;
    public float nodeRadius = 0.5f;
    public Transform selectUnitTransform;

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    public int maxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }
    // The singleton for Grid
    public static Grid Instance
    {
        get; private set;
    }

    private void Awake()
    {
        // Setting up the singleton!
        if (Instance && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateGrid();
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x * 0.5f - Vector3.up * gridWorldSize.y * 0.5f;

        for (int x = 0; x < gridSizeX; ++x)
        {
            for (int y = 0; y < gridSizeY; ++y)
            {
                Vector3 worldPt = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                // Have to minus 0.1f otherwise it will occupy more than 1 cube!
                bool walkable = !Physics2D.OverlapCircle(worldPt, nodeRadius - 0.1f, unwalkableMask);
                grid[x, y] = new Node(walkable, worldPt, x, y);
            }
        }
    }

    public Node NodeFromWorldPt(Vector3 worldPos)
    {
        float percentX = ((worldPos.x - transform.position.x) + gridWorldSize.x * 0.5f) / gridWorldSize.x;
        float percentY = ((worldPos.y - transform.position.y) + gridWorldSize.y * 0.5f) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Node> path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
        if (grid != null)
        {
            Node playerNode = null;
            if (selectUnitTransform)
                playerNode = NodeFromWorldPt(selectUnitTransform.position);


            foreach (Node n in grid)
            {
                //if (playerNode != null && playerNode == n)
                //{
                //    Gizmos.color = Color.cyan;
                //}
                //else if (path != null && path.Contains(n))
                //{
                //    Gizmos.color = Color.black;
                //}
                //else
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawWireCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> listOfNeighbours = new List<Node>();

        for (int x = -1; x <= 1; ++x)
        {
            for (int y = -1; y <= 1; ++y)
            {
                // center and corners are not allowed!
                if (x == 0 && y == 0 || (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1))
                    continue;
                // Center is not allowed
                //if (x == 0 && y == 0)
                //    continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    listOfNeighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return listOfNeighbours;
    }

}
