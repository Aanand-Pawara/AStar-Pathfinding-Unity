using System;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    
    [SerializeField] private WorldPrefabs worldPrefabs;
    [SerializeField] private int gridSize = 10;
    [SerializeField] private float noiseScale = 0.3f;
    [SerializeField] private float lakeThreshold = 0.3f;
    [SerializeField] private float sandThreshold = 0.35f;
    [SerializeField] private int Seed = 134;

    [SerializeField] [Range(0.1f, 20f)] private float simulationSpeed = 1.0f;
    public float DELTA_TIME { get; private set; }

    private GameObject[,] gridArray;
    public static Generator Instance { get; private set; }

    [field: SerializeField] public Grid mGrid { get; private set; }
    [field: SerializeField] public Pathfinding mPathfinding { get; private set; }

    private Camera mainCamera;
    private Plane[] frustumPlanes;

    public bool UseCulling = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        mainCamera = Camera.main;
        Generate();
    }

    private void Start()
    {
        
        UpdateTileVisibility();
    }

    private void Update()
    {
        UpdateTileVisibility();

       DELTA_TIME = Time.deltaTime * simulationSpeed;
    }

    public void Generate()
    {
        DestroyAllChildren();

        gridArray = new GameObject[gridSize, gridSize];
        mGrid.CreateGrid(gridSize);
        float[,] heightMap = GenerateHeightMap(Seed);

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                TileType type = DetermineTileType(heightMap[x, z]);
                GameObject prefab = GetPrefabByTileType(type);
                Vector3 position = new Vector3(x, type == TileType.Stone ? 1 : 0, z);

                Quaternion rotation = Quaternion.identity;
                if (type == TileType.Water)
                {
                    position.y = -0.25f;
                }

                GameObject tile = Instantiate(prefab, position, rotation, transform);
                tile.name = $"Tile_{x}_{z}_{type}";

                bool IsObstacle = type == TileType.Water;
                mGrid.mGrid[x, z] = new Node(IsObstacle, position, x, z);

                tile.GetComponent<TileInfo>().SetTile(type, mGrid.mGrid[x, z]);
                gridArray[x, z] = tile;
            }
        }

        PlaceSandTiles();
    }

    public void DestroyAllChildren()
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        foreach (GameObject child in children)
        {
            DestroyImmediate(child);
        }
    }

    private float[,] GenerateHeightMap(int seed = 1)
    {
        float[,] heightMap = new float[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                // Offset the coordinates using the seed
                float xCoord = ((float)x / gridSize * noiseScale) + seed;
                float zCoord = ((float)z / gridSize * noiseScale) + seed;

                heightMap[x, z] = Mathf.PerlinNoise(xCoord, zCoord);
            }
        }
        return heightMap;
    }


    private TileType DetermineTileType(float height)
    {
        if (height < lakeThreshold)
        {
            return TileType.Water;
        }
        else if (height < sandThreshold)
        {
            return TileType.Sand;
        }
        else if (height < 0.6f)
        {
            return TileType.Dirt;
        }
        else if (height < 0.7f)
        {
            return TileType.Grass;
        }
        else if (height < 0.9f)
        {
            return TileType.Stone;
        }
        else
        {
            return TileType.Grass;
        }
    }

    private void PlaceSandTiles()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                if (gridArray[x, z].GetComponent<TileInfo>().tileType == TileType.Water)
                {
                    PlaceSandAroundTile(x, z);
                }
            }
        }
    }

    private void PlaceSandAroundTile(int x, int z)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dz = -1; dz <= 1; dz++)
            {
                if (dx == 0 && dz == 0) continue;
                int nx = x + dx;
                int nz = z + dz;
                if (IsInBounds(nx, nz) && gridArray[nx, nz].GetComponent<TileInfo>().tileType != TileType.Water)
                {
                    ReplaceTile(nx, nz, TileType.Sand);
                }
            }
        }
    }

    private bool IsInBounds(int x, int z)
    {
        return x >= 0 && x < gridSize && z >= 0 && z < gridSize;
    }

    private void ReplaceTile(int x, int z, TileType type)
    {
        DestroyImmediate(gridArray[x, z]);
        GameObject prefab = GetPrefabByTileType(type);
        Vector3 position = new Vector3(x, type == TileType.Stone ? 1 : 0, z);
        Quaternion rotation = Quaternion.identity;
        if (type == TileType.Water)
        {
            position.y = -0.25f;
        }
        GameObject tile = Instantiate(prefab, position, rotation, transform);
        tile.name = $"Tile_{x}_{z}_{type}";
        tile.GetComponent<TileInfo>().SetTile(type, mGrid.mGrid[x, z]);
        gridArray[x, z] = tile;
    }

    private GameObject GetPrefabByTileType(TileType type)
    {
        switch (type)
        {
            case TileType.Water:
                return worldPrefabs.cubeWater;
            case TileType.Sand:
                return worldPrefabs.cubeSand;
            case TileType.Grass:
                return worldPrefabs.cubeGrass;
            case TileType.Dirt:
                return worldPrefabs.cubeDirt;
            case TileType.Stone:
                return worldPrefabs.cubeStone;
            default:
                return worldPrefabs.cubeDirt;
        }
    }

    private void UpdateTileVisibility()
    {
        if (!UseCulling) return;

        frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                if (gridArray[x, z] != null)
                {
                    Vector3 position = gridArray[x, z].transform.position;
                    Bounds bounds = new Bounds(position, Vector3.one);
                    bool isVisible = GeometryUtility.TestPlanesAABB(frustumPlanes, bounds);
                    gridArray[x, z].SetActive(isVisible);
                }
            }
        }
    }

    public enum TileType
    {
        Dirt,
        Water,
        Grass,
        Stone,
        Sand
    }
}
