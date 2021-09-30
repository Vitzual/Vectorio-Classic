using UnityEngine;

public class Bullet
{
    public Bullet(Transform obj, BaseEntity target, float speed, int pierces, float damage, float time, bool tracking, Material material, ParticleSystem particle)
    {
        this.obj = obj;
        this.target = target;
        this.speed = speed;
        this.pierces = pierces;
        this.damage = damage;
        this.time = time;
        this.tracking = tracking;
        this.material = material;
        this.particle = particle;
    }

    public Transform obj;
    public BaseEntity target;
    public float speed;
    public int pierces;
    public float damage;
    public float time;
    public bool tracking;
    public Material material;
    public ParticleSystem particle;
}
