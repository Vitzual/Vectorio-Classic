using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Buildables
{
    // List of all buildables
    public static Dictionary<Entity, Buildable> active;

    // List of all buildable unlockables
    public static Dictionary<Unlockable.UnlockType, List<Buildable>> unlockables;

    // Generate buildables
    public static void Register(Building building)
    {
        if (!active.ContainsKey(building))
        {
            Buildable newBuildable = new Buildable(building);
            active.Add(building, newBuildable);

            // Setup unlockable
            if (building.unlockable.unlocked || Gamemode.active.unlockEverything)
            {
                if (unlockables.ContainsKey(building.unlockable.type))
                    unlockables[building.unlockable.type].Add(newBuildable);
                else
                {
                    unlockables.Add(building.unlockable.type, new List<Buildable>());
                    unlockables[building.unlockable.type].Add(newBuildable);
                }
            }
        }
        else Debug.Log("There is already a buildable registered for " + building.name);
    }

    // Retrieves a buildable by name
    public static Buildable RequestBuildable(Entity entity)
    {
        if (active.ContainsKey(entity))
            return active[entity];
        Debug.Log("Could not retrieve object " + entity.name + " via entity request");
        return null;
    }

    // Retrieves a buildable by object
    public static Buildable RequestBuildable(BaseEntity obj)
    {
        foreach (KeyValuePair<Entity, Buildable> buildable in active)
            if (buildable.Value.obj == obj)
                return buildable.Value;
        Debug.Log("Could not retrieve object " + obj.name + " via object request");
        return null;
    }

    // Unlock buildable
    public static void UnlockBuildable(Buildable buildable)
    {
        buildable.isUnlocked = true;
        buildable.button.SetBuilding(buildable);

        if (unlockables.ContainsKey(buildable.unlockable.type))
            unlockables[buildable.unlockable.type].Remove(buildable);
    }

    // Update resource unlocks
    public static void UpdateResourceUnlockables(Resource.CurrencyType resourceType, int amount)
    {
        Unlockable.UnlockType unlockType = Unlockable.UnlockType.ReachResourceAmount;

        // Check if unlock is enabled
        if (Gamemode.active.unlockEverything ||
            !unlockables.ContainsKey(unlockType)) return;

        // Update all unlockables
        Unlockable unlockable;
        for (int i = 0; i < unlockables[unlockType].Count; i++)
        {
            unlockable = unlockables[unlockType][i].unlockable;
            if (unlockable.resource == resourceType)
            {
                unlockable.tracked = amount;
                if (unlockable.tracked >= unlockable.amount)
                    UnlockBuildable(unlockables[unlockType][i]);
            }
            else
            {
                // Update bar
            }
        }
    }

    // Update placement unlocks
    public static void UpdateEntityUnlockables(Unlockable.UnlockType unlockType, Entity entity, int amount)
    {
        // Check if unlock is enabled
        if (Gamemode.active.unlockEverything ||
            !unlockables.ContainsKey(unlockType)) return;

        // Update all unlockables
        Unlockable unlockable;
        for (int i = 0; i < unlockables[unlockType].Count; i++)
        {
            unlockable = unlockables[unlockType][i].unlockable;
            if (unlockable.entity == entity)
            {
                unlockable.tracked += amount;
                if (unlockable.tracked >= unlockable.amount)
                    UnlockBuildable(unlockables[unlockType][i]);
            }
            else
            {
                // Update bar
            }
        }
    }
}
