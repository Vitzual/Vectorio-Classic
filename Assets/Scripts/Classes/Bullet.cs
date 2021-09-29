using UnityEngine;

public class Bullet
{
    public Bullet(Transform obj, DefaultEntity target, float speed, int pierces, float damage, float time, bool tracking, Material material)
    {
        this.obj = obj;
        this.target = target;
        this.speed = speed;
        this.pierces = pierces;
        this.damage = damage;
        this.time = time;
        this.tracking = tracking;
        this.material = material;
    }

    public Transform obj;
    public DefaultEntity target;
    public float speed;
    public int pierces;
    public float damage;
    public float time;
    public bool tracking;
    public Material material;
}
