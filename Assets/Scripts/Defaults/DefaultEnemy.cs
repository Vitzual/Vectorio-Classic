using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DefaultEnemy : DefaultEntity
{
    // Class variables
    public Enemy enemy;
    [HideInInspector] public Variant variant;
    public Transform rotator;
    [HideInInspector] public DefaultBuilding target;

    // Sprite info
    public SpriteRenderer[] border;
    public SpriteRenderer[] fill;
    public TrailRenderer[] trail;

    [HideInInspector] public int raycastCooldown;

    public override void Setup()
    {
        raycastCooldown = Random.Range(0, 5); 

        variant = enemy.variant;
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

    public virtual void GiveDamage(DefaultBuilding building)
    {
        building.DamageEntity(enemy.damage);
    }

    public override void DestroyEntity()
    {
        ParticleSystemRenderer holder = Instantiate(enemy.variant.particle, transform.position,
            Quaternion.identity).GetComponent<ParticleSystemRenderer>();
        holder.material = enemy.variant.border;
        holder.trailMaterial = enemy.variant.border;
        Destroy(gameObject);
    }

    public virtual void MoveTowards(Transform obj, Transform target)
    {
        float step = enemy.moveSpeed * Time.deltaTime;
        obj.position = Vector2.MoveTowards(obj.position, target.position, step);
    }
}
