using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    // Resource class
    public class SpawnedResource
    {
        public SpawnedResource(Resource.CurrencyType type, bool unlimited, int amount)
        {
            this.type = type;
            this.unlimited = unlimited;
            this.amount = amount;
        }

        public Resource.CurrencyType type;
        public bool unlimited;
        public int amount;
    }

    // Starting resource class
    [System.Serializable]
    public class StartingResource
    {
        public Transform transform;
        public Currency type;
    }

    // Active instance
    public static WorldGenerator active;

    // Starting resources
    public Currency startingCurrency; 
    public List<Transform> startingResources;

    // List of resources
    public Tilemap resourceGrid;
    public int borderSize = 750;
    public float perlinScale = 500;
    [HideInInspector] public Dictionary<Vector2, SpawnedResource> spawnedResources;

    public void Awake() { active = this; }

    [System.Obsolete]
    public void GenerateWorldData(string seed)
    {
        // Create a new resource grid
        spawnedResources = new Dictionary<Vector2, SpawnedResource>();

        // Set random seed
        Random.seed = seed.GetHashCode();

        // Set starting resources
        SpawnStartingResources();

        // Begin generating
        GenerateWorld();
    }

    // Regenerate a world
    [System.Obsolete]
    public void Reseed()
    {
        // Clear previous data
        resourceGrid.ClearAllTiles();
        spawnedResources = new Dictionary<Vector2, SpawnedResource>();

        // Set new random variables
        int previousSeed = Random.seed;
        int randomNumber = Random.Range(0, 999999999);
        Random.seed = randomNumber.ToString().GetHashCode();
        Gamemode.seed = randomNumber.ToString();

        // Debug
        Debug.Log("Reseeded from " + previousSeed + " to " + Gamemode.seed);

        // Generate world
        GenerateWorld();

        // Close menu
        NewInterface.active.ToggleQuitMenu();
    }

    // Loops through a new chunk and spawns resources based on perlin noise values
    private void GenerateWorld()
    {
        // Loop through x and y coordinates
        for (int x = -borderSize; x < borderSize; x++)
        {
            for (int y = -borderSize; y < borderSize; y++)
            {
                foreach (Currency currency in ScriptableLoader.currencies)
                {
                    // Calculate perlin noise pixel
                    float xCoord = ((float)x / currency.perlin.spawnScale) + currency.perlin.spawnOffset;
                    float yCoord = ((float)y / currency.perlin.spawnScale) + currency.perlin.spawnOffset;
                    float value = Mathf.PerlinNoise(xCoord, yCoord);

                    // Get adjustment based on difficulty
                    float adjustment = 0;

                    // If value exceeds threshold, try and generate
                    if (value >= currency.perlin.spawnThreshold - adjustment)
                    {
                        TrySpawnResource(currency, x, y);
                        break;
                    }
                }
            }
        }
    }

    // Creates the starting resources
    public void SpawnStartingResources()
    {
        foreach (Transform resource in startingResources)
        {
            if (resource != null)
            {
                Vector2Int coords = new Vector2Int((int)resource.position.x / 5, (int)resource.position.y / 5);
                resourceGrid.SetTile(new Vector3Int(coords.x, coords.y, 0), startingCurrency.tile);
                spawnedResources.Add(coords, new SpawnedResource(startingCurrency.type, startingCurrency.unlimited,
                    Random.Range(startingCurrency.minAmount, startingCurrency.maxAmount)));
                Recycler.AddRecyclable(resource.transform);
            }
        }
    }

    // Try and spawn a resource
    private void TrySpawnResource(Currency resource, int x, int y)
    {
        // Get x and y pos
        Vector2Int coords = new Vector2Int(x, y);

        // Check cell to make sure it's empty
        if (!spawnedResources.ContainsKey(coords) && CheckDistance(resource.perlin, coords))
        {
            // Create the resource
            resourceGrid.SetTile(new Vector3Int(coords.x, coords.y, 0), resource.tile);
            spawnedResources.Add(coords, new SpawnedResource(resource.type, resource.unlimited,
                Random.Range(resource.minAmount, resource.maxAmount)));
        }
    }

    // Checks distance based on resource bounds
    public bool CheckDistance(Perlin resource, Vector2Int coords)
    {
        return (coords.x > resource.minSpawnDistance || coords.x < -resource.minSpawnDistance ||
               coords.y > resource.minSpawnDistance || coords.y < -resource.minSpawnDistance) &&
               coords.x < resource.maxSpawnDistance && coords.x > -resource.maxSpawnDistance &&
               coords.y < resource.maxSpawnDistance && coords.y > -resource.maxSpawnDistance;
    }

    // Check if a resource node exists
    public bool CheckNode(Vector2 coords, Resource.CurrencyType type)
    {
        Vector2 adjustedCoords = new Vector2(coords.x / 5, coords.y / 5);

        if (spawnedResources.ContainsKey(adjustedCoords) &&
            spawnedResources[adjustedCoords].type == type) return true;
        else return false;
    }

    // Get resource :)
    public float GetResourceModification(Resource.CurrencyType type)
    {
        switch (type)
        {
            case Resource.CurrencyType.Gold:
                if (Gamemode.difficulty.goldSpawnModifier > 0.5f) return 0.1f;
                return Gamemode.difficulty.goldSpawnModifier;
            case Resource.CurrencyType.Essence:
                if (Gamemode.difficulty.goldSpawnModifier > 0.5f) return 0.1f;
                return Gamemode.difficulty.goldSpawnModifier;
            case Resource.CurrencyType.Iridium:
                if (Gamemode.difficulty.goldSpawnModifier > 0.5f) return 0.1f;
                return Gamemode.difficulty.goldSpawnModifier;
            default:
                return 0.1f;
        }
    }
}