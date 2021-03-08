using UnityEngine;

public class MinigunAI : TurretClass
{
    // Multi barrel
    public Transform FirePoint2;
    public Transform FirePoint3;
    public Transform FirePoint4;
    public int FireTracker = 0;

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
                switch(FireTracker)
                {
                    case 0:
                        Shoot(Bullet, Point);
                        FireTracker = 1;
                        break;
                    case 1:
                        Shoot(Bullet, FirePoint2);
                        FireTracker = 2;
                        break;
                    case 2:
                        Shoot(Bullet, FirePoint3);
                        FireTracker = 3;
                        break;
                    case 3:
                        Shoot(Bullet, FirePoint4);
                        FireTracker = 0;
                        break;
                }
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
