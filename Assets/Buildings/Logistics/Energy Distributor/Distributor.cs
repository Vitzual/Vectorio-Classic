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
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].name != "Energizer")
            {
                try
                {
                    colliders[i].GetComponent<TileClass>().DestroyTile();
                }
                catch
                {
                    continue;
                }
            }
        }
        GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
