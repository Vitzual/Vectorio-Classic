using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunshipPort : BaseTile
{
    // Spawn ship
    GunshipDrone drone;

    // Top and bottom panels
    public Transform topPanel;
    public Transform bottomPanel;

    // Locate nearby buildings for drone
    public override void Setup()
    {
        // Spawn ship
        CreateDrone(Drone.DroneType.Gunship);

        // Set material
        material = buildable.building.material;
    }

    // Create drone method
    public void CreateDrone(Drone.DroneType type)
    {
        // Set metadata based on drone type
        metadata = (int)type;

        // Loop through drones, and create new one
        drone = Instantiate(DroneManager.active.GetDrone(type), transform.position, Quaternion.identity).GetComponent<GunshipDrone>();

        // Set home
        drone.homePort = this;

        // Add drone to active drone list
        DroneManager.active.activeDrones.Add(drone);
    }

    // Change panel layers
    public void ChangePanelLayers()
    {
        topPanel.GetComponent<SpriteRenderer>().sortingOrder = 10;
        bottomPanel.GetComponent<SpriteRenderer>().sortingOrder = 10;
    }

    // Open doors
    public bool OpenDoors()
    {
        topPanel.Translate(Vector3.up * Time.deltaTime * 2f);
        bottomPanel.Translate(Vector3.down * Time.deltaTime * 2f);
        return topPanel.localPosition.y >= 5;
    }

    // Close doors
    public bool CloseDoors()
    {
        topPanel.Translate(Vector3.down * Time.deltaTime * 2f);
        bottomPanel.Translate(Vector3.up * Time.deltaTime * 2f);
        return topPanel.localPosition.y <= 0;
    }
}
