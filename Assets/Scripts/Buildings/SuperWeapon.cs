using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperWeapon : DefaultTurret
{
    public void Update()
    {
        if (target == null)
        {
            if (GuardianHandler.active.guardians.Count > 0)
                target = GuardianHandler.active.guardians[0];
            else if (EnemyHandler.active.enemies.Count > 0)
                target = EnemyHandler.active.GetStrongestEnemy();
        }
        else RotateTurret();
    }

    public override void Setup()
    {
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

            if (turret.randomizeCooldown)
                cooldown = Random.Range(turret.cooldown, turret.cooldown + turret.cooldown);
            else cooldown = turret.cooldown / Research.firerateBoost;
        }
    }
}
