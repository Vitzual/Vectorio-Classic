using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDrone : Drone
{
    
    public override void AddTarget(BaseTile tile)
    {
        if (tile == null) return;
        else if (tile.GetComponent<ResourceTile>() != null) nearbyTargets.Add(tile);
    }
}
