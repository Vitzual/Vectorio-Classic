using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDrone : BaseDrone
{
    // Create the drone
    public void Start()
    {
        drone = Instantiate(_droneObj, transform.position, Quaternion.identity).GetComponent<Drone>();
        DroneManager.active.resourceDrones.Add(this);
        drone.type = Drone.Type.Resource;
        drone.home = this;
    }

    public override void AddTarget(BaseTile tile)
    {
        if (tile == null) return;
        else if (tile.GetComponent<ResourceTile>() != null) nearbyTargets.Add(tile);
    }
}
