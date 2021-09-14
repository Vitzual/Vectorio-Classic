using UnityEngine;

public class Bullet
{
    public Bullet(Transform obj, ActiveEnemy target, float speed, int pierces, float damage, float time, bool tracking)
    {
        this.obj = obj;
        this.target = target;
        this.speed = speed;
        this.pierces = pierces;
        this.damage = damage;
        this.time = time;
        this.tracking = tracking;
    }

    public Transform obj;
    public ActiveEnemy target;
    public float speed;
    public int pierces;
    public float damage;
    public float time;
    public bool tracking;
}
