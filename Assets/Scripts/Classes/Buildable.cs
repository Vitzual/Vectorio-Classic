using System.Collections.Generic;
using UnityEngine;

// List of all buildables
public class Buildable
{
    // Constructor
    public Buildable(Building building)
    {
        this.building = building;
        obj = building.obj;
        discount = 1f;

        showButtons = new List<MenuButton>();
        unlockable = building.unlockable;
        resources = building.resources;
        isUnlocked = building.unlockable.unlocked || Gamemode.active.unlockEverything;
        blueprintSlots = new CollectedBlueprint[building.engineeringSlots];

        isCollector = obj.GetComponent<DefaultCollector>() != null;
        isStorage = obj.GetComponent<DefaultStorage>() != null;

        Debug.Log("Registered " + building.name + " buildable and linked to " + building.InternalID);
    }

    // Buildable variables
    public Building building;
    public MenuButton button;
    public GameObject obj;
    public List<MenuButton> showButtons;
    public Unlockable unlockable;
    public int tracked;
    public bool isUnlocked;

    // Resource variables (THIS NEEDS TO BE CHANGED)
    public bool isCollector;
    public bool isStorage;

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