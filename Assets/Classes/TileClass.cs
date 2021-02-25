using UnityEngine;

public abstract class TileClass : MonoBehaviour
{

    // Tile class variables
    [SerializeField]
    protected ParticleSystem Effect;
    public float maxhp = 1;
    public float health = 1;
    public int heat = 1;
    public int cost = 1;
    public int level = 1;
    public int power = 1;
    public int ID = 0;
    [TextArea] public string description = "No description provided.";

    // Abstract methods
    public abstract void DestroyTile();

    // Update power
    public void UpdatePower()
    {
        RaycastHit2D aocbHit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("AOCB"));
        Debug.Log(transform.position);
        if (aocbHit.collider == null) DestroyTile();
    }

    // Apply damage to entity
    public void DamageTile(int dmgRecieved)
    {
        health -= dmgRecieved;
        if (health + Research.bonus_health <= 0)
        {
            GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
            DestroyTile();
        }
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
        return cost;
    }
    public void SetLevel(int a)
    {
        level = a;
    }
    public void IncreaseLevel(int a)
    {
        level += a;
    }
    public int GetLevel()
    {
        return level;
    }
    public string GetTier()
    {
        if (level == 1)
        {
            return "Tier I";
        } 
        else if (level == 2)
        {
            return "Tier II";
        } 
        else if (level == 3)
        {
            return "Tier III";
        } 
        else
        {
            return "Tier IV";
        }
    }
    public int GetHeat()
    {
        return heat;
    }
}
