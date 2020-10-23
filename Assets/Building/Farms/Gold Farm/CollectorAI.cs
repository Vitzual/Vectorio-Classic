using UnityEngine;

public class CollectorAI : TileClass
{
    public Transform rotator;
    protected Survival player;

    private void Start()
    {
        player = GameObject.Find("Survival").GetComponent<Survival>();
        InvokeRepeating("GiveMoney", 0f, 2f);
        health = 10;
        maxhp = 10;
        level = 1;
        cost = 25;
    }

    void Update()
    {
        rotator.Rotate(0, 0, 50 * Time.deltaTime);
    }

    private void GiveMoney()
    {
        player.AddGold(1 * level);
    }

    public override void DestroyTile()
    {
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
