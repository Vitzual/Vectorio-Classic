using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulties : MonoBehaviour
{
    [System.Serializable]
    public class GameDifficulties
    {
        public string name;
        public int startingGold;
        public int startingPower;
        public float mapSize;
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
        public float additionalEnemyHealth;
        public float additionalPlacementCost;
        public float additionalEnemyHP;
        public float additionalEnemyDMG;
    }

    public GameDifficulties[] difficulties;
}
