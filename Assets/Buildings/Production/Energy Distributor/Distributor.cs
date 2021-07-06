using UnityEngine;

public class Distributor : TileClass
{
    // Internal placement variables
    [SerializeField] private LayerMask TileLayer;
    public Collider2D[] colliders;

    // Internal placement variables
    public Transform AOCB;
    public Transform rotator;
    public float speed;

    // Update is called once per frame
    void Start()
    {
        GameObject.Find("Rotation Handler").GetComponent<RotationHandler>().registerRotator(rotator, speed);
    }

    public override void UpdateEnergizer()
    {
        // Check if there is still an AOCB tile under each building
        var colliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y), new Vector2(50, 50), 0, 1 << LayerMask.NameToLayer("Building"));
        for (int i = 0; i < colliders.Length; i++)
            if (colliders[i].name != "Energizer" && colliders[i].name != "Hub")
                colliders[i].GetComponent<TileClass>().UpdatePower(transform.GetChild(0));

        // Destroy ghost buildings in the area
        DroneManager droneManager = GameObject.Find("Drone Handler").GetComponent<DroneManager>();
        colliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y), new Vector2(50, 50), 0, 1 << LayerMask.NameToLayer("Ghost"));
        bool holder;
        for (int i = 0; i < colliders.Length; i++)
        {
            try
            {
                holder = droneManager.dequeueBuilding(colliders[i].transform);
                if (!holder) Destroy(colliders[i].transform.gameObject);
            }
            catch
            {
                continue;
            }
        }

        transform.GetChild(0).gameObject.SetActive(false);
    }
}
