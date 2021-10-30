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
}
