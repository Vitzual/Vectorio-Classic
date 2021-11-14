using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Blueprint", menuName = "Building/Blueprint")]
public class Blueprint : IdentifiableScriptableObject
{
    public enum Effect
    {
        Health,
        Damage,
        Range,
        RotationSpeed,
        ReloadRate,
        BulletPierces,
        BulletAmount,
        BulletSpeed,
        BulletSpread,
        BulletLifetime,
        BulletTracking,
        BulletSticking,
        BulletExplosion,
        Discount,
        Power,
        Heat,
        CollectAmount,
        CollectRate,
        AutoStorage
    }

    public enum RarityType
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public enum Type
    {
        Building,
        Resource,
        Defense,
        All
    }

    [System.Serializable]
    public class EffectType
    {
        [TableColumnWidth(110)]
        public Effect effect;
        [TableColumnWidth(10)]
        public bool negative;
    }

    [System.Serializable]
    public class Rarity
    {
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 0)]
        [Title("Rarity Modifier")]
        public RarityType rarity;
        public float[] modifier;
        [Range(0f, 0.1f)]
        public float dropChance;
        public Resource.Cost applicationCost;
        [PropertySpace(SpaceBefore = 0, SpaceAfter = 20)]
        public Resource.Cost removalCost;
    }

    [FoldoutGroup("Blueprint Info")]
    public new string name;
    [FoldoutGroup("Blueprint Info")]
    [TextArea] public string description;
    [FoldoutGroup("Blueprint Info")]
    public Type type;
    [FoldoutGroup("Blueprint Info")]
    public Sprite icon;
    [FoldoutGroup("Blueprint Info")]
    public List<Blueprint> blacklist;

    [FoldoutGroup("Blueprint Effect")]
    public List<EffectType> effects;
    
    [FoldoutGroup("Blueprint Rarities")]
    public List<Rarity> rarities;
}
//