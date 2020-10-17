using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPodAI : EntityClass
{
    // Turret AI variables
    [SerializeField]
    private Transform FirePoint;
    private Rigidbody2D turret;
    private Vector2 TargetPosition;

    // Default weapon variables
    protected float thrust = 1.0f;
    protected float fireRate = 0.5f;
    protected float nextFire = -1f;
    protected float bulletForce = 20f;
    protected int offset = 0;

    // Base weapon objects
    public GameObject BulletPrefab;

    // On start, grab RB2D component
    void Start()
    {
        turret = this.GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(BulletPrefab.transform.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
    }

    void Update()
    {
        // Find closest enemy 
        var target = EnemyPool.FindClosestEnemy(transform.position);

        // If a target exists, shoot at it
        if (target != null)
        {
            // Rotate turret towards target
            TargetPosition = new Vector2(target.gameObject.transform.position.x, target.gameObject.transform.position.y);
            Vector2 lookDirection = TargetPosition - turret.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            turret.rotation = angle;

            // Shoot bullet
            if (nextFire > 0)
            {
                nextFire -= Time.deltaTime;
                return;
            }

            // Call shoot function
            Shoot();
        }

        // Update cooldown
        nextFire = fireRate;
    }

    // Creates bullet object
    void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
        bullet.transform.Rotate(0, 0, offset);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(BulletSpread(FirePoint.up, Random.Range(-0.1f, 0.1f)) * bulletForce, ForceMode2D.Impulse);
        Destroy(bullet, 1f);
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
