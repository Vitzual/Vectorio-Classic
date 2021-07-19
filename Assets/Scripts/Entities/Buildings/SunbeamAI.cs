using UnityEngine;

public class SunbeamAI : TurretClass
{
    // Rotation variables
    public Transform rotator1;
    public Transform rotator2;

    // Enemy handler thing
    private EnemyHandler enemies;

    // Targetting system
    void Update()
    {
        // If animation is playing, wait
        if (animPlaying)
        {
            PlayAnim();
            return;
        }

        /* Sunbeams contain custom rotation hanlding as they require a large range input
        if (isRotating)
        {
            if (scanThisFrame || target != null)
            {
                scanThisFrame = false;
                if (!hasTarget)
                {
                    if (enemies == null) enemies = GameObject.Find("Enemy Handler").GetComponent<EnemyHandler>();
                    else enemies.findClosest(transform.position);
                }
                if (target != null)
                    RotationHandler();
            }
            else scanThisFrame = true;
        }*/

        // If a target exists, shoot at it
        if (target != null && !isRotating)
        {
            // Unflag hasTarget
            hasTarget = false;

            // Call shoot function
            Shoot(Bullet, FirePoints[0]);
        }
        else
        {
            // Unflag hasTarget when target is null
            hasTarget = false;
        }
    }
}
