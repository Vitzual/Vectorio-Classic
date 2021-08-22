using UnityEngine;

public class Bullet
{
    public Bullet(Transform bullet, Transform target, float speed, int pierces, int damage)
    {
        this.bullet = bullet;
        this.target = target;
        this.speed = speed;
        this.pierces = pierces;
        this.damage = damage;
    }

    public Transform bullet;
    public Transform target;
    public float speed;
    public int pierces;
    public int damage;
}
