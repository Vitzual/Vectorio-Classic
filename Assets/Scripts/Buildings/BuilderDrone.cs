using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderDrone : Drone
{


    public override void AddTarget(BaseTile tile)
    {
        if (tile == null) return;
        else if (tile.GetComponent<GhostTile>() != null) nearbyTargets.Add(tile);
    }

    public override void Destroy()
    {
        if (stage == Stage.ExitingPort || stage == Stage.MovingToTarget)
            Resource.active.RevertResources(target.GetComponent<GhostTile>().building);
        base.Destroy();
    }
}
