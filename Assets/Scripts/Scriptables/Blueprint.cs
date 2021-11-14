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
        [TableColumnWidth(30)]
        public Effect effect;
        [TableColumnWidth(85)]
        [Range(0f, 5f)]
        public float modifier;
        [TableColumnWidth(5)]
        public bool negative;
    }

    [System.Serializable]
    public class Rarity
    {
        public RarityType rarity;
        [TableList(AlwaysExpanded = true, DrawScrollView = false)]
        public List<EffectType> effects = new List<EffectType>();
        [Range(0f, 0.1f)]
        public float dropChance;
    }

    [FoldoutGroup("Blueprint Info")]
    public new string name;
    [FoldoutGroup("Blueprint Info")]
    [TextArea] public string description;
    [FoldoutGroup("Blueprint Info")]
    public Type type;
    [FoldoutGroup("Blueprint Info")]
    public Sprite icon;
    [FoldoutGroup("Effects")]
    public List<Rarity> rarities;
}
