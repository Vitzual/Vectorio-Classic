using UnityEngine;
using Mirror;
using System.Collections.Generic;

[HideInInspector]
public class DefaultBuilding : NetworkBehaviour, IDamageable
{
    // IDamageable interface variables
    public float health { get; set; }
    public float maxHealth { get; set; }

    public Entity entity;

    public void SetStats()
    {
        health = entity.health;
        maxHealth = health;
    }

    // Damages the entity (IDamageable interface method)
    public bool DamageEntity(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            DestroyEntity();
            return true;
        }
        else return false;
    }

    // Destroys the entity (IDamageable interface method)
    public void DestroyEntity()
    {
        // Create the particle
        //Instantiate(Resources.Load<ParticleSystem>("Particles/Death"), 
        //    transform.position, Quaternion.identity).GetComponent<ParticleSystemRenderer>();

        Destroy(gameObject);
    }

    // Heals the entity (IDamageable interface method)
    public void HealEntity(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }
}
