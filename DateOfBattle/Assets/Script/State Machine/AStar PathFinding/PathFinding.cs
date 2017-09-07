#define OPTIMIZE_HEAP
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class PathFinding : MonoBehaviour {
    Grid m_grid;
    PathRequestManager requestManager;
#if UNITY_EDITOR
    public Transform seeker, target;
#endif
    private void Awake()
    {
        m_grid = GetComponent<Grid>();
        requestManager = GetComponent<PathRequestManager>();
#if UNITY_EDITOR
        StartCoroutine(FindPath(seeker.position, target.position));
#endif
    }
#if UNITY_EDITOR
    /// <summary>
    /// This is just for experimenting!
    /// </summary>
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //FindPath(seeker.position, target.position);
        FindPathEditor(seeker.position, target.position);
    }
#endif
    IEnumerator FindPath(Vector3 startPos, Vector3 endPos)
    {
#if UNITY_EDITOR
        Stopwatch sw = new Stopwatch();
        sw.Start();
#endif
        Vector3[] wayPoints = new Vector3[0];
        bool isSuccessFind = false;
        Node startNode = m_grid.NodeFromWorldPt(startPos);
        Node endNode = m_grid.NodeFromWorldPt(endPos);
        if (startNode.walkable && endNode.walkable)
        {
#if OPTIMIZE_HEAP
            Heap<Node> openset = new Heap<Node>(m_grid.maxSize);
#else
        List<Node> openset = new List<Node>();
#endif
            HashSet<Node> closedSet = new HashSet<Node>();
            openset.Add(startNode);
            while (openset.Count > 0)
            {
#if OPTIMIZE_HEAP
                Node currentNode = openset.RemoveFirst();
#else
            Node currentNode = openset[0];
            for (int num = 1; num < openset.Count; ++num)
            {
                if (openset[num].f_cost < currentNode.f_cost || (openset[num].f_cost == currentNode.f_cost && openset[num].h_cost < currentNode.h_cost))
                {
                    currentNode = openset[num];
                }
            }
             openset.Remove(currentNode);
#endif
                closedSet.Add(currentNode);
                if (currentNode == endNode)
                {
#if UNITY_EDITOR
                    sw.Stop();
                    print("Path found around at: " + sw.ElapsedMilliseconds);
#endif
                    //RetracePath(startNode, endNode);
                    isSuccessFind = true;
                    //yield break;
                    break;
                }

                foreach (Node node in m_grid.GetNeighbours(currentNode))
                {
                    if (!node.walkable || closedSet.Contains(node))
                        continue;
                    int newMovementCost = currentNode.g_cost + GetDistance(currentNode, node);
                    if (newMovementCost < node.g_cost || !openset.Contains(node))
                    {
                        node.g_cost = newMovementCost;
                        node.h_cost = GetDistance(node, endNode);
                        node.parent = currentNode;

                        if (!openset.Contains(node))
                        {
                            openset.Add(node);
                        }
                        else
                            openset.UpdateItem(node);
                    }
                }
            }
        }
        yield return null;
        if (isSuccessFind)
        {
            wayPoints = RetracePath(startNode, endNode);
        }
        // TODO: Uncomment this!
        //requestManager.FinishedProcessingPath(wayPoints, isSuccessFind);
    }

    void FindPathEditor(Vector3 startPos, Vector3 endPos)
    {
#if UNITY_EDITOR
        Stopwatch sw = new Stopwatch();
        sw.Start();
#endif
        Vector3[] wayPoints = new Vector3[0];
        Node startNode = m_grid.NodeFromWorldPt(startPos);
        Node endNode = m_grid.NodeFromWorldPt(endPos);
        if (startNode.walkable && endNode.walkable)
        {
#if OPTIMIZE_HEAP
            Heap<Node> openset = new Heap<Node>(m_grid.maxSize);
#else
        List<Node> openset = new List<Node>();
#endif
            HashSet<Node> closedSet = new HashSet<Node>();
            openset.Add(startNode);
            while (openset.Count > 0)
            {
#if OPTIMIZE_HEAP
                Node currentNode = openset.RemoveFirst();
#else
            Node currentNode = openset[0];
            for (int num = 1; num < openset.Count; ++num)
            {
                if (openset[num].f_cost < currentNode.f_cost || (openset[num].f_cost == currentNode.f_cost && openset[num].h_cost < currentNode.h_cost))
                {
                    currentNode = openset[num];
                }
            }
             openset.Remove(currentNode);
#endif
                closedSet.Add(currentNode);
                if (currentNode == endNode)
                {
#if UNITY_EDITOR
                    sw.Stop();
                    print("Path found around at: " + sw.ElapsedMilliseconds);
#endif
                    RetracePath(startNode, endNode);
                    break;
                }

                foreach (Node node in m_grid.GetNeighbours(currentNode))
                {
                    if (!node.walkable || closedSet.Contains(node))
                        continue;
                    int newMovementCost = currentNode.g_cost + GetDistance(currentNode, node);
                    if (newMovementCost < node.g_cost || !openset.Contains(node))
                    {
                        node.g_cost = newMovementCost;
                        node.h_cost = GetDistance(node, endNode);
                        node.parent = currentNode;

                        if (!openset.Contains(node))
                        {
                            openset.Add(node);
                        }
                        else
                            openset.UpdateItem(node);
                    }
                }
            }
        }
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node anotherCurrentNode = endNode;
        while (anotherCurrentNode != startNode)
        {
            path.Add(anotherCurrentNode);
            anotherCurrentNode = anotherCurrentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        //path.Reverse();
        Array.Reverse(waypoints);
#if UNITY_EDITOR
        m_grid.path = path;
#endif
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = new Vector2();
        for (int i = 1; i < path.Count; ++i)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPos);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        // According to a formula: 14y - 10(x - y)
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridY);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distY + 10 * (distY - distX);
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }
}
