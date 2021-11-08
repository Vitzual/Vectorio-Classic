// This script handles all active drones each frame
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    // Get active instance
    public static DroneManager active;
    public void Awake() { active = this; }

    // Target lists
    public List<GhostTile> ghostTiles;
    public List<ResourceTile> resourceTiles;
    public List<BaseTile> damagedTiles;

    // Available drones 
    public List<BuilderDrone> builderDrones;
    public List<ResourceDrone> resourceDrones;
    public List<FixerDrone> fixerDrones;

    // Drones actively moving
    public List<Drone> activeDrones;

    // Move drones
    public void Start()
    {
        UpdateConstructionDrones();
    }
    
    // Check construction drones
    public void UpdateConstructionDrones()
    {






        /*
        if (ghostTiles.Count > 0 && constructionDrones.Count > 0)
            for (int a = 0; a < ghostTiles.Count; a++)
                if (InstantiationHandler.active.CheckResources(ghostTiles[a].building))
                    for (int b = 0; b < constructionDrones.Count; b++)
                        if (constructionDrones[b].home.nearbyTargets.Contains(ghostTiles[a]))
                        {
                            SetConstructionDroneTarget(constructionDrones[b], ghostTiles[a]);
                            a--; b--;
                        }
        */
    }

}
