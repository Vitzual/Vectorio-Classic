using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DifficultyData
{
    public int startingGold;
    public int startingPower;
    public int enemyBases;
    public int[] goldSpawn;
    public int[] essenceSpawn;
    public int[] iridiumSpawn;
    public float defenseHP;
    public float enemyHP;
    public float enemyDMG;
    public float additionalCost;

    public DifficultyData(int startingGold, int startingPower, int enemyBases, int[] goldSpawn, int[] essenceSpawn, int[] iridiumSpawn, float defenseHP, float enemyHP, float enemyDMG, float additionalCost) 
    {
        this.startingGold = startingGold;
        this.startingPower = startingPower;
        this.enemyBases = enemyBases;
        this.goldSpawn = goldSpawn;
        this.essenceSpawn = essenceSpawn;
        this.iridiumSpawn = iridiumSpawn;
        this.defenseHP = defenseHP;
        this.enemyHP = enemyHP;
        this.enemyDMG = enemyDMG;
        this.additionalCost = additionalCost;
    }
}
