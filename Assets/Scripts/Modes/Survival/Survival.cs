using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// This is a WIP rewrite script of Survival.cs
public class Survival : Gamemode
{
    public override void PlaceBuilding()
    {
        if (BuildingSystem.active != null)
        {
            BuildingSystem.active.CmdCreateBuilding();

            
        }
    }

    // Updates the resources for all clients
    public void RpcPlaceBuilding(Resource.Currency[] resources)
    {
        
    }
}
