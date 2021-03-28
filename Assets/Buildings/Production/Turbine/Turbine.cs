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
        GameObject.Find("Rotation Handler").GetComponent<RotationHandler>().registerRotator(rotator, speed);
        SRV = GameObject.Find("Survival").GetComponent<Survival>();
        TurretHandler.buildings.Add(transform);
        SRV.increaseAvailablePower(amount);
    }

    // Update is called once per frame
    void Update()
    {
        rotator.Rotate(Vector3.forward, 150f * Time.deltaTime);
    }

    // Kill defense
    public override void DestroyTile()
    {
        SRV.decreasePowerConsumption(power);
        SRV.decreaseAvailablePower(amount);
        TurretHandler.buildings.Remove(transform);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.Euler(90f, 0f, 0f));
        Destroy(gameObject);
    }
}
