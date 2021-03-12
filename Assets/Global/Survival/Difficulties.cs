using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulties : MonoBehaviour
{
    public int difficultyID = 2;

    [System.Serializable]
    public class GameDifficulties
    {
        public string name;
        public int startingGold;
        public int startingPower;
        public int enemyBases;
        public int goldSpawnFrequency;
        public int goldSpawnSize;
        public int goldSpawnNoise;
        public int essenceSpawnFrequency;
        public int essenceSpawnSize;
        public int essenceSpawnNoise;
        public int iridiumSpawnFrequency;
        public int iridiumSpawnSize;
        public int iridiumSpawnNoise;
        public float additionalDefenseHealth;
        public float additionalPlacementCost;
        public float additionalEnemyHP;
        public float additionalEnemyDMG;
    }

    public GameDifficulties[] difficulties;

    public void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public int[] GetGold()
    {
        int[] holder = new int[3];
        holder[0] = difficulties[difficultyID].goldSpawnFrequency;
        holder[1] = difficulties[difficultyID].goldSpawnSize;
        holder[2] = difficulties[difficultyID].goldSpawnNoise;
        return holder;
    }

    public void SetGold(int[] holder)
    {
        difficulties[difficultyID].goldSpawnFrequency = holder[0];
        difficulties[difficultyID].goldSpawnSize = holder[0];
        difficulties[difficultyID].goldSpawnNoise = holder[0];
    }

    public int[] GetEssence()
    {
        int[] holder = new int[3];
        holder[0] = difficulties[difficultyID].essenceSpawnFrequency;
        holder[1] = difficulties[difficultyID].essenceSpawnSize;
        holder[2] = difficulties[difficultyID].essenceSpawnNoise;
        return holder;
    }

    public void SetEssence(int[] holder)
    {
        difficulties[difficultyID].essenceSpawnFrequency = holder[0];
        difficulties[difficultyID].essenceSpawnSize = holder[0];
        difficulties[difficultyID].essenceSpawnNoise = holder[0];
    }

    public int[] GetIridium()
    {
        int[] holder = new int[3];
        holder[0] = difficulties[difficultyID].iridiumSpawnFrequency;
        holder[1] = difficulties[difficultyID].iridiumSpawnSize;
        holder[2] = difficulties[difficultyID].iridiumSpawnNoise;
        return holder;
    }

    public void SetIridium(int[] holder)
    {
        difficulties[difficultyID].iridiumSpawnFrequency = holder[0];
        difficulties[difficultyID].iridiumSpawnSize = holder[0];
        difficulties[difficultyID].iridiumSpawnNoise = holder[0];
    }

    public int GetBases()
    {
        return difficulties[difficultyID].enemyBases;
    }

    public void SetBases(int a)
    {
        difficulties[difficultyID].enemyBases = a;
    }

    public void SetDifficulty(int a)
    {
        difficultyID = a;
    }

    public int GetDifficulty()
    {
        return difficultyID;
    }

    public void SetStartingGold(int a)
    {
        difficulties[difficultyID].startingGold = a;
    }

    public int GetStartingGold()
    {
        return difficulties[difficultyID].startingGold;
    }

    public void SetStartingPower(int a)
    {
        difficulties[difficultyID].startingPower = a;
    }

    public int GetStartingPower()
    {
        return difficulties[difficultyID].startingPower;
    }

    public void SetDefenseHP(float a)
    {
        difficulties[difficultyID].additionalDefenseHealth = a;
    }

    public float GetDefenseHP()
    {
        return difficulties[difficultyID].additionalDefenseHealth;
    }

    public void SetEnemyHP(float a)
    {
        difficulties[difficultyID].additionalEnemyHP = a;
    }

    public float GetEnemyHP()
    {
        return difficulties[difficultyID].additionalEnemyHP;
    }

    public void SetEnemyDMG(float a)
    {
        difficulties[difficultyID].additionalEnemyDMG = a;
    }

    public float GetEnemyDMG()
    {
        return difficulties[difficultyID].additionalEnemyDMG;
    }

    public void SetAdditionalCost(float a)
    {
        difficulties[difficultyID].additionalPlacementCost = a;
    }

    public float GetAdditionalCost()
    {
        return difficulties[difficultyID].additionalPlacementCost;
    }

}
