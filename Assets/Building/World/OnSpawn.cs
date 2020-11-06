using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSpawn : MonoBehaviour
{
    // Tile layer
    [SerializeField]
    private LayerMask TileLayer;

    // Gold ore placements
    public GameObject GoldOre;
    public int GoldRegionSize;
    public int GoldSpawnAmount;
    public int GoldVeinSize;
    public int GoldVeinNoise;
    public int WorldSeed;

    // Start is called before the first frame update
    void Start()
    {
        // Specify the seed - random by default
        if (WorldSeed == 0)
        {
            // Generates random seed between 1000000 and 9999999 (all inclusive)
            WorldSeed = Random.Range(1000000, 10000000);
        }

        // Sets the seed - This method is deprecated but works so who cares amiright?
        Random.seed = WorldSeed;

        GenGold();
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
                var temp = Instantiate(GoldOre, new Vector3(x, y, 0), Quaternion.identity);
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

                    if ((d.collider != null || e.collider != null || f.collider != null || g.collider != null) && h.collider == null)
                    {
                        temp = Instantiate(GoldOre, new Vector3(x + a, y + b, 0), Quaternion.identity);
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
}
