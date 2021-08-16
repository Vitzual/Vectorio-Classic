using UnityEngine;
using UnityEngine.SceneManagement;

public class Enhancer : DefaultBuilding
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

    /*
    // Kill defense
    public override void UpdateEnhancer()
    {
        var colliders = Physics2D.OverlapBoxAll(this.gameObject.transform.position, new Vector2(7, 7), 1 << LayerMask.NameToLayer("Building"));
        for (int i = 0; i < colliders.Length; i++)
        {
            try
            {
                if (colliders[i].name.Contains("Collector"))
                    colliders[i].GetComponent<CollectorAI>().deenhanceCollector();
            }
            catch { continue; }
        }

        Survival srv = GameObject.Find("Survival").GetComponent<Survival>();
        srv.decreasePowerConsumption(power);
        BuildingHandler.removeBuilding(transform);
        GameObject.Find("Spawner").GetComponent<Spawner>().decreaseHeat(heat);
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    */
}
