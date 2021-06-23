// This script handles all active drones each frame
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        public ResourceDrone(Transform body, Transform port, Transform[] plates)
        {
            this.body = body;
            this.port = port;
            this.plates = plates;

            collected = 0;
            targetsLeft = Research.research_resource_amount;
            platesOpening = true;
            platesClosing = false;
            check = true;
            targetType = 1;
            storagesAvailable = true;

            visitedCollectors = new List<CollectorAI>();
            availableCollectors = new List<CollectorAI>();
            availableStorages = new List<StorageAI>();
        }

        public Transform[] plates;
        public bool storagesAvailable;
        public List<CollectorAI> visitedCollectors;
        public List<CollectorAI> availableCollectors;
        public List<StorageAI> availableStorages;
        public Transform body;
        public Transform port;
        public Transform target;
        public CollectorAI targetScript;
        public int collected;
        public int targetsLeft;
        public int targetType;
        public bool check;

        // Animation factors
        public bool platesOpening;
        public bool platesClosing;
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

    // Camera zoom object
    public CameraScroll cameraScroll;
    public AudioClip placementSound;

    // Survival
    public Survival survival;
    public LayerMask layer;
    public bool isMenu = false;

    // Update is called once per frame
    void Update()
    {
        // Check available drones
        checkConstructionDrones();

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
                continue;
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
                continue;
            }

            // Run through drone movement and collision detection
            else
            {
                // Update plates, if true continue (plates still opening)
                if (UpdateResourcePlates(drone)) continue;

                // Move towards target
                if (drone.target != null)
                {
                    drone.body.position = Vector2.MoveTowards(drone.body.position, drone.target.position, Research.research_resource_speed * Time.deltaTime);

                    if (Vector2.Distance(drone.body.position, drone.target.position) < 0.1f)
                    {

                        // Check what the current target is
                        switch (drone.targetType)
                        {
                            // Collector target reached
                            case 1:

                                // Add gold to drone and mark off collector
                                drone.collected += drone.targetScript.GrabResources();
                                drone.visitedCollectors.Add(drone.targetScript);
                                drone.targetsLeft -= 1;

                                // Attempt to find another collector
                                bool validCollectorTarget = false;
                                if (drone.targetsLeft > 0)
                                    validCollectorTarget = findResourceTarget(drone);

                                // If no valid collector found, locate storage
                                if (!validCollectorTarget)
                                {
                                    bool validStorageTarget = findStorageTarget(drone);
                                    if (!validStorageTarget) returnResourceToParent(drone);
                                }

                                break;

                            // Storage target reached
                            case 2:

                                // See how much gold the storage can hold
                                drone.collected = drone.target.GetComponent<StorageAI>().addResources(drone.collected);

                                // Animate building
                                AnimateThenStop animScript = drone.target.GetComponent<AnimateThenStop>();
                                animScript.resetAnim();
                                animScript.animEnabled = true;
                                animScript.enabled = true;

                                // Set drone to return to parent or look for anoter storage
                                if (drone.collected <= 0) returnResourceToParent(drone);
                                else if(!findStorageTarget(drone)) returnResourceToParent(drone);

                                break;

                            // Port target reached (reset drone)
                            case 3:
                                resetResourceDrone(drone);
                                break;
                        }
                    }
                }
                else
                {
                    if (!findResourceTarget(drone))
                        if (drone.collected <= 0 || !findStorageTarget(drone))
                            returnResourceToParent(drone);
                }
            }
        }
    }

    public void setupResourceDrone(ResourceDrone drone)
    {
        var colliders = Physics2D.OverlapCircleAll(drone.port.transform.position, Research.research_resource_range, layer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.name.Contains("Collector")) drone.availableCollectors.Add(collider.GetComponent<CollectorAI>());
            else if (collider.name.Contains("Storage")) drone.availableStorages.Add(collider.GetComponent<StorageAI>());
        }
    }

    public void forceUpdateResourceDrones()
    {
        for (int i = 0; i < resourceDrone.Count; i++)
            if (!resourceDrone[i].storagesAvailable)
                resourceDrone[i].storagesAvailable = true;
    }

    public void updateResourceDrones(Transform building)
    {
        var colliders = Physics2D.OverlapCircleAll(building.position, Research.research_resource_range, layer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.name == "Drone Port")
            {
                foreach(ResourceDrone drone in resourceDrone)
                {
                    if (drone.port == collider.transform)
                    {
                        if (building.name.Contains("Collector")) drone.availableCollectors.Add(building.GetComponent<CollectorAI>());
                        else if (building.name.Contains("Storage"))
                        {
                            drone.availableStorages.Add(building.GetComponent<StorageAI>());
                            drone.storagesAvailable = true;
                        }
                        break;
                    }
                }
            }
        }
    }

    public bool findResourceTarget(ResourceDrone drone)
    {
        if (isMenu)
        {
            CollectorAI randomizer = drone.availableCollectors[Random.Range(0, drone.availableCollectors.Count)];
            drone.target = randomizer.getPosition();
            Vector2 lookDirection = new Vector2(drone.target.position.x, drone.target.position.y) - new Vector2(drone.body.position.x, drone.body.position.y);
            drone.body.eulerAngles = new Vector3(0, 0, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f);
            drone.targetScript = randomizer;
            drone.targetType = 1;
            return true;
        }

        CollectorAI holder = null;
        int mostResources = -1;

        for(int i = 0; i < drone.availableCollectors.Count; i++)   
        {
            CollectorAI collector = drone.availableCollectors[i];
            if (collector == null)
            {
                drone.availableCollectors.Remove(collector);
                continue;
            }
            else if (!drone.visitedCollectors.Contains(collector) && collector.collected > mostResources) 
            {
                holder = collector;
                mostResources = collector.collected;
                drone.target = collector.getPosition();
                Vector2 lookDirection = new Vector2(drone.target.position.x, drone.target.position.y) - new Vector2(drone.body.position.x, drone.body.position.y);
                drone.body.eulerAngles = new Vector3(0, 0, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f);
                drone.targetScript = collector;
                drone.targetType = 1;
            }
        }

        if (holder != null) return true;
        else return false;
    }

    public bool findStorageTarget(ResourceDrone drone)
    {
        // Keep track of closest target
        float closest = Mathf.Infinity;
        int total = drone.availableStorages.Count;
        int index = -1;

        // Loop through all available storages
        StorageAI storage;
        for (int i = 0; i < total; i++)
        {
            // Check if storage is null
            storage = drone.availableStorages[i];
            if (storage == null)
            {
                drone.availableStorages.Remove(storage);
                total -= 1;
                i--;
                continue;
            }

            // If storage is not full, grab it's distance from the drones current position 
            else if (!storage.isFull)
            {
                // If storage is not full, grab it's distance from the drones current position 
                float distance = Vector2.Distance(storage.getPosition().position, drone.body.transform.position);
                if (distance < closest)
                {
                    index = i;
                    closest = distance;
                }
            }
        }

        // Set drone target if a storage is available, or go home.
        if (index != -1)
        {
            drone.target = drone.availableStorages[index].getPosition();
            Vector2 lookDirection = new Vector2(drone.target.position.x, drone.target.position.y) - new Vector2(drone.body.position.x, drone.body.position.y);
            drone.body.eulerAngles = new Vector3(0, 0, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f);
            drone.targetType = 2;
            return true;
        }
        else
        {
            drone.storagesAvailable = false;
            return false;
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
        if (isMenu)
        {
            clearResourceDrone(drone);
            return;
        }

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
        if (drone.body != null)
        {
            drone.target = drone.port;
            Vector2 lookDirection = new Vector2(drone.port.position.x, drone.port.position.y) - new Vector2(drone.body.position.x, drone.body.position.y);
            drone.body.eulerAngles = new Vector3(0, 0, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f);
            drone.targetType = 3;
        }
        else resourceDrone.Remove(drone);
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
        if (isMenu)
        {
            drone.platesOpening = false;
            return false;
        }

        // Check on the first run that there are valid targets
        if (drone.check)
            if (drone.storagesAvailable && drone.availableCollectors.Count > 0 && drone.availableStorages.Count > 0)
                drone.check = false;
            else return true;

        // Open the plates
        if (drone.platesOpening)
        {
            drone.plates[0].Translate(Vector3.left * Time.deltaTime * 4f);
            drone.plates[1].Translate(Vector3.right * Time.deltaTime * 4f);

            if (drone.body.localScale.x <= 1f)
                drone.body.localScale += new Vector3(0.002f, 0.002f, 0f);

            if (drone.plates[1].localPosition.x >= 2)
            {
                // Set port opening to false
                drone.platesOpening = false;

                // Make drone appear above all panels
                int tracker = 21;
                foreach (Transform child in drone.body)
                {
                    child.GetComponent<SpriteRenderer>().sortingOrder = tracker;
                    tracker++;
                }
            }
            else return true;
        }

        // Close the plates
        else if (drone.platesClosing)
        {
            drone.plates[0].Translate(Vector3.right * Time.deltaTime * 4f);
            drone.plates[1].Translate(Vector3.left * Time.deltaTime * 4f);

            if (drone.body.localScale.x >= 0.8f)
                drone.body.localScale -= new Vector3(0.002f, 0.002f, 0f);

            if (drone.plates[1].localPosition.x <= 0)
            {
                drone.plates[0].localPosition = new Vector2(0, 0);
                drone.plates[1].localPosition = new Vector2(0, 0);
                drone.platesClosing = false;

                // Reset drone so it's ready to go again
                clearResourceDrone(drone);
            }
            else return true;
        }
        return false;
    }

    public void clearResourceDrone(ResourceDrone drone)
    {
        drone.collected = 0;
        drone.targetType = 1;
        drone.targetsLeft = Research.research_resource_amount;
        drone.platesOpening = true;
        drone.platesClosing = false;
        drone.body.position = drone.port.position;
        drone.body.localScale = new Vector2(0.8f, 0.8f);
        drone.visitedCollectors = new List<CollectorAI>();
        drone.target = null;
        drone.targetScript = null;
        drone.check = true;
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
    public ResourceDrone registerResourceDrone(Transform body, Transform port, Transform[] plates)
    {
        ResourceDrone drone = new ResourceDrone(body, port, plates);
        setupResourceDrone(drone);
        resourceDrone.Add(drone);
        return drone;
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
