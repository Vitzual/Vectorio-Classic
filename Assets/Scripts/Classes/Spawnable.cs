using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Spawnable
{
    public Resource.CurrencyType type;
    public TileBase tile;
    public float spawnScale;
    public float spawnThreshold;
    public float minSpawnDistance;
    public float maxSpawnDistance;
    [HideInInspector] public float spawnOffset;
}
