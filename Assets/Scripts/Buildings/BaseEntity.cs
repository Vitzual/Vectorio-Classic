using UnityEngine;
//using Mirror;
using System.Collections.Generic;


public class BaseEntity : MonoBehaviour, IDamageable
{
    // IDamageable interface variables
    public float health { get; set; }
    public float maxHealth { get; set; }
    public int metadata = -1;

    protected ParticleSystem particle;
    [HideInInspector] public Material material;

    public virtual void Setup() 
    {
        // Override this method to use Setup() call from Instation Handler
    }

    public virtual void ApplyMetadata(int data)
    {
        metadata = data;
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
        Destroy(gameObject);
    }

    // Heals the entity (IDamageable interface method)
    public virtual void HealEntity(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }
}
