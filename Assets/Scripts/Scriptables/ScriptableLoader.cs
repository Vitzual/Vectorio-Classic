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

    public static Dictionary<string, Building> buildings;
    public static Dictionary<string, Enemy> enemies;
    public static Dictionary<string, Guardian> guardians;
    public static Dictionary<string, Variant> variants;

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
        buildings = new Dictionary<string, Building>();
        List<Building> loaded = Resources.LoadAll(BuildingPath, typeof(Building)).Cast<Building>().ToList();

        Debug.Log("Loading " + loaded.Count + " buildings from " + BuildingPath + "...");
        foreach (Building building in loaded)
        {
            buildings.Add(building.InternalID, building);
            Debug.Log("Loaded " + building.name + " with UUID " + building.InternalID);
            if (Gamemode.active.initBuildings)
                Buildables.Register(building);
        }
        if (Gamemode.active.initBuildings)
            Inventory.active.GenerateBuildings(loaded.ToArray());
        
        // Set requirements
        if (!Gamemode.active.unlockEverything)
            Buildables.HideUnmetRequirements();
    }

    // Generates enemies on run
    public static void GenerateEnemies()
    {
        enemies = new Dictionary<string, Enemy>();
        List<Enemy> loaded = Resources.LoadAll(EnemyPath, typeof(Enemy)).Cast<Enemy>().ToList();

        Debug.Log("Loaded " + loaded.Count + " enemies from " + EnemyPath);
        foreach (Enemy enemy in loaded)
        {
            enemies.Add(enemy.InternalID, enemy);
            Debug.Log("Loaded " + enemy.name + " with UUID " + enemy.InternalID);
        }
        if (Gamemode.active.initEnemies)
            Inventory.active.GenerateEntities(loaded.ToArray());
    }

    // Generates guardians on run
    public static void GenerateGuardians()
    {
        guardians = new Dictionary<string, Guardian>();
        List<Guardian> loaded = Resources.LoadAll(GuardianPath, typeof(Guardian)).Cast<Guardian>().ToList();

        Debug.Log("Loaded " + loaded.Count + " guardians from " + GuardianPath);
        foreach (Guardian guardian in loaded)
        {
            guardians.Add(guardian.InternalID, guardian);
            Debug.Log("Loaded " + guardian.name + " with UUID " + guardian.InternalID);
        }
        if (Gamemode.active.initGuardians)
            Inventory.active.GenerateEntities(loaded.ToArray());
    }

    // Generates guardians on run
    public static void GenerateVariants()
    {
        variants = new Dictionary<string, Variant>();
        List<Variant> loaded = Resources.LoadAll(VariantPath, typeof(Variant)).Cast<Variant>().ToList();
        Debug.Log("Loaded " + loaded.Count + " variants from " + VariantPath);

        foreach (Variant variant in loaded)
        {
            variants.Add(variant.InternalID, variant);
            Debug.Log("Loaded " + variant.name + " with UUID " + variant.InternalID);
        }
    }
}