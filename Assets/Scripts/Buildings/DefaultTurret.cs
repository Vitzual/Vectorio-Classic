using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefaultTurret : BaseTile, IAudible
{
    // IAudible interface variables
    public AudioClip sound { get; set; }

    // Barrel thing
    public Turret turret;

    // Base turret object variables
    public Transform[] firePoints;
    public Transform cannon;
    [HideInInspector] public BaseEntity target;
    public Queue<BaseEntity> targets = new Queue<BaseEntity>();
    [HideInInspector] public float cooldown;

    // Bullet model
    [HideInInspector] public bool useBulletModel = false;
    [HideInInspector] public Sprite bulletModel;
    public bool selfEnable = false;
    public bool overrideRange = false;

    public void Start()
    {
        if (selfEnable)
        {
            if (!overrideRange)
            {
                CircleCollider2D collider = GetComponent<CircleCollider2D>();
                if (collider != null) collider.radius = turret.range;
            }

            Events.active.RegisterTurret(this);

            if (cosmetic != null)
                ApplyCosmetic(cosmetic);
        }
    }

    public override void Setup()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();

        if (collider != null)
            collider.radius = turret.range;
        else Debug.Log("Turret does not have a circle collider!");

        if (turret.bulletSpriteName != "")
        {
            bulletModel = Sprites.GetSprite(turret.bulletSpriteName);
            useBulletModel = bulletModel != null;
        }

        base.Setup();
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

            if (turret.randomizeCooldown)
                cooldown = Random.Range(turret.cooldown, turret.cooldown + turret.cooldown);
            else cooldown = turret.cooldown / Research.firerateBoost;
        }
    }

    // Attempts to fire a bullet and returns true if fired
    public virtual void Shoot()
    {
        foreach (Transform firePoint in firePoints)
            for (int i = 0; i < turret.bulletAmount + Research.bulletBoost; i += 1)
                CreateBullet(firePoint.position);
    }
    
    // Create a bullet object
    public virtual void CreateBullet(Vector2 position)
    {
        // Play sound if available
        if (turret.sound != null)
            AudioSource.PlayClipAtPoint(turret.sound, transform.position, Settings.sound);

        // Set bullet variables
        DefaultBullet bullet;

        // Setup bullet
        if (cosmetic == null || !cosmetic.useBullet)
        {
            // Create the bullet
            GameObject holder = Instantiate(turret.bullet.gameObject, position, cannon.rotation);
            holder.transform.rotation = cannon.rotation;
            holder.transform.Rotate(0f, 0f, Random.Range(-turret.bulletSpread, turret.bulletSpread));

            // Set bullet variables
            bullet = holder.GetComponent<DefaultBullet>();
            bullet.Setup(turret);

            if (turret.useBulletSprite) 
                bullet.SetupModel(bulletModel);
        }
        else
        {
            // Create the bullet
            GameObject holder = Instantiate(cosmetic.bullet.obj.gameObject, position, cannon.rotation);
            holder.transform.rotation = cannon.rotation;
            holder.transform.Rotate(0f, 0f, Random.Range(-turret.bulletSpread, turret.bulletSpread));

            // Set bullet variables
            bullet = holder.GetComponent<DefaultBullet>();
            bullet.Setup(turret, cosmetic.bullet);

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

    // IAudible sound method
    public void PlaySound()
    {
        AudioSource.PlayClipAtPoint(sound, gameObject.transform.position, Settings.sound);
    }

    public override void OnCircleCollision(DefaultEnemy enemy)
    {
        AddTarget(enemy);
    }

    public override void OnCircleLeave(DefaultEnemy enemy)
    {
        RemoveTarget(enemy);
    }

    public virtual void AddTarget(BaseEntity enemy)
    {
        if (!targets.Contains(enemy))
        {
            targets.Enqueue(enemy);
            if (target == null && GetNewTarget())
                Events.active.RegisterTurret(this);
        }
    }

    public virtual void RemoveTarget(BaseEntity enemy)
    {
        targets = new Queue<BaseEntity>(targets.Where(x => x != enemy));
        if (target == enemy) { target = null; GetNewTarget(); }
    }

    public virtual bool GetNewTarget()
    {
        if (targets.Count > 0)
        {
            target = targets.Dequeue();
            return true;
        }
        else return false;
    }
}
