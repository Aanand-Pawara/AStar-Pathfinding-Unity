using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Grid mGrid => Generator.Instance.mGrid;

    public List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = mGrid.GetNode(Mathf.RoundToInt(startPos.x), Mathf.RoundToInt(startPos.z));
        Node targetNode = mGrid.GetNode(Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.z));

        if (startNode == null || targetNode == null || startNode.IsObstacle || targetNode.IsObstacle)
        {
            return null;
        }

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                List<Node> path = RetracePath(startNode, targetNode);
                return SmoothPath(path);
            }

            foreach (Node neighbor in mGrid.GetNeighbors(currentNode))
            {
                if (neighbor.IsObstacle || closedSet.Contains(neighbor))
                {
                    continue;
                }

                float newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        return null;
    }



    private List<Vector3> SmoothPath(List<Node> path)
    {
        List<Vector3> smoothedPath = new List<Vector3>();

        if (path.Count < 2)
        {
            return smoothedPath;
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 currentWaypoint = path[i].worldPosition;
            Vector3 nextWaypoint = path[i + 1].worldPosition;
            Vector3 direction = (nextWaypoint - currentWaypoint).normalized;
            Vector3 offset = direction * (1f / 2f);

            smoothedPath.Add(currentWaypoint + offset);
            smoothedPath.Add(nextWaypoint - offset);
        }

        smoothedPath.Add(path[path.Count - 1].worldPosition);

        return smoothedPath;
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    private float GetDistance(Node nodeA, Node nodeB)
    {
        return Vector3.Distance(nodeA.worldPosition, nodeB.worldPosition);
    }
}
