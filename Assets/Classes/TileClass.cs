using UnityEngine;

public abstract class TileClass : MonoBehaviour
{

    [SerializeField]
    protected ParticleSystem Effect;
    public float maxhp = 1;
    public float health = 1;
    public int heat = 1;
    public int cost = 1;
    public int level = 1;
    public int power = 1;
    public bool isPowered = false;
    [TextArea] public string description = "No description provided.";

    // Abstract methods
    public abstract void DestroyTile();

    // Apply damage to entity
    public void DamageTile(int dmgRecieved)
    {
        health -= dmgRecieved;
        if (health <= 0)
        {
            GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
            DestroyTile();
        }
    }

    // Check if tile should be powered
    public bool checkPower()
    {
        // Raycast adjacent tiles 
        RaycastHit2D a = Physics2D.Raycast(new Vector2(transform.position.x + 5f, transform.position.y), Vector2.zero, Mathf.Infinity);
        RaycastHit2D b = Physics2D.Raycast(new Vector2(transform.position.x - 5f, transform.position.y), Vector2.zero, Mathf.Infinity);
        RaycastHit2D c = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 5f), Vector2.zero, Mathf.Infinity);
        RaycastHit2D d = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 5f), Vector2.zero, Mathf.Infinity);

        // If wire is one of them and wire is powered, return powered
        if (a.collider != null && a.collider.name == "Wire" && a.collider.GetComponent<TileClass>().getPower() == true) return true;
        if (b.collider != null && b.collider.name == "Wire" && b.collider.GetComponent<TileClass>().getPower() == true) return true;
        if (c.collider != null && c.collider.name == "Wire" && c.collider.GetComponent<TileClass>().getPower() == true) return true;
        if (d.collider != null && d.collider.name == "Wire" && d.collider.GetComponent<TileClass>().getPower() == true) return true;

        return false;
    }

    public void setConsumption(int a)
    {
        power = a;
    }

    public int getConsumption()
    {
        return power;
    }

    public void updatePower()
    {
        isPowered = checkPower();
    }

    public bool getPower()
    {
        return isPowered;
    }

    public void setPower(bool a)
    {
        isPowered = a;
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
