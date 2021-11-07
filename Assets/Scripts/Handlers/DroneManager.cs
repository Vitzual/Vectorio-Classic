// This script handles all active drones each frame
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    // Get active instance
    public static DroneManager active;
    public void Awake() { active = this; }

    // Drone coverage dictionary
    public Dictionary<Vector2Int, List<Dronehub>> coverage;





    
}
