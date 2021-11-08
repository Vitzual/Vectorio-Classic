using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixerDrone : BaseDrone
{
    // Create the drone
    public void Start()
    {
        drone = Instantiate(_droneObj, transform.position, Quaternion.identity).GetComponent<Drone>();
        DroneManager.active.fixerDrones.Add(this);
        drone.type = Drone.Type.Fixer;
        drone.home = this;
    }
}
