using System.Collections.Generic;
using UnityEngine;

// List of all buildables
public class Buildable
{
    // Constructor
    public Buildable(Building building)
    {
        this.building = building;
        discount = 1f;
        resources = building.resources;
        obj = building.obj;
        isUnlocked = building.unlockable.unlocked;
        blueprintSlots = new CollectedBlueprint[building.engineeringSlots];

        Debug.Log("Registered " + building.name);
    }

    // Buildable variables
    public Building building;
    public float discount;
    public Cost[] resources;
    public GameObject obj;
    public bool isUnlocked;

    // Blueprints applied to this buildable
    public CollectedBlueprint[] blueprintSlots;

    // Update stats
    public void UpdateStats()
    {

    }

    // Applies a blueprint
    public bool ApplyBlueprint(CollectedBlueprint blueprint)
    {
        for (int i = 0; i < blueprintSlots.Length; i++)
        {
            if (blueprintSlots[i] == null)
            {
                blueprintSlots[i] = blueprint;
                UpdateStats();
                return true;
            }
        }
        return false;
    }
}