using UnityEngine;

public class TurretAI : MonoBehaviour
{
    // Turret AI variables 
    [SerializeField]
    private Transform FirePoint;
    [SerializeField]
    private Rigidbody2D TurretGun;
    private Vector2 TargetPosition;

    // Default weapon variables
    protected float FireRate = 0.5f;
    protected float NextFire = -1f;
    protected float BulletForce = 50f;
    protected float TotalRange = 1f;
    protected int Lifetime = 1;
    protected int Range = 1500;
    protected int Offset = 0;

    // Base weapon objects
    public GameObject BulletPrefab;
    public ParticleSystem DecayEffect;

    // On start, grab RB2D component
    void Start()
    {
        Physics2D.IgnoreCollision(BulletPrefab.transform.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
    }

    void Update()
    {
        // Find closest enemy 
        var target = EnemyPool.FindClosestEnemy(transform.position, Range);

        // If a target exists, shoot at it
        if (target != null)
        {
            // Rotate turret towards target
            TargetPosition = new Vector2(target.gameObject.transform.position.x, target.gameObject.transform.position.y);
            Vector2 lookDirection = (TargetPosition - TurretGun.position);
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;

            // Smooth rotation when targetting enemies
            if (TurretGun.rotation >= angle && !((TurretGun.rotation - angle) <= 0.3 && (TurretGun.rotation - angle) >= -0.3))
            {
                TurretGun.rotation -= 0.3f;
            } else if (TurretGun.rotation <= angle && !((TurretGun.rotation - angle) <= 0.3 && (TurretGun.rotation - angle) >= -0.3))
            {
                TurretGun.rotation += 0.3f;
            }
            
            if ((TurretGun.rotation - angle) <= 5 && (TurretGun.rotation - angle) >= -5)
            {
                // Shoot bullet
                if (NextFire > 0)
                {
                    NextFire -= Time.deltaTime;
                    return;
                }

                // Call shoot function
                Shoot();
            }
        }

        // Update cooldown
        NextFire = FireRate;
    }

    // Creates bullet object
    void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
        bullet.transform.Rotate(0, 0, Offset);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(BulletSpread(FirePoint.up, Random.Range(-0.1f, 0.1f)) * BulletForce, ForceMode2D.Impulse);
    }

    // Calculate bullet spread
    private static Vector2 BulletSpread(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
}
