using UnityEngine;
using System.Collections.Generic;

public class TileClass : MonoBehaviour, IDamageable
{
    // Tile class stat variables
    public int cost;
    public int power;
    public int heat;

    // IDamageable interface variables
    public int health { get; set; }
    public int maxHealth { get; set; }

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
    }




    // OLD CODE //

    // Tile class variables
    public ParticleSystem Effect;
    public string tileType = "Default";
    public Stack<int> heatStack = new Stack<int>();
    public int ID = 0;
    public bool isBig = false;
    [TextArea] public string description = "No description provided.";

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
        Instantiate(Effect, transform.position, Quaternion.identity);
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

    public void DecreaseHeat(int amount)
    {
        GameObject.Find("Spawner").GetComponent<Spawner>().decreaseHeat(heatStack.Peek());
        if (heatStack.Peek() - amount >= 0)
            heatStack.Push(heat - amount);
        else
            heatStack.Push(0);
        GameObject.Find("Spawner").GetComponent<Spawner>().increaseHeat(heatStack.Peek());
    }

    public void IncreaseHeat()
    {
        if (heatStack.Count == 0)
        {
            heatStack.Push(heat);
            heatStack.Push(heat);
        }
        int holder = heatStack.Pop();
        GameObject.Find("Spawner").GetComponent<Spawner>().increaseHeat(heatStack.Peek() - holder);
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

    public string GetDescription()
    {
        return description;
    }

    public float GetPercentage()
    {
        return (health / maxHealth) * 100;
    }

    public int GetCost()
    {
        // Set additional costs via multiplier 
        return cost;
    }

    public int GetHeat()
    {
        if (heatStack.Count == 0)
            heatStack.Push(heat);
        return heatStack.Peek();
    }

    public int GetHealth() { return (int) health; }
    public void SetHealth(int a) { health = a; }
}
