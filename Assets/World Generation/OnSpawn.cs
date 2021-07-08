using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSpawn : MonoBehaviour
{
    // Tile layer
    [SerializeField]
    private LayerMask TileLayer;
    public LayerMask HiveLayer;
    public int WorldSeed;

    // Difficulty class
    public GameObject DefaultOre;

    // Border size
    public float BorderSize = 750;

    // Gold ore placements
    public GameObject GoldOre;
    public int GoldSpawnSize;
    public int GoldSpawnScale;
    public int GoldSpawnOffset;
    public float GoldSpawnThreshold;
    public int GoldMinDistance;

    // Essence ore placements
    public GameObject EssenceOre;
    public int EssenceSpawnSize;
    public int EssenceSpawnScale;
    public int EssenceSpawnOffset;
    public float EssenceSpawnThreshold;
    public int EssenceMinDistance;

    // Iridium ore placements
    public GameObject IridiumOre;
    public int IridiumSpawnSize;
    public int IridiumSpawnScale;
    public int IridiumSpawnOffset;
    public float IridiumSpawnThreshold;
    public int IridiumMinDistance;

    // Enemy base spawning 
    public int BaseRegionSize;
    public GameObject[] EnemyBases;
    public int maxBases;

    public void GenerateWorldData(string a, bool save, bool defOverride = false)
    {
        try
        {
            Debug.Log("Generating world data with seed " + a);
        }
        catch
        {
            Debug.Log("Could not set seed properly");
            Random.seed = Random.Range(0, 1000000000);
        }

        // Sets the seed - This method is deprecated but idc 
        #pragma warning disable CS0618
        try { Random.seed = a.GetHashCode(); }
        catch { Random.seed = Random.Range(0, 1000000000); }
        #pragma warning restore CS0618

        // Offset the perlin noise generation based on the seed provided
        GoldSpawnOffset += Random.Range(0, 10000);
        EssenceSpawnOffset += Random.Range(10000, 20000);
        IridiumSpawnOffset += Random.Range(20000, 30000);

        // If def set to override, skip difficulty setting and use default values
        try
        {
            if (!defOverride)
            {
                if (Difficulties.goldMulti >= 250) { Difficulties.goldMulti = 250; }
                if (Difficulties.essenceMulti >= 250) { Difficulties.essenceMulti = 250; }
                if (Difficulties.iridiumMulti >= 250) { Difficulties.iridiumMulti = 250; }

                GoldSpawnScale -= (int)(Difficulties.goldMulti / 100);
                GoldSpawnThreshold -= Difficulties.goldMulti / 2500;

                EssenceSpawnScale -= (int)(Difficulties.essenceMulti / 100);
                EssenceSpawnThreshold -= Difficulties.essenceMulti / 2500;

                IridiumSpawnScale -= (int)(Difficulties.iridiumMulti / 100);
                IridiumSpawnThreshold -= Difficulties.iridiumMulti / 2500;

                maxBases = (int)(maxBases * (Difficulties.enemyAmountMulti / 50));
            }
        }
        catch { Debug.Log("Could not set difficulty data"); }

        // Gen the world
        GenGold();
        GenEssence();
        GenIridium();

        // Gen bases ONLY if a fresh save is being loaded
        if (defOverride || (!save && Difficulties.enemyOutposts)) { GenBases(); }
    }

    // Enemy Base Generation
    private void GenBases()
    {
        int x;
        int y;
        int att = 0;

        bool valid = false;

        for (int i = 0; i < maxBases; i++)
        {
            while (!valid)
            {
                x = Random.Range(-BaseRegionSize, BaseRegionSize) * 5;
                y = Random.Range(-BaseRegionSize, BaseRegionSize) * 5;

                if (x > 150 || x < -150 || y > 150 || y < -150)
                {
                    var colliders = Physics2D.OverlapBoxAll(new Vector2(x, y), new Vector2(100, 100), 0, 1 << LayerMask.NameToLayer("Enemy"));
                    if (colliders.Length == 0)
                    {
                        // Generate base
                        GameObject enemyBase = EnemyBases[Random.Range(0, EnemyBases.Length)];
                        var temp = Instantiate(enemyBase, new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
                        temp.name = enemyBase.name;

                        valid = true;
                    }
                }

                att++;
                if (att > 10)
                    valid = true;
            }
            att = 0;
            valid = false;
        }
    }

    // Gold Generation
    private void GenGold()
    {
        int totalSpawned = 0;
        float spawnOffset = (GoldSpawnSize * 5f) / 2;

        for (int x = 0; x < GoldSpawnSize; x++)
        {
            for (int y = 0; y < GoldSpawnSize; y++)
            {
                float xCoord = ((float)x / GoldSpawnSize * GoldSpawnScale) + GoldSpawnOffset;
                float yCoord = ((float)y / GoldSpawnSize * GoldSpawnScale) + GoldSpawnOffset;
                float value = Mathf.PerlinNoise(xCoord, yCoord);

                if (value >= GoldSpawnThreshold)
                {
                    totalSpawned++;

                    float xPos = (x * 5f) - spawnOffset;
                    float yPos = (y * 5f) - spawnOffset;

                    if (!checkCoords(xPos, yPos, GoldMinDistance)) continue;

                    RaycastHit2D rayCheck = Physics2D.Raycast(new Vector2(xPos, yPos), Vector2.zero, Mathf.Infinity, TileLayer);
                    if (rayCheck.collider == null)
                    {
                        var temp = Instantiate(GoldOre, new Vector3(xPos, yPos, -1), Quaternion.identity);
                        temp.name = GoldOre.name;
                    }
                }
            }
        }

        Debug.Log("Spawned " + totalSpawned + " gold nodes");
    }

    private void GenEssence()
    {
        int totalSpawned = 0;
        float spawnOffset = (EssenceSpawnSize * 5f) / 2;

        for (int x = 0; x < EssenceSpawnSize; x++)
        {
            for (int y = 0; y < EssenceSpawnSize; y++)
            {
                float xCoord = ((float)x / EssenceSpawnSize * EssenceSpawnScale) + EssenceSpawnOffset;
                float yCoord = ((float)y / EssenceSpawnSize * EssenceSpawnScale) + EssenceSpawnOffset;
                float value = Mathf.PerlinNoise(xCoord, yCoord);

                if (value >= EssenceSpawnThreshold)
                {
                    totalSpawned++;

                    float xPos = (x * 5f) - spawnOffset;
                    float yPos = (y * 5f) - spawnOffset;

                    if (!checkCoords(xPos, yPos, EssenceMinDistance)) continue;

                    RaycastHit2D rayCheck = Physics2D.Raycast(new Vector2(xPos, yPos), Vector2.zero, Mathf.Infinity, TileLayer);
                    if (rayCheck.collider == null)
                    {
                        var temp = Instantiate(EssenceOre, new Vector3(xPos, yPos, -1), Quaternion.identity);
                        temp.name = EssenceOre.name;
                    }
                }
            }
        }

        Debug.Log("Spawned " + totalSpawned + " essence nodes");
    }

    private void GenIridium()
    {
        int totalSpawned = 0;
        float spawnOffset = (IridiumSpawnSize * 5f) / 2;

        for (int x = 0; x < IridiumSpawnSize; x++)
        {
            for (int y = 0; y < IridiumSpawnSize; y++)
            {
                float xCoord = ((float)x / IridiumSpawnSize * IridiumSpawnScale) + IridiumSpawnOffset;
                float yCoord = ((float)y / IridiumSpawnSize * IridiumSpawnScale) + IridiumSpawnOffset;
                float value = Mathf.PerlinNoise(xCoord, yCoord);

                if (value >= IridiumSpawnThreshold)
                {
                    totalSpawned++;

                    float xPos = (x * 5f) - spawnOffset;
                    float yPos = (y * 5f) - spawnOffset;

                    if (!checkCoords(xPos, yPos, IridiumMinDistance)) continue;

                    RaycastHit2D rayCheck = Physics2D.Raycast(new Vector2(xPos, yPos), Vector2.zero, Mathf.Infinity, TileLayer);
                    if (rayCheck.collider == null)
                    {
                        var temp = Instantiate(IridiumOre, new Vector3(xPos, yPos, -1), Quaternion.identity);
                        temp.name = IridiumOre.name;
                    }
                }
            }
        }

        Debug.Log("Spawned " + totalSpawned + " iridium nodes");
    }

    public bool checkCoords(float x, float y, int minDistance)
    {
        if ((x >= minDistance || x <= -minDistance || y >= minDistance || y <= -minDistance) && x <= BorderSize && x >= -BorderSize+5 && y >= -BorderSize+5 && y <= BorderSize) return true;
        else return false;
    }
}
