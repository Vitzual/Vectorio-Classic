using UnityEngine;

public class Distributor : TileClass
{
    // Internal placement variables
    [SerializeField] private LayerMask TileLayer;
    public Transform rotator;
    public Collider2D[] colliders;

    // Update is called once per frame
    void Update()
    {
        rotator.Rotate(Vector3.forward, 50f * Time.deltaTime);
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
        GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, transform.rotation * Quaternion.Euler(90f, 0f, 0f));
        Destroy(gameObject);
    }
}
