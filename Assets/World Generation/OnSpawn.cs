using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSpawn : MonoBehaviour
{
    // Tile layer
    [SerializeField]
    private LayerMask TileLayer;
    public int WorldSeed;

    // Gold ore placements
    public GameObject GoldOre;
    public int GoldRegionSize;
    public int GoldSpawnAmount;
    public int GoldVeinSize;
    public int GoldVeinNoise;

    // Essence ore placements
    public GameObject EssenceOre;
    public int EssenceRegionSize;
    public int EssenceSpawnAmount;
    public int EssenceVeinSize;
    public int EssenceVeinNoise;

    // Iridium ore placements
    public GameObject IridiumOre;
    public int IridiumRegionSize;
    public int IridiumSpawnAmount;
    public int IridiumVeinSize;
    public int IridiumVeinNoise;

    public void GenerateWorldData(int a)
    {
        // Sets the seed - This method is deprecated but works so who cares amiright?
        Random.seed = a;

        GenGold();
        GenEssence();
        GenIridium();
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
            x = Random.Range(-GoldRegionSize, GoldRegionSize)*5;
            y = Random.Range(-GoldRegionSize, GoldRegionSize)*5;

            if ((x >= 15 + GoldVeinSize || x <= -15 - GoldVeinSize) && (y >= 15 + GoldVeinSize || y <= -15 - GoldVeinSize))
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

                    if ((d.collider != null || e.collider != null || f.collider != null || g.collider != null) && h.collider == null && (x + a >= 15 || x + a <= -15) && (y + b >= 15 || y - b <= 15))
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

            if ((x >= 35 + EssenceVeinSize || x <= -35 - EssenceVeinSize) && (y >= 35 + EssenceVeinSize || y <= -35 - EssenceVeinSize))
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

                    if ((d.collider != null || e.collider != null || f.collider != null || g.collider != null) && h.collider == null && (x + a >= 15 || x + a <= -15) && (y + b >= 15 || y - b <= 15))
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

            if ((x >= 55 + IridiumVeinSize || x <= -55 - IridiumVeinSize) && (y >= 55 + IridiumVeinSize || y <= -55 - IridiumVeinSize))
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

                    if ((d.collider != null || e.collider != null || f.collider != null || g.collider != null) && h.collider == null && (x + a >= 15 || x + a <= -15) && (y + b >= 15 || y - b <= 15))
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
}
