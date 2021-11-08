using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixerDrone : Drone
{
    public override void AddTarget(BaseTile tile)
    {
        if (tile == null) return;
        else nearbyTargets.Add(tile);
    }
}
