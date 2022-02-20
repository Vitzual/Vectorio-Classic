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

    // Materials and stuff
    [FoldoutGroup("Variant Material")]
    public Material border;
    [FoldoutGroup("Variant Material")]
    public Material fill;
    [FoldoutGroup("Variant Material")]
    public Material trail;
    [FoldoutGroup("Variant Material")]
    public ParticleSystem particle;
}
