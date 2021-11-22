using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DifficultyData 
{
    // Difficulty values
    public string name;
    public string description;

    // Gameplay modifiers
    public bool enableInstaPlace;
    public bool naturalHeatGrowth;

    // Resource modifiers
    public float goldSpawnModifier;
    public float essenceSpawnModifier;
    public float iridiumSpawnModifier;

    // Start resources
    public int startingGold;
    public int startingEssence = 0;
    public int startingIridium = 0;
    public int startingPower;
    public int startingHeat;

    // Cost modifiers
    public float buildingCostModifier;
    public float buildingHealthModifier;
    public float buildingDamageModifier;

    // Enemy modifiers
    public float enemyHealthModifier;
    public float enemyDamageModifier;
    public float enemySpeedModifier;
    public float enemySpawnrateModifier;
}
