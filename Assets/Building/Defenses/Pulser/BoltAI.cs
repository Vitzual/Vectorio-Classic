using UnityEngine;

public class BoltAI : TurretClass
{
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
        GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
