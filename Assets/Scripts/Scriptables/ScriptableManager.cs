using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Loads scriptables at runtime which can then be accesed from anywhere.

public static class ScriptableManager
{
    // Resource paths
    public static string BuildingPath = "Scriptables/Buildings";
    public static string EnemyPath = "Scriptables/Enemies";

    // Scriptable lists
    public static List<Building> buildings = new List<Building>();

    // Generates buildings on run
    public static void GenerateBuildings()
    {
        // Load buildings
        buildings = Resources.LoadAll(BuildingPath, typeof(Building)).Cast<Building>().ToList();
        Debug.Log("Loaded " + buildings.Count + " buildings from " + BuildingPath);
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