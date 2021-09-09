using UnityEngine;

public class Bullet
{
    public Bullet(Transform obj, Transform target, float speed, int pierces, float damage)
    {
        this.obj = obj;
        this.target = target;
        this.speed = speed;
        this.pierces = pierces;
        this.damage = damage;
    }

    public Transform obj;
    public Transform target;
    public float speed;
    public int pierces;
    public float damage;
    public bool tracking;
}
