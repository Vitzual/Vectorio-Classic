using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survival : Gamemode
{
    // Hub object
    public Building hub;

    // Instantiate hub
    public override void Setup()
    {
        Buildable hubBuildable = Buildables.RequestBuildable(hub);
        InstantiationHandler.active.RpcInstantiateBuilding(hubBuildable, Vector2.zero, Quaternion.identity);
        base.Setup();
    }
}
