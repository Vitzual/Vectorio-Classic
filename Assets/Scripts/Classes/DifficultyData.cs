using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DifficultyData 
{
    // Default constructor
    public DifficultyData()
    {
        name = "Unnamed Save";
        heatTracked = 0;
        useDroneConstruction = true;
        naturalHeatGrowth = false;
        autoSpawnGuardians = false;
        enemySpawnrateModifier = 1f;
        enemyHealthModifier = 1f;
        enemySpeedModifier = 1f;
        enemyGroupSpawnrate = 1f;
        enemyGroupSpawnsize = 1f;
        goldSpawnModifier = 1f;
        essenceSpawnModifier = 1f;
        iridiumSpawnModifier = 1f;
        vectoriumSpawnModifier = 1f;
    }

    // Difficulty values
    public string name;

    // Heat tracking
    public int heatTracked;

    // Gameplay settings
    public bool useDroneConstruction;
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
