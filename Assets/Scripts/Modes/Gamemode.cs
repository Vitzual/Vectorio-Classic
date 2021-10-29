using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Mirror;

// TODO:
// - Fix building creation in MenuButton and Hotbar (commented out)
// - Get resources properly connected to survival

public class Gamemode : MonoBehaviour
{
    // Gamemode information
    [Header("Gamemode Info")]
    public new string name;
    public string version;

    [Header("Gamemode Settings")]
    public bool useResources;
    public bool initBuildings;
    public bool initEnemies;
    public bool initGuardians;

    // Register for events
    public void Start()
    {
        Events.active.setupBuildables += InitEntities;
    }

    // Tells the gamemode how to generate inventory
    public void InitEntities()
    {
        ScriptableManager.GenerateAllScriptables();

        if (initBuildings) Inventory.active.GenerateEntities(ScriptableManager.buildings.ToArray());
        if (initGuardians) Inventory.active.GenerateEntities(ScriptableManager.enemies.ToArray());
        if (initGuardians) Inventory.active.GenerateEntities(ScriptableManager.guardians.ToArray());
        
        BuildingHandler.useResources = useResources;
    }
}
