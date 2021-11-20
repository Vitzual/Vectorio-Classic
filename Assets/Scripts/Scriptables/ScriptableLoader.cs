using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Loads scriptables at runtime which can then be accesed from anywhere.

public static class ScriptableLoader
{
    // Resource paths
    public static string BuildingPath = "Scriptables/Buildings";
    public static string EnemyPath = "Scriptables/Enemies";
    public static string GuardianPath = "Scriptables/Guardians";
    public static string VariantPath = "Scriptables/Variants";

    public static List<Building> buildings;
    public static List<Enemy> enemies;
    public static List<Guardian> guardians;
    public static List<Variant> variants;

    // Generate all scriptables
    public static void GenerateAllScriptables()
    {
        Buildables.active = new Dictionary<Entity, Buildable>();

        GenerateBuildings();
        GenerateEnemies();
        GenerateGuardians();
        GenerateVariants();
    }

    // Generates buildings on run
    public static void GenerateBuildings()
    {
        buildings = Resources.LoadAll(BuildingPath, typeof(Building)).Cast<Building>().ToList();
        Debug.Log("Loading " + buildings.Count + " buildings from " + BuildingPath + "...");
        foreach (Building building in buildings)
        {
            Debug.Log("Loaded " + building.name + " with UUID " + building.InternalID);
            if (Gamemode.active.initBuildings)
                Buildables.Register(building);
        }
        if (Gamemode.active.initBuildings)
            Inventory.active.GenerateBuildings(buildings.ToArray());
        
        // Set requirements
        if (!Gamemode.active.unlockEverything)
            Buildables.HideUnmetRequirements();
    }

    // Generates enemies on run
    public static void GenerateEnemies()
    {
        enemies = Resources.LoadAll(EnemyPath, typeof(Enemy)).Cast<Enemy>().ToList();
        Debug.Log("Loaded " + enemies.Count + " enemies from " + EnemyPath);
        foreach (Enemy enemy in enemies)
            Debug.Log("Loaded " + enemy.name + " with UUID " + enemy.InternalID);
        if (Gamemode.active.initEnemies)
            Inventory.active.GenerateEntities(enemies.ToArray());
    }

    // Generates guardians on run
    public static void GenerateGuardians()
    {
        guardians = Resources.LoadAll(GuardianPath, typeof(Guardian)).Cast<Guardian>().ToList();
        Debug.Log("Loaded " + guardians.Count + " guardians from " + GuardianPath);
        foreach (Guardian guardian in guardians)
            Debug.Log("Loaded " + guardian.name + " with UUID " + guardian.InternalID);
        if (Gamemode.active.initGuardians)
            Inventory.active.GenerateEntities(guardians.ToArray());
    }

    // Generates guardians on run
    public static void GenerateVariants()
    {
        variants = Resources.LoadAll(VariantPath, typeof(Variant)).Cast<Variant>().ToList();
        Debug.Log("Loaded " + variants.Count + " guardians from " + VariantPath);
    }
}