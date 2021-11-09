// This script handles all active drones each frame
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    // Get active instance
    public static DroneManager active;
    public void Awake() { active = this; }

    // List of all drone types
    public List<Drone> droneTypes;

    // Target lists
    public List<GhostTile> ghostTiles;
    public List<ResourceTile> resourceTiles;
    public List<BaseTile> damagedTiles;

    // Available drones 
    public List<Drone> hubDrones;
    public List<Drone> resourceDrones;
    public List<Drone> fixerDrones;

    // Drones actively moving
    public List<Drone> activeDrones;

    // Add a drone
    public void AddDrone(Drone drone)
    {
        if (drone.type == Drone.DroneType.Builder && drone.home.hubDrone)
            hubDrones.Add(drone);
        else if (drone.type == Drone.DroneType.Resource)
            resourceDrones.Add(drone);
        else if (drone.type == Drone.DroneType.Fixer)
            fixerDrones.Add(drone);
    }

    // Move drones
    public void Update()
    {
        UpdateConstructionDrones();
        UpdateActiveDrones();
    }

    // Update active drones
    public void UpdateActiveDrones()
    {
        for(int i = 0; i < activeDrones.Count; i++)
        {
            switch(activeDrones[i].stage)
            {
                // Reset drone
                case Drone.Stage.ReadyToDeploy:
                    active.AddDrone(activeDrones[i]);
                    activeDrones.RemoveAt(i);
                    break;

                // Open plates
                case Drone.Stage.ExitingPort:
                    activeDrones[i].ExitingPort();
                    break;

                // Move to target
                case Drone.Stage.MovingToTarget:
                    activeDrones[i].Move();
                    break;

                // Target reached
                case Drone.Stage.ReturningToPort:
                    activeDrones[i].Move();
                    break;

                // Entering port
                case Drone.Stage.EnteringPort:
                    activeDrones[i].EnteringPort();
                    break;
            }
        }
    }

    // Check construction drones
    public void UpdateConstructionDrones()
    {
        if (ghostTiles.Count > 0)
        {
            for (int a = 0; a < ghostTiles.Count; a++)
            {
                if (ghostTiles[a] != null)
                {
                    if (InstantiationHandler.active.CheckResources(ghostTiles[a].building))
                    {
                        // Check all active builder ports
                        for (int b = 0; b < ghostTiles[a].nearbyPorts.Count; b++)
                        {
                            // Get droneport
                            Drone drone = ghostTiles[a].nearbyPorts[b].drone;
                            if (drone.stage != Drone.Stage.ReadyToDeploy) continue;

                            // Set target
                            drone.SetTarget(ghostTiles[a]);
                            drone.ExitPort();

                            // Take resources
                            Resource.active.ApplyResources(ghostTiles[a].building);

                            // Update lsits
                            activeDrones.Add(drone);
                            ghostTiles.RemoveAt(a);

                            // End loop
                            return;
                        }

                        // If no builder ports available, default to hub ports
                        if (hubDrones.Count > 0)
                        {
                            // Set target
                            hubDrones[0].SetTarget(ghostTiles[a]);
                            hubDrones[0].ExitPort();

                            // Take resources
                            Resource.active.ApplyResources(ghostTiles[a].building);

                            // Update lsits
                            activeDrones.Add(hubDrones[0]);
                            ghostTiles.RemoveAt(a);
                            hubDrones.RemoveAt(0);

                            // End loop
                            return;
                        }
                    }
                }
                else
                {
                    ghostTiles.RemoveAt(a);
                    a--;
                }
            }
        }
    }

    // Return a drone
    public Drone GetDrone(Drone.DroneType type)
    {
        foreach (Drone drone in droneTypes)
            if (drone.type == type)
                return drone;
        return null;
    }

    // Attempts to return all nearby ghosts
    public void UpdateNearbyGhosts(Droneport port, Vector2 position)
    {
        // Loop through all nearby drone ports
        int adjustment = Research.drone_tile_coverage * 5;
        int xTile = (int)position.x;
        int yTile = (int)position.y;

        // Loop through all tiles and try to find drones
        for (int x = xTile - adjustment; x <= xTile + adjustment; x += 5)
        {
            for (int y = yTile - adjustment; y <= yTile + adjustment; y += 5)
            {
                BaseTile holder = InstantiationHandler.active.TryGetBuilding(new Vector2(x, y));
                if (holder != null)
                {
                    GhostTile ghost = holder.GetComponent<GhostTile>();
                    if (ghost != null) ghost.nearbyPorts.Add(port);
                }
            }
        }
    }

    // Attempts to return all nearby ports
    public List<Droneport> GetNearbyPorts(Vector2 position)
    {
        // Create new list
        List<Droneport> nearbyPorts = new List<Droneport>();

        // Loop through all nearby drone ports
        int adjustment = Research.drone_tile_coverage * 5;
        int xTile = (int)position.x;
        int yTile = (int)position.y;

        // Loop through all tiles and try to find drones
        for (int x = xTile - adjustment; x <= xTile + adjustment; x += 5)
        {
            for (int y = yTile - adjustment; y <= yTile + adjustment; y += 5)
            {
                BaseTile holder = InstantiationHandler.active.TryGetBuilding(new Vector2(x, y));
                if (holder != null)
                {
                    Droneport droneport = holder.GetComponent<Droneport>();
                    if (droneport != null) nearbyPorts.Add(droneport);
                }
            }
        }

        return nearbyPorts;
    }

    // Attempts to set targets for all nearby ports
    public void UpdateNearbyPorts(BaseTile tile, Vector2 position)
    {
        // Loop through all nearby drone ports
        int adjustment = Research.drone_tile_coverage * 5;
        int xTile = (int)position.x;
        int yTile = (int)position.y;

        // Loop through all tiles and try to find drones
        for (int x = xTile - adjustment; x <= xTile + adjustment; x += 5)
        {
            for (int y = yTile - adjustment; y <= yTile + adjustment; y += 5)
            {
                BaseTile holder = InstantiationHandler.active.TryGetBuilding(new Vector2(x, y));
                if (holder != null)
                {
                    Droneport droneport = holder.GetComponent<Droneport>();
                    if (droneport != null) droneport.AddTarget(tile);
                }
            }
        }
    }
}
