using UnityEngine;

public class SniperAI : TurretClass
{
    public int level = 1;
    public int cost = 10;

    // On start, assign weapon variables
    void Start()
    {
        fireRate = 3f;
        bulletForce = 180f;
        bulletSpread = 0f;
        bulletAmount = 1;
        rotationSpeed = 0.3f;
        range = 50;
        health = 3;
        maxhp = 3;
        cost = 8;
    }

    // Targetting system
    void Update()
    {
        RotateTowardNearestEnemy();

        // If a target exists, shoot at it
        if (target != null)
        {            
            // If turret is pointing at target, fire at it
            if ((gunRotation - enemyAngle) <= 1 && (gunRotation - enemyAngle) >= -1)
            {
                // Unflag hasTarget
                hasTarget = false;
                
                // Call shoot function
                Shoot(Bullet, Point);
            }
        } else {
            // Unflag hasTarget when target is null
            hasTarget = false;
        }
    }

    // Kill defense
    public override void DestroyTile()
    {
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public override int GetCost()
    {
        return cost;
    }

    public override int GetLevel()
    {
        return level;
    }
}
