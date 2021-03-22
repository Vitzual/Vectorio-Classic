using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //public int[][] Buildings;
    public int[,] Locations;
    public float[,] Enemies;
    public int PowerUsage;
    public int PowerAvailable;
    public int Gold;
    public int Essence;
    public int Iridium;
    public int HeatUsage;
    public int UnlockLevel;
    public int WorldSeed;
    public bool UnlocksLeft;
    public int RLevel;
    public bool[] ResearchedTiers;
    public int Difficulty;
    public int startingGold;
    public int startingPower;
    public float defenseHP;
    public float enemyHP;
    public float enemyDMG;
    public float additionalCost;
    public int[] goldSpawn;
    public int[] essenceSpawn;
    public int[] iridiumSpawn;
    public int enemyBases;

    public SaveData (Survival data, Technology unlock, WaveSpawner heat, Research research, Difficulties difficulty) 
    {
        //Buildings = data.GetSaveData();
        WorldSeed = data.seed;
        Locations = data.GetLocationData();
        Enemies = data.GetEnemyData();
        PowerUsage = data.PowerConsumption;
        PowerAvailable = data.AvailablePower;
        Gold = data.gold;
        Essence = data.essence;
        Iridium = data.iridium; 
        HeatUsage = heat.htrack;
        UnlockLevel = unlock.UnlockAmount;
        UnlocksLeft = unlock.UnlocksLeft;
        ResearchedTiers = research.GetResearchData();
        startingGold = difficulty.GetStartingGold();
        startingPower = difficulty.GetStartingPower();
        defenseHP = difficulty.GetDefenseHP();
        enemyHP = difficulty.GetEnemyHP();
        enemyDMG = difficulty.GetEnemyDMG();
        additionalCost = difficulty.GetAdditionalCost();
        goldSpawn = difficulty.GetGold();
        essenceSpawn = difficulty.GetEssence();
        iridiumSpawn = difficulty.GetIridium();
        enemyBases = difficulty.GetBases();
}
}
