// This script handles all active drones each frame
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    // Get active instance
    public static DroneManager active;
    public void Awake() { active = this; }

    // Active drones
    public List<Drone> drones;

    // Move drones
    public void Start()
    {
        for (int i = 0; i < drones.Count; i++)
        {

        }
    }
}
