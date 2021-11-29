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
    [HideInInspector] public BaseEntity target;
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
        // Set turret SO
        this.turret = turret;
        
        // Get trail renderer component
        trail = GetComponent<TrailRenderer>();
        trail.material = turret.material;

        // If model is not null, set it
        if (model != null)
        {
            this.model = GetComponent<SpriteRenderer>();
            this.model.sprite = model;
            this.model.material = turret.material;
        }

        // Apply research to variables
        damage = turret.damage * Research.damageBoost;
        pierces = turret.bulletPierces + Research.pierceBoost;

        // Set speed (and randomize if applicable)
        if (turret.randomizeSpeed) speed = Random.Range(turret.bulletSpeed - 5, turret.bulletSpeed + 5);
        else speed = turret.bulletSpeed;

        // Set bullet lifetime
        time = turret.bulletTime;
    }

    // Move bullet
    public virtual void Move()
    {
        if (tracking && target != null)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
        }
        else transform.position += transform.up * speed * Time.deltaTime;
    }

    // Hold info
    public virtual void DestroyBullet(Material material, BaseEntity entity = null)
    {
        if (entity != null)
            material = entity.GetMaterial();

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
    public virtual void OnCollision(BaseEntity entity)
    {
        if (!ignoreList.Contains(entity))
        {
            entity.DamageEntity(damage);

            pierces -= 1;
            if (pierces <= 0)
                DestroyBullet(entity.GetMaterial(), entity);
            else ignoreList.Add(entity);

            tracking = false;
        }
    }
}
