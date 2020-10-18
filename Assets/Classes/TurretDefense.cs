using UnityEngine;

public class TurretDefense : MonoBehaviour
{

    // Weapon variables
    protected float fireRate;
    protected float bulletForce;
    protected float bulletSpread;
    protected int range;

    // Global variables
    protected float nextFire = 0;

    // Creates bullet object
    protected void Shoot(GameObject prefab, Transform pos)
    {
        GameObject bullet = Instantiate(prefab, pos.position, pos.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(BulletSpread(pos.up, Random.Range(bulletSpread, -bulletSpread)) * bulletForce, ForceMode2D.Impulse);
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
