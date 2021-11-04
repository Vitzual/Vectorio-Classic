using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Unlockable
{
    // Unlock type
    public enum UnlockType
    {
        ReachHeat,
        ReachPower,
        PlaceBuildings,
        DestroyEnemies,
        DestroyGuardian,
        ReachGoldPerSecond,
        ReachEssencePerSecond,
        ReachIridiumPerSecond
    }


    public string unlockDescription;
    public bool unlockedByDefault;
    
}
