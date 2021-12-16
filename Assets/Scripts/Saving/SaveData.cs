using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    // Building struct
    [Serializable]
    public class BuildingData
    {
        public string id;
        public string cosmetic_id;
        public float xCoord, yCoord;
        public float health;
        public int[] metadata;
        public string[] blueprintIDs;
        public bool ghostBuilding;
    }

    // Enemy struct
    [Serializable]
    public class EnemyData
    {
        public string id;
        public int xCoord, yCoord;
        public float health;
        public int[] metadata;
        public string variantID;
    }

    // Location data
    public BuildingData[] buildings;
    public EnemyData[] enemies;
    public string[] unlocked;
    public string[] research;
    public string[] hotbar;
    public string stage;

    // Difficulty data
    public DifficultyData difficultyData;

    // Online data
    public OnlineData onlineData;

    // Resource data
    public int gold;
    public int essence;
    public int iridium;

    // Save data
    public string worldName;
    public string worldMode;
    public string worldSeed;
    public string worldVersion;
    public float worldPlaytime;
    public float worldCompletion;
}
