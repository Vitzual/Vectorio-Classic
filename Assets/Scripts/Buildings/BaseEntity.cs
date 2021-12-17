using UnityEngine;
using System.Collections.Generic;

public class BaseEntity : MonoBehaviour, IDamageable
{
    // IDamageable interface variables
    public float health { get; set; }
    public float maxHealth { get; set; }
    public SpriteRenderer[] models;
    public Cosmetic cosmetic;
    [HideInInspector]
    public List<ParticleSystem> particles;
    public int metadata = -1;
    public int runtimeID = -1;
    public string internalID = "";
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
        Events.active.ChangeMetadata(runtimeID, data);
        metadata = data;
    }

    // If entity has button click available, override here
    public virtual void ButtonClicked()
    {
        Debug.Log("This building has no button to use");
    }

    // Collision methods
    public virtual void OnBoxCollision(DefaultEnemy enemy)
    {
        //Debug.Log("This building has no box collision override");
    }

    // Collision methods
    public virtual void OnCircleCollision(DefaultEnemy enemy)
    {
        //Debug.Log("This building has no circle collision override");
    }

    // Collision methods
    public virtual void OnCircleLeave(DefaultEnemy enemy)
    {
        //Debug.Log("This building has no circle collision override");
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
    public virtual void SyncDestroy()
    {
        if (Communicator.active != null)
            Communicator.active.SyncEntityDestroyed(runtimeID);
        else DestroyEntity();
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

    // Syncs an entity for all clients
    public virtual void SyncEntity(int freeVar)
    {
        Debug.Log("Entity has no sync method!");
    }

    // Applies a cosmetic
    public virtual void ApplyCosmetic(Cosmetic cosmetic)
    {
        // Check model being applied
        if (models.Length == 0)
        {
            Debug.Log("This build is not setup for cosmetics!");
            return;
        }

        // Check layering 
        if (cosmetic.layers.Count > models.Length)
        {
            Debug.Log("Layers on cosmetic exceed layers on " + transform.name + "!");
            return;
        }

        // Set cosmetic
        this.cosmetic = cosmetic;

        // Reset all particles
        if (particles != null && particles.Count > 0)
            for (int a = 0; a < particles.Count; a++)
                Recycler.AddRecyclable(particles[a].transform);
        particles = new List<ParticleSystem>();

        // Reset all models
        for (int b = 0; b < models.Length; b++)
            models[b].gameObject.SetActive(false);

        // Loop through all layers and apply
        int index = 0;
        foreach (Cosmetic.Layer layer in cosmetic.layers)
        {
            if (index >= models.Length) break;

            // Set model
            models[index].gameObject.SetActive(true);
            models[index].sprite = layer.model;
            models[index].color = layer.color;
            models[index].sortingOrder = layer.order;
            if (layer.material != null)
                models[index].material = layer.material;

            index += 1;
        }

        // Loop through all particles and instantiate
        foreach (Cosmetic.Particle particle in cosmetic.particles)
        {
            // Create particle
            ParticleSystemRenderer newParticle = Instantiate(particle.effect, particle.position, Quaternion.identity).GetComponent<ParticleSystemRenderer>();

            // Setup particle
            if (newParticle != null)
            {
                newParticle.material = particle.material;
                newParticle.sortingOrder = particle.order;
            }
            else Debug.Log("No particle renderer on new particle system!");
        }
    }

    // Get material
    public virtual Material GetMaterial()
    {
        return null;
    }
}
