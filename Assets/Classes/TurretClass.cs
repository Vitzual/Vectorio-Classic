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

    // Global variables
    protected float nextFire = 0;
    protected float timePassed = 0;

    

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
}
