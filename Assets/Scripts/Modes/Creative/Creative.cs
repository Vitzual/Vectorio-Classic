    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creative : Gamemode
{
    public override void PlaceBuilding()
    {
        if (BuildingSystem.active != null)
            BuildingSystem.active.CmdCreateBuilding();
    }
}
