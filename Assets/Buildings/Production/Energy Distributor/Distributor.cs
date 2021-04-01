using UnityEngine;

public class Distributor : TileClass
{
    // Internal placement variables
    [SerializeField] private LayerMask TileLayer;
    public Collider2D[] colliders;

    // Internal placement variables
    public Transform rotator;
    public float speed;

    // Update is called once per frame
    void Start()
    {
        BuildingHandler.buildings.Add(transform);
        GameObject.Find("Rotation Handler").GetComponent<RotationHandler>().registerRotator(rotator, speed);
    }

    // Kill defense
    public override void DestroyTile()
    {
        var colliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y), new Vector2(50, 50), 0, 1 << LayerMask.NameToLayer("Building"));
        transform.Find("AOCB").gameObject.SetActive(false);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].name != "Energizer" && colliders[i].name != "Hub")
            {
                try
                {
                    colliders[i].GetComponent<TileClass>().UpdatePower();
                }
                catch
                {
                    continue;
                }
            }
        }
        Survival srv = GameObject.Find("Survival").GetComponent<Survival>();
        srv.decreasePowerConsumption(power);
        BuildingHandler.buildings.Remove(transform);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, transform.rotation * Quaternion.Euler(90f, 0f, 0f));
        Destroy(gameObject);
    }
}
