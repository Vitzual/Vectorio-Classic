using UnityEngine;
using System.Collections.Generic;

public class BaseEntity : MonoBehaviour, IDamageable
{
    // IDamageable interface variables
    public float health { get; set; }
    public float maxHealth { get; set; }
    public int metadata = -1;
    public int runtimeID = -1;
    protected ParticleSystem particle;

    public virtual void Setup() 
    {
        // Override this method to use Setup() call from Instation Handler
    }

    public virtual void OnClick()
    {
        Events.active.EntityClicked(this);
    }

    public virtual void ApplyMetadata(int data)
    {
        metadata = data;
    }

    public virtual void ApplySettings(int settingType, int newSetting)
    {
        Debug.Log("This building has no settings to apply");
    }

    // If entity has button click available, override here
    public virtual void ButtonClicked()
    {
        Debug.Log("This building has no button to use");
    }

    // Collision methods
    public virtual void OnBoxCollision(DefaultEnemy enemy)
    {
        Debug.Log("This building has no box collision override");
    }

    // Collision methods
    public virtual void OnCircleCollision(DefaultEnemy enemy)
    {
        Debug.Log("This building has no circle collision override");
    }

    // Collision methods
    public virtual void OnCircleLeave(DefaultEnemy enemy)
    {
        Debug.Log("This building has no circle collision override");
    }

    // Damages the entity (IDamageable interface method)
    public virtual void DamageEntity(float dmg)
    {
        health -= dmg;

        if (health <= 0)
            DestroyEntity();
        else
        {
            // Update damage handler
            Events.active.BuildingHurt(this);
        }
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

    // Resets a tile (free method)
    public virtual void ResetTile()
    {
        Debug.Log("This tile cannot be reset!");
    }

    public virtual void SyncEntity(int freeVar)
    {
        Debug.Log("Entity has no sync method!");
    }

    // Get material
    public virtual Material GetMaterial()
    {
        return null;
    }
}
