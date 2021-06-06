using UnityEngine;
using UnityEngine.SceneManagement;

public class Enhancer : TileClass
{
    // Internal placement variables
    [SerializeField] private LayerMask TileLayer;
    public int IncreaseAmount;
    public Collider2D[] colliders;

    // Internal placement variables
    public Transform rotator;
    public float speed;

    private void Start()
    {
        try
        {
            GameObject.Find("Rotation Handler").GetComponent<RotationHandler>().registerRotator(rotator, speed);
        }
        catch { }
        BuildingHandler.buildings.Add(transform);
        if (SceneManager.GetActiveScene().name != "Menu")
            InvokeRepeating("CheckAdjacentTiles", 0f, 1f);
    }


    // Check for collectors
    private void CheckAdjacentTiles()
    {
        var colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(7, 7), 1 << LayerMask.NameToLayer("Building"));
        for (int i = 0; i < colliders.Length; i++)
            if (colliders[i].name.Contains("Collector"))
                colliders[i].GetComponent<CollectorAI>().enhanceCollector();
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
            else if (colliders[i].name.Contains("Essence Drill"))
            {
                colliders[i].GetComponent<CollectorAI>().deenhanceCollector();
            }
            else if (colliders[i].name.Contains("Iridium Mine"))
            {
                colliders[i].GetComponent<CollectorAI>().deenhanceCollector();
            }
        }

        Survival srv = GameObject.Find("Survival").GetComponent<Survival>();
        srv.decreasePowerConsumption(power);
        BuildingHandler.buildings.Remove(transform);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
