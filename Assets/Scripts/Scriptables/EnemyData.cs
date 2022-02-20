using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Vectorio/Enemy/Enemy")]
public class EnemyData : Entity
{
    // Enemy stats
    [SerializeField] private List<VariantStats> _variants;
    public Dictionary<Variant, VariantStats> variants;

    // Enemy variables
    public bool spawnInWaves;
    public float rotationSpeed;

    // Spawn on death
    [System.Serializable]
    public class EnemySpawn
    {
        public EnemyData enemy;
        public int amount;
        public float radius;
    }
    [FoldoutGroup("Enemy Stats")]
    public EnemySpawn[] spawnsOnDeath;

    // Generate variants on runtime
    public void GenerateVariants()
    {
        variants = new Dictionary<Variant, VariantStats>();
        foreach (VariantStats variant in _variants)
            variants.Add(variant.type, variant);
    }

    // Set panel stats
    // This gets used to set the stats on the building menu panel
    public override void CreateStats(Panel panel)
    {
        // Base method
        base.CreateStats(panel);
    }
}
