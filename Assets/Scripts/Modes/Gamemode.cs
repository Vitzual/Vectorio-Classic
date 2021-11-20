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
    public static bool loadGame = false;

    // Gamemode information
    [Header("Gamemode Info")]
    public new string name;
    public static string saveName = "Unnamed Save";
    public static string savePath = "/world_1.vectorio";
    public string version;
    public Difficulty _difficulty;
    public static Difficulty difficulty;
    public static string seed = "Vectorio";
    public static float time = 0;

    [Header("Gamemode Settings")]
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
        if (difficulty == null) difficulty = _difficulty;
        GameManager.SetupGame(difficulty, loadGame);
        InitGamemode();

        if (loadGame) NewSaveSystem.LoadGame(savePath);
    }

    // Save game
    public void SaveGame()
    {
        NewSaveSystem.SaveGame(savePath);
    }

    // Update playtime
    public void Update()
    {
        time += Time.deltaTime;
    }

    // Tells the gamemode how to generate inventory
    public void InitGamemode()
    {
        ScriptableLoader.GenerateAllScriptables();
        EnemyHandler.active.UpdateVariant();

        #pragma warning disable CS0612
        if (!loadGame && generateWorld) WorldGenerator.active.GenerateWorldData(seed);
        #pragma warning restore CS0612
    }
}
