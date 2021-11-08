using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    // Drone type
    public enum Type
    {
        Builder,
        Resource,
        Fixer
    }
    [HideInInspector] public Type type;

    // Stage of the current drone
    public enum Stage
    {
        ReadyToDeploy,
        ExitingPort,
        MovingToTarget,
        ReturningToPort,
        EnteringPort
    }
    [HideInInspector] public Stage stage;

    // Drone variables
    [HideInInspector] public BaseDrone home;
    [HideInInspector] public BaseEntity target;
    [HideInInspector] public float droneSpeed;
    public SpriteRenderer droneIcon;
    public SpriteRenderer targetIcon;
}
    
