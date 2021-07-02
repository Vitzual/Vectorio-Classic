using UnityEngine;

public class Turbine : TileClass
{
    public Survival SRV;
    public int amount;

    // Internal placement variables
    public Transform rotator;
    public float speed;

    private void Start()
    {
        GameObject.Find("Rotation Handler").GetComponent<RotationHandler>().registerRotator(rotator, 150f);
        SRV = GameObject.Find("Survival").GetComponent<Survival>();
        SRV.increaseAvailablePower(amount);
    }

    // Kill defense
    public override void DestroyTile()
    {
        SRV.decreasePowerConsumption(power);
        SRV.decreaseAvailablePower(amount);
        BuildingHandler.removeBuilding(transform);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.Euler(90f, 0f, 0f));
        Destroy(gameObject);
    }
}
