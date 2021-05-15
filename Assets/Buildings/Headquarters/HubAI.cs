﻿using UnityEngine;
using System.Collections.Generic;

public class HubAI : TileClass
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
        BuildingHandler.buildings.Add(transform);

        foreach (Transform drone in drones)
            droneManager.registerAvailableDrone(drone.GetChild(0), drone, 1, new Transform[] { drone.GetChild(1), drone.GetChild(2) });

        health = 100;
        maxhp = 100;
    }

    // Kill defense
    public override void DestroyTile()
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
        BuildingHandler.buildings.Remove(transform);
    }
}