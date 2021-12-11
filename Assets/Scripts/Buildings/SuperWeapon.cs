using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperWeapon : DefaultTurret
{
    public enum Type
    {
        Trident
    }
    public Type type;

    public static SuperWeapon trident;

    public void Update()
    {
        if (target != null)
            RotateTurret();
    }

    public override void Setup()
    {
        trident = this;

        if (turret.bulletSpriteName != "")
        {
            bulletModel = Sprites.GetSprite(turret.bulletSpriteName);
            useBulletModel = bulletModel != null;
        }

        health = turret.health;
        maxHealth = health;
    }

    public override void RotateTurret()
    {
        // Fire once cooldown reached
        if (cooldown > 0) cooldown -= Time.deltaTime;
        else
        {
            // Calculate the rotation towards the enemy
            Vector3 dir = cannon.position - target.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            cannon.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);

            Shoot();

            cooldown = turret.cooldown;
        }
    }

    // Attempts to fire a bullet and returns true if fired
    public override void Shoot()
    {
        foreach (Transform firePoint in firePoints)
            for (int i = 0; i < turret.bulletAmount; i += 1)
                CreateBullet(firePoint.position);
    }
}
