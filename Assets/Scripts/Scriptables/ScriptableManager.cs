using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Loads scriptables at runtime which can then be accesed from anywhere.

public static class ScriptableManager
{
    // Resource paths
    public static string BuildingPath = "Scriptables/Buildings";
    public static string EnemyPath = "Scriptables/Enemies";
    public static string GuardianPath = "Scriptables/Guardian";
    public static string VariantPath = "Scriptables/Variants";

    // Scriptable lists
    public static List<Building> buildings = new List<Building>();
    public static List<Enemy> enemies = new List<Enemy>();
    public static List<Guardian> guardians = new List<Guardian>();
    public static List<Variant> variants = new List<Variant>();

    // Generates buildings on run
    public static void GenerateBuildings()
    {
        // Load buildings
        buildings = Resources.LoadAll(BuildingPath, typeof(Building)).Cast<Building>().ToList();
        Debug.Log("Loaded " + buildings.Count + " buildings from " + BuildingPath);
        Events.active.InitBuildables(BuildingPath);
    }

    // Generates enemies on run
    public static void GenerateEnemies()
    {
        // Load buildings
        enemies = Resources.LoadAll(EnemyPath, typeof(Enemy)).Cast<Enemy>().ToList();
        Debug.Log("Loaded " + enemies.Count + " enemies from " + EnemyPath);
        Events.active.InitBuildables(EnemyPath);
    }

    // Generates guardians on run
    public static void GenerateGuardians()
    {
        // Load buildings
        buildings = Resources.LoadAll(GuardianPath, typeof(Building)).Cast<Building>().ToList();
        Debug.Log("Loaded " + buildings.Count + " guardians from " + GuardianPath);
        Events.active.InitBuildables(GuardianPath);
    }

    // Retrieves a building object by name
    public static GameObject RequestBuildingByName(string name)
    {
        foreach (Building building in buildings)
            if (building.name == name) return building.obj;
        Debug.Log("Could not retrieve object with name " + name);
        return null;
    }

    // Retrieves a tile scriptable by name
    public static Building RequestTileByName(string name)
    {
        foreach (Building building in buildings)
            if (building.name == name) return building;
        Debug.Log("Could not retrieve scriptable with name " + name);
        return null;
    }
}