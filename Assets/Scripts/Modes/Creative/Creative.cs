    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creative : Gamemode
{
    public override void PlaceBuilding()
    {
        BuildingSystem.CmdCreateBuilding();
    }
}
