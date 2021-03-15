using UnityEngine;

public class Solar : TileClass
{
    public Survival SRV;

    public void Start()
    {
        SRV = GameObject.Find("Survival").GetComponent<Survival>();
        SRV.increaseAvailablePower(500);
    }

    // Kill defense
    public override void DestroyTile()
    {
        SRV.decreaseAvailablePower(500);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
