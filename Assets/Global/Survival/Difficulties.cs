using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulties : MonoBehaviour
{
    public int difficultyID;
    public string savename = "UNNAMED SAVE";
    public string modename = "CUSTOM";

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
        if (SceneManager.GetActiveScene().name == "Menu")
            DontDestroyOnLoad(transform.gameObject);
    }

    public void SetSaveName(string a)
    {
        savename = a;
    }

    public void SetModeName(string a)
    {
        modename = a;
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
        difficulties[difficultyID].goldSpawnSize = holder[1];
        difficulties[difficultyID].goldSpawnNoise = holder[2];
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
        difficulties[difficultyID].essenceSpawnSize = holder[1];
        difficulties[difficultyID].essenceSpawnNoise = holder[2];
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
        difficulties[difficultyID].iridiumSpawnSize = holder[1];
        difficulties[difficultyID].iridiumSpawnNoise = holder[2];
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
