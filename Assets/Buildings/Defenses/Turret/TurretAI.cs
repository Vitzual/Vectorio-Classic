using UnityEngine;

public class TurretAI : TurretClass
{
    // Engineer variables
    private float damageBoost = 1;

    // Engineer transforms
    public GameObject[] normalObjects;
    public GameObject[] engineerObjects;

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
                if (engineerModifications.Contains(1))
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

    // Applies a modification specific to this building
    // 1 = Dual barrel
    // 2 = Quad barrel
    // 3 = Increased Damage
    // 4 = Increased HP
    // 5 = Decreased power + heat + hp
    public override void ApplyModification(int modID)
    {
        // Check to see if modification has already been applied
        if (engineerModifications.Count == 0)
        { 
            switch (modID)
            {
                // Dual barrel
                case 1:

                    // Enable / disable child objects
                    normalObjects[0].SetActive(false);
                    engineerObjects[0].SetActive(true);
                    engineerObjects[1].SetActive(false);

                    // Apply variable modifications
                    Gun = engineerObjects[0].transform.GetChild(0).GetComponent<Rigidbody2D>();
                    if (fireRate - 0.2f == 0)
                        fireRate = 0.1f;
                    else
                        fireRate -= 0.2f;

                    break;

                default:
                    return;
            }

            engineerModifications.Add(modID);
        }
    }

    //

    // Kill defense
    public override void DestroyTile()
    {
        GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
