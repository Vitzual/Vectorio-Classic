using UnityEngine;

public class ChillerAI : TurretClass
{
    // Targetting system
    void Update()
    {
        if (isRotating)
            RotateTowardNearestEnemy();

        // If a target exists, shoot at it
        if (target != null && !isRotating)
        {
            // If turret is pointing at target, fire at it
            if ((gunRotation - enemyAngle) <= 1 && (gunRotation - enemyAngle) >= -1)
            {
                // Unflag hasTarget
                hasTarget = false;

                // Call shoot function
                Shoot(Bullet, FirePoints[0]);
            }
        }
        else
        {
            // Unflag hasTarget when target is null
            hasTarget = false;
        }
    }

    // Kill defense
    public override void DestroyTile()
    {
        GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
