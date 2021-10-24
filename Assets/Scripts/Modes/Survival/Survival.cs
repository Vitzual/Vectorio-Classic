using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// This is a WIP rewrite script of Survival.cs
public class Survival : Gamemode
{
    public override void PlaceBuilding()
    {
        
    }

    public override void InitEntities()
    {
        ScriptableManager.GenerateBuildings();
    }
}
