using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Gamemode : NetworkBehaviour
{
    // Gamemode information
    public new string name;
    public string version;

    // Register for events
    public void Start()
    {
        Events.active.onBuildingPlaced += CmdPlaceBuilding;
    }

    // Attempts to get the BaseBuilding script from the building
    public virtual void CmdPlaceBuilding()
    {
        Debug.Log("Mode does not contain definition for building placed");
    }
}
