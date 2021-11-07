using System.Collections.Generic;
using UnityEngine;

public class Drone
{
    // Stage of the current drone
    public enum Stage
    {
        ExitingPort,
        MovingToTarget,
        ReturningToPort,
        EnteringPort
    }
    public Stage stage;

    // Target locating variables
    public int searchRadius;

    // Target variables
    public BaseEntity target;
    public Sprite targetIcon;

    // Drone variables
    public float droneSpeed;
    public int droneVariable;

    // Home variables
    public Dronehub home;

    // Find target method
    public void FindTarget()
    {
        
    }

    // Reach target method
    public virtual void TargetReached()
    {
        stage = Stage.ReturningToPort;
        target = home;
    }
}
    
