using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    // Stage of the current drone
    public enum Stage
    {
        ReadyToDeploy,
        ExitingPort,
        MovingToTarget,
        ReturningToPort,
        EnteringPort
    }
    public Stage stage;

    // Drone variables
    public Dronehub home;
    public BaseEntity target;
    public SpriteRenderer droneIcon;
    public SpriteRenderer targetIcon;
    public float droneSpeed;
    public int droneVariable;

    // Setup drone
    public void SetupDrone()
    {
        if (home.droneType == Dronehub.DroneType.Construction)
            droneIcon.sprite = Sprites.GetSprite("Construction Drone");
        else if (home.droneType == Dronehub.DroneType.Resource)
            droneIcon.sprite = Sprites.GetSprite("Resource Drone");
        else if (home.droneType == Dronehub.DroneType.Fixer)
            droneIcon.sprite = Sprites.GetSprite("Fixer Drone");
        
        ResetDrone();
    }

    // Resets a drone
    public void ResetDrone()
    {
        transform.position = home.transform.position;
        droneIcon.sortingOrder = 0;
        stage = Stage.ReadyToDeploy;
    }

    // Reach target method
    public virtual void TargetReached()
    {
        stage = Stage.ReturningToPort;
        target = home;
    }
}
    
