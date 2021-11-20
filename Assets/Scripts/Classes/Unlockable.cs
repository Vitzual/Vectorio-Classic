using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Unlockable
{
    // Unlock type
    public enum UnlockType
    {
        ReachResourceAmount,
        PlaceBuildingAmount,
        DestroyEnemyAmount,
        DestroyGuardianAmount
    }

    [Header("Unlock Type")]
    public UnlockType type;
    public string description;
    public Building requirement;
    public bool unlocked;

    [Header("Unlock Requirement")]
    public Resource.CurrencyType resource;
    public Entity entity;
    public int amount;
}
