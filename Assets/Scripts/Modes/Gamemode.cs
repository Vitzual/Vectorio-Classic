using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
//using Mirror;

// TODO:
// - Fix building creation in MenuButton and Hotbar (commented out)
// - Get resources properly connected to survival

public class Gamemode : MonoBehaviour
{
    // Active instance
    public static Gamemode active;
    public static Stage stage;
    public static SaveData saveData;
    public static bool loadGame = false;

    // Static save variables
    public static string saveName = "Unnamed Save";
    public static string savePath = "/world_1.vectorio";
    public static DifficultyData difficulty;
    public static string seed = "Vectorio";
    public static float time = 0;

    // Gamemode information
    [Header("Gamemode Info")]
    public new string name;
    public string version;
    public Stage _stage;
    public Difficulty _difficulty;
    public float naturalHeatTimer = 1f;

    [Header("Gamemode Settings")]
    public bool naturalHeatGrowth;
    public bool useEnergizers;
    public bool useResources;
    public bool useDroneConstruction;
    public bool useEngineering;
    public bool spawnBlueprints;
    public bool generateWorld;
    public bool unlockEverything;
    public bool initBuildings;
    public bool initEnemies;
    public bool initGuardians;

    // Set active instance
    public void Awake()
    {
        // Get active instance
        active = this;
    }

    // Setup game
    public void Start()
    {
        // Set target frame rate
        Application.targetFrameRate = 999;

        // Generate all scriptables
        ScriptableLoader.GenerateAllScriptables();

        // Check difficulty variable
        if (difficulty == null)
        {
            Debug.Log("Difficulty data missing. Creating new one");
            difficulty = _difficulty.SetData(new DifficultyData());
        }

        // Check stage variable
        if (stage == null)
        {
            Debug.Log("Stage data missing. Setting to default");
            stage = _stage;
        }

        // Initialize gamemode
        InitGamemode();

        // Load game 
        if (loadGame && saveData != null)
        {
            NewSaveSystem.LoadGame(saveData);
            Border.UpdateStage();
            Events.active.ChangeBorderColor(stage.borderOutline, stage.borderFill);
        }

        // Setup starting resources
        SetupStartingResources();

        // Initialize research
        ResearchHandler.active.Setup();

        loadGame = false;
    }

    // Update playtime
    public void Update()
    {
        // Increment time
        time += Time.deltaTime;

        // Check heat growth
        if (naturalHeatGrowth)
        {
            naturalHeatTimer -= Time.deltaTime;
            if (naturalHeatTimer <= 0)
            {
                Resource.active.Add(Resource.CurrencyType.Heat, 1, false);
                difficulty.startingHeat += 1;
                naturalHeatTimer = 1f;
            }
        }
    }

    // Save game
    public void SaveGame()
    {
        NewSaveSystem.SaveGame(savePath);
    }

    // Tells the gamemode how to generate inventory
    public virtual void InitGamemode()
    {
        useDroneConstruction = !difficulty.enableInstaPlace;
        naturalHeatGrowth = difficulty.naturalHeatGrowth;

        #pragma warning disable CS0612
        if (!loadGame && generateWorld) WorldGenerator.active.GenerateWorldData(seed);
        #pragma warning restore CS0612
    }

    // Setup starting resources
    public void SetupStartingResources()
    {
        // Set heat
        Resource.active.SetStorage(Resource.CurrencyType.Heat, stage.heat);

        // Adjust power storage
        if (difficulty.startingPower == 0) Resource.active.SetStorage(Resource.CurrencyType.Power, 5000);
        else Resource.active.SetStorage(Resource.CurrencyType.Power, difficulty.startingPower);

        // Setup heat storage
        Resource.active.SetStorage(Resource.CurrencyType.Heat, stage.heat);
        Resource.active.Add(Resource.CurrencyType.Heat, difficulty.startingHeat, false);
    }
}
