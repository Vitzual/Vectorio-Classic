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
    public static List<Building> buildings;
    public static List<Enemy> enemies;
    public static List<Guardian> guardians;
    public static List<Variant> variants;

    // Generate all scriptables
    public static void GenerateAllScriptables()
    {
        GenerateBuildings();
        GenerateEnemies();
        GenerateGuardians();
        GenerateVariants();
    }

    // Generates buildings on run
    public static void GenerateBuildings()
    {
        buildings = new List<Building>();
        buildings = Resources.LoadAll(BuildingPath, typeof(Building)).Cast<Building>().ToList();
        Debug.Log("Loaded " + buildings.Count + " buildings from " + BuildingPath);
    }

    // Generates enemies on run
    public static void GenerateEnemies()
    {
        enemies = new List<Enemy>();
        enemies = Resources.LoadAll(EnemyPath, typeof(Enemy)).Cast<Enemy>().ToList();
        Debug.Log("Loaded " + enemies.Count + " enemies from " + EnemyPath);
    }

    // Generates guardians on run
    public static void GenerateGuardians()
    {
        guardians = new List<Guardian>();
        guardians = Resources.LoadAll(GuardianPath, typeof(Guardian)).Cast<Guardian>().ToList();
        Debug.Log("Loaded " + guardians.Count + " guardians from " + GuardianPath);
    }

    // Generates guardians on run
    public static void GenerateVariants()
    {
        variants = new List<Variant>();
        variants = Resources.LoadAll(VariantPath, typeof(Variant)).Cast<Variant>().ToList();
        Debug.Log("Loaded " + variants.Count + " guardians from " + VariantPath);
    }

    // Retrieves an enemy or guardian by name
    public static GameObject RequestEnemyByName(string name)
    {
        foreach (Enemy enemy in enemies)
            if (enemy.name == name) return enemy.obj;
        foreach (Guardian guardian in guardians)
            if (guardian.name == name) return guardian.obj;
        Debug.Log("Could not retrieve object with name " + name);
        return null;
    }

    // Retrieves a building object by name
    public static GameObject RequestObjectByName(string name)
    {
        foreach (Building building in buildings)
            if (building.name == name) return building.obj;
        Debug.Log("Could not retrieve object with name " + name);
        return null;
    }

    // Retrieves a tile scriptable by name
    public static Building RequestBuildingByName(string name)
    {
        foreach (Building building in buildings)
            if (building.name == name) return building;
        Debug.Log("Could not retrieve scriptable with name " + name);
        return null;
    }
}