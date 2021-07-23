using UnityEngine;
using System.Collections.Generic;

public class TileClass : MonoBehaviour, IDamageable
{
    // Tile class stat variables
    protected int cost;
    protected int power;
    protected int heat;

    // IDamageable interface variables
    public int health { get; set; }
    public int maxHealth { get; set; }
    public ParticleSystem deathParticle { get; set; }

    // Tile class variables
    public int ID = 0;
    public string tileType = "Default";
    public bool isBig = false;
    [SerializeField] protected string description;

    // IDamageable damage method
    public void DamageEntity(int dmg)
    {
        health -= dmg;
        if (health <= 0) DestroyEntity();
    }

    // IDamageable destroy method
    public void DestroyEntity()
    {
        Destroy(gameObject);
        // Do other stuff
    }

    // IDamageable heal method
    public void HealEntity(int amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }

    // Plays the death particle from the IDamageable interface
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

    // Abstract methods
    public void DestroyTile(bool takeResources)
    {
        switch (tileType)
        {
            // If the building is of type storage, decrease storage capacity
            case "Storage":
                UpdateStorage();    
                break;

            // If the building is of type power, decrease available power
            case "Power":
                UpdatePower();
                break;

            // If the building is of type energizer, destroy buildings in the area
            case "Energizer":
                UpdateEnergizer();
                break;

            // If the building is of type wall, update nearby walls
            case "Wall":
                UpdateWalls();
                break;

            // If the building is of type enhancer, update the enhancer
            case "Enhancer":
                UpdateEnhancer();
                break;

            // If the building is the hub, end the game
            case "Hub":
                EndGame();
                break;
        }

        // If take resources set true, remove heat and power
        if (takeResources)
        {
            GameObject.Find("Spawner").GetComponent<Spawner>().decreaseHeat(heat);
            GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power);
        }

        // Remove building from the building handler
        BuildingHandler.removeBuilding(transform);
        Destroy(gameObject);
    }

    // Sets an engineer button based on the modID
    public void SetEngineerButton(Transform building, int modID) 
    {
        if (name == "Turret")
            Debug.Log("a");
    }

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

    public int getID()
    {
        return ID;
    }

    public void setConsumption(int a)
    {
        power = a;
    }

    public int getConsumption()
    {
        return power;
    }

    public float GetPercentage()
    {
        return (health / maxHealth) * 100;
    }

    public int GetHealth() { return (int) health; }
    public void SetHealth(int a) { health = a; }
}
