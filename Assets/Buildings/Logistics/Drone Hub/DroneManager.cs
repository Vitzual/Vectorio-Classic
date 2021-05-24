// This script handles all active drones each frame

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------------- //
    // CONSTRUCTION DRONE CLASS
    // Scans through the building queue list and tries to find a building to place
    // If at any point during flight the target becomes null, drone will return to it's port
    // ----------------------------------------------------------------------------------- //

    [System.Serializable]
    public class ConstructionDrone
    {
        public ConstructionDrone(Transform body, Transform target, Transform targetBuilding, Transform spawnPosition, Transform[] plates)
        {
            this.body = body;
            this.target = target;
            this.targetBuilding = targetBuilding;
            this.spawnPosition = spawnPosition;
            this.plates = plates;

            targetPos = target.position;
            droneSpeed = 25f;
            droneType = 1;

            platesOpening = true;
            platesClosing = false;
            droneReturning = false;

            Vector2 lookDirection = targetPos - new Vector2(body.position.x, body.position.y);
            body.eulerAngles = new Vector3(0, 0, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f);
        }

        public Transform body;
        public Transform target;
        public Vector2 targetPos;
        public Transform targetBuilding;
        public Transform spawnPosition;
        public Transform[] plates;
        public float droneSpeed;
        public int distanceToMove;
        public int droneType;

        // Animation factors
        public bool platesOpening;
        public bool platesClosing;
        public bool droneReturning;
    }
    public List<ConstructionDrone> constructionDrones;


    // ----------------------------------------------------------------------------------- //
    // AVAILABLE DRONE CLASS
    // These are stagnant drones that are ready to be activated
    // When registering an active drone, simply assign the variables using the internal values
    // ----------------------------------------------------------------------------------- //

    [System.Serializable]
    public class AvailableDrones
    {
        public AvailableDrones(Transform body, Transform port, int droneType, Transform[] plates)
        {
            this.body = body;
            this.port = port;
            this.droneType = droneType;
            this.plates = plates;
        }

        public Transform[] plates;
        public Transform body;
        public Transform port;
        public int droneType;
    }
    public List<AvailableDrones> availableDrones;



    // ----------------------------------------------------------------------------------- //
    // BUILDING QUEUE LIST
    // Holds the position of the building that needs to be placed, and which building it is
    // ----------------------------------------------------------------------------------- //

    [System.Serializable]
    public class BuildingQueue
    {
        public BuildingQueue(Transform building, Transform buildingPos)
        {
            this.building = building;
            this.buildingPos = buildingPos;
        }

        public Transform building;
        public Transform buildingPos;
    }
    public List<BuildingQueue> buildingQueue;



    // Camera zoom object
    public CameraScroll cameraScroll;
    public AudioClip placementSound;

    // Survival
    public Survival survival;

    // Update is called once per frame
    void Update()
    {
        // Check available drones
        checkDrones();

        // Handles the construction drone logic

        for (int i = 0; i < constructionDrones.Count; i++)
        {
            ConstructionDrone drone = constructionDrones[i];

            // If the drone becomes null (parent is removed), remove drone
            if (drone.body == null)
            {
                constructionDrones.Remove(drone);
                i--;
                return;
            }
            else
            {
                // Update plates
                if (UpdatePlates(drone))
                    if (drone.droneReturning) i--;
                    else continue;

                // Move drone forward towards target if not null.
                if (drone.target != null)
                {
                    drone.body.position = Vector2.MoveTowards(drone.body.position, drone.targetPos, drone.droneSpeed * Time.deltaTime);

                    if (Vector2.Distance(drone.body.position, drone.targetPos) < 0.1f)
                    {
                        // If target is not the port, place the building
                        if (drone.target != drone.spawnPosition)
                        {
                            // Create the new building and remove the ghost version
                            var LastObj = Instantiate(drone.targetBuilding, drone.targetPos, Quaternion.Euler(new Vector3(0, 0, 0)));
                            LastObj.name = drone.targetBuilding.name;
                            survival.ghostBuildings.Remove(new Vector2(drone.target.position.x, drone.target.position.y));
                            Destroy(drone.target.gameObject);

                            // Play audio
                            float audioScale = cameraScroll.getZoom() / 1400f;
                            AudioSource.PlayClipAtPoint(placementSound, LastObj.transform.position, Settings.soundVolume - audioScale);

                            // Set drone target back to parent
                            returnToParent(drone);
                        }
                        else
                        {
                            // Reset drone so it's ready to go again
                            if (!drone.platesClosing) resetDrone(drone);
                        }
                    }
                }

                // If target becomes null, return to parent port.
                else
                {
                    drone.targetPos = drone.spawnPosition.position;
                    drone.target = drone.spawnPosition;
                    
                    Vector2 lookDirection = drone.targetPos - new Vector2(transform.position.x, transform.position.y);
                    drone.body.eulerAngles = new Vector3(0, 0, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f);

                    drone.droneReturning = true;
                }
            }
        }
    }

    // Scans through the building queue and assigns a drone if there's a task for it
    public void checkDrones()
    {
        float closest = Mathf.Infinity;
        int[] available = new int[] { -1, -1 };
        for (int a = 0; a < buildingQueue.Count; a++)
        {
            for (int b = 0; b < availableDrones.Count; b++)
            {
                // Check if all drones still exist
                if (availableDrones[b].body == null)
                {
                    availableDrones.Remove(availableDrones[b]);
                    continue;
                }

                // Check if there's a drone available to build
                if (availableDrones[b].droneType == 1)
                {
                    float distance = Vector2.Distance(availableDrones[b].body.position, buildingQueue[a].buildingPos.position);
                    if (distance < closest)
                    {
                        available = new int[] { a, b };
                        closest = distance;
                    }
                }
            }
        }

        // Deploy drone if one found (closest)
        if (available[0] != -1)
        {
            int a = available[0];
            int b = available[1];
            registerConstructionDrone(availableDrones[b].body, buildingQueue[a].buildingPos, buildingQueue[a].building, availableDrones[b].port, availableDrones[b].plates);
            availableDrones.Remove(availableDrones[b]);
            buildingQueue.Remove(buildingQueue[a]);
        }
    }

    // Reset the drone after it's task is complete
    public void resetDrone(ConstructionDrone drone)
    {
        drone.platesClosing = true;

        // Make drone appear below all panels
        int tracker = 0;
        foreach (Transform child in drone.body)
        {
            child.GetComponent<SpriteRenderer>().sortingOrder = tracker;
            tracker++;
        }
    }

    // Set the drone to return back to the parent
    public void returnToParent(ConstructionDrone drone)
    {
        drone.targetPos = drone.spawnPosition.position;
        drone.target = drone.spawnPosition;
        Vector2 lookDirection = new Vector2(drone.spawnPosition.position.x, drone.spawnPosition.position.y) - new Vector2(drone.body.position.x, drone.body.position.y);
        drone.body.eulerAngles = new Vector3(0, 0, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f);
        drone.droneReturning = true;
    }

    // Update the positioning and layering of the drones plates
    public bool UpdatePlates(ConstructionDrone drone)
    {
        // Open the plates
        if (drone.platesOpening)
        {
            drone.plates[0].Translate(Vector3.left * Time.deltaTime * 1.5f);
            drone.plates[1].Translate(Vector3.right * Time.deltaTime * 1.5f);

            if (!drone.droneReturning && drone.body.localScale.x <= 1f)
            {
                drone.body.localScale += new Vector3(0.001f, 0.001f, 0f);
            }

            if (drone.plates[1].localPosition.x >= 2)
            {
                drone.platesOpening = false;

                // Make drone appear above all panels
                int tracker = 21;
                foreach(Transform child in drone.body)
                {
                    child.GetComponent<SpriteRenderer>().sortingOrder = tracker;
                    tracker++;
                }
            }

            if (!drone.droneReturning) return true;
        }

        // Close the plates
        else if (drone.platesClosing)
        {
            drone.plates[0].Translate(Vector3.right * Time.deltaTime * 1.5f);
            drone.plates[1].Translate(Vector3.left * Time.deltaTime * 1.5f);

            if (drone.droneReturning && drone.body.localScale.x >= 0.8f)
            {
                drone.body.localScale -= new Vector3(0.001f, 0.001f, 0f);
            }

            if (drone.plates[1].localPosition.x <= 0)
            {
                drone.plates[0].localPosition = new Vector2(0, 0);
                drone.plates[1].localPosition = new Vector2(0, 0);
                drone.platesClosing = false;

                if (drone.droneReturning)
                {
                    // Reset drone so it's ready to go again
                    registerAvailableDrone(drone.body, drone.spawnPosition, drone.droneType, drone.plates);
                    drone.body.position = drone.spawnPosition.position;
                    drone.body.localScale = new Vector2(0.8f, 0.8f);

                    // Remove the drone
                    switch (drone.droneType)
                    {
                        case 1:
                            constructionDrones.Remove(drone);
                            return true;
                    }
                }
            }
        }
        return false;
    }

    // Register an active construction drone
    public void registerConstructionDrone(Transform body, Transform targetPos, Transform targetBuilding, Transform startingPos, Transform[] plates)
    {
        constructionDrones.Add(new ConstructionDrone(body, targetPos, targetBuilding, startingPos, plates));
    }

    // Register an available drone
    public void registerAvailableDrone(Transform body, Transform port, int droneType, Transform[] plates)
    {
        availableDrones.Add(new AvailableDrones(body, port, droneType, plates));
    }

    // Queue a building to be placed
    public void queueBuilding(Transform building, Transform ghostBuilding)
    {
        buildingQueue.Add(new BuildingQueue(building, ghostBuilding));
    }
}
