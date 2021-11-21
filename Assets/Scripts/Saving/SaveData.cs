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
        public int xCoord, yCoord;
        public float health;
        public int[] metadata;
        public string[] blueprintIDs;
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
    public string[] hotbar;
    public Border.Stage stage;

    // Difficulty data
    public DifficultyData difficultyData; 

    // Resource data
    public Dictionary<Resource.CurrencyType, int> resources;

    // Save data
    public string worldName;
    public string worldMode;
    public string worldSeed;
    public string worldVersion;
    public float worldPlaytime;
    public float worldCompletion;
}
