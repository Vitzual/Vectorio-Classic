using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : ScriptableObject
{
    // Resource class
    [System.Serializable]
    public class Resources
    {
        public Resource.CurrencyType resource;
        public int amount;
    }

    // Cell class
    [System.Serializable]
    public struct Cell
    {
        public float x;
        public float y;
    }


    // Entity description
    [Header("Entity Info")]
    public new string name;
    [TextArea] public string description;
    public GameObject obj;
    public Material material;
    public bool gridSnap;
    public float hologramSize = 1f;

    // Inventory variables
    [Header("Interface Ordering")]
    public int invOrder;
    public int invIndex;

    // Refers to how many cells this entity will occupy
    [Header("Tile cells")]
    public Cell[] cells;
    public Vector2 offset;

    // Building base variables
    [Header("Base Stats")]
    public int health;
    public Resources[] resources;

    // Creates stats
    public virtual void CreateStats(Panel panel)
    {
        // Resource stats
        foreach (Resources type in resources)
        {
            string name = Resource.active.GetName(type.resource);
            Sprite sprite = Resource.active.GetSprite(type.resource);
            panel.CreateStat(new Stat(name, type.amount, 0, sprite, true));
        }

        panel.CreateStat(new Stat("Health", health, 0, Sprites.GetSprite("Health")));
    }
}
