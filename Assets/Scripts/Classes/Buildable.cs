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
        blueprintSlots = new Blueprint[building.engineeringSlots];

        Debug.Log("Registered " + building.name);
    }

    public Building building;
    public float discount;
    public Cost[] resources;
    public GameObject obj;
    public bool isUnlocked;
    public Blueprint[] blueprintSlots;
}