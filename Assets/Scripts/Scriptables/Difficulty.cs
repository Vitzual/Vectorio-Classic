using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Difficulty", menuName = "Difficulty")]
public class Difficulty : ScriptableObject
{
    [Header("Difficulty Settings")]
    public new string name;
    [TextArea] public string description;

    [Header("World Settings")]
    public float goldSpawnModifier;
    public float essenceSpawnModifier;
    public float iridiumSpawnModifier;

    [Header("Gameplay Modifiers")]
    public bool enableInstaPlace;
    public bool naturalHeatGrowth;

    [Header("Starting Resources")]
    public int startingGold;
    public int startingEssence;
    public int startingIridium;
    public int startingPower;
    public int startingHeat;

    [Header("Building Difficulty")]
    public float buildingCostModifier;
    public float buildingHealthModifier;
    public float buildingDamageModifier;

    [Header("Enemy Difficulty")]
    public float enemyHealthModifier;
    public float enemyDamageModifier;
    public float enemySpeedModifier;
    public float enemySpawnrateModifier;

    public DifficultyData SetData(DifficultyData difficulty)
    {
        difficulty.name = name;
        difficulty.description = description;

        difficulty.goldSpawnModifier = goldSpawnModifier;
        difficulty.essenceSpawnModifier = essenceSpawnModifier;
        difficulty.iridiumSpawnModifier = iridiumSpawnModifier;

        difficulty.startingGold = startingGold;
        difficulty.startingEssence = startingEssence;
        difficulty.startingIridium = startingIridium;
        difficulty.startingPower = startingPower;
        difficulty.startingHeat = startingHeat;

        difficulty.buildingCostModifier = buildingCostModifier;
        difficulty.buildingHealthModifier = buildingHealthModifier;
        difficulty.buildingDamageModifier = buildingDamageModifier;

        difficulty.enemyHealthModifier = enemyHealthModifier;
        difficulty.enemyDamageModifier = enemyDamageModifier;
        difficulty.enemySpeedModifier = enemySpeedModifier;
        difficulty.enemySpawnrateModifier = enemySpawnrateModifier;

        return difficulty;
    }
}
