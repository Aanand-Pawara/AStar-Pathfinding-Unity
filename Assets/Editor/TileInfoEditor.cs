using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileInfo))]
public class TileInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TileInfo tileInfo = (TileInfo)target;

        if (GUILayout.Button(tileInfo.Node != null && tileInfo.Node.IsObstacle ? "Make Obstacle" : "Make Walkable"))
        {
            ToggleWalkable(tileInfo);
        }
    }

    private void ToggleWalkable(TileInfo tileInfo)
    {
        if (tileInfo.Node != null)
        {
            tileInfo.Node.SetObstacle(!tileInfo.Node.IsObstacle);
            Debug.Log($"Tile at ({tileInfo.Node.gridX}, {tileInfo.Node.gridY}) walkable status toggled to {tileInfo.Node.IsObstacle}");
        }
        else
        {
            Debug.LogWarning("Node is not assigned.");
        }
    }
}
