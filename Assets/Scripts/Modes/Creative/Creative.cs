    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creative : Gamemode
{
    public override void PlaceBuilding()
    {
        if (BuildingHandler.active != null)
            BuildingHandler.active.CmdCreateBuilding();
    }

    public override void InitEntities()
    {
        Events.active.InitBuildables("Scriptables/Buildings");
        Events.active.InitBuildables("Scriptables/Enemies");
        Events.active.InitBuildables("Scriptables/Guardians");
    }
}
