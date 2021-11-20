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
    public static bool loadGame;
    public static string loadLocation;

    // Gamemode information
    [Header("Gamemode Info")]
    public new string name;
    public static string worldName;
    public string version;
    public static Difficulty difficulty;
    public static string seed;
    public static float time;

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
        GameManager.SetupGame(difficulty, loadGame);
        InitGamemode();
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
