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
        public ConstructionDrone(Transform body, Transform target, Transform targetBuilding, Transform spawnPosition, Transform[] plates, bool isHubDrone, int gold, int power, int heat)
        {
            this.body = body;
            this.target = target;
            this.targetBuilding = targetBuilding;
            this.spawnPosition = spawnPosition;
            this.plates = plates;
            this.isHubDrone = isHubDrone;

            goldCost = gold;
            powerCost = power;
            heatCost = heat;

            targetPos = target.position;
            droneType = 1;

            platesOpening = true;
            platesClosing = false;
            droneReturning = false;

            Vector2 lookDirection = targetPos - new Vector2(body.position.x, body.position.y);
            body.eulerAngles = new Vector3(0, 0, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f);
        }

        public int goldCost;
        public int powerCost;
        public int heatCost;

        public Transform body;
        public Transform target;
        public Vector2 targetPos;
        public Transform targetBuilding;
        public Transform spawnPosition;
        public Transform[] plates;
        public int distanceToMove;
        public int droneType;
        public bool isHubDrone;

        // Animation factors
        public bool platesOpening;
        public bool platesClosing;
        public bool droneReturning;
    }
    public List<ConstructionDrone> constructionDrones;


    // ----------------------------------------------------------------------------------- //
    // RESOURCE DRONE CLASS
    // These drones automatically go to collectors and remove the gold inside, then transport
    // the gold to an available storage. If no collectors, the loop will be skipped!
    // ----------------------------------------------------------------------------------- //

    [System.Serializable]
    public class ResourceDrone
    {
        public ResourceDrone(Transform body, Transform port, Transform[] plates, Transform target)
        {
            this.body = body;
            this.port = port;
            this.plates = plates;
            this.target = target;

            goldCollected = 0;
            targetsLeft = Research.research_resource_amount;
            targetScript = target.GetComponent<CollectorAI>();
            platesOpening = true;
            platesClosing = false;
            droneReturning = false;
        }

        public Transform[] plates;
        public List<Transform> collected;
        public Transform body;
        public Transform port;
        public Transform target;
        public CollectorAI targetScript;
        public int goldCollected;
        public int targetsLeft;

        // Animation factors
        public bool platesOpening;
        public bool platesClosing;
        public bool droneReturning;
    }
    public List<ResourceDrone> resourceDrone;


    // ----------------------------------------------------------------------------------- //
    // AVAILABLE CONSTRUCTION DRONE CLASS
    // These are stagnant drones that are ready to be activated
    // When registering an active drone, simply assign the variables using the internal values
    // ----------------------------------------------------------------------------------- //

    [System.Serializable]
    public class AvailableConstructionDrones
    {
        public AvailableConstructionDrones(Transform body, Transform port, Transform[] plates, bool isHubDrone)
        {
            this.body = body;
            this.port = port;
            this.plates = plates;
            this.isHubDrone = isHubDrone;
        }

        public Transform[] plates;
        public Transform body;
        public Transform port;
        public bool isHubDrone;
    }
    public List<AvailableConstructionDrones> availableConstructionDrones;


    // ----------------------------------------------------------------------------------- //
    // AVAILABLE RESOURCE DRONE CLASS
    // These are stagnant drones that are ready to be activated
    // When registering an active drone, simply assign the variables using the internal values
    // ----------------------------------------------------------------------------------- //

    [System.Serializable]
    public class AvailableResourceDrones
    {
        public AvailableResourceDrones(Transform body, Transform port, Transform[] plates)
        {
            this.body = body;
            this.port = port;
            this.plates = plates;
        }

        public Transform[] plates;
        public Transform body;
        public Transform port;
    }
    public List<AvailableResourceDrones> availableResourceDrones;


    // ----------------------------------------------------------------------------------- //
    // BUILDING QUEUE LIST
    // Holds the position of the building that needs to be placed, and which building it is
    // ----------------------------------------------------------------------------------- //

    [System.Serializable]
    public class BuildingQueue
    {
        public BuildingQueue(Transform building, Transform buildingPos, int gold, int power, int heat)
        {
            this.building = building;
            this.buildingPos = buildingPos;
            goldCost = gold;
            powerCost = power;
            heatCost = heat;
        }

        public Transform building;
        public Transform buildingPos;
        public int goldCost;
        public int powerCost;
        public int heatCost;
    }
    public List<BuildingQueue> buildingQueue;


    // ----------------------------------------------------------------------------------- //
    // COLLECTOR LIST
    // Holds the position of the building that needs to be placed, and which building it is
    // ----------------------------------------------------------------------------------- //

    [System.Serializable]
    public class CollectorList
    {
        public CollectorList(Transform building, CollectorAI script, int type)
        {
            this.building = building;
            this.script = script;
            this.type = type;

            hasDrone = false;
        }

        public Transform building;
        public CollectorAI script;
        public int type;
        public bool hasDrone;
    }
    public List<CollectorList> collectorList;


    // ----------------------------------------------------------------------------------- //
    // STORAGE LIST
    // Holds the position of the building that needs to be placed, and which building it is
    // ----------------------------------------------------------------------------------- //

    [System.Serializable]
    public class StorageList
    {
        public StorageList(Transform building, CollectorAI script, int type)
        {
            this.building = building;
            this.script = script;
            this.type = type;
        }

        public Transform building;
        public CollectorAI script;
        public int type;
    }
    public List<StorageList> storageList;

    // Camera zoom object
    public CameraScroll cameraScroll;
    public AudioClip placementSound;

    // Survival
    public Survival survival;

    // Update is called once per frame
    void Update()
    {
        // Check available drones
        checkConstructionDrones();
        checkResourceDrones();

        // Handles the construction drone logic
        for (int i = 0; i < constructionDrones.Count; i++)
        {
            ConstructionDrone drone = constructionDrones[i];

            // If the drone becomes null (parent is removed), remove drone and add back building to available buildings
            if (drone.body == null)
            {
                queueBuilding(drone.targetBuilding, drone.target, drone.goldCost, drone.powerCost, drone.heatCost);
                constructionDrones.Remove(drone);
                i--;
                return;
            }
            else
            {
                // Update plates
                if (UpdateConstructionPlates(drone))
                    if (drone.droneReturning) i--;
                    else continue;

                // Move drone forward towards target if not null.
                if (drone.target != null)
                {
                    drone.body.position = Vector2.MoveTowards(drone.body.position, drone.targetPos, Research.research_construction_speed * Time.deltaTime);

                    if (Vector2.Distance(drone.body.position, drone.targetPos) < 0.1f)
                    {
                        // If target is not the port, place the building
                        if (drone.target != drone.spawnPosition)
                        {
                            // Attempt to place a building
                            placeBuilding(drone);
                            returnConstructionToParent(drone);
                        }
                        else
                        {
                            // Reset drone so it's ready to go again
                            if (!drone.platesClosing) resetConstructionDrone(drone);
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

        // Handles the resource drone logic
        for (int i = 0; i < resourceDrone.Count; i++)
        {
            ResourceDrone drone = resourceDrone[i];

            // Check if body is null
            if (drone.body == null)
            {
                resourceDrone.Remove(drone);
                i--;
                return;
            }
            else
            {
                // Update plates
                if (UpdateResourcePlates(drone))
                    if (drone.droneReturning) i--;
                    else continue;

                // Move towards target
                if (drone.target != null)
                {
                    drone.body.position = Vector2.MoveTowards(drone.body.position, drone.target.position, Research.research_resource_speed * Time.deltaTime);

                    if (Vector2.Distance(drone.body.position, drone.target.position) < 0.1f)
                    {
                        // If target is not the port, place the building
                        if (drone.target != drone.port)
                        {
                            // Add gold to collected and choose next target
                            drone.goldCollected += drone.targetScript.GrabResources();
                            drone.collected.Add(drone.target);
                            drone.targetsLeft -= 1;

                            // Find next target. If none, return to port.
                            if (drone.targetsLeft == 0 || !findResourceCollector(drone))
                                returnResourceToParent(drone);
                        }
                        else
                        {
                            // Reset drone so it's ready to go again
                            if (!drone.platesClosing) resetResourceDrone(drone);
                        }
                    }
                }

            }
        }
    }

    // Scans through the building queue and assigns a drone if there's a task for it
    public void checkConstructionDrones()
    {
        float closest = Mathf.Infinity;
        int[] available = new int[] { -1, -1 };

        for (int a = 0; a < buildingQueue.Count; a++)
        {
            // Check if building queue still available
            if (buildingQueue[a].buildingPos == null)
                buildingQueue.Remove(buildingQueue[a]);

            // Check if adequate resources for a drone to be deployed
            if (survival.Spawner.htrack >= survival.Spawner.maxHeat && buildingQueue[a].heatCost > 0) continue;
            if (survival.PowerConsumption >= survival.AvailablePower && buildingQueue[a].powerCost > 0) continue;
            if (buildingQueue[a].goldCost > survival.gold && buildingQueue[a].goldCost > 0) continue;
  
            for (int b = 0; b < availableConstructionDrones.Count; b++)
            {
                // Check if all drones still exist
                if (availableConstructionDrones[b].body == null)
                {
                    availableConstructionDrones.Remove(availableConstructionDrones[b]);
                    continue;
                }

                float distance = Vector2.Distance(availableConstructionDrones[b].body.position, buildingQueue[a].buildingPos.position);
                if (distance < closest)
                {
                    available = new int[] { a, b };
                    closest = distance;
                }
            }
        }

        // Deploy drone if one found (closest)
        if (available[0] != -1)
        {
            int a = available[0];
            int b = available[1];
            registerConstructionDrone(availableConstructionDrones[b].body, buildingQueue[a].buildingPos, buildingQueue[a].building, availableConstructionDrones[b].port, availableConstructionDrones[b].plates, availableConstructionDrones[b].isHubDrone, buildingQueue[a].goldCost, buildingQueue[a].powerCost, buildingQueue[a].heatCost);
            availableConstructionDrones.Remove(availableConstructionDrones[b]);
            buildingQueue.Remove(buildingQueue[a]);
            survival.UI.updateDronesUI(availableConstructionDrones.Count, availableConstructionDrones.Count + constructionDrones.Count);
        }
    }

    // Scans through the collector list and assigns a drone if there's one available
    public void checkResourceDrones()
    {
        foreach (CollectorList collector in collectorList)
        {
            if (!collector.hasDrone)
            {
                // thing here logic
            }
        }
    }

    // Reset the drone after it's task is complete
    public void resetConstructionDrone(ConstructionDrone drone)
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

    // Reset the drone after it's task is complete
    public void resetResourceDrone(ResourceDrone drone)
    {
        survival.AddGold(drone.goldCollected);
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
    public void returnConstructionToParent(ConstructionDrone drone)
    {
        drone.targetPos = drone.spawnPosition.position;
        drone.target = drone.spawnPosition;
        Vector2 lookDirection = new Vector2(drone.spawnPosition.position.x, drone.spawnPosition.position.y) - new Vector2(drone.body.position.x, drone.body.position.y);
        drone.body.eulerAngles = new Vector3(0, 0, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f);
        drone.droneReturning = true;
    }

    // Set the drone to return back to the parent
    public void returnResourceToParent(ResourceDrone drone)
    {
        drone.target = drone.port;
        Vector2 lookDirection = new Vector2(drone.port.position.x, drone.port.position.y) - new Vector2(drone.body.position.x, drone.body.position.y);
        drone.body.eulerAngles = new Vector3(0, 0, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f);
        drone.droneReturning = true;
    }

    // Finds a new target for a resource drone
    public bool findResourceCollector(ResourceDrone drone)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (CollectorList collector in collectorList)
        {
            if (collector.hasDrone && !drone.collected.Contains(collector.building)) continue;
            Vector3 directionToTarget = collector.building.position - drone.body.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = collector.building;
            }
        }

        drone.target = bestTarget;
        return drone.target != null;
    }

    // Update the positioning and layering of the drones plates
    public bool UpdateConstructionPlates(ConstructionDrone drone)
    {
        // Open the plates
        if (drone.platesOpening)
        {
            drone.plates[0].Translate(Vector3.left * Time.deltaTime * 4f);
            drone.plates[1].Translate(Vector3.right * Time.deltaTime * 4f);

            if (!drone.droneReturning)
            {
                if (drone.isHubDrone && drone.body.localScale.x <= 1.4f)
                    drone.body.localScale += new Vector3(0.002f, 0.002f, 0f);
                else if (drone.body.localScale.x <= 1f)
                    drone.body.localScale += new Vector3(0.002f, 0.002f, 0f);
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
            drone.plates[0].Translate(Vector3.right * Time.deltaTime * 4f);
            drone.plates[1].Translate(Vector3.left * Time.deltaTime * 4f);

            if (drone.droneReturning)
            {
                if (drone.isHubDrone && drone.body.localScale.x >= 1.2f)
                    drone.body.localScale -= new Vector3(0.002f, 0.002f, 0f);
                else if (drone.body.localScale.x >= 0.8f)
                    drone.body.localScale -= new Vector3(0.002f, 0.002f, 0f);
            }

            if (drone.plates[1].localPosition.x <= 0)
            {
                drone.plates[0].localPosition = new Vector2(0, 0);
                drone.plates[1].localPosition = new Vector2(0, 0);
                drone.platesClosing = false;

                if (drone.droneReturning)
                {
                    // Reset drone so it's ready to go again
                    registerAvailableConstructionDrone(drone.body, drone.spawnPosition, drone.plates, drone.isHubDrone);
                    survival.UI.updateDronesUI(availableConstructionDrones.Count, availableConstructionDrones.Count + constructionDrones.Count - 1);
                    drone.body.position = drone.spawnPosition.position;
                    if (!drone.isHubDrone) drone.body.localScale = new Vector2(0.8f, 0.8f);
                    else drone.body.localScale = new Vector2(1.2f, 1.2f);

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

    // Update the positioning and layering of the drones plates
    public bool UpdateResourcePlates(ResourceDrone drone)
    {
        // Open the plates
        if (drone.platesOpening)
        {
            drone.plates[0].Translate(Vector3.left * Time.deltaTime * 4f);
            drone.plates[1].Translate(Vector3.right * Time.deltaTime * 4f);

            if (!drone.droneReturning && drone.body.localScale.x <= 1f)
                drone.body.localScale += new Vector3(0.002f, 0.002f, 0f);

            if (drone.plates[1].localPosition.x >= 2)
            {
                drone.platesOpening = false;

                // Make drone appear above all panels
                int tracker = 21;
                foreach (Transform child in drone.body)
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
            drone.plates[0].Translate(Vector3.right * Time.deltaTime * 4f);
            drone.plates[1].Translate(Vector3.left * Time.deltaTime * 4f);

            if (drone.droneReturning && drone.body.localScale.x >= 0.8f)
                drone.body.localScale -= new Vector3(0.002f, 0.002f, 0f);
            
            if (drone.plates[1].localPosition.x <= 0)
            {
                drone.plates[0].localPosition = new Vector2(0, 0);
                drone.plates[1].localPosition = new Vector2(0, 0);
                drone.platesClosing = false;

                if (drone.droneReturning)
                {
                    // Reset drone so it's ready to go again
                    registerAvailableResourceDrone(drone.body, drone.port, drone.plates);
                    drone.body.position = drone.port.position;
                    drone.body.localScale = new Vector2(0.8f, 0.8f);
                    resourceDrone.Remove(drone);
                }
            }
        }
        return false;
    }

    // Attempt to place a building
    public void placeBuilding(ConstructionDrone drone)
    {
        // Check if adequate resources for a drone to be deployed
        if (survival.Spawner.htrack >= survival.Spawner.maxHeat && drone.heatCost > 0) 
        { 
            queueBuilding(drone.targetBuilding, drone.target, drone.goldCost, drone.powerCost, drone.heatCost);
            return; 
        }
        if (survival.PowerConsumption >= survival.AvailablePower && drone.powerCost > 0)
        {
            queueBuilding(drone.targetBuilding, drone.target, drone.goldCost, drone.powerCost, drone.heatCost);
            return;
        }
        if (drone.goldCost > survival.gold && drone.goldCost > 0)
        {
            queueBuilding(drone.targetBuilding, drone.target, drone.goldCost, drone.powerCost, drone.heatCost);
            return;
        }

        // Set component values
        survival.increasePowerConsumption(drone.powerCost);
        survival.Spawner.increaseHeat(drone.heatCost);
        survival.RemoveGold(drone.goldCost);

        // Create the new building and remove the ghost version
        var LastObj = Instantiate(drone.targetBuilding, drone.targetPos, Quaternion.Euler(new Vector3(0, 0, 0)));
        LastObj.name = drone.targetBuilding.name;
        survival.ghostBuildings.Remove(new Vector2(drone.target.position.x, drone.target.position.y));
        Destroy(drone.target.gameObject);

        // Create a UI resource popup thing idk lmaooo
        survival.UI.CreateResourcePopup("- " + drone.goldCost, "Gold", drone.targetPos);

        // Play audio
        float audioScale = cameraScroll.getZoom() / 1400f;
        AudioSource.PlayClipAtPoint(placementSound, LastObj.transform.position, Settings.soundVolume - audioScale);

        // Check for nearby enemy buildings
        var colliders = Physics2D.OverlapCircleAll(
            this.gameObject.transform.position,
            100 + Research.research_range,
            1 << LayerMask.NameToLayer("Enemy Defense"));

        foreach (Collider2D enemyTurret in colliders)
        {
            enemyTurret.gameObject.GetComponent<TurretClass>().enabled = true;
        }

        return;
    }

    // Register an active resource drone
    public void registerResourceDrone(Transform body, Transform port, Transform[] plates, Transform target)
    {
        resourceDrone.Add(new ResourceDrone(body, port, plates, target));
    }

    // Register an available resource drone
    public void registerAvailableResourceDrone(Transform body, Transform port, Transform[] plates)
    {
        availableResourceDrones.Add(new AvailableResourceDrones(body, port, plates));
    }

    // Register an active construction drone
    public void registerConstructionDrone(Transform body, Transform targetPos, Transform targetBuilding, Transform startingPos, Transform[] plates, bool isHubDrone, int gold, int power, int heat)
    {
        constructionDrones.Add(new ConstructionDrone(body, targetPos, targetBuilding, startingPos, plates, isHubDrone, gold, power, heat));
    }

    // Register an available construction drone
    public void registerAvailableConstructionDrone(Transform body, Transform port, Transform[] plates, bool isHubDrone)
    {
        availableConstructionDrones.Add(new AvailableConstructionDrones(body, port, plates, isHubDrone));
    }

    // Register an available collector
    public void registerCollector(Transform building, CollectorAI script, int type)
    {
        collectorList.Add(new CollectorList(building, script, type));
    }

    // Register an available collector
    public void registerStorage(Transform building, CollectorAI script, int type)
    {
        storageList.Add(new StorageList(building, script, type));
    }

    // Queue a building to be placed
    public void queueBuilding(Transform building, Transform ghostBuilding, int gold, int power, int heat)
    {
        buildingQueue.Add(new BuildingQueue(building, ghostBuilding, gold, power, heat));
    }

    // Remove a queued building
    public bool dequeueBuilding(Transform ghost)
    {
        // Check the building queue
        for (int i = 0; i < buildingQueue.Count; i++)
        {
            if (buildingQueue[i].buildingPos.position == ghost.position)
            {
                survival.ghostBuildings.Remove(ghost.position);
                Destroy(ghost.gameObject);
                buildingQueue.RemoveAt(i);
                return true;
            }
        }

        // Check active drone targets
        for (int i = 0; i < constructionDrones.Count; i++)
        {
            if (constructionDrones[i].targetPos == new Vector2(ghost.position.x, ghost.position.y))
            {
                survival.ghostBuildings.Remove(ghost.position);
                Destroy(ghost.gameObject);
                returnConstructionToParent(constructionDrones[i]);
                return true;
            }
        }

        return false;
    }

    // Forces a UI update
    public void forceUI()
    {
        survival.UI.updateDronesUI(availableConstructionDrones.Count, availableConstructionDrones.Count + constructionDrones.Count);
    }
}
