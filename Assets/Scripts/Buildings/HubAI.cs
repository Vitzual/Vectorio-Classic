using UnityEngine;
using System.Collections.Generic;

public class HubAI : BaseBuilding
{
    protected bool gameOver;
    public GameObject EndScreen;
    public GameObject BigBoom;
    public GameObject survival;
    public List<Transform> drones;
    public DroneManager droneManager;

    // On start, assign weapon variables
    void Start()
    {
        //BuildingSystem.addBuilding(transform);

        foreach (Transform drone in drones)
            droneManager.registerAvailableConstructionDrone(drone.GetChild(0), drone, 
                new Transform[] { drone.GetChild(1), drone.GetChild(2), }, true, 
                drone.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>());
    }

    // Kill defense
    /*
    public override void EndGame()
    {
        // Take control away from player
        GameObject.Find("Main Camera").GetComponent<CameraMovement>().enabled = false;
        GameObject.Find("Camera").GetComponent<CameraScroll>().enabled = false;
        GameObject.Find("Camera").GetComponent<Transform>().position = Vector3.zero;

        // Set end screen to true
        EndScreen.GetComponent<EndCanvas>().SetAlpha(0);
        EndScreen.SetActive(true);
        gameOver = true;

        // Instante big boom effect
        Instantiate(BigBoom, gameObject.transform.position, gameObject.transform.rotation);
        survival.SetActive(false);
        gameObject.SetActive(false);

        Survival srv = GameObject.Find("Survival").GetComponent<Survival>();
        srv.decreasePowerConsumption(power);
        BuildingHandler.removeBuilding(transform);
    }
    */
}
