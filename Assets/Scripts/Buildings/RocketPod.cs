using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPod : DefaultTurret
{
    public int podIndex = 0;

    public override void Setup()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();

        if (collider != null) collider.radius = turret.range;
        else Debug.LogError("Turret does not have a circle collider!");

        SetupBase();
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

    // Create a bullet object
    public override void CreateBullet(Vector2 position)
    {
        if (turret.sound != null)
            AudioSource.PlayClipAtPoint(turret.sound, transform.position, 0.5f);

        GameObject holder = Instantiate(turret.bullet.gameObject, position, firePoints[podIndex].rotation);
        holder.transform.rotation = firePoints[podIndex].rotation;
        holder.transform.Rotate(0f, 0f, Random.Range(-turret.bulletSpread, turret.bulletSpread));

        // Set bullet variables
        DefaultBullet bullet = holder.GetComponent<DefaultBullet>();

        // Setup bullet
        bullet.Setup(turret);
        if (cosmetic == null || !cosmetic.useBullet)
        {
            if (turret.useBulletSprite)
                bullet.SetupModel(bulletModel);
        }
        else
        {
            bullet.SetupModel(cosmetic.bullet);
        }

        // Dependent on the bullet, register under the correct master script
        if (turret.bulletLock)
        {
            bullet.target = target;
            bullet.tracking = true;
            Events.active.BulletFired(bullet);
        }
        else Events.active.BulletFired(bullet);
    }
}
