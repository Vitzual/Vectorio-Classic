using UnityEngine;

public class CollectorAI : TileClass
{
    public GameObject gold;
    private GameObject lastSpawn;
    public Transform spawnPos;
    protected Survival player;

    private void Start()
    {
        player = GameObject.Find("Survival").GetComponent<Survival>();
        InvokeRepeating("SpawnMoney", 0f, 2f);
        health = 10;
        maxhp = 10;
        level = 1;
        cost = 25;
    }

    private void SpawnMoney()
    {
        lastSpawn = Instantiate(gold, spawnPos.position, Quaternion.identity);
        lastSpawn.GetComponent<Rigidbody2D>().AddForce(-transform.up * 7f, ForceMode2D.Impulse);
    }

    public override void DestroyTile()
    {
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
