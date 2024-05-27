using System;
using UnityEngine;

[Serializable]
public class Node
{
    public int gridX, gridY;
    [field: SerializeField] public bool IsObstacle { get; private set; }
    public Vector3 worldPosition;

    public float gCost;
    public float hCost;
    public Node parent;

    public Node(bool IsObstacle, Vector3 worldPosition, int gridX, int gridY)
    {
        this.IsObstacle = IsObstacle;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public float fCost
    {
        get { return gCost + hCost; }
    }
    public void SetObstacle(bool value)
    {
        IsObstacle = value;
    }
}
