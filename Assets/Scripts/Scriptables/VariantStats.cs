using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class VariantStats
{
    // Enemy stats
    [FoldoutGroup("Variant Stats")]
    public Variant type;
    [FoldoutGroup("Variant Stats")]
    public float health;
    [FoldoutGroup("Variant Stats")]
    public float damage;
    [FoldoutGroup("Variant Stats")]
    public float moveSpeed;
}
