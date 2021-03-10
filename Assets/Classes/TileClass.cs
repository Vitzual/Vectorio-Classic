using UnityEngine;
using System.Collections.Generic;

public abstract class TileClass : MonoBehaviour
{
    // Tile class variables
    [SerializeField]
    protected ParticleSystem Effect;
    public float maxhp = 1;
    public float health = 1;
    public int heat = 1;
    public Stack<int> heatStack = new Stack<int>();
    public int cost = 1;
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
        return cost;
    }
    public int GetHeat()
    {
        if (heatStack.Count == 0)
            heatStack.Push(heat);
        return heatStack.Peek();
    }
    public int GetHealth() { return (int) health; }
    public void SetHealth(int a) { health = (float) a; }
}
