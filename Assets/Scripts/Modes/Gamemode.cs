using UnityEngine;
using System.Collections.Generic;
using System.Linq;
//using Mirror;

// TODO:
// - Fix building creation in MenuButton and Hotbar (commented out)
// - Get resources properly connected to survival

public class Gamemode : MonoBehaviour
{
    // Active instance
    public static Gamemode active;
    public static SaveData saveData;
    public static bool loadGame = false;
    public bool isMenu;

    // Gamemode information
    [Header("Gamemode Info")]
    public new string name;
    public static string saveName = "Unnamed Save";
    public static string savePath = "/world_1.vectorio";
    public string version;
    public Difficulty _difficulty;
    public static DifficultyData difficulty;
    public static string seed = "Vectorio";
    public static float time = 0;
    private static float heatTimer = 1f;

    [Header("Gamemode Settings")]
    public bool naturalHeatGrowth;
    public bool useEnergizers;
    public bool useResources;
    public bool useHeat; 
    public bool usePower;
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
        active = this;
    }

    // Setup game
    public void Start()
    {
        if (isMenu) return;

        Application.targetFrameRate = 999;

        if (difficulty == null)
        {
            Debug.Log("Difficulty data missing. Creating new one");
            difficulty = _difficulty.SetData(new DifficultyData());
        }
        GameManager.SetupGame(difficulty, loadGame);

        useDroneConstruction = !difficulty.enableInstaPlace;
        naturalHeatGrowth = difficulty.naturalHeatGrowth;

        InitGamemode();

        if (loadGame)
        {
            Resource.active.AddStorage(Resource.CurrencyType.Power, 5000);
            if (saveData != null) NewSaveSystem.LoadGame(saveData);
            else Debug.Log("Save data could not be passed!");
        }

        loadGame = false;
    }

    // Update playtime
    public void Update()
    {
        if (isMenu) return;

        // Increment time
        time += Time.deltaTime;

        // Check heat growth
        if (naturalHeatGrowth)
        {
            heatTimer -= Time.deltaTime;
            if (heatTimer <= 0)
            {
                Resource.active.Add(Resource.CurrencyType.Heat, 1);
                heatTimer = 1f;
            }
        }
    }

    // Save game
    public void SaveGame()
    {
        if (isMenu) return;
        NewSaveSystem.SaveGame(savePath);
    }

    // Tells the gamemode how to generate inventory
    public void InitGamemode()
    {
        if (isMenu) return;

        ScriptableLoader.GenerateAllScriptables();
        EnemyHandler.active.UpdateVariant();

        #pragma warning disable CS0612
        if (!loadGame && generateWorld) WorldGenerator.active.GenerateWorldData(seed);
        #pragma warning restore CS0612
    }
}
