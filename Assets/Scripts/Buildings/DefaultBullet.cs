using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : MonoBehaviour
{
    // Turret variable
    [HideInInspector] public Turret turret;

    // Bullet variables
    [HideInInspector] public float damage;
    [HideInInspector] public float speed;
    [HideInInspector] public int pierces;
    [HideInInspector] public float time;

    // Bullet movement variables
    [HideInInspector] public bool tracking = false;
    [HideInInspector] public bool recycling = false;

    // Renderering variables
    [HideInInspector] private TrailRenderer trail;
    [HideInInspector] private ParticleSystemRenderer particle;

    // Setup bullet
    public virtual void Setup(Turret turret)
    {
        this.turret = turret;

        trail = GetComponent<TrailRenderer>();
        particle = GetComponent<ParticleSystemRenderer>();

        if (trail != null) trail.material = turret.material;
        if (particle != null) particle.material = turret.material;

        damage = turret.damage + Research.research_damage;
        speed = Random.Range(turret.bulletSpeed - 2, turret.bulletSpeed + 2);
        pierces = turret.bulletPierces + Research.research_pierce;

        time = turret.bulletTime;
    }

    // Hold info
    public virtual void DestroyBullet(Material material)
    {
        recycling = true;
        CreateParticle(material);
        Recycler.AddRecyclable(transform);
    }

    // Creates a particle and sets the material
    public ParticleSystemRenderer CreateParticle(Material material)
    {
        ParticleSystemRenderer holder = Instantiate(turret.bulletParticle, transform.position,
                transform.rotation).GetComponent<ParticleSystemRenderer>();
        holder.transform.rotation *= Quaternion.Euler(0, 0, 180f);
        holder.material = material;
        holder.trailMaterial = material;

        return holder;
    }

    // If a collision is detected, apply damage
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        BaseEntity entity = other.GetComponent<BaseEntity>();
        entity.DamageEntity(damage);

        pierces -= 1;
        if (pierces <= 0)
            DestroyBullet(entity.material);

        tracking = false;
    }
}
