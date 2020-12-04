using UnityEngine;

public class Enhancer : TileClass
{
    // Internal placement variables
    [SerializeField] private LayerMask TileLayer;
    public Transform rotator;
    public int IncreaseAmount;
    public Collider2D[] colliders;

    private void Start()
    {
        InvokeRepeating("CheckAdjacentTiles", 0f, 1f);
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
            if (colliders[i].name == "Collector")
            {
                colliders[i].GetComponent<CollectorAI>().enhanceCollector();
            }
            else if (colliders[i].name == "Essence Drill")
            {
                colliders[i].GetComponent<EssenceAI>().enhanceCollector();
            }
            else if (colliders[i].name == "Iridium Mine")
            {
                colliders[i].GetComponent<IridiumAI>().enhanceCollector();
            }
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
                colliders[i].GetComponent<CollectorAI>().deenhanceCollector();
            }
        }

        GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
