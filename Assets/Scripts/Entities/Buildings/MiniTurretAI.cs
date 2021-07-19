using UnityEngine;

public class MiniTurretAI : TurretClass
{
    // Targetting system
    void Update()
    {
        if (isRotating && target != null)
            RotationHandler();

        // If a target exists, shoot at it
        if (target != null)
        {            
            // If turret is pointing at target, fire at it
            if ((gunRotation - enemyAngle) <= 1 && (gunRotation - enemyAngle) >= -1)
            {
                // Unflag hasTarget
                hasTarget = false;
                
                // Call shoot function
                Shoot(Bullet, FirePoints[0]);
            }
        } else {
            // Unflag hasTarget when target is null
            hasTarget = false;
        }
    }
}
