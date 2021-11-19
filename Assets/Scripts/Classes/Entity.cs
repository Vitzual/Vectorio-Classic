using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Entity : IdentifiableScriptableObject
{
    public enum InvHeader
    {
        Defense = 0,
        Building = 1,
        Enemy = 2,
        Guardian = 3
    }

    // Entity description
    [FoldoutGroup("Entity Info")]
    public GameObject obj;
    [FoldoutGroup("Entity Info")]
    public new string name;
    [FoldoutGroup("Entity Info")]
    [TextArea] public string description;
    [FoldoutGroup("Entity Info")]
    public int health;
    public Tuple<Effect, float> _health;
    [FoldoutGroup("Entity Info")]
    public Material material;

    // Inventory variables
    [FoldoutGroup("Inventory Variables")]
    public InvHeader inventoryHeader;
    [FoldoutGroup("Inventory Variables")]
    public int inventoryIndex;
    [FoldoutGroup("Inventory Variables")]
    public Unlockable unlockable;

    // Grid variables
    [FoldoutGroup("Grid Variables")]
    public bool gridSnap;
    [FoldoutGroup("Grid Variables")]
    public Vector2 gridOffset;
    [FoldoutGroup("Grid Variables")]
    public float hologramSize = 1f;

    // Creates stats
    public virtual void CreateStats(Panel panel)
    {
        panel.CreateStat(new Stat("Health", health, 0, Sprites.GetSprite("Health")));
    }
}
