using UnityEngine;
using System.Collections.Generic;

public class DefaultBuilding : MonoBehaviour, IDamageable
{
    // IDamageable interface variables
    public int health { get; set; }
    public int maxHealth { get; set; }
    public ParticleSystem deathParticle { get; set; }

    // Tile class stat variables
    protected int cost;
    protected int power;
    protected int heat;

    // Damages the entity (IDamageable interface method)
    public void DamageEntity(int dmg)
    {
        bool isFull = health == maxHealth;
        health -= dmg;
        if (health <= 0) DestroyEntity();
        else if (isFull) BuildingHandler.damagedBuildings.Add(transform);
    }

    // Destroys the entity (IDamageable interface method)
    public void DestroyEntity()
    {
        Destroy(gameObject);
        // Do other stuff
    }

    // Heals the entity (IDamageable interface method)
    public void HealEntity(int amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }

    // Death effect for the entity (IDamageable interface method)
    public void PlayDeathEffect()
    {
        Instantiate(deathParticle, transform.position, Quaternion.identity); 
    }

    // Set base turret variables
    public void InitTileStats()
    {
        // Grab values from BuildingRegistrar
        BuildingRegistrar buildingRegistrar = ScriptHandler.buildingRegistrar;
        BuildingRegistrar.TileStats tileStats = buildingRegistrar.getTileStats(transform);

        // Check to make sure the stats exist
        if (tileStats == null)
        {
            Debug.LogError("Could not find stats in registrar for " + transform.name);
            return;
        }

        // Set values returned from BuildingRegistrar 
        health = tileStats.health;
        maxHealth = tileStats.maxHealth;
        cost = tileStats.cost;
        power = tileStats.power;
        heat = tileStats.heat;
        deathParticle = tileStats.deathParticle;
        description = tileStats.description;
    }

    public int GetCost() { return cost; }
    public int GetPower() { return power; }
    public int GetHeat() { return heat; }
    public string GetDescription() { return description; }



    // OLD CODE //


    public virtual void UpdateWalls() { Debug.Log(transform.name + " is not a wall!"); }
    public virtual void UpdateStorage() { Debug.Log(transform.name + " is not a storage!"); }
    public virtual void UpdatePower() { Debug.Log(transform.name + " does not produce power!"); }
    public virtual void UpdateEnergizer() { Debug.Log(transform.name + " is not an energizer!"); }
    public virtual void UpdateEnhancer() { Debug.Log(transform.name + " is not an enhancer!"); }
    public virtual void EndGame() { Debug.Log(transform.name + " is not allowed to end the game!"); }
    public virtual void ModifyResearch() { Debug.Log(transform.name + " is not allowed to modify research!"); }

    // Update power
    public void UpdatePower(Transform sender)
    {
        RaycastHit2D[] aocbHit = Physics2D.RaycastAll(transform.position, Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("AOCB"));
        foreach (RaycastHit2D ray in aocbHit)
            if (ray.collider.transform != null && ray.collider.transform != sender) return;
        DestroyTile(true);
    }

    // Apply damage to entity
    public bool DamageTile(int dmgRecieved)
    {
        health -= dmgRecieved;
        if (health + Research.research_health <= 0)
        {
            DestroyTile(true);
            return true;
        }

        // Add to damaged buildings list
        if (!BuildingHandler.damagedBuildings.Contains(transform))
            BuildingHandler.damagedBuildings.Add(transform);

        return false;
    }
}
