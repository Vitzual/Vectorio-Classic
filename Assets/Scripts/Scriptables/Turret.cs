using UnityEngine;

[CreateAssetMenu(fileName = "New Turret", menuName = "Building/Turret")]
public class Turret : Building
{
    // IAudible interface variables
    public AudioClip sound;

    // Base turret stat variables
    public float damage;
    public float range;
    public float rotationSpeed;
    public float fireRate;
    public int bulletPierces;
    public int bulletAmount;
    public float bulletSpeed;
    public float bulletSpread;
    public bool bulletLock;

    // Base turret modifiers
    [HideInInspector] public float damageModifier;
    [HideInInspector] public float rangeModifier;
    [HideInInspector] public float rotationSpeedModifier;
    [HideInInspector] public float fireRateModifier;
    [HideInInspector] public int bulletPiercesModifier;
    [HideInInspector] public int bulletAmountModifier;
    [HideInInspector] public float bulletSpeedModifier;
    [HideInInspector] public float bulletSpreadModifier;

    // Set panel stats
    // This gets used to set the stats on the building menu panel
    public override void CreateStats(Panel panel)
    {
        panel.CreateStat(new Stat("Health", health, healthModifier, Sprites.GetSprite("Health")));
        panel.CreateStat(new Stat("Damage", damage, damageModifier, Sprites.GetSprite("Damage")));
        panel.CreateStat(new Stat("Range", range, damageModifier, Sprites.GetSprite("Range")));
        panel.CreateStat(new Stat("Firerate", fireRate, damageModifier, Sprites.GetSprite("Firerate")));
        panel.CreateStat(new Stat("Pierces", bulletPierces, damageModifier, Sprites.GetSprite("Pierces")));
        panel.CreateStat(new Stat("Bullets", bulletAmount, damageModifier, Sprites.GetSprite("Bullets")));
        panel.CreateStat(new Stat("Spread", bulletSpread, damageModifier, Sprites.GetSprite("Spread")));

        // Base method
        base.CreateStats(panel);
    }

    public virtual void RotateTurret(ActiveTurret entity)
    {
        // Get target position relative to this entity
        Vector2 targetPosition = new Vector2(entity.target.obj.position.x, entity.target.obj.position.y);

        // Get the distance from the turret to the target
        Vector2 distance = targetPosition - new Vector2(entity.barrel.position.x, entity.barrel.position.y);

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
        float difference = targetAngle - (entity.barrel.rotation.eulerAngles.z);

        if ((difference < 0 || difference >= 180) && !(difference < -180))
        {
            // Calculate how far to rotate the turret given how long since the last frame
            float distanceToRotate = -entity.turret.rotationSpeed * Time.deltaTime;

            // If distance to rotate would rotate past the target only rotate the distance
            if (distanceToRotate < difference)
                distanceToRotate = difference;

            // Rotate the turret
            entity.barrel.Rotate(Vector3.forward, distanceToRotate);
        }
        else if (!(difference <= 5 && difference >= -5))
        {
            // Calculate how far to rotate the turret given how long since the last frame
            float distanceToRotate = -entity.turret.rotationSpeed * Time.deltaTime;

            // If distance to rotate would rotate past the target only rotate the distance
            if (distanceToRotate > difference)
                distanceToRotate = difference;

            // Rotate the turret
            entity.barrel.Rotate(Vector3.forward, distanceToRotate);
        }
        else
        {
            entity.barrel.transform.eulerAngles = new Vector3(0, 0, targetAngle);
            if (entity.cooldown > 0) 
            {
                entity.cooldown -= Time.deltaTime;
            }
            else
            {
                Shoot(entity);
                entity.cooldown = entity.turret.fireRate;
            }
        }

    }

    // Attempts to fire a bullet and returns true if fired
    public virtual void Shoot(ActiveTurret entity)
    {
        foreach (Transform firePoint in entity.firePoints)
            for (int i = 0; i < entity.turret.bulletAmount; i += 1)
                CreateBullet(entity, firePoint.position);
    }

    // Create a bullet object
    public virtual void CreateBullet(ActiveTurret entity, Vector2 position)
    {
        if (entity.turret.sound != null)
            Debug.Log("Playing sound!");

        GameObject bullet = Instantiate(entity.bullet, position, entity.barrel.rotation);
        bullet.transform.rotation = entity.barrel.rotation;
        bullet.transform.Rotate(0f, 0f, Random.Range(-entity.turret.bulletSpread, entity.turret.bulletSpread));

        float speed = Random.Range(entity.turret.bulletSpeed - 10, entity.turret.bulletSpeed + 10);
        int pierces = entity.turret.bulletPierces + Research.research_pierce;
        float damage = entity.turret.damage + Research.research_damage;

        // Dependent on the bullet, register under the correct master script
        Events.active.BulletFired(new Bullet(bullet.transform, entity.target, speed, pierces, damage, bulletLock));
        Debug.Log("Broadcasting bullet fired event");
    }
}
