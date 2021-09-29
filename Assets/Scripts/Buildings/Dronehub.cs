using UnityEngine;
using System.Collections;

public class Dronehub : BaseTile
{
    // Drone logic registrar script
    public DroneManager droneManager;

    // Lights
    public GameObject BlueLight;
    public GameObject YellowLight;

    // Drone variables
    public bool droneActive = false;
    public int checkDrone = 0;
    public int droneType = 1;
    public bool offsetStart = false;

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
    public SpriteRenderer buildingIcon;

    // Start is called before the first frame update
    void Start()
    {
        // Offsetting the start will make sure all ports are divided up equally
        droneManager = GameObject.Find("Drone Handler").GetComponent<DroneManager>();
        if (droneManager.isMenu) { changeDroneType(2); return; }
        
        if (offsetStart) { StartCoroutine(StartOffset()); }
        else changeDroneType(droneType, false);
    }

    public IEnumerator StartOffset()
    {
        yield return new WaitForSeconds(Random.Range(0f, 5f));
        changeDroneType(droneType, false);
    }

    public DroneManager.ResourceDrone getDrone()
    {
        if (resourcePort) return resourceScript;
        else return null;
    }

    // Change the drone type
    public void changeDroneType(int a, bool check = true)
    {
        if (check && droneType == a) return;

        droneType = a;
        leftPanel.localPosition = new Vector2(0, 0);
        rightPanel.localPosition = new Vector2(0, 0);

        switch (droneType)
        {
            case 1:
                if (activeDrone != null) Destroy(activeDrone.gameObject);
                activeDrone = Instantiate(constructorDrone, transform.position, Quaternion.identity);
                activeDrone.name = constructorDrone.name;
                activeDrone.parent = transform;
                activeDrone.localScale = new Vector2(0.8f, 0.8f);
                droneManager.RegisterAvailableConstructionDrone(activeDrone, transform, new Transform[] { leftPanel, rightPanel }, false, activeDrone.GetChild(0).GetComponent<SpriteRenderer>());
                droneManager.ForceUI();
                resourcePort = false;
                droneManager.ForceCheckAvailableDrones();
                BlueLight.SetActive(true);
                YellowLight.SetActive(false);
                break;
            case 2:
                if (!droneManager.isMenu && droneManager.tutorial.tutorialSlide == 5) droneManager.tutorial.nextSlide();
                if (activeDrone != null) Destroy(activeDrone.gameObject);
                activeDrone = Instantiate(resourceDrone, transform.position, Quaternion.identity);
                activeDrone.name = resourceDrone.name;
                activeDrone.parent = transform;
                activeDrone.localScale = new Vector2(0.8f, 0.8f);
                resourceScript = droneManager.RegisterResourceDrone(activeDrone, transform, new Transform[] { leftPanel, rightPanel });
                resourcePort = true;
                BlueLight.SetActive(false);
                YellowLight.SetActive(true);
                break;
            case 3:
                Debug.Log("No drone type 3");
                resourcePort = false;
                break;
        }
    }
}
