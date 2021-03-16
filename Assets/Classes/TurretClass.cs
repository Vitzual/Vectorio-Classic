using System.Collections.Generic;
using UnityEngine;

public abstract class TurretClass : TileClass
{
    // Bullet Handler
    private BulletHandler bulletHandler;
    private EnemyBulletHandler enemyHandler;
    private bool isEnemy = false;

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
    public Transform Gun;
    public GameObject Bullet;
    public bool hasAudio = false;
    public HashSet<GameObject> nearbyEnemies = new HashSet<GameObject>();

    // Global variables
    protected float nextFire = 0;
    protected float timePassed = 0;
    protected bool hasTarget = false;
    public GameObject target = null;
    protected float enemyAngle;
    protected float gunRotation;
    public bool isRotating = true;

    // Let's see if this shit works amiright my dude
    private void Start()
    {
        if (transform.name.Contains("Enemy"))
        {
            enemyHandler = GameObject.Find("Enemy Bullet Handler").GetComponent<EnemyBulletHandler>();
            isEnemy = true;
        }
        else bulletHandler = GameObject.Find("Bullet Handler").GetComponent<BulletHandler>();
    }

    public void PlayAudio()
    {
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sound/" + transform.name), this.gameObject.transform.position, Settings.soundVolume);
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
            target = FindNearestPlayer();
        if (target != null)
            RotationHandler();
    }

    protected void RotateTowardNearestEnemy() 
    {
        if (!hasTarget)
            target = FindNearestEnemy();
        if (target != null)
            RotationHandler();
    }

    protected void RotationHandler()
    {
        // If a target exists, shoot at it
        if (target != null)
        {
            // Set turret to rotating state
            isRotating = true;

            // Flag hasTarget
            hasTarget = true;

            // Get target position relative to this entity
            Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);

            // Get the distance from the turret to the target
            Vector2 distance = targetPosition - new Vector2(Gun.position.x, Gun.position.y);

            // Get the angle between the gun position and the target position
            float targetAngle = Mathf.Atan(distance.y / distance.x) * Mathf.Rad2Deg + 90f;
            if (distance.x > 0) targetAngle += 180;

            // Calculate the difference between the target angle and the current angle
            float difference = targetAngle - (Gun.rotation.eulerAngles.z);

            if ((difference < 0 || difference >= 180) && !(difference < -180))
            {
                // Calculate how far to rotate the turret given how long since the last frame
                float distanceToRotate = -rotationSpeed * Time.deltaTime;

                // If distance to rotate would rotate past the target only rotate the distance
                if (distanceToRotate < difference)
                {
                    isRotating = false;
                    distanceToRotate = difference;
                }

                // Rotate the turret
                Gun.Rotate(Vector3.forward, distanceToRotate);
            }
            else if (!(difference <= 5 && difference >= -5))
            {
                // Calculate how far to rotate the turret given how long since the last frame
                float distanceToRotate = rotationSpeed * Time.deltaTime;

                // If distance to rotate would rotate past the target only rotate the distance
                if (distanceToRotate > difference)
                {
                    isRotating = false;
                    distanceToRotate = difference;
                }

                // Rotate the turret
                Gun.Rotate(Vector3.forward, distanceToRotate);
            }
            else
            {
                Gun.transform.eulerAngles = new Vector3(0, 0, targetAngle);
                isRotating = false;
            }
        }
    }

    // Attempts to fire a bullet and returns true if fired
    protected bool Shoot(GameObject prefab, Transform pos, float multiplier = 1)
    {
        isRotating = true;
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
        if (hasAudio) try { PlayAudio(); } catch { }

        pos.position = new Vector3(pos.position.x, pos.position.y, 0);
        GameObject bullet = Instantiate(prefab, pos.position, pos.rotation);
        bullet.transform.rotation = pos.rotation;
        bullet.transform.Rotate(0f, 0f, Random.Range(-bulletSpread, bulletSpread));

        // Register the bullet
        float speed = Random.Range(bulletSpeed - 10, bulletSpeed + 10) * Research.bonus_bulletspeed;
        int pierces = bulletPierces + Research.bonus_pierce;
        int damage = bulletDamage + Research.bonus_damage;

        // Dependent on the bullet, register under the correct master script
        if (isEnemy)
            enemyHandler.RegisterBullet(bullet.transform, bullet.GetComponent<EnemyBullet>(), speed, pierces, damage);
        else
            bulletHandler.RegisterBullet(bullet.transform, speed, pierces, damage);
    }
}
