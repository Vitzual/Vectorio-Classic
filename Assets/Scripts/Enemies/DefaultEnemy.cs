using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DefaultEnemy : BaseEntity
{
    // Class variables
    public Enemy enemy;
    [HideInInspector] public Variant variant;
    [HideInInspector] public BaseTile target;

    // Sprite info
    public SpriteRenderer[] border;
    public SpriteRenderer[] fill;
    public TrailRenderer[] trail;

    public override void Setup()
    {
        material = variant.border;

        health = enemy.health;
        maxHealth = health;

        foreach (SpriteRenderer a in border)
            a.material = variant.border;

        foreach (SpriteRenderer a in fill)
            a.material = variant.fill;

        foreach (TrailRenderer a in trail)
            a.material = variant.trail;

        Events.active.EnemySpawned(this);
    }

    public virtual void GiveDamage(BaseTile building)
    {
        building.DamageEntity(enemy.damage);
    }

    public override void DestroyEntity()
    {
        ParticleSystemRenderer holder = Instantiate(variant.particle, transform.position,
            Quaternion.identity).GetComponent<ParticleSystemRenderer>();
        holder.material = variant.border;
        holder.trailMaterial = variant.border;
        Destroy(gameObject);
    }

    public virtual void MoveTowards(Transform obj, Transform target)
    {
        float step = enemy.moveSpeed * Time.deltaTime;
        obj.position = Vector2.MoveTowards(obj.position, target.position, step);
    }

    // If a collision is detected, destroy the other entity and apply damage to self
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other is BoxCollider2D)
        {
            BaseTile building = other.GetComponent<BaseTile>();

            if (building != null)
            {
                GiveDamage(building);
                if (building != null)
                    Destroy(gameObject);
            }
        }
        else
        {
            DefaultTurret turret = other.GetComponent<DefaultTurret>();

            if (turret != null)
                turret.AddTarget(this);
        }
    }
}
