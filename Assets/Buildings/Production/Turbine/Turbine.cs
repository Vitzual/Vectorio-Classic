using UnityEngine;

public class Turbine : TileClass
{
    public Transform rotator;
    public Survival SRV;

    public void Start()
    {
        SRV = GameObject.Find("Survival").GetComponent<Survival>();
        SRV.increaseAvailablePower(100);
    }

    // Update is called once per frame
    void Update()
    {
        rotator.Rotate(Vector3.forward, 150f * Time.deltaTime);
    }

    // Kill defense
    public override void DestroyTile()
    {
        SRV.decreaseAvailablePower(100);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
