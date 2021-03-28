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
        TurretHandler.buildings.Add(transform);
        SRVSC.goldStorage += amount;
        SRVSC.UI.GoldStorage.text = SRVSC.goldStorage + " MAX";
    }

    // Kill defense
    public override void DestroyTile()
    {
        SRVSC.decreasePowerConsumption(power);
        TurretHandler.buildings.Remove(transform);
        SRVSC.UpdateGoldStorage(amount);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
