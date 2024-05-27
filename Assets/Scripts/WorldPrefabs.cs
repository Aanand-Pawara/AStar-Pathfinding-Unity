using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldPrefabs", menuName = "ScriptableObjects/WorldPrefabs", order = 1)]
public class WorldPrefabs : ScriptableObject
{
    public GameObject cubeGrass;
    public GameObject cubeDirt;
    public GameObject cubeStone;
    public GameObject cubeWater;
    public GameObject cubeSand;

}
