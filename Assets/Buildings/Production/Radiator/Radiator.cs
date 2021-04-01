using UnityEngine;

public class Radiator : TileClass
{
    public void Start()
    {
        BuildingHandler.buildings.Add(transform);
    }

    // Kill defense
    public override void DestroyTile()
    {
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Survival srv = GameObject.Find("Survival").GetComponent<Survival>();
        srv.decreasePowerConsumption(power);
        BuildingHandler.buildings.Remove(transform);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
