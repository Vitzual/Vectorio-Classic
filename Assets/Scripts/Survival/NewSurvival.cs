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

    }
}
