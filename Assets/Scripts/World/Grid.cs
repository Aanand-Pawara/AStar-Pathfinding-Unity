using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Node[,] mGrid { get; set; }
    private int gridSize { get; set; }

    public void CreateGrid(int size)
    {
        gridSize = size;
        mGrid = new Node[size, size];
    }

    public Node GetNode(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < gridSize && z < gridSize)
        {
            return mGrid[x, z];
        }
        return null;
    }
    public Node GetNode(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x);
        int z = Mathf.RoundToInt(position.z);
       

        if (x >= 0 && z >= 0 && x < gridSize && z < gridSize)
        {
            return mGrid[x, z];
        }
        return null;
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int newX = node.gridX + dx[i];
            int newY = node.gridY + dy[i];

            Node neighbor = GetNode(newX, newY);
            if (neighbor != null && !neighbor.IsObstacle)
            {
                neighbors.Add(neighbor);
            }
        }
        return neighbors;
    }

    void OnDrawGizmos()
    {
        if (mGrid != null)
        {
            foreach (Node node in mGrid)
            {
                Gizmos.color = (node.IsObstacle) ? Color.red : Color.white;
                Gizmos.DrawSphere(node.worldPosition, 0.1f);
            }
        }
    }
}
