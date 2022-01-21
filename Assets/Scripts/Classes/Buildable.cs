using System.Collections.Generic;
using UnityEngine;

// This is a class that holds runtime building data, such as
// level, cost modifiers, cosmetics, and so on. You don't really
// need to worry about this class; if you need a building to
// hold something at runtime, just add it here. 

public class Buildable
{
    // Turret parameterized constructor
    public Buildable(Turret turret)
    {
        this.turret = turret;
        building = turret;
        GenerateNewInstance();
    }

    // Parameterized constructor (create a new buildable instance)
    public Buildable(Building building)
    {
        turret = null;
        this.building = building;
        GenerateNewInstance();
    }

    // Generate new instance
    public void GenerateNewInstance()
    {
        // Grab available cosmetics
        availableCosmetics = new List<Cosmetic>();
        foreach (KeyValuePair<string, Cosmetic> cosmetic in ScriptableLoader.cosmetics)
            if (cosmetic.Value.building == building) availableCosmetics.Add(cosmetic.Value);

        // Set runtime variables to default
        obj = building.obj;
        unlockable = building.unlockable;
        isUnlocked = building.unlockable.unlocked || Gamemode.active.unlockEverything;

        // Set default resource costs
        resources = new Cost[building.resources.Length];
        for (int i = 0; i < resources.Length; i++)
        {
            resources[i] = new Cost();
            resources[i].type = building.resources[i].type;
            resources[i].storage = building.resources[i].storage;
            resources[i].amount = building.resources[i].amount;
        }

        // Generate new button list
        showButtons = new List<MenuButton>();

        // Set internal flags
        isCollector = obj.GetComponent<DefaultCollector>() != null;
        isStorage = obj.GetComponent<DefaultStorage>() != null;
        isDefense = obj.GetComponent<DefaultTurret>() != null;
        isDroneport = obj.GetComponent<Droneport>() != null;

        Debug.Log("Registered " + building.name + " buildable and linked to " + building.InternalID);
    }

    // Scriptable references
    public Building building;
    public Turret turret;
    public Cosmetic cosmetic;
    public List<Cosmetic> availableCosmetics;

    // Runtime variables
    public int level = 1;
    public int tracked = 0;
    public GameObject obj;
    public Unlockable unlockable;
    public bool isUnlocked;

    // Building variables
    public Cost[] resources;

    // Interface variables
    public MenuButton button;
    public List<MenuButton> showButtons;

    // Last positions
    public Vector2 lastBuildPos;
    public Vector2 lastDestroyPos;

    // Resource variables (THIS NEEDS TO BE CHANGED)
    public bool isCollector;
    public bool isStorage;
    public bool isDefense;
    public bool isDroneport;

    // Blueprints applied to this buildable
    public CollectedBlueprint[] blueprintSlots;
    
    // Apply cosmetic
    public void ApplyCosmetic(Cosmetic cosmetic)
    {
        if (cosmetic.building == building && cosmetic.validateLocalApplication()) this.cosmetic = cosmetic;
        else Debug.Log("Cannot apply " + cosmetic.name + " to " + building.name);
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

    // Get resource
    public virtual int GetResource(Resource.CurrencyType type)
    {
        foreach (Cost cost in resources)
            if (cost.type == type)
                return cost.amount;
        return 0;
    }

    // Update active amount
    public void UpdateActiveAmount(bool add, Vector2 pos)
    {
        if (add && pos != lastBuildPos)
        {
            lastBuildPos = pos;
            tracked += 1;

            if (lastBuildPos == lastDestroyPos)
                lastDestroyPos = Vector2.zero;
        }
        else if (!add && pos != lastDestroyPos)
        {
            lastDestroyPos = pos;
            tracked -= 1;

            if (tracked < 0) tracked = 0;

            if (lastDestroyPos == lastBuildPos)
                lastBuildPos = Vector2.zero;
        }
    }
}