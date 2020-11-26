using UnityEngine;
using Michsky.UI.ModernUIPack;

public abstract class TileClass : MonoBehaviour
{

    [SerializeField]
    protected ParticleSystem Effect;
    public float maxhp = 1;
    public float health = 1;
    public int heat = 1;
    public int cost = 0;
    public int level = 1;
    [TextArea] public string description = "No description provided.";

    // Abstract methods
    public abstract void DestroyTile();

    // Apply damage to entity
    public void DamageTile(int dmgRecieved)
    {
        health -= dmgRecieved;
        if (health <= 0)
        {
            DestroyTile();
        }
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
