using UnityEngine;
using Mirror;
using System.Collections.Generic;


public class DefaultEntity : NetworkBehaviour, IDamageable
{
    // IDamageable interface variables
    public float health { get; set; }
    public float maxHealth { get; set; }

    protected ParticleSystem particle;

    public virtual void Setup()
    {
        health = 1;
        maxHealth = 1;
    }

    // Damages the entity (IDamageable interface method)
    public virtual void DamageEntity(float dmg)
    {
        health -= dmg;
        if (health <= 0)
            DestroyEntity();
    }

    // Destroys the entity (IDamageable interface method)
    public virtual void DestroyEntity()
    {
        if (particle != null)
            Instantiate(particle, transform.position, transform.rotation);
        Recycler.AddRecyclable(transform);
    }

    // Heals the entity (IDamageable interface method)
    public virtual void HealEntity(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }
}
