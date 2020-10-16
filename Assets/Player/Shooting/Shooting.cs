using System.Globalization;
using UnityEngine;

public class Shooting : Player
{

    // Build system object
    [SerializeField]
    private GameObject BuildSystem;
    private GameObject BuildObject;
    private Vector2 mousePosition;

    // Base player variables
    public Transform firePoint;
    public GameObject bulletPrefab;
    public ParticleSystem hitEffect;

    // Default weapon variables
    protected float thrust = 1.0f;
    protected float fireRate = 0.1f;
    protected float nextFire = -1f;
    protected float bulletForce = 20f;
    protected int offset = 0;

    void Start()
    {
        
    }

    // Firing system
    void Update()
    {
        if (Input.GetButton("Fire1") && BuildMode == false)
        {
            if (nextFire > 0)
            {
                nextFire -= Time.deltaTime;
                return;
            }
            Shoot();
            Cooldown();
            Recoil();
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.F) && BuildMode == false)
        {
            // Get mouse position and round to middle grid coordinate
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            BuildObject = Instantiate(BuildSystem, mousePosition, Quaternion.identity);
            BuildMode = true;
        }
        else if (Input.GetKeyDown(KeyCode.F) && BuildMode == true)
        {
            // On build mode disable, destroy object
            if (BuildObject.activeSelf)
            {
                Destroy(BuildObject);
                BuildMode = false;
            }
        }
    }

    // Creates bullet object
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.Rotate(0, 0, offset);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(rotate(firePoint.up, Random.Range(-0.1f, 0.1f)) * bulletForce, ForceMode2D.Impulse);
        Destroy(bullet, 1f);
    }

    // Add recoil to player
    void Recoil()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.Normalize();
        player.AddForce((player.position - mousePosition) * 3f);
    }

    // Bullet cooldown when holding
    void Cooldown()
    {
        nextFire = fireRate;
    }

    // Rotation function
    public static Vector2 rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
}

