using UnityEngine;

public class TurretAI : TurretClass
{
    // On start, assign weapon variables
    void Start()
    {
        fireRate = 0.5f;
        bulletForce = 100f;
        bulletSpread = 0.1f;
        bulletAmount = 1;
        rotationSpeed = 0.8f;
        range = 50;
        health = 5;
        maxhp = 5;
    }

    ///// IMPORTANT! ///////////////////////////////////////////////////
    // Because Unity is seriously stupid af, you have to set the cost //
    // of the object here and not in the parent class. You also have  //
    // to change the name of the variable or else Unity dies          //
    ////////////////////////////////////////////////////////////////////

    // Cost & level variables
    private int TurretCost = 10;
    private int TurretLevel = 1;

    // How much the object costs
    public override int GetCost()
    {
        return TurretCost;
    }

    // Default level of the object
    public override int GetLevel()
    {
        return TurretLevel;
    }

    //////////////////////////////////////////////////////////////////

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
