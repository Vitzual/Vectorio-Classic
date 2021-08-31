using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// This is a WIP rewrite script of Survival.cs
public class Survival : Gamemode
{
    // Attempts to get the BaseBuilding script from the building
    public override void PlaceBuilding()
    {
        // Checks resources
    }

    // Updates the resources for all clients
    public void RpcPlaceBuilding(int gold, int power, int heat)
    {
        BuildingSystem.CmdCreateBuilding();
        Resource.Remove(Resource.Currency.Gold, gold);
        Resource.Remove(Resource.Currency.Power, power);
        Resource.Remove(Resource.Currency.Heat, heat);
    }
}
