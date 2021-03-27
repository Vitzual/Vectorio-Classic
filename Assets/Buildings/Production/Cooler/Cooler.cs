using UnityEngine;

public class Cooler : TileClass
{
    // Internal placement variables
    public Transform rotator;
    public float speed;

    // Update is called once per frame
    void Start()
    {
        GameObject.Find("Survival").GetComponent<Survival>().buildings.Add(transform);
        GameObject.Find("Rotation Handler").GetComponent<RotationHandler>().registerRotator(rotator, speed);
    }

    // Kill defense
    public override void DestroyTile()
    {
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Survival srv = GameObject.Find("Survival").GetComponent<Survival>();
        srv.decreasePowerConsumption(power);
        srv.buildings.Remove(transform);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
