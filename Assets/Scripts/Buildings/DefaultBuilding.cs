using UnityEngine;

public class DefaultBuilding : MonoBehaviour, IDamageable
{
    // IDamageable interface variables
    public int health { get; set; }
    public int maxHealth { get; set; }
    public ParticleSystem deathParticle { get; set; }

    // Building default stat variables
    protected int cost;
    protected int power;
    protected int heat;

    // Building default description
    protected string description;

    // Damages the entity (IDamageable interface method)
    public void DamageEntity(int dmg)
    {
        bool isFull = health == maxHealth;
        health -= dmg;
        if (health <= 0) DestroyEntity();
        else if (isFull) BuildingSystem.damagedBuildings.Add(transform);
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
        BuildingRegistrar.BuildingStats tileStats = buildingRegistrar.GetBuildingStats(transform);

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
}
