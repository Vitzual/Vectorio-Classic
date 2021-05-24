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

    // Gold ore placements
    public GameObject GoldOre;
    public int GoldRegionSize;
    public int GoldSpawnAmount;
    public int GoldVeinSize;
    public int GoldVeinNoise;
    public int GoldMinDistance;

    // Essence ore placements
    public GameObject EssenceOre;
    public int EssenceRegionSize;
    public int EssenceSpawnAmount;
    public int EssenceVeinSize;
    public int EssenceVeinNoise;
    public int EssenceMinDistance;

    // Iridium ore placements
    public GameObject IridiumOre;
    public int IridiumRegionSize;
    public int IridiumSpawnAmount;
    public int IridiumVeinSize;
    public int IridiumVeinNoise;
    public int IridiumMinDistance;

    // Enemy spawner thing
    public GameObject Hive;
    public int SpawnerRegionSize;
    public int SpawnerSpawnAmount;

    // Enemy base spawning 
    public int BaseRegionSize;
    public GameObject[] EnemyBases;
    public int maxBases;

    public void GenerateWorldData(string a, bool save, bool defOverride = false)
    {
        Debug.Log("Generating world data with seed " + a);

        // Sets the seed - This method is deprecated but works so who cares amiright?
        #pragma warning disable CS0618
        try { Random.seed = a.GetHashCode(); }
        catch { Random.seed = Random.Range(0, 1000000000); }
        #pragma warning restore CS0618

        if (!defOverride)
        {
            if (Difficulties.goldMulti >= 250) { Difficulties.goldMulti = 250; }
            if (Difficulties.essenceMulti >= 250) { Difficulties.essenceMulti = 250; }
            if (Difficulties.iridiumMulti >= 250) { Difficulties.iridiumMulti = 250; }

            GoldSpawnAmount = (int)(GoldSpawnAmount * (Difficulties.goldMulti / 100));
            GoldVeinSize = (int)(GoldVeinSize * (Difficulties.goldMulti / 100));
            GoldVeinNoise = (int)(GoldVeinNoise * (Difficulties.goldMulti / 100));

            EssenceSpawnAmount = (int)(EssenceSpawnAmount * (Difficulties.essenceMulti / 100));
            EssenceVeinSize = (int)(EssenceVeinSize * (Difficulties.essenceMulti / 100));
            EssenceVeinNoise = (int)(EssenceVeinNoise * (Difficulties.essenceMulti / 100));

            IridiumSpawnAmount = (int)(IridiumSpawnAmount * (Difficulties.iridiumMulti / 100));
            IridiumVeinSize = (int)(IridiumVeinSize * (Difficulties.iridiumMulti / 100));
            IridiumVeinNoise = (int)(IridiumVeinNoise * (Difficulties.iridiumMulti / 100));

            SpawnerSpawnAmount = (int)(SpawnerSpawnAmount * (Difficulties.enemyAmountMulti / 100));
            maxBases = (int)(maxBases * (Difficulties.enemyAmountMulti / 50));
        }

        GenGold();
        GenEssence();
        GenIridium();

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

                if (x > 100 || x < -100 || y > 100 || y < -100)
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

    // Enemy Hive Generation
    private void GenHives()
    {
        int x;
        int y;
        int max = 10;
        int att = 0;

        bool valid = false;

        for (int i = 0; i < SpawnerSpawnAmount; i++)
        {
            while (!valid)
            {
                x = Random.Range(-SpawnerRegionSize, SpawnerRegionSize) * 5;
                y = Random.Range(-SpawnerRegionSize, SpawnerRegionSize) * 5;

                if (x > 100 || x < -100 || y > 100 || y < -100)
                {
                    RaycastHit2D a = Physics2D.Raycast(new Vector2(x, y), Vector2.zero, Mathf.Infinity, HiveLayer);
                    RaycastHit2D b = Physics2D.Raycast(new Vector2(x, y), Vector2.zero, Mathf.Infinity, TileLayer);

                    if (a.collider == null && b.collider == null)
                    {
                        valid = true;
                        var temp = Instantiate(Hive, new Vector3(x, y, -1), Quaternion.Euler(0, 0, 0));
                        temp.name = Hive.name;
                    }
                }

                att++;
                if (att >= max)
                    valid = true;
            }
            att = 0;
            valid = false;
        }
    }

    // Gold Generation
    private void GenGold()
    {
        int x;
        int y;
        int a;
        int b;

        for (int i = 0; i < GoldSpawnAmount; i++)
        {
            x = Random.Range(-GoldRegionSize, GoldRegionSize) * 5;
            y = Random.Range(-GoldRegionSize, GoldRegionSize) * 5;

            RaycastHit2D rayCheck = Physics2D.Raycast(new Vector2(x, y), Vector2.zero, Mathf.Infinity, TileLayer);

            if (rayCheck.collider == null && checkCoords(x, y, GoldMinDistance))
            {
                var temp = Instantiate(GoldOre, new Vector3(x, y, -1), Quaternion.identity);
                temp.name = GoldOre.name;

                for (int c = 0; c < GoldVeinNoise; c++)
                {
                    a = Random.Range(-GoldVeinSize, GoldVeinSize) * 5;
                    b = Random.Range(-GoldVeinSize, GoldVeinSize) * 5;

                    RaycastHit2D d = Physics2D.Raycast(new Vector2(x + a + 5f, y + b), Vector2.zero, Mathf.Infinity, TileLayer);
                    RaycastHit2D e = Physics2D.Raycast(new Vector2(x + a - 5f, y + b), Vector2.zero, Mathf.Infinity, TileLayer);
                    RaycastHit2D f = Physics2D.Raycast(new Vector2(x + a, y + b + 5f), Vector2.zero, Mathf.Infinity, TileLayer);
                    RaycastHit2D g = Physics2D.Raycast(new Vector2(x + a, y + b - 5f), Vector2.zero, Mathf.Infinity, TileLayer);
                    RaycastHit2D h = Physics2D.Raycast(new Vector2(x + a, y + b), Vector2.zero, Mathf.Infinity, TileLayer);

                    if (checkCoords(x + a, y + b, GoldMinDistance) && (d.collider != null || e.collider != null || f.collider != null || g.collider != null) && h.collider == null)
                    {
                        temp = Instantiate(GoldOre, new Vector3(x + a, y + b, -1), Quaternion.identity);
                        temp.name = GoldOre.name;
                    }
                }
            } 
            else
            {
                i--;
            }
        }
    }

    private void GenEssence()
    {
        int x;
        int y;
        int a;
        int b;

        for (int i = 0; i < EssenceSpawnAmount; i++)
        {
            x = Random.Range(-EssenceRegionSize, EssenceRegionSize) * 5;
            y = Random.Range(-EssenceRegionSize, EssenceRegionSize) * 5;

            RaycastHit2D rayCheck = Physics2D.Raycast(new Vector2(x, y), Vector2.zero, Mathf.Infinity, TileLayer);

            if (rayCheck.collider == null && checkCoords(x, y, EssenceMinDistance))
            {
                var temp = Instantiate(EssenceOre, new Vector3(x, y, -1), Quaternion.identity);
                temp.name = EssenceOre.name;

                for (int c = 0; c < EssenceVeinNoise; c++)
                {
                    a = Random.Range(-EssenceVeinSize, EssenceVeinSize) * 5;
                    b = Random.Range(-EssenceVeinSize, EssenceVeinSize) * 5;

                    RaycastHit2D d = Physics2D.Raycast(new Vector2(x + a + 5f, y + b), Vector2.zero, Mathf.Infinity, TileLayer);
                    RaycastHit2D e = Physics2D.Raycast(new Vector2(x + a - 5f, y + b), Vector2.zero, Mathf.Infinity, TileLayer);
                    RaycastHit2D f = Physics2D.Raycast(new Vector2(x + a, y + b + 5f), Vector2.zero, Mathf.Infinity, TileLayer);
                    RaycastHit2D g = Physics2D.Raycast(new Vector2(x + a, y + b - 5f), Vector2.zero, Mathf.Infinity, TileLayer);
                    RaycastHit2D h = Physics2D.Raycast(new Vector2(x + a, y + b), Vector2.zero, Mathf.Infinity, TileLayer);

                    if (checkCoords(x + a, y + b, EssenceMinDistance) && (d.collider != null || e.collider != null || f.collider != null || g.collider != null) && h.collider == null)
                    {
                        temp = Instantiate(EssenceOre, new Vector3(x + a, y + b, -1), Quaternion.identity);
                        temp.name = EssenceOre.name;
                    }
                }
            }
            else
            {
                i--;
            }
        }
    }

    private void GenIridium()
    {
        int x;
        int y;
        int a;
        int b;

        for (int i = 0; i < IridiumSpawnAmount; i++)
        {
            x = Random.Range(-IridiumRegionSize, IridiumRegionSize) * 5;
            y = Random.Range(-IridiumRegionSize, IridiumRegionSize) * 5;

            RaycastHit2D rayCheck = Physics2D.Raycast(new Vector2(x, y), Vector2.zero, Mathf.Infinity, TileLayer);

            if (rayCheck.collider == null && checkCoords(x, y, IridiumMinDistance))
            {
                var temp = Instantiate(IridiumOre, new Vector3(x, y, -1), Quaternion.identity);
                temp.name = IridiumOre.name;

                for (int c = 0; c < IridiumVeinNoise; c++)
                {
                    a = Random.Range(-IridiumVeinSize, IridiumVeinSize) * 5;
                    b = Random.Range(-IridiumVeinSize, IridiumVeinSize) * 5;

                    RaycastHit2D d = Physics2D.Raycast(new Vector2(x + a + 5f, y + b), Vector2.zero, Mathf.Infinity, TileLayer);
                    RaycastHit2D e = Physics2D.Raycast(new Vector2(x + a - 5f, y + b), Vector2.zero, Mathf.Infinity, TileLayer);
                    RaycastHit2D f = Physics2D.Raycast(new Vector2(x + a, y + b + 5f), Vector2.zero, Mathf.Infinity, TileLayer);
                    RaycastHit2D g = Physics2D.Raycast(new Vector2(x + a, y + b - 5f), Vector2.zero, Mathf.Infinity, TileLayer);
                    RaycastHit2D h = Physics2D.Raycast(new Vector2(x + a, y + b), Vector2.zero, Mathf.Infinity, TileLayer);

                    if (checkCoords(x + a, y + b, IridiumMinDistance) && (d.collider != null || e.collider != null || f.collider != null || g.collider != null) && h.collider == null)
                    {
                        temp = Instantiate(IridiumOre, new Vector3(x + a, y + b, -1), Quaternion.identity);
                        temp.name = IridiumOre.name;
                    }
                }
            }
            else
            {
                i--;
            }
        }
    }

    public bool checkCoords(int x, int y, int minDistance)
    {
        if ((x >= minDistance || x <= -minDistance || y >= minDistance || y <= -minDistance) && x <= 750 && x >= -745 && y >= -745 && y <= 750) return true;
        else return false;
    }
}
