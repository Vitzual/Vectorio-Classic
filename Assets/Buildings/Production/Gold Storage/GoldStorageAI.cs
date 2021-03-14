using UnityEngine;

public class GoldStorageAI: TileClass
{
    // Declare local object variables
    public int amount;
    private Survival SRVSC;

    // On start, invoke repeating SendGold() method
    private void Start()
    {
        SRVSC = GameObject.Find("Survival").GetComponent<Survival>();
    }

    // Kill defense
    public override void DestroyTile()
    {
        SRVSC.decreasePowerConsumption(power);
        SRVSC.goldStorage -= amount;
        if (SRVSC.gold > SRVSC.goldStorage)
            SRVSC.gold = SRVSC.goldStorage;
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
