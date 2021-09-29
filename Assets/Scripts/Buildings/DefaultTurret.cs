using System.Collections.Generic;
using UnityEngine;

[HideInInspector]
public class DefaultTurret : BaseTile, IAudible
{
    // IAudible interface variables
    public AudioClip sound { get; set; }

    // Barrel thing
    public Turret turret;

    // Base turret object variables
    public Transform[] firePoints;
    public Transform cannon;
    public GameObject bullet;
    [HideInInspector] public BaseEntity target;
    public Queue<BaseEntity> targets = new Queue<BaseEntity>();
    [HideInInspector] public float cooldown;

    public override void Setup()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();

        if (collider != null)
            collider.radius = turret.range;
        else Debug.LogError("Turret does not have a circle collider!");

        material = turret.material;
        cooldown = turret.cooldown;

        health = turret.health;
        maxHealth = health;
    }

    public virtual void RotateTurret()
    {
        // Calculate the rotation towards the enemy
        Vector3 dir = cannon.position - target.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        cannon.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);

        // Fire once cooldown reached
        if (cooldown > 0) cooldown -= Time.deltaTime;
        else
        {
            Shoot();
            cooldown = turret.cooldown;
        }
    }

    // Attempts to fire a bullet and returns true if fired
    public virtual void Shoot()
    {
        foreach (Transform firePoint in firePoints)
            for (int i = 0; i < turret.bulletAmount; i += 1)
                CreateBullet(firePoint.position);
    }

    // Create a bullet object
    public virtual void CreateBullet(Vector2 position)
    {
        //if (turret.sound != null)
        //    AudioSource.PlayClipAtPoint(turret.sound, transform.position);

        GameObject bullet = Instantiate(this.bullet, position, cannon.rotation);
        bullet.transform.rotation = cannon.rotation;
        bullet.transform.Rotate(0f, 0f, Random.Range(-turret.bulletSpread, turret.bulletSpread));

        bullet.GetComponent<TrailRenderer>().material = turret.material;

        float speed = Random.Range(turret.bulletSpeed - 2, turret.bulletSpeed + 2);
        int pierces = turret.bulletPierces + Research.research_pierce;
        float damage = turret.damage + Research.research_damage;

        // Dependent on the bullet, register under the correct master script
        Events.active.BulletFired(new Bullet(bullet.transform, target, speed, pierces, 
            damage, turret.bulletTime, turret.bulletLock, turret.material));
    }

    // IAudible sound method
    public void PlaySound()
    {
        float audioScale = CameraScroll.getZoom() / 1400f;
        AudioSource.PlayClipAtPoint(sound, gameObject.transform.position, Settings.soundVolume - audioScale);
    }

    public void AddTarget(BaseEntity enemy)
    {
        if (!targets.Contains(enemy))
        {
            targets.Enqueue(enemy);
            if (target == null && GetNewTarget())
                Events.active.RegisterTurret(this);
        }
    }

    public bool GetNewTarget()
    {
        if (targets.Count > 0)
        {
            target = targets.Dequeue();
            return true;
        }
        else return false;
    }
}
