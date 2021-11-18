using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Buildables
{
    // List of all buildables
    public static Dictionary<Entity, Buildable> active;

    // Generate buildables
    public static void Register(Building building)
    {
        if (!active.ContainsKey(building))
            active.Add(building, new Buildable(building));
        else Debug.Log("There is already a buildable registered for " + building.name);
    }

    // Retrieves a building object by name
    public static Buildable RequestBuildable(Entity entity)
    {
        if (active.ContainsKey(entity))
        {
            if (active[entity].isUnlocked)
            return active[entity];
        }
        Debug.Log("Could not retrieve object" + entity.name);
        return null;
    }
}
