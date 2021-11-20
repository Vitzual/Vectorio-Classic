using System.Collections.Generic;
using UnityEngine;

// List of all buildables
public class Buildable
{
    // Constructor
    public Buildable(Building building)
    {
        this.building = building;
        unlockable = building.unlockable;
        discount = 1f;
        resources = building.resources;
        obj = building.obj;
        isUnlocked = building.unlockable.unlocked || Gamemode.active.unlockEverything;
        blueprintSlots = new CollectedBlueprint[building.engineeringSlots];

        Debug.Log("Registered " + building.name);
    }

    // Buildable variables
    public Building building;
    public MenuButton button;
    public Unlockable unlockable;
    public GameObject obj;
    public bool isUnlocked;

    // Building variables
    public float discount;
    public Cost[] resources;

    // Blueprints applied to this buildable
    public CollectedBlueprint[] blueprintSlots;
    
    // Update stats
    public void GetModifier(string name)
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
                Panel.active.SetPanel(building);
                return true;
            }
        }
        return false;
    }
}