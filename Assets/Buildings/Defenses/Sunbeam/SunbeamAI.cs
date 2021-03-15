using UnityEngine;

public class SunbeamAI : TurretClass
{
    // Rotation variables
    public Transform rotator1;
    public Transform rotator2;

    // Targetting system
    void Update()
    {
        // Handles rotation
        rotator1.Rotate(Vector3.forward, 50f * Time.deltaTime);
        rotator2.Rotate(-Vector3.forward, 50f * Time.deltaTime);

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
