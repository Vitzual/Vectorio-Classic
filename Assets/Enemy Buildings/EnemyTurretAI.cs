using UnityEngine;

public class EnemyTurretAI : TurretClass
{
    public bool SwitchBarrel = false;

    // Targetting system
    void Update()
    {
        RotateTowardsPlayer();

        // If a target exists, shoot at it
        if (target != null)
        {            
            // If turret is pointing at target, fire at it
            if ((gunRotation - enemyAngle) <= 1 && (gunRotation - enemyAngle) >= -1)
            {
                // Unflag hasTarget
                hasTarget = false;
                if (!SwitchBarrel) 
                { 
                    SwitchBarrel = true;
                    Shoot(Bullet, FirePoints[1]);
                }
                else 
                { 
                    SwitchBarrel = false;
                    Shoot(Bullet, FirePoints[0]);
                }
                Shoot(Bullet, FirePoints[0]);
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
