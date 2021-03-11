using UnityEngine;

public class TurretAI : TurretClass
{
    // Engineer stuff
    public float damageBoost = 1;
    public bool firerateBoost;

    // Default stuff
    public bool SwitchBarrel = false;

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
                if (firerateBoost)
                { 
                    if (!SwitchBarrel) 
                    { 
                        SwitchBarrel = true; 
                        Shoot(Bullet, FirePoints[2], damageBoost); 
                    }
                    else 
                    { 
                        SwitchBarrel = false; 
                        Shoot(Bullet, FirePoints[1], damageBoost); 
                    }
                } 
                else Shoot(Bullet, FirePoints[0], damageBoost);
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
