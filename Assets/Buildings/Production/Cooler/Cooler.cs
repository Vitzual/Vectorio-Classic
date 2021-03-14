using UnityEngine;

public class Cooler : TileClass
{
    // Internal placement variables
    [SerializeField] private LayerMask TileLayer;
    public Transform rotator;
    public Collider2D[] colliders;

    private void Start()
    {
        CheckAdjacentTiles();
    }

    // Update is called once per frame
    void Update()
    {
        rotator.Rotate(Vector3.forward, 50f * Time.deltaTime);
    }

    // Check for collectors
    private void CheckAdjacentTiles()
    {
        var colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(7, 7), 1 << LayerMask.NameToLayer("Building"));
        for (int i = 0; i < colliders.Length; i++)
        {
            try
            {
                if (colliders[i].name != "Hub" && colliders[i].name != "Area Cooler" && colliders[i].name != "AOCB")
                {
                    colliders[i].GetComponent<TileClass>().DecreaseHeat(25);
                }
            } catch { Debug.Log("Ignoring cooling for " + colliders[i].transform.name); }
        }
    }

    // Kill defense
    public override void DestroyTile()
    {
        var colliders = Physics2D.OverlapBoxAll(this.gameObject.transform.position, new Vector2(7, 7), 1 << LayerMask.NameToLayer("Building"));
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].name.Contains("Collector"))
            {
                if (colliders[i].name != "Hub")
                    colliders[i].GetComponent<CollectorAI>().IncreaseHeat();
            }
        }

        GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
