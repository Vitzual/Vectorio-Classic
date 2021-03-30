using System.Collections.Generic;
using UnityEngine;

public abstract class TurretClass : TileClass
{
    // Bullet Handler
    private BulletHandler bulletHandler;
    private EnemyBulletHandler enemyHandler;
    private bool isEnemy = false;

    private LayerMask EnemyLayer;
    public bool scanThisFrame;

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

    protected CameraScroll cameraScript;

    // Gun shot particle
    public bool gunShotParticles = false;
    public ParticleSystem shotParticle;

    // Shot anim variables
    public bool animationEnabled = false;
    protected bool animPlaying = false;
    protected bool animRebound = false;
    public int animTracker;
    private int animHolder;
    public float animMovement = 4f;

    // Let's see if this shit works amiright my dude
    private void Start()
    {
        scanThisFrame = Random.Range(0, 2) > 0;

        if (transform.name.Contains("Enemy"))
        {
            EnemyLayer = 1 << LayerMask.NameToLayer("Building");
            enemyHandler = GameObject.Find("Enemy Bullet Handler").GetComponent<EnemyBulletHandler>();
            isEnemy = true;
        }
        else
        {
            TurretHandler.buildings.Add(transform);
            EnemyLayer = 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Enemy Defense");
            bulletHandler = GameObject.Find("Bullet Handler").GetComponent<BulletHandler>();
        }
        if (animationEnabled) animHolder = animTracker;
        cameraScript = GameObject.Find("Camera").GetComponent<CameraScroll>();
    }

    public void PlayAnim()
    {
        if (!animRebound)
        {
            animTracker -= 1;
            Gun.localPosition -= Gun.up * animMovement * Time.deltaTime;
            if (animTracker == animHolder/2)
            {
                animTracker = 0;
                animRebound = true;
            }
        }
        else
        {
            animTracker += 1;
            Gun.localPosition += Gun.up * animMovement/2 * Time.deltaTime;
            if (animTracker == animHolder)
            {
                Gun.localPosition = new Vector2(0, 0);
                animRebound = false;
                animPlaying = false;
            }
        }
        return;
    }

    public void PlayAudio()
    {
        float audioScale = cameraScript.getZoom() / 1400f;
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sound/" + transform.name), this.gameObject.transform.position, Settings.soundVolume - audioScale);
    }

    protected GameObject FindNearestEnemy()
    {
        scanThisFrame = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            this.gameObject.transform.position, 
            range + Research.bonus_range, EnemyLayer);

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
        scanThisFrame = false;

        var colliders = Physics2D.OverlapCircleAll(
            this.gameObject.transform.position,
            range + Research.bonus_range,
            EnemyLayer);
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
        if (scanThisFrame || target != null)
        {
            if (!hasTarget)
                target = FindNearestPlayer();
            if (target != null)
            {
                // Temporary fix to the enemy turret rotation logic
                // Uses targetting logic from RotationHandler() but just applies rotation towards target 
                Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
                Vector2 distance = targetPosition - new Vector2(Gun.position.x, Gun.position.y);
                float targetAngle = Mathf.Atan(distance.y / distance.x) * Mathf.Rad2Deg + 90f;
                if (distance.x > 0) targetAngle += 180;
                Gun.transform.eulerAngles = new Vector3(0, 0, targetAngle);
            }
        }
        else scanThisFrame = true;
    }

    protected void RotateTowardNearestEnemy() 
    {
        if (scanThisFrame || target != null)
        {
            if (!hasTarget)
                target = FindNearestEnemy();
            if (target != null)
                RotationHandler();
        }
        else scanThisFrame = true;
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

            // Correct for if target is directly above or below the turret
            if (distance.x == 0)
            {
                if (distance.y > 0) targetAngle = 0;
                else targetAngle = 180;
            }

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
        if (gunShotParticles) Instantiate(shotParticle, FirePoints[0].position, Quaternion.Euler(0, 0, Gun.localEulerAngles.z + 180f));
        if (hasAudio) try { PlayAudio(); } catch { }
        if (animationEnabled) animPlaying = true;

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
