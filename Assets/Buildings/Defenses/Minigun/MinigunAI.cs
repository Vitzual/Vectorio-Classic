using UnityEngine;

public class MinigunAI : TurretClass
{
    // Multi barrel
    public int FireTracker = 0;

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
            switch(FireTracker)
            {
                case 0:
                    Shoot(Bullet, FirePoints[0]);
                    FireTracker = 1;
                    break;
                case 1:
                    Shoot(Bullet, FirePoints[1]);
                    FireTracker = 2;
                    break;
                case 2:
                    Shoot(Bullet, FirePoints[2]);
                    FireTracker = 3;
                    break;
                case 3:
                    Shoot(Bullet, FirePoints[3]);
                    FireTracker = 0;
                    break;
            }
        } else {
            // Unflag hasTarget when target is null
            hasTarget = false;
        }
    }

    // Kill defense
    public override void DestroyTile()
    {
        Survival srv = GameObject.Find("Survival").GetComponent<Survival>();
        srv.decreasePowerConsumption(power);
        BuildingHandler.removeBuilding(transform);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
