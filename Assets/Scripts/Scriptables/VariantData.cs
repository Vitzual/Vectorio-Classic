using Sirenix.OdinInspector;
using UnityEngine;

public class VariantData : MonoBehaviour
{
    // Enemy stats
    [FoldoutGroup("Variant Stats")]
    public float damage;
    [FoldoutGroup("Variant Stats")]
    public float moveSpeed;
    [FoldoutGroup("Variant Stats")]
    public float explosiveRadius;
    [FoldoutGroup("Variant Stats")]
    public float explosiveDamage;
    [FoldoutGroup("Variant Stats")]
    public float rotationSpeed;
    [FoldoutGroup("Variant Stats")]
    public float spawnChance;
    [FoldoutGroup("Variant Stats")]
    public bool largeEnemy;

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
