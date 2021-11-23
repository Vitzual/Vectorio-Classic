using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Buildables
{
    // List of all buildables
    public static Dictionary<Entity, Buildable> active = new Dictionary<Entity, Buildable>();

    // List of all buildable unlockables
    public static Dictionary<Unlockable.UnlockType, List<Buildable>> unlockables =
              new Dictionary<Unlockable.UnlockType, List<Buildable>>();

    // Reset buildables
    public static void ClearRegistry()
    {
        Debug.Log("Building registry cleared");
        active = new Dictionary<Entity, Buildable>();
        unlockables = new Dictionary<Unlockable.UnlockType, List<Buildable>>();
    }

    // Generate buildables
    public static void Register(Building building)
    {
        if (!active.ContainsKey(building))
        {
            Buildable newBuildable = new Buildable(building);
            active.Add(building, newBuildable);

            // Setup unlockable
            if (!building.unlockable.unlocked && !Gamemode.active.unlockEverything)
            {
                if (unlockables.ContainsKey(building.unlockable.type))
                {
                    Debug.Log("Setting " + building.name + " status to locked with requirement: [" + building.unlockable.amount + "]: " + building.unlockable.description);
                    unlockables[building.unlockable.type].Add(newBuildable);
                }
                else
                {
                    Debug.Log("Setting " + building.name + " status to locked with requirement [" + building.unlockable.amount + "]: " + building.unlockable.description);
                    unlockables.Add(building.unlockable.type, new List<Buildable>());
                    unlockables[building.unlockable.type].Add(newBuildable);
                }
            }
        }
        else Debug.Log("There is already a buildable registered for " + building.name);
    }

    // Iterate over dictionary and disable buttons that have requirements
    public static void HideUnmetRequirements()
    {
        foreach (KeyValuePair<Entity, Buildable> buildable in active)
        {
            Buildable requirement = RequestBuildable(buildable.Value.building.unlockable.requirement);

            if (requirement != null && !requirement.unlockable.unlocked)
            {
                buildable.Value.button.gameObject.SetActive(false);
                requirement.showButtons.Add(buildable.Value.button);
            }
        }
    }

    // Retrieves a buildable by name
    public static Buildable RequestBuildable(Entity entity)
    {
        if (entity == null) return null;

        if (active.ContainsKey(entity))
            return active[entity];
        Debug.Log("Could not retrieve object " + entity.name + " via entity request");
        return null;
    }

    // Retrieves a buildable by object
    public static Buildable RequestBuildable(string name)
    {
        foreach (KeyValuePair<Entity, Buildable> buildable in active)
            if (buildable.Value.building.name == name)
                return buildable.Value;
        Debug.Log("Could not retrieve object " + name + " via name request");
        return null;
    }

    // Unlock buildable
    public static void UnlockBuildable(Buildable buildable)
    {
        buildable.isUnlocked = true;
        buildable.button.SetBuilding(buildable);

        // Remove from unlockable list
        if (unlockables.ContainsKey(buildable.unlockable.type))
            unlockables[buildable.unlockable.type].Remove(buildable);

        // Show requirements
        foreach (MenuButton button in buildable.showButtons)
            button.gameObject.SetActive(true);

        // Display notification
        Events.active.BuildingUnlocked(buildable);
    }

    // Update resource unlocks
    public static void UpdateResourceUnlockables(Resource.CurrencyType resourceType)
    {
        // Get unlock type
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
                unlockables[unlockType][i].tracked = Resource.active.currencies[resourceType].amount;
                if (unlockables[unlockType][i].tracked >= unlockable.amount)
                {
                    UnlockBuildable(unlockables[unlockType][i]);
                }
                else
                {
                    unlockables[unlockType][i].button.progress.currentPercent = (float)unlockables[unlockType][i].tracked / (float)unlockable.amount;
                    unlockables[unlockType][i].button.progress.UpdateUI();
                }
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
                unlockables[unlockType][i].tracked += amount;
                if (unlockables[unlockType][i].tracked >= unlockable.amount)
                {
                    UnlockBuildable(unlockables[unlockType][i]);
                }
                else
                {
                    unlockables[unlockType][i].button.progress.currentPercent = (float)unlockables[unlockType][i].tracked / (float)unlockable.amount;
                    unlockables[unlockType][i].button.progress.UpdateUI();
                }
            }
        }
    }
}
