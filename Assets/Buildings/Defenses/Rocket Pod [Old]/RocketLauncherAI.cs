using UnityEngine;

public class RocketLauncherAI : TurretClass
{
    // Targetting system
    void Update()
    {
        //target = FindNearestEnemy();

        if (target != null)
        {            
            hasTarget = false;
            Shoot(Bullet, FirePoints[0]);
            Shoot(Bullet, FirePoints[1]);
            Shoot(Bullet, FirePoints[2]);
            Shoot(Bullet, FirePoints[3]);
        } else {
            hasTarget = false;
        }
    }

    // Kill defense
    public override void DestroyTile()
    {
        GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
