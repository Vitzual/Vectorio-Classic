using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //public int[][] Buildings;
    public int[,] Locations;
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
    public int[] UnlockProgress;

    public SaveData (Survival data, WaveSpawner heat) 
    {
        //Buildings = data.GetSaveData();
        WorldSeed = data.seed;
        Locations = data.GetLocationData();
        PowerUsage = data.PowerConsumption;
        PowerAvailable = data.AvailablePower;
        Gold = data.gold;
        Essence = data.essence;
        Iridium = data.iridium;
        HeatUsage = heat.htrack;
        UnlockLevel = data.UnlockLvl;
        UnlocksLeft = data.UnlocksLeft;
        UnlockProgress = data.GetAmountTracked();
    }
}
