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
    public Material material;
    [FoldoutGroup("Entity Info")]
    public Color lowresColor;

    // Inventory variables
    [FoldoutGroup("Inventory Variables")]
    public InvHeader inventoryHeader;
    [FoldoutGroup("Inventory Variables")]
    public int inventoryIndex;
    [FoldoutGroup("Inventory Variables")]
    public Unlockable unlockable;
    [FoldoutGroup("Inventory Variables")]
    public bool display = true;

    // Creates stats
    public virtual void CreateStats(Panel panel) { }
}
