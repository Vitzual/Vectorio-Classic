using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "Buildings/Building")]
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
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int healthModifier;
    public bool isDefensive;

    // Lock things
    public string unlockDesc;
    public bool isUnlocked = true;

    // Resources
    public Resources[] resources;
    public Material material;

    public virtual void CreateStat()
    {
        // Resource stats

        UIEvents.active.CreateStat(new Stat("Health", health, healthModifier, Sprites.active.health));
    }
}
