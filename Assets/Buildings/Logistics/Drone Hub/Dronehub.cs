using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dronehub : TileClass
{
    // Drone logic registrar script
    public DroneManager droneManager;

    // Drone variables
    public bool droneActive = false;
    public int checkDrone = 0;
    public int droneType;

    // Hold drone type
    public Transform activeDrone;
    public Transform constructorDrone;
    public Transform resourceDrone;
    public Transform combatDrone;

    // Side panels 
    public Transform leftPanel;
    public Transform rightPanel;

    // Animation variables
    public bool isAnimating;
    public float animSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        droneManager = GameObject.Find("Drone Handler").GetComponent<DroneManager>();
        changeDroneType(2);
    }

    // Change the drone type
    public void changeDroneType(int a)
    {
        droneType = a;
        switch (droneType)
        {
            case 1:
                activeDrone = Instantiate(constructorDrone, transform.position, Quaternion.identity);
                activeDrone.name = constructorDrone.name;
                activeDrone.parent = transform;
                activeDrone.localScale = new Vector2(0.8f, 0.8f);
                droneManager.registerAvailableConstructionDrone(activeDrone, transform, new Transform[] { leftPanel, rightPanel }, false);
                droneManager.forceUI();
                break;
            case 2:
                activeDrone = Instantiate(resourceDrone, transform.position, Quaternion.identity);
                activeDrone.name = resourceDrone.name;
                activeDrone.parent = transform;
                activeDrone.localScale = new Vector2(0.8f, 0.8f);
                droneManager.registerAvailableResourceDrone(activeDrone, transform, new Transform[] { leftPanel, rightPanel });
                break;
            case 3:
                activeDrone = Instantiate(combatDrone, transform.position, Quaternion.identity);
                activeDrone.name = combatDrone.name;
                activeDrone.parent = transform;
                activeDrone.localScale = new Vector2(0.8f, 0.8f);
                droneManager.registerAvailableConstructionDrone(activeDrone, transform, new Transform[] { leftPanel, rightPanel }, false);
                break;
        }
    }

    // Kill defense
    public override void DestroyTile()
    {
        Survival srv = GameObject.Find("Survival").GetComponent<Survival>();
        srv.decreasePowerConsumption(power);
        BuildingHandler.buildings.Remove(transform);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
