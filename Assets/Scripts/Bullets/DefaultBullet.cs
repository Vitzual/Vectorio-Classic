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
    [HideInInspector] public SpriteRenderer model;
    [HideInInspector] public TrailRenderer trail;

    // Ignore list
    [HideInInspector]
    public List<BaseEntity> ignoreList = new List<BaseEntity>();

    // Setup bullet
    public virtual void Setup(Turret turret, Sprite model = null)
    {
        this.turret = turret;

        this.model = GetComponent<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();

        if (model != null)
        {
            this.model.sprite = model;
            this.model.material = turret.material;
        }

        trail.material = turret.material;

        damage = turret.damage + Research.damage;
        speed = Random.Range(turret.bulletSpeed - 2, turret.bulletSpeed + 2);
        pierces = turret.bulletPierces + Research.pierce;

        time = turret.bulletTime;
    }

    // Hold info
    public virtual void DestroyBullet(Material material, BaseEntity entity = null)
    {
        recycling = true;
        CreateParticle(material);
        Recycler.AddRecyclable(transform);
    }

    // Creates a particle and sets the material
    public void CreateParticle(Material material)
    {
        ParticleSystemRenderer holder = Instantiate(turret.bulletParticle, transform.position,
                transform.rotation).GetComponent<ParticleSystemRenderer>();
        holder.transform.rotation *= Quaternion.Euler(0, 0, 180f);
        holder.material = material;
        holder.trailMaterial = material;
    }

    // If a collision is detected, apply damage
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        BaseEntity entity = other.GetComponent<BaseEntity>();

        if (!ignoreList.Contains(entity))
        {
            entity.DamageEntity(damage);

            pierces -= 1;
            if (pierces <= 0)
                DestroyBullet(entity.material, entity);
            else ignoreList.Add(entity);

            tracking = false;
        }
    }
}
