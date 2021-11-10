using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Blueprint", menuName = "Building/Blueprint")]
public class Blueprint : ScriptableObject
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
        Resource,
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
        Legendary,
        Mythic
    }

    public enum Type
    {
        Building,
        Resource,
        Defense,
        All
    }

    [Header("Blueprint Info")]
    public new string name;
    [TextArea] public string description;
    
}
