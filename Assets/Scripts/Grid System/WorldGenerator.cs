using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    // Active instance
    public static WorldGenerator active;

    // List of resource tiles
    public int borderSize = 750;
    public float perlinScale = 500;
    [SerializeField] protected List<Spawnable> spawnables;
    [HideInInspector] public Dictionary<Vector2Int, Spawnable> spawnedResources;

    public void GenerateWorldData()
    {
        // Get active instance
        active = this;

        // Create a new resource grid
        spawnedResources = new Dictionary<Vector2Int, Spawnable>();

        // Set random seed
        Random.InitState(GameManager.seed.GetHashCode());

        // Iterate through offset values
        int startingRange = 0;
        foreach (Spawnable spawnable in spawnables)
            spawnable.spawnOffset = Random.Range(startingRange, startingRange += 10000);

        // Begin generating
        GenerateWorld();
    }


    // Loops through a new chunk and spawns resources based on perlin noise values
    private void GenerateWorld()
    {
        // Loop through x and y coordinates
        for (int x = -borderSize; x < borderSize; x++)
        {
            for (int y = -borderSize; y < borderSize; y++)
            {
                foreach (Spawnable spawnable in spawnables)
                {
                    // Calculate perlin noise pixel
                    float xCoord = ((float)x / spawnable.spawnScale) + spawnable.spawnOffset;
                    float yCoord = ((float)y / spawnable.spawnScale) + spawnable.spawnOffset;
                    float value = Mathf.PerlinNoise(xCoord, yCoord);

                    // If value exceeds threshold, try and generate
                    if (value >= spawnable.spawnThreshold)
                    {
                        TrySpawnResource(spawnable, x, y);
                        break;
                    }
                }
            }
        }
    }

    // Try and spawn a resource
    private void TrySpawnResource(Spawnable resource, int x, int y)
    {
        // Get x and y pos
        int xPos = x * 5;
        int yPos = y * 5;
        Vector2Int coords = new Vector2Int(xPos, yPos);

        // Check cell to make sure it's empty
        if (!spawnedResources.ContainsKey(coords))
        {
            // Create the resource
            Spawnable temp = Instantiate(resource.gameObject, new Vector3(xPos, yPos, -1), Quaternion.identity).GetComponent<Spawnable>();
            spawnedResources.Add(coords, temp);
            temp.name = resource.name;
        }
    }
}