using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Droneport : BaseTile
{
    // Hub drone boolean
    public bool droneCreated = false;
    public bool hubDrone = false;

    // Holds a reference to scriptable object
    public Building building;

    // Holds the drone type and object
    [HideInInspector] public Drone drone;

    // Side panels 
    public Transform leftPanel;
    public Transform rightPanel;

    // Lights
    public GameObject blueLight;
    public GameObject yellowLight;
    public GameObject greenLight;

    // Only for hub drones
    public void Start()
    {
        if (hubDrone) CreateDrone(Drone.DroneType.Resource);
    }

    // Apply metadata
    public override void ApplyMetadata(int data)
    {
        if (data == 0) CreateDrone(Drone.DroneType.Resource);
        else if (data == 1) CreateDrone(Drone.DroneType.Resource);
        else if (data == 2) CreateDrone(Drone.DroneType.Fixer);
    }

    // Locate nearby buildings for drone
    public override void Setup()
    {
        // Check if drone was set via metadata
        if (!droneCreated) CreateDrone(Drone.DroneType.Resource);

        // Reset nearby targets
        drone.nearbyTargets = new List<BaseEntity>();

        // Loop through all nearby drone ports
        int adjustment = Research.drone_tile_coverage * 5;
        int xTile = (int)transform.position.x;
        int yTile = (int)transform.position.y;

        // If not builder, update nearby buildings
        if (drone.type != Drone.DroneType.Resource)
        {
            // Loop through all tiles and try to find drones
            for (int x = xTile - adjustment; x <= xTile + adjustment; x += 5)
                for (int y = yTile - adjustment; y <= yTile + adjustment; y += 5)
                    drone.AddTarget(InstantiationHandler.active.TryGetBuilding(new Vector2(x, y)));
        }

        // Set material
        material = building.material;

        // Update nearby targets
        UpdateNearbyTargets();
    }

    // Create drone method
    public void CreateDrone(Drone.DroneType type)
    {
        // See if drone already created
        if (droneCreated) return;
        else droneCreated = true;

        // Loop through drones, and create new one
        drone = Instantiate(DroneManager.active.GetDrone(type), transform.position, Quaternion.identity).GetComponent<Drone>();

        // If a drone still hasn't been created, just set to default
        if (drone == null)
        {
            Debug.Log("A drone with a specified type could not be created. Please add it to Drone list");
            drone = Instantiate(DroneManager.active.droneTypes[0], transform.position, Quaternion.identity).GetComponent<Drone>();
        }

        // Set home
        drone.home = this;

        // Add drone to active drone list
        DroneManager.active.AddDrone(drone);

        // Update lights
        if (!hubDrone)
        {
            if (type == Drone.DroneType.Builder)
            {
                blueLight.SetActive(true);
                yellowLight.SetActive(false);
                greenLight.SetActive(false);
            }
            else if (type == Drone.DroneType.Resource)
            {
                blueLight.SetActive(false);
                yellowLight.SetActive(true);
                greenLight.SetActive(false);
            }
            else
            {
                blueLight.SetActive(false);
                yellowLight.SetActive(false);
                greenLight.SetActive(true);
            }
        }
    }

    // Check for nearby targets
    public void UpdateNearbyTargets()
    {
        // Loop through all nearby drone ports
        int adjustment = Research.drone_tile_coverage * 5;
        int xTile = (int)transform.position.x;
        int yTile = (int)transform.position.y;

        // Loop through all tiles and try to find drones
        for (int x = xTile - adjustment; x <= xTile + adjustment; x += 5)
        {
            for (int y = yTile - adjustment; y <= yTile + adjustment; y += 5)
            {
                BaseTile holder = InstantiationHandler.active.TryGetBuilding(new Vector2(x, y));
                if (holder != null) AddTarget(holder);
            }
        }
    }

    // Open doors
    public bool OpenDoors()
    {
        leftPanel.Translate(Vector3.left * Time.deltaTime * Research.drone_deployment_speed);
        rightPanel.Translate(Vector3.right * Time.deltaTime * Research.drone_deployment_speed);
        return rightPanel.localPosition.x >= 2;
    }

    // Close doors
    public bool CloseDoors()
    {
        leftPanel.Translate(Vector3.right * Time.deltaTime * Research.drone_deployment_speed);
        rightPanel.Translate(Vector3.left * Time.deltaTime * Research.drone_deployment_speed);
        return rightPanel.localPosition.x <= 0;
    }

    // Add a target
    public void AddTarget(BaseTile tile)
    {
        if (drone != null)
            drone.AddTarget(tile);
    }

    // Destroy entity
    public override void DestroyEntity()
    {
        if (hubDrone) return;
        drone.Destroy();
        base.DestroyEntity();
    }
}
