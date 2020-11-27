using UnityEngine;

public class RocketLauncherAI : TurretClass
{
    // Rocket launcher thing
    public Transform Point1;
    public Transform Point2;
    public Transform Point3;

    // Targetting system
    void Update()
    {
        target = FindNearestEnemy();

        if (target != null)
        {            
            hasTarget = false;
            Shoot(Bullet, Point);
            Shoot(Bullet, Point1);
            Shoot(Bullet, Point2);
            Shoot(Bullet, Point3);
        } else {
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
