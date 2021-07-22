using UnityEngine;

public class BaseTurret : MonoBehaviour, IDamageable
{
    // IDamageable interface variables
    public int health { get; set; }
    public int maxHealth { get; set; }

    // Base turret stat variables
    public int damage;
    public int range;
    public int rotationSpeed;
    public float fireRate;
    public int bulletPierces;
    public int bulletAmount;
    public float bulletSpeed;
    public float bulletSpread;

    // Base turret object variables
    public Transform[] FirePoints;
    public Transform Gun;
    public GameObject Bullet;

    // IDamageable damage method
    public void damageEntity(int dmg)
    {
        health -= dmg;
        if (health <= 0) destroyEntity();
    }

    // IDamageable destroy method
    public void destroyEntity()
    {
        Destroy(gameObject);
        // Do other stuff
    }

    // IDamageable heal method
    public void healEntity(int amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }
}
