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
    public Transform fixerDrone;
    public Transform resourceDrone;
    public Transform attackDrone;
    public Transform engineerDrone;

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
        checkDrone = Random.Range(0, 100);
    }

    public void setType(int a)
    {
        droneType = a;
    }

    public void spawnDrone()
    {
        switch(droneType)
        {
            case 0:
                activeDrone = Instantiate(fixerDrone, transform.position, Quaternion.identity);
                activeDrone.name = fixerDrone.name;
                droneManager.registerFixerDrone(activeDrone, 2, 10, 1, 10);
                break;
            case 1:
                activeDrone = Instantiate(resourceDrone, transform.position, Quaternion.identity);
                activeDrone.name = resourceDrone.name;
                // REGISTER DRONE HERE (WITH VARIABLES)
                break;
            case 2:
                activeDrone = Instantiate(attackDrone, transform.position, Quaternion.identity);
                activeDrone.name = attackDrone.name;
                // REGISTER DRONE HERE (WITH VARIABLES)
                break;
            case 3:
                activeDrone = Instantiate(engineerDrone, transform.position, Quaternion.identity);
                activeDrone.name = engineerDrone.name;
                // REGISTER DRONE HERE (WITH VARIABLES)
                break;
            default:
                activeDrone = Instantiate(fixerDrone, transform.position, Quaternion.identity);
                activeDrone.name = fixerDrone.name;
                droneManager.registerFixerDrone(activeDrone, 2, 10, 1, 10);
                break;
        }
    }

    public void Update()
    {
        if (checkDrone == 100)
        {
            // Check if drone active, if not, spawn it
            isAnimating = true;

            checkDrone = 0;
        }
        else checkDrone++;

        if (isAnimating) PlayAnim();
    }

    public void PlayAnim()
    {
        leftPanel.transform.position -= leftPanel.transform.right * animSpeed * Time.deltaTime;
        rightPanel.transform.position += rightPanel.transform.right * animSpeed * Time.deltaTime;

        // Set the anim after finished
        if (leftPanel.transform.localPosition.x <= -1)
        {
            leftPanel.transform.localPosition = new Vector3(-1f, 0, 0);
            rightPanel.transform.localPosition = new Vector3(1f, 0, 0);

            isAnimating = false;
            animSpeed = -1f;
        }
        else if (leftPanel.transform.localPosition.x >= 0)
        {
            leftPanel.transform.localPosition = new Vector3(0, 0, 0);
            rightPanel.transform.localPosition = new Vector3(0, 0, 0);

            isAnimating = false;
            animSpeed = 1f;
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
