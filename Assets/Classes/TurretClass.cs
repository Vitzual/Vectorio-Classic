using System.Collections.Generic;
using UnityEngine;

public abstract class TurretClass : TileClass
{
    // Bullet Handler
    private BulletHandler bulletHandler;

    // Weapon variables
    public int range;
    public float rotationSpeed;
    public int bulletDamage = 1;
    public int bulletPierces = 1;
    public int bulletAmount = 1;
    public float bulletSpeed = 80;
    public float bulletSpread = 1;
    public float fireRate;
    public Transform[] FirePoints;
    public Rigidbody2D Gun;
    public GameObject Bullet;
    public HashSet<GameObject> nearbyEnemies = new HashSet<GameObject>();

    // Global variables
    protected float nextFire = 0;
    protected float timePassed = 0;
    protected bool hasTarget = false;
    protected GameObject target = null;
    protected float enemyAngle;
    protected float gunRotation;

    // Let's see if this shit works amiright my dude
    private void Start()
    {
        bulletHandler = GameObject.Find("Bullet Handler").GetComponent<BulletHandler>();
    }

    protected GameObject FindNearestEnemy()
    {
        var layermask1 = 1 << LayerMask.NameToLayer("Enemy");
        var layermask2 = 1 << LayerMask.NameToLayer("Enemy Defense");
        var finalmask = layermask1 | layermask2; 

        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            this.gameObject.transform.position, 
            range + Research.bonus_range, finalmask);

        GameObject result = null;
        float closest = float.PositiveInfinity;

        foreach (Collider2D collider in colliders)
        {
            float distance = (collider.transform.position - this.transform.position).sqrMagnitude;
            if (distance < closest) {
                result = collider.gameObject;
                closest = distance;
            }
        }
        return result;
    }

    protected GameObject FindNearestPlayer()
    {
        var colliders = Physics2D.OverlapCircleAll(
            this.gameObject.transform.position,
            range + Research.bonus_range,
            1 << LayerMask.NameToLayer("Building"));
        GameObject result = null;
        float closest = float.PositiveInfinity;

        foreach (Collider2D collider in colliders)
        {
            float distance = (collider.transform.position - this.transform.position).sqrMagnitude;
            if (distance < closest)
            {
                result = collider.gameObject;
                closest = distance;
            }
        }
        return result;
    }

    protected void RotateTowardsPlayer()
    {
        if (!hasTarget)
        {
            target = FindNearestPlayer();
        }

        // If a target exists, shoot at it
        if (target != null)
        {
            // Flag hasTarget
            hasTarget = true;

            // Rotate turret towards target
            Vector2 TargetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
            Vector2 lookDirection = (TargetPosition - Gun.position);
            enemyAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            enemyAngle = AlignRotation(enemyAngle);
            gunRotation = AlignRotation(Gun.rotation);

            // Smooth rotation when targetting enemies
            if (((gunRotation >= enemyAngle && !(gunRotation >= 270 && enemyAngle <= 90)) || (gunRotation <= 90 && enemyAngle >= 270))
            && !((gunRotation - enemyAngle) <= 0.3 && (gunRotation - enemyAngle) >= -0.3))
            {
                if (Gun.rotation - rotationSpeed < enemyAngle)
                {
                    Gun.rotation = enemyAngle;
                }
                else
                {
                    Gun.rotation -= rotationSpeed;
                }
            }
            else if (!((gunRotation - enemyAngle) <= 0.3 && (gunRotation - enemyAngle) >= -0.3))
            {
                if (Gun.rotation + rotationSpeed > enemyAngle)
                {
                    Gun.rotation = enemyAngle;
                }
                else
                {
                    Gun.rotation += rotationSpeed;
                }
            }
        }
    }

    protected void RotateTowardNearestEnemy() 
    {
        if (!hasTarget) {
            target = FindNearestEnemy();
        }

        // If a target exists, shoot at it
        if (target != null)
        {
            // Flag hasTarget
            hasTarget = true;

            // Rotate turret towards target
            Vector2 TargetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
            Vector2 lookDirection = (TargetPosition - Gun.position);
            enemyAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            enemyAngle = AlignRotation(enemyAngle);
            gunRotation = AlignRotation(Gun.rotation);

            // Smooth rotation when targetting enemies
            if (((gunRotation >= enemyAngle && !(gunRotation >= 270 && enemyAngle <= 90)) || (gunRotation <= 90 && enemyAngle >= 270))
            && !((gunRotation - enemyAngle) <= 0.3 && (gunRotation - enemyAngle) >= -0.3))
            {
                if (Gun.rotation - rotationSpeed < enemyAngle) 
                {
                    Gun.rotation = enemyAngle;
                } else 
                {
                    Gun.rotation -= rotationSpeed;
                }
            }
            else if (!((gunRotation - enemyAngle) <= 0.3 && (gunRotation - enemyAngle) >= -0.3))
            {
                if (Gun.rotation + rotationSpeed > enemyAngle) 
                {
                    Gun.rotation = enemyAngle;
                } else 
                {
                    Gun.rotation += rotationSpeed;
                }
            }
        }
    }

    // Attempts to fire a bullet and returns true if fired
    protected bool Shoot(GameObject prefab, Transform pos, float multiplier = 1)
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
            return false;
        }
        else
        {
            for (int i = 0; i < bulletAmount; i += 1)
            {
                CreateBullet(prefab, pos, multiplier);
            }
            if (fireRate - Research.bonus_firerate <= 0.03f) nextFire = 0.03f;
            else nextFire = fireRate - Research.bonus_firerate;
            return true;
        }
    }

    // Createa a bullet object
    protected void CreateBullet(GameObject prefab, Transform pos, float multiplier = 1)
    {
        pos.position = new Vector3(pos.position.x, pos.position.y, 0);
        GameObject bullet = Instantiate(prefab, pos.position, pos.rotation);
        bullet.transform.rotation = pos.rotation;
        bullet.transform.Rotate(0f, 0f, Random.Range(-bulletSpread, bulletSpread));

        // Register the bullet
        float speed = Random.Range(bulletSpeed - 10, bulletSpeed + 10) * Research.bonus_bulletspeed;
        int pierces = bulletPierces + Research.bonus_pierce;
        int damage = bulletDamage + Research.bonus_damage;
        bulletHandler.RegisterBullet(bullet.transform, speed, pierces, damage);
    }

    protected static float AlignRotation(float r) {
        float temp = r;
        while (temp < 0f) 
        {
            temp += 360f;
        }
        while (temp > 360f)
        {
            temp -= 360f;
        }
        return temp;
    }
}
