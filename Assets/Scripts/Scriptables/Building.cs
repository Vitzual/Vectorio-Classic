using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building/Building")]
public class Building : Tile
{
    // Resource class
    [System.Serializable]
    public class Resources
    {
        public Resource.Currency resource;
        public int amount;
        [HideInInspector] public int modifier;
    }

    // Building base variables
    public int health;
    [HideInInspector] public int level = 1;
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int healthModifier;
    public bool isDefensive;

    // Lock things
    public int unlockOrder;
    public string unlockDesc;
    public bool isUnlocked = true;

    // Resources
    public Resources[] resources;
    public Material material;

    public virtual void CreateStats(Panel panel)
    {
        // Resource stats
        foreach (Resources type in resources)
            panel.CreateStat(new Stat(Resource.GetName(type.resource), type.amount, type.modifier, Resource.GetSprite(type.resource), true));

        panel.CreateStat(new Stat("Health", health, healthModifier, Sprites.GetSprite("Health")));
    }
}
