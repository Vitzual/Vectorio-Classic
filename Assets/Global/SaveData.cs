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

    public SaveData (Survival data, Technology unlock, WaveSpawner heat, Research research) 
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
        UnlockLevel = unlock.UnlockLvl;
        UnlocksLeft = unlock.UnlocksLeft;
        ResearchedTiers = research.GetResearchData();
    }
}
