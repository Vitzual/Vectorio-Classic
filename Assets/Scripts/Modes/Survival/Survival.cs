using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// This is a WIP rewrite script of Survival.cs
public class Survival : Gamemode
{
    // Attempts to get the BaseBuilding script from the building
    [Command]
    public override void CmdPlaceBuilding()
    {
        // Checks resources
    }

    // Updates the resources for all clients
    [ClientRpc]
    public void RpcPlaceBuilding(int gold, int power, int heat)
    {
        BuildingSystem.CmdCreateBuilding();
        Resource.Remove(Resource.Currency.Gold, gold);
        Resource.Remove(Resource.Currency.Power, power);
        Resource.Remove(Resource.Currency.Heat, heat);
    }
}
