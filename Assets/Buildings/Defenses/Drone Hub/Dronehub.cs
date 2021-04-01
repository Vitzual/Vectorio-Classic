using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dronehub : TileClass
{
    // Drone logic registerar script
    public DroneManager droneManager;

    // Hold drone type
    public Transform droneType;

    // Side panels 
    public Transform leftPanel;
    public Transform rightPanel;

    // Animation variables
    public bool isAnimating;
    public float animSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Update()
    {
        
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

    public void SpawnDrone()
    {
        var holder = Instantiate(droneType, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        holder.name = droneType.name;

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
