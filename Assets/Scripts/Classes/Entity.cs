using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : ScriptableObject
{
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

    // Cell class
    [System.Serializable]
    public struct Cell
    {
        public float x;
        public float y;
    }

    // Refers to how many cells this building will occupy
    [Header("Tile cells")]
    public Cell[] cells;
    public Vector2 offset;

    // Building base variables
    [Header("Base Stats")]
    public int health;
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int healthModifier;

    // Holds active amount in scene
    [HideInInspector] public int active = 0;

    public virtual void CreateStats(Panel panel)
    {
        panel.CreateStat(new Stat("Health", health, 0, Sprites.GetSprite("Health")));
    }
}
