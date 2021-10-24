using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// This is a WIP rewrite script of Survival.cs
public class Survival : Gamemode
{
    public override void PlaceBuilding()
    {
        if (BuildingHandler.active != null)
            BuildingHandler.active.CmdCreateBuilding();
    }

    public override void InitEntities()
    {
        Events.active.InitBuildables("Scriptables/Buildings");
    }
}
