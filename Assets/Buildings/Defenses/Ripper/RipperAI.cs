using UnityEngine;

public class RipperAI : TurretClass
{
    public Transform rotator;

    // Update is called once per frame
    void Start()
    {
        GameObject.Find("Rotation Handler").GetComponent<RotationHandler>().registerRotator(rotator, 50f);
    }

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
                
            // Call shoot function
            Shoot(Bullet, FirePoints[0]);
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
        BuildingHandler.buildings.Remove(transform);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
