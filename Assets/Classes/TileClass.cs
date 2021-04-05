using UnityEngine;
using System.Collections.Generic;

public abstract class TileClass : MonoBehaviour
{
    // Tile class variables
    [SerializeField]
    protected ParticleSystem Effect;
    protected Difficulties difficulties;
    public float maxhp = 1;
    public float health = 1;
    public int heat = 1;
    public Stack<int> heatStack = new Stack<int>();
    public int cost = 1;
    public int power = 1;
    public int ID = 0;
    public bool isBig = false;
    [TextArea] public string description = "No description provided.";

    // Engineer mods
    [System.Serializable]
    public class EngineerMods
    {
        public string title;
        [TextArea] public string description;
        public int upgradeTime;
        public int successRate;
        public int iridiumCost;
        public GameObject originalObj;
        public GameObject engineerObj;
    }
    public EngineerMods[] EngineerModifications;

    private void Start()
    {
        difficulties = GameObject.Find("Difficulty").GetComponent<Difficulties>();
    }

    // Engineer variables
    public List<int> AppliedModification = new List<int>();

    // Engineer variables
    public bool isEngineered = false;

    // Create empty applyModification() method
    public virtual void ApplyModification(int modID) { Debug.Log(transform.name + " does not contain a modification with ID " + modID); }

    // Abstract methods
    public abstract void DestroyTile();

    // Returns the modification time of a unit
    public int GetModificationTime(int modID)
    {
        return EngineerModifications[modID].upgradeTime;
    }

    // Sets an engineer button based on the modID
    public void SetEngineerButton(Transform building, int modID) 
    {
        if (name == "Turret")
            Debug.Log("a");
    }

    // Check if the building is engineered
    public bool IsModifiable()
    {
        return AppliedModification.Count == 0;
    }

    // Update power
    public void UpdatePower()
    {
        RaycastHit2D aocbHit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("AOCB"));
        if (aocbHit.collider == null) DestroyTile();
    }

    // Apply damage to entity
    public bool DamageTile(int dmgRecieved)
    {
        health -= dmgRecieved;
        if (health + Research.bonus_health <= 0)
        {
            DestroyTile();
            return true;
        }

        // Add to damaged buildings list
        if (!BuildingHandler.damagedBuildings.Contains(transform))
            BuildingHandler.damagedBuildings.Add(transform);

        return false;
    }

    public void DecreaseHeat(int amount)
    {
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heatStack.Peek());
        if (heatStack.Peek() - amount >= 0)
            heatStack.Push(heat - amount);
        else
            heatStack.Push(0);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().increaseHeat(heatStack.Peek());
    }

    public void IncreaseHeat()
    {
        if (heatStack.Count == 0)
        {
            heatStack.Push(heat);
            heatStack.Push(heat);
        }
        int holder = heatStack.Pop();
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().increaseHeat(heatStack.Peek() - holder);
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
        return (health / maxhp) * 100;
    }
    public int GetCost()
    {
        return (int)(cost * GameObject.Find("Difficulty").GetComponent<Difficulties>().GetAdditionalCost());
    }
    public int GetHeat()
    {
        if (heatStack.Count == 0)
            heatStack.Push(heat);
        return heatStack.Peek();
    }
    public int GetHealth() { return (int) health; }
    public void SetHealth(int a) { health = (float) a; }
    public void IncreaseHealth() { health = health * GameObject.Find("Difficulty").GetComponent<Difficulties>().GetDefenseHP(); }
    public void setEngineered(bool a)
    {
        isEngineered = a;
    }
    public bool checkEngineered()
    {
        return isEngineered;
    }
}
