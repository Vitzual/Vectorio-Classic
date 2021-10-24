    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creative : Gamemode
{
    public override void PlaceBuilding()
    {
        
    }

    public override void InitEntities()
    {
        ScriptableManager.GenerateAllScriptables();
        Inventory.active.GenerateEntities(ScriptableManager.buildings.ToArray());
        Inventory.active.GenerateEntities(ScriptableManager.enemies.ToArray());
        Inventory.active.GenerateEntities(ScriptableManager.guardians.ToArray());
    }
}
