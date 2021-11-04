using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Mirror;

// TODO:
// - Fix building creation in MenuButton and Hotbar (commented out)
// - Get resources properly connected to survival

public class Gamemode : MonoBehaviour
{
    // Active instance
    public static Gamemode active;

    // Gamemode information
    [Header("Gamemode Info")]
    public new string name;
    public string version;
    public Difficulty difficulty;

    [Header("Gamemode Settings")]
    public bool useResources;
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
        GameManager.SetupGame(difficulty);
        InitGamemode();
    }

    // Tells the gamemode how to generate inventory
    public void InitGamemode()
    {
        ScriptableManager.GenerateAllScriptables();

        if (initBuildings) Inventory.active.GenerateBuildings(ScriptableManager.buildings.ToArray());
        if (initGuardians) Inventory.active.GenerateEntities(ScriptableManager.enemies.ToArray());
        if (initGuardians) Inventory.active.GenerateEntities(ScriptableManager.guardians.ToArray());

        EnemyHandler.active.UpdateVariant();

        if (generateWorld) WorldGenerator.active.GenerateWorldData();
    }
}
