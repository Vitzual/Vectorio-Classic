using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dronehub : BaseTile
{
    // Holds drone type
    public enum DroneType
    {
        Construction,
        Resource,
        Fixer
    }
    public DroneType droneType;
    public Drone drone;

    // Nearby targets
    public List<BaseEntity> nearbyTargets;

    // Lights
    public GameObject BlueLight;
    public GameObject YellowLight;
    public GameObject GreenLight;

    // Side panels 
    public Transform leftPanel;
    public Transform rightPanel;

    // Apply metadata 
    public override void ApplyMetadata(int data)
    {
        if (data == 0) SetDroneType(DroneType.Construction);
        else if (data == 1) SetDroneType(DroneType.Resource);
        else if (data == 2) SetDroneType(DroneType.Fixer);
    }

    // Set drone type
    public void SetDroneType(DroneType type)
    {
        droneType = type;
        drone.SetupDrone();

        if (droneType == DroneType.Construction)
        {
            BlueLight.SetActive(true);
            YellowLight.SetActive(false);
            GreenLight.SetActive(false);
        }
        else if (droneType == DroneType.Resource)
        {
            BlueLight.SetActive(false);
            YellowLight.SetActive(true);
            GreenLight.SetActive(false);
        }
        else if (droneType == DroneType.Fixer)
        {
            BlueLight.SetActive(false);
            YellowLight.SetActive(false);
            GreenLight.SetActive(true);
        }
    }

    // Locate nearby buildings for drone
    public override void Setup()
    {
        // Loop through all nearby drone ports
        int adjustment = Research.research_drone_coverage * 5;
        int xTile = (int)transform.position.x;
        int yTile = (int)transform.position.y;

        // Loop through all tiles and try to find drones
        for (int x = xTile - adjustment; x <= xTile + adjustment; x += 5)
            for (int y = yTile - adjustment; y <= yTile + adjustment; y += 5)
                AddTarget(InstantiationHandler.active.TryGetBuilding(new Vector2(x, y)));
    }

    // Add a target
    public void AddTarget(BaseTile tile)
    {
        if (tile == null) return;

        if (droneType == DroneType.Construction && tile.GetComponent<GhostTile>() != null) nearbyTargets.Add(tile);
        else if (droneType == DroneType.Resource && tile.GetComponent<BaseResource>() != null) nearbyTargets.Add(tile);
        else if (droneType == DroneType.Fixer) nearbyTargets.Add(tile);
    }

}
