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
    }

    public virtual void RotateTurret()
    {
        // Get target position relative to this entity
        Vector2 targetPosition = new Vector2(target.transform.transform.position.x, target.transform.transform.position.y);

        // Get the distance from the turret to the target
        Vector2 distance = targetPosition - new Vector2(barrel.position.x, barrel.position.y);

        // Get the angle between the gun position and the target position
        float targetAngle = Mathf.Atan(distance.y / distance.x) * Mathf.Rad2Deg + 90f;
        if (distance.x > 0) targetAngle += 180;

        // Correct for if target is directly above or below the turret
        if (distance.x == 0)
        {
            if (distance.y > 0) targetAngle = 0;
            else targetAngle = 180;
        }

        // Calculate the difference between the target angle and the current angle
        float difference = targetAngle - (barrel.rotation.eulerAngles.z);

        if ((difference < 0 || difference >= 180) && !(difference < -180))
        {
            // Calculate how far to rotate the turret given how long since the last frame
            float distanceToRotate = -turret.rotationSpeed * Time.deltaTime;

            // If distance to rotate would rotate past the target only rotate the distance
            if (distanceToRotate < difference)
                distanceToRotate = difference;

            // Rotate the turret
            barrel.Rotate(Vector3.forward, distanceToRotate);
        }
        else if (!(difference <= 5 && difference >= -5))
        {
            // Calculate how far to rotate the turret given how long since the last frame
            float distanceToRotate = -turret.rotationSpeed * Time.deltaTime;

            // If distance to rotate would rotate past the target only rotate the distance
            if (distanceToRotate > difference)
                distanceToRotate = difference;

            // Rotate the turret
            barrel.Rotate(Vector3.forward, distanceToRotate);
        }
        else
        {
            barrel.transform.eulerAngles = new Vector3(0, 0, targetAngle);
            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
            }
            else
            {
                Shoot();
                cooldown = turret.fireRate;
            }
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
        targets.Enqueue(enemy);
        if (target == null && GetNewTarget())
            Events.active.RegisterTurret(this);
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
