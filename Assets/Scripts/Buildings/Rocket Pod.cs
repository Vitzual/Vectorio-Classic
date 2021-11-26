using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPod : DefaultTurret
{
    public int podIndex = 0;

    public override void Setup()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();

        if (collider != null)
            collider.radius = turret.range;
        else Debug.LogError("Turret does not have a circle collider!");

        GetComponent<BaseEntity>().Setup();
    }

    public override void RotateTurret()
    {
        // Fire once cooldown reached
        if (cooldown > 0) cooldown -= Time.deltaTime;
        else
        {
            Shoot();

            if (turret.randomizeCooldown)
                cooldown = Random.Range(turret.cooldown, turret.cooldown + turret.cooldown);
            else cooldown = turret.cooldown / Research.firerateBoost;
        }
    }

    public override void Shoot()
    {
        CreateBullet(firePoints[podIndex].position);

        podIndex += 1;
        if (podIndex >= firePoints.Length)
            podIndex = 0;
    }
}
