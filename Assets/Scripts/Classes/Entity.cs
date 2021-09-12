using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : ScriptableObject
{
    // Entity description
    public new string name;
    [TextArea] public string description;
    public GameObject obj;

    // Tile variables 
    public Tile tile;
    public bool snap;

    // Building base variables
    public int health;
    public int level;
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int healthModifier;

    // Inventory variables
    public int invOrder;
    public int invIndex;

    // Holds active amount in scene
    [HideInInspector] public int active = 0;

    public virtual void CreateStats(Panel panel)
    {
        panel.CreateStat(new Stat("Health", health, 0, Sprites.GetSprite("Health")));
    }
}
