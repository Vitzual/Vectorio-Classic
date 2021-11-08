using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderDrone : BaseDrone
{
    // Create the drone
    public void Start()
    {
        drone = Instantiate(_droneObj, transform.position, Quaternion.identity).GetComponent<Drone>();
        DroneManager.active.builderDrones.Add(this);
        drone.type = Drone.Type.Builder;
        drone.home = this;
    }
}
