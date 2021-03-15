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
    protected GameObject target = null;
    protected float enemyAngle;
    protected float gunRotation;
    protected bool isRotating = false;

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
            // Flag hasTarget
            hasTarget = true;

            // Get target position relative to this entity
            Vector2 TargetPosition = new Vector2(target.transform.position.x, target.transform.position.y);

            // Get the direction towards that unit from this entity
            Vector2 lookDirection = TargetPosition - new Vector2(transform.position.x, transform.position.y);

            // Get the angle between the target and this transform
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;

            if(angle > 0)
                Gun.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            else if (angle < 0)
                Gun.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
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
