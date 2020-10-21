using UnityEngine;

public class SMGAI : TurretClass
{
    // On start, assign weapon variables
    void Start()
    {
        fireRate = .05f;
        bulletForce = 125f;
        bulletSpread = 0.2f;
        bulletAmount = 1;
        rotationSpeed = 1.5f;
        range = 650;
        health = 10;
        maxhp = 10;
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
}
