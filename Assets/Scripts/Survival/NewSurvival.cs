using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a WIP rewrite script of Survival.cs
public class NewSurvival : MonoBehaviour
{
    public void Start()
    {
        Events.active.onBuildingPlaced += BuildingPlaced;
    }

    public void BuildingPlaced(Transform building)
    {
        BaseBuilding script = building.GetComponent<BaseBuilding>();

        if (script != null)
        {
            Resource.Remove(Resource.Currency.Gold, script.building.cost);
            Resource.Remove(Resource.Currency.Power, script.building.power);
            Resource.Remove(Resource.Currency.Heat, script.building.heat);
        }
        else Debug.LogError("Could not retrieve script from " + transform.name);
    }
}
