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

        if (isRotating)
            RotateTowardNearestEnemy();

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

    // Kill defense
    public override void DestroyTile()
    {
        Survival srv = GameObject.Find("Survival").GetComponent<Survival>();
        srv.decreasePowerConsumption(power);
        srv.buildings.Remove(transform);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
