using System.Collections.Generic;
using UnityEngine;

[HideInInspector]
public class DefaultTurret : DefaultBuilding, IAudible
{
    // IAudible interface variables
    public AudioClip sound { get; set; }

    // Barrel thing
    public Turret turret;

    // Base turret object variables
    public Transform[] firePoints;
    public Transform barrel;
    public GameObject bullet;
    [HideInInspector] public DefaultEnemy target;
    public Queue<DefaultEnemy> targets = new Queue<DefaultEnemy>();
    [HideInInspector] public float cooldown;

    public override void Setup()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();

        if (collider != null)
            collider.radius = turret.range;
        else Debug.LogError("Turret does not have a circle collider!");

        cooldown = turret.cooldown;
    }

    public virtual void RotateTurret()
    {
        // Calculate the rotation towards the enemy
        Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
        Vector2 distance = targetPosition - new Vector2(transform.position.x, transform.position.y);
        float targetAngle = Mathf.Atan(distance.y / distance.x) * Mathf.Rad2Deg + 90f;

        // Face the enemy
        barrel.transform.eulerAngles = new Vector3(0, 0, targetAngle);
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
        if (turret.sound != null)
            AudioSource.PlayClipAtPoint(turret.sound, transform.position);

        GameObject bullet = Instantiate(this.bullet, position, barrel.rotation);
        bullet.transform.rotation = barrel.rotation;
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

    public void AddTarget(DefaultEnemy enemy)
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
