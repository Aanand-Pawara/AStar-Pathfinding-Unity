using UnityEngine;
using static Generator;

public class TileInfo : MonoBehaviour
{
    public TileType tileType;

    [field:SerializeField] public Node Node { get; private set; }

    public void SetTile(TileType type, Node node)
    {
        this.tileType = type;
        this.Node = node;
    }
}