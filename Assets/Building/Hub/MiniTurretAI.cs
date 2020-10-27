using UnityEngine;

public class MiniTurretAI : TurretClass
{
    protected int level = 1;

    // On start, assign weapon variables
    void Start()
    {
        fireRate = 0.05f;
        bulletForce = 150f;
        bulletSpread = 0.1f;
        bulletAmount = 1;
        rotationSpeed = .95f;
        range = 60;
        health = 5;
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

    public override int GetCost() { return 0; }

    public override int GetLevel()
    {
        return level;
    }
}
