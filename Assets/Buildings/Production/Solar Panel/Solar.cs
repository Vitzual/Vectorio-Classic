using UnityEngine;

public class Solar : TileClass
{
    public Survival SRV;
    public int amount;

    public void Start()
    {
        SRV = GameObject.Find("Survival").GetComponent<Survival>();
        BuildingHandler.buildings.Add(transform);
        SRV.increaseAvailablePower(amount);
    }

    // Kill defense
    public override void DestroyTile()
    {
        SRV.decreasePowerConsumption(power);
        SRV.decreaseAvailablePower(amount);
        BuildingHandler.buildings.Remove(transform);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
