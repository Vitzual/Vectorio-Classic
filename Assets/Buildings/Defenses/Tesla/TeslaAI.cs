using UnityEngine;

public class TeslaAI : TurretClass
{
    // Targetting system
    void Update()
    {
        if (isRotating)
            RotationHandler();

        // If a target exists, shoot at it
        if (target != null && !isRotating)
        {
            // Unflag hasTarget
            hasTarget = false;

            // Call shoot function
            Shoot(Bullet, FirePoints[0]);
        } else {
            // Unflag hasTarget when target is null
            hasTarget = false;
        }
    }
}
