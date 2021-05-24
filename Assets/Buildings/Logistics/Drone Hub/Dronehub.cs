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
        switch (droneType)
        {
            case 1:
                activeDrone = Instantiate(constructorDrone, transform.position, Quaternion.identity);
                activeDrone.name = constructorDrone.name;
                activeDrone.parent = transform;
                activeDrone.localScale = new Vector2(0.8f, 0.8f);
                droneManager.registerAvailableDrone(activeDrone, transform, droneType, new Transform[] { leftPanel, rightPanel }, false);
                droneManager.forceUI();
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
