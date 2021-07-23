using UnityEngine;
using System.Collections.Generic;

public class TurretAI : TurretClass
{
    // Engineer variables
    private float damageBoost = 1;

    // Default stuff
    public int SwitchBarrel = 0;

    // Targetting system
    void Update()
    {
        // If animation is playing, wait
        if (animPlaying)
        {
            PlayAnim();
            return;
        }

        if (isRotating)
            RotationHandler();

        // If a target exists, shoot at it
        if (target != null && !isRotating)
        {            
            // Unflag hasTarget
            hasTarget = false;
            Shoot(Bullet, FirePoints[SwitchBarrel], damageBoost);
        } 
        else
        {
            // Unflag hasTarget when target is null
            hasTarget = false;
        }
    }
}
