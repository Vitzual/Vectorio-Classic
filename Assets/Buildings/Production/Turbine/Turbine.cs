using UnityEngine;

public class Turbine : TileClass
{
    public Transform rotator;
    public Survival SRV;
    public int amount;

    public void Start()
    {
        SRV = GameObject.Find("Survival").GetComponent<Survival>();
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
        SRV.decreaseAvailablePower(amount);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.Euler(90f, 0f, 0f));
        Destroy(gameObject);
    }
}
