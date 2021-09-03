    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creative : Gamemode
{
    public override void PlaceBuilding()
    {
        BuildingSystem.CmdCreateBuilding();
    }

    public override void LoadScriptables(List<Building> tiles)
    {
        // Load the scriptables
    }
}