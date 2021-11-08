using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Droneport : BaseTile
{
    // Holds a reference to scriptable object
    public Building building;

    // Holds the drone type and object
    public Drone drone;

    // List of drones
    public List<Drone> _drones;

    // Side panels 
    public Transform leftPanel;
    public Transform rightPanel;

    // Apply metadata
    public override void ApplyMetadata(int data)
    {
        if (data == 0) CreateDrone(Drone.DroneType.Builder);
        else if (data == 1) CreateDrone(Drone.DroneType.Resource);
        else if (data == 2) CreateDrone(Drone.DroneType.Fixer);
    }

    // Create drone method
    public void CreateDrone(Drone.DroneType type)
    {
        // If a drone has been instantiated, remove it
        if (drone != null) Destroy(drone.gameObject);

        // Loop through drones, and create new one
        foreach (Drone newDrone in _drones)
            if (newDrone.type == type)
                drone = Instantiate(newDrone, transform.position, Quaternion.identity).GetComponent<Drone>();

        // If a drone still hasn't been created, just set to default
        if (drone == null)
        {
            Debug.Log("A drone with a specified type could not be created. Please add it to Drone list");
            drone = Instantiate(_drones[0], transform.position, Quaternion.identity).GetComponent<Drone>();
        }

        // Add drone to active drone list
        DroneManager.active.AddDrone(drone);
    }

    // Locate nearby buildings for drone
    public override void Setup()
    {
        // Check if drone was set via metadata
        if (drone == null) CreateDrone(Drone.DroneType.Builder);

        // Reset nearby targets
        drone.nearbyTargets = new List<BaseEntity>();

        // Loop through all nearby drone ports
        int adjustment = Research.drone_tile_coverage * 5;
        int xTile = (int)transform.position.x;
        int yTile = (int)transform.position.y;

        // Loop through all tiles and try to find drones
        for (int x = xTile - adjustment; x <= xTile + adjustment; x += 5)
            for (int y = yTile - adjustment; y <= yTile + adjustment; y += 5)
                drone.AddTarget(InstantiationHandler.active.TryGetBuilding(new Vector2(x, y)));

        // Set material
        material = building.material;
    }

    // Open doors
    public bool OpenDoors()
    {
        leftPanel.Translate(Vector3.left * Time.deltaTime * Research.drone_deployment_speed);
        rightPanel.Translate(Vector3.right * Time.deltaTime * Research.drone_deployment_speed);
        return leftPanel.localPosition.x >= 2;
    }

    // Close doors
    public bool CloseDoors()
    {
        leftPanel.Translate(Vector3.right * Time.deltaTime * Research.drone_deployment_speed);
        rightPanel.Translate(Vector3.left * Time.deltaTime * Research.drone_deployment_speed);
        return leftPanel.localPosition.x <= 0;
    }

    // Destroy entity
    public override void DestroyEntity()
    {
        Recycler.AddRecyclable(drone.transform);
        base.DestroyEntity();
    }
}
