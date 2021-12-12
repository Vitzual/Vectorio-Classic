using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Difficulty", menuName = "Difficulty")]
public class Difficulty : ScriptableObject
{
    [Header("Difficulty Settings")]
    public new string name;
    [TextArea] public string description;

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

    public DifficultyData SetData(DifficultyData difficulty)
    {
        // Gameplay modifiers
        difficulty.useDroneConstruction = useDroneConstruction;
        difficulty.naturalHeatGrowth = naturalHeatGrowth;

        // Gameplay settings
        difficulty.enemySpawnrateModifier = enemySpawnrateModifier;
        difficulty.enemyHealthModifier = enemyHealthModifier;
        difficulty.enemySpeedModifier = enemySpeedModifier;
        difficulty.enemyGroupSpawnrate = enemyGroupSpawnrate;
        difficulty.enemyGroupSpawnsize = enemyGroupSpawnsize;

        // Difficulty modifiers
        difficulty.goldSpawnModifier = goldSpawnModifier;
        difficulty.essenceSpawnModifier = essenceSpawnModifier;
        difficulty.iridiumSpawnModifier = iridiumSpawnModifier;
        difficulty.vectoriumSpawnModifier = vectoriumSpawnModifier;

        return difficulty;
    }
}
