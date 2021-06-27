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

    // Script holder
    public bool resourcePort = false;
    public DroneManager.ResourceDrone resourceScript;

    // Animation variables
    public bool isAnimating;
    public float animSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        droneManager = GameObject.Find("Drone Handler").GetComponent<DroneManager>();
        changeDroneType(2);
    }

    public DroneManager.ResourceDrone getDrone()
    {
        if (resourcePort) return resourceScript;
        else return null;
    }

    // Change the drone type
    public void changeDroneType(int a)
    {
        droneType = a;
        switch (droneType)
        {
            case 1:
                if (activeDrone != null) Destroy(activeDrone.gameObject);
                activeDrone = Instantiate(constructorDrone, transform.position, Quaternion.identity);
                activeDrone.name = constructorDrone.name;
                activeDrone.parent = transform;
                activeDrone.localScale = new Vector2(0.8f, 0.8f);
                droneManager.registerAvailableConstructionDrone(activeDrone, transform, new Transform[] { leftPanel, rightPanel }, false);
                droneManager.forceUI();
                resourcePort = false;
                break;
            case 2:
                if (activeDrone != null) Destroy(activeDrone.gameObject);
                activeDrone = Instantiate(resourceDrone, transform.position, Quaternion.identity);
                activeDrone.name = resourceDrone.name;
                activeDrone.parent = transform;
                activeDrone.localScale = new Vector2(0.8f, 0.8f);
                resourceScript = droneManager.registerResourceDrone(activeDrone, transform, new Transform[] { leftPanel, rightPanel });
                resourcePort = true;
                break;
            case 3:
                if (activeDrone != null) Destroy(activeDrone.gameObject);
                activeDrone = Instantiate(combatDrone, transform.position, Quaternion.identity);
                activeDrone.name = combatDrone.name;
                activeDrone.parent = transform;
                activeDrone.localScale = new Vector2(0.8f, 0.8f);
                droneManager.registerAvailableConstructionDrone(activeDrone, transform, new Transform[] { leftPanel, rightPanel }, false);
                resourcePort = false;
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
