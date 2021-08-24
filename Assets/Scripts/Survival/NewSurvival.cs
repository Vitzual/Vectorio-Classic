using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// This is a WIP rewrite script of Survival.cs
public class NewSurvival : NetworkBehaviour
{
    public void Start()
    {
        Events.active.onBuildingPlaced += CmdBuildingPlaced;
        Events.active.onSurvivalLoaded += LoadResources;
    }

    // Gets called on load. Applies resources for all clients
    [ClientRpc]
    public void LoadResources(SurvivalData data)
    {
        Resource.Add(Resource.Currency.Gold, data.gold);
        Resource.Add(Resource.Currency.Essence, data.essence);
        Resource.Add(Resource.Currency.Iridium, data.iridium);
        Resource.Add(Resource.Currency.Power, data.power);
        Resource.Add(Resource.Currency.Heat, data.heat);
    }

    // Attempts to get the BaseBuilding script from the building
    [Command]
    public void CmdBuildingPlaced(Transform building)
    {
        BaseBuilding script = building.GetComponent<BaseBuilding>();

        // Update resources for all clients
        if (script != null) RpcBuildingPlaced(script.building.cost, script.building.power, script.building.heat);
        else Debug.LogError("Could not retrieve script from " + transform.name);
    }

    // Updates the resources for all clients
    [ClientRpc]
    public void RpcBuildingPlaced(int gold, int power, int heat)
    {
        Resource.Remove(Resource.Currency.Gold, gold);
        Resource.Remove(Resource.Currency.Power, power);
        Resource.Remove(Resource.Currency.Heat, heat);
    }
}
