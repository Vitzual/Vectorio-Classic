using UnityEngine;

public class EnemyTurretAI : TurretClass
{
    public bool SwitchBarrel = false;
    public Transform Point2;

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
                    Shoot(Bullet, Point2);
                }
                else 
                { 
                    SwitchBarrel = false;
                    Shoot(Bullet, Point);
                }
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
