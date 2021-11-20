using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    // Building struct
    public struct BuildingData
    {
        public string id;
        public Tuple<int, int> coords;
        public float health;
        public int[] metadata;
        public string[] blueprintIDs;
    }
    
    // Enemy struct
    public struct EnemyData
    {
        public string id;
        public Tuple<int, int> coords;
        public float health;
        public int[] metadata;
        public string variantID;
    }

    // Difficulty struct
    public struct DifficultyData
    {
        public Dictionary<string, int> integerValues;
        public Dictionary<string, float> floatValues;
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
}
