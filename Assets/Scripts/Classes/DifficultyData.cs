using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DifficultyData 
{
    // Difficulty values
    public string name;

    // Heat tracking
    public int heatTracked;

    // Gameplay settings
    public bool enableInstaPlace;
    public bool naturalHeatGrowth;
    public bool autoSpawnGuardians;

    // Difficulty modifiers
    public float enemySpawnrateModifier;
    public float enemyHealthModifier;
    public float enemySpeedModifier;
    public float enemyGroupSpawnrate;
    public float enemyGroupSpawnsize;

    // Resource modifiers
    public float goldSpawnModifier;
    public float essenceSpawnModifier;
    public float iridiumSpawnModifier;
    public float vectoriumSpawnModifier;
}
