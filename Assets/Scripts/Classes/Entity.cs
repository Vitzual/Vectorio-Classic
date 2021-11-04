using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : ScriptableObject
{
    public enum InvHeader
    {
        Defense = 0,
        Building = 1,
        Enemy = 2,
        Guardian = 3
    }

    // Entity description
    [Header("Entity Variables")]
    public new string name;
    [TextArea] public string description;
    public InvHeader inventoryHeader;
    public int inventoryIndex;
    public GameObject obj;
    public Material material;
    public int health;
    public Unlockable unlockable;

    [Header("Grid Variables")]
    public bool gridSnap;
    public Vector2 gridOffset;
    public float hologramSize = 1f;

    // Creates stats
    public virtual void CreateStats(Panel panel)
    {
        panel.CreateStat(new Stat("Health", health, 0, Sprites.GetSprite("Health")));
    }
}
