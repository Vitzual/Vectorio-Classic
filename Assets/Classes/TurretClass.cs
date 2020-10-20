using UnityEngine;

public abstract class TurretClass : TileClass
{

    // Weapon variables
    protected float fireRate;
    protected float bulletForce;
    protected float bulletSpread;
    protected float bulletAmount;
    protected float rotationSpeed;
    protected int range;
    public Transform Point;
    public Rigidbody2D Gun;
    public GameObject Bullet;

    // Global variables
    protected float nextFire = 0;
    protected float timePassed = 0;
    protected bool hasTarget = false;
    protected EnemyPool target = null;
    protected float enemyAngle;
    protected float gunRotation;

    
    protected void RotateTowardNearestEnemy() {
        if (!hasTarget) {
            // Find closest enemy 
            target = EnemyPool.FindClosestEnemy(Point.position, range); 
        }

        // If a target exists, shoot at it
        if (target != null)
        {
            // Flag hasTarget
            hasTarget = true;

            // Rotate turret towards target
            Vector2 TargetPosition = new Vector2(target.gameObject.transform.position.x, target.gameObject.transform.position.y);
            Vector2 lookDirection = (TargetPosition - Gun.position);
            enemyAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            enemyAngle = AlignRotation(enemyAngle);
            gunRotation = AlignRotation(Gun.rotation);
            
            // Smooth rotation when targetting enemies
            if (((gunRotation >= enemyAngle && !(gunRotation >= 270 && enemyAngle <= 90)) || (gunRotation <= 90 && enemyAngle >= 270))
            && !((gunRotation - enemyAngle) <= 0.3 && (gunRotation - enemyAngle) >= -0.3))
            {
                Gun.rotation -= rotationSpeed;
            }
            else if (!((gunRotation - enemyAngle) <= 0.3 && (gunRotation - enemyAngle) >= -0.3))
            {
                Gun.rotation += rotationSpeed;
            }
        }
    }

    // Creates bullet object
    protected void Shoot(GameObject prefab, Transform pos)
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
            return;
        }
        else
        {
            for(int i=0; i<bulletAmount; i+=1)
            {
                pos.position = new Vector3(pos.position.x, pos.position.y, 0);
                GameObject bullet = Instantiate(prefab, pos.position, pos.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(BulletSpread(pos.up, Random.Range(bulletSpread, -bulletSpread)) * bulletForce, ForceMode2D.Impulse);
            }
            nextFire = fireRate;
        }
    }

    // Calculate bullet spread
    private static Vector2 BulletSpread(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
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
