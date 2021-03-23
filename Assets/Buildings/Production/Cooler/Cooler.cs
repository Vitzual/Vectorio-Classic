using UnityEngine;

public class Cooler : TileClass
{
    // Internal placement variables
    public Transform rotator;
    public int amount = 25;

    private void Start()
    {
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(amount);
    }

    // Update is called once per frame
    void Update()
    {
        rotator.Rotate(Vector3.forward, 50f * Time.deltaTime);
    }

    // Kill defense
    public override void DestroyTile()
    {
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().increaseHeat(amount);
        GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
