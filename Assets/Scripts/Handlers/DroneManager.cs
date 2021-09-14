// This script handles all active drones each frame
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
        public ConstructionDrone(Transform body, Transform target, Transform targetBuilding, Transform spawnPosition, Transform[] plates, bool isHubDrone, int gold, int power, int heat, SpriteRenderer buildingIcon, bool freeBuilding = false)
        {
            this.body = body;
            this.target = target;
            this.targetBuilding = targetBuilding;
            this.spawnPosition = spawnPosition;
            this.plates = plates;
            this.isHubDrone = isHubDrone;
            this.buildingIcon = buildingIcon;
            buildingIcon.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/" + targetBuilding.name);

            goldCost = gold;
            powerCost = power;
            heatCost = heat;
            this.freeBuilding = freeBuilding;

            targetPos = target.position;
            targetPosHolder = targetPos;
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
        public bool freeBuilding;

        public SpriteRenderer buildingIcon;
        public Transform body;
        public Transform target;
        public Vector2 targetPos;
        public Vector2 targetPosHolder;
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

            collectedGold = 0;
            collectedEssence = 0;
            collectedIridium = 0;
            visitedGold = false;
            visitedEssence = false;
            visitedIridium = false;
            targetsLeft = Research.research_resource_amount;
            platesOpening = true;
            platesClosing = false;
            check = true;
            targetType = 1;
            storagesAvailable = true;

            visitedCollectors = new List<Collector>();
            availableCollectors = new List<Collector>();
            availableStorages = new List<Storage>();
        }

        public Transform[] plates;
        public bool storagesAvailable;
        public List<Collector> visitedCollectors;
        public List<Collector> availableCollectors;
        public List<Storage> availableStorages;
        public Transform body;
        public Transform port;
        public Transform target;
        public Collector targetScript;
        public int collectedGold;
        public bool visitedGold;
        public int collectedEssence;
        public bool visitedEssence;
        public int collectedIridium;
        public bool visitedIridium;
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
        public AvailableConstructionDrones(Transform body, Transform port, Transform[] plates, SpriteRenderer buildingIcon, bool isHubDrone)
        {
            this.body = body;
            this.port = port;
            this.plates = plates;
            this.isHubDrone = isHubDrone;
            this.buildingIcon = buildingIcon;
        }

        public Transform[] plates;
        public Transform body;
        public Transform port;
        public bool isHubDrone;
        public SpriteRenderer buildingIcon;
    }
    public List<AvailableConstructionDrones> availableConstructionDrones;


    // ----------------------------------------------------------------------------------- //
    // BUILDING QUEUE LIST
    // Holds the position of the building that needs to be placed, and which building it is
    // ----------------------------------------------------------------------------------- //

    [System.Serializable]
    public class BuildingQueue
    {
        public BuildingQueue(Transform building, Transform buildingPos, int gold, int power, int heat, bool isFree = false)
        {
            this.building = building;
            this.buildingPos = buildingPos;
            goldCost = gold;
            powerCost = power;
            heatCost = heat;
            freeBuilding = isFree;
        }

        public Transform building;
        public Transform buildingPos;
        public int goldCost;
        public int powerCost;
        public int heatCost;
        public bool freeBuilding;
    }
    public List<BuildingQueue> buildingQueue;

    // Camera zoom object
    public CameraScroll cameraScroll;
    public AudioClip placementSound;
    public Tutorial tutorial;

    // Survival
    public LayerMask layer;
    public bool isMenu = false;

    public Sprite transparent;

    public void Start()
    {
        Events.active.onCollectorPlaced += AddCollectorReference;
        Events.active.onStoragePlaced += AddStorageReference;
    }

    // Update is called once per frame
    public void Update()
    {
        // Check available drones
        CheckConstructionDrones();

        // Handles the construction drone logic
        for (int i = 0; i < constructionDrones.Count; i++)
        {
            ConstructionDrone drone = constructionDrones[i];

            // If the drone becomes null (parent is removed), remove drone and add back building to available buildings
            if (drone.body == null)
            {
                // Update resource values
                RevertResources(drone.goldCost, drone.powerCost, drone.heatCost);

                // Queue the building if not yet placed
                if (!drone.droneReturning) QueueBuilding(drone.targetBuilding, drone.target, drone.goldCost, drone.powerCost, drone.heatCost);
                
                // Remove from active pool and do not add back to inactive
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
                            PlaceBuilding(drone);
                            ReturnConstructionToParent(drone);
                        }
                        else
                        {
                            // Reset drone so it's ready to go again
                            if (!drone.platesClosing) ResetConstructionDrone(drone);
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
                                int holder;
                                switch(drone.targetScript.collectorType)
                                {
                                    case 1:
                                        drone.visitedGold = true;
                                        holder = drone.collectedGold;
                                        drone.collectedGold += drone.targetScript.GrabResources();
                                        if (holder != drone.collectedGold) drone.targetsLeft -= 1;
                                        break;
                                    case 2:
                                        drone.visitedEssence = true;
                                        holder = drone.collectedEssence;
                                        drone.collectedEssence += drone.targetScript.GrabResources();
                                        if(holder != drone.collectedEssence) drone.targetsLeft -= 1;
                                        break;
                                    case 3:
                                        drone.visitedIridium = true;
                                        holder = drone.collectedIridium;
                                        drone.collectedIridium += drone.targetScript.GrabResources();
                                        if(holder != drone.collectedIridium) drone.targetsLeft -= 1;
                                        break;
                                    default:
                                        drone.visitedGold = true;
                                        holder = drone.collectedGold;
                                        drone.collectedGold += drone.targetScript.GrabResources();
                                        if(holder != drone.collectedGold) drone.targetsLeft -= 1;
                                        break;
                                }
                                drone.visitedCollectors.Add(drone.targetScript);

                                // Attempt to find another collector
                                bool validCollectorTarget = false;
                                if (drone.targetsLeft > 0)
                                    validCollectorTarget = FindResourceTarget(drone);

                                // If no valid collector found, locate storage
                                if (!validCollectorTarget)
                                {
                                    bool validStorageTarget = FindStorageTarget(drone);
                                    if (!validStorageTarget) ReturnResourceToParent(drone);
                                }

                                break;

                            // Storage target reached
                            case 2:

                                // See how much gold the storage can hold
                                if (drone.collectedGold > 0) drone.collectedGold = drone.target.GetComponent<Storage>().AddResources(drone.collectedGold);
                                else if (drone.collectedEssence > 0) drone.collectedEssence = drone.target.GetComponent<Storage>().AddResources(drone.collectedEssence);
                                else if (drone.collectedIridium > 0) drone.collectedIridium = drone.target.GetComponent<Storage>().AddResources(drone.collectedIridium);

                                // Animate building
                                AnimateThenStop animScript = drone.target.GetComponent<AnimateThenStop>();
                                animScript.resetAnim();
                                animScript.animEnabled = true;
                                animScript.enabled = true;

                                // Set values
                                if (drone.visitedGold && drone.collectedGold <= 0) drone.visitedGold = false;
                                if (drone.visitedEssence && drone.collectedEssence <= 0) drone.visitedEssence = false;
                                if (drone.visitedIridium && drone.collectedIridium <= 0) drone.visitedIridium = false;

                                // Set drone to return to parent or look for anoter storage
                                if (!drone.visitedGold && !drone.visitedEssence && !drone.visitedIridium) ReturnResourceToParent(drone);
                                else if(!FindStorageTarget(drone)) ReturnResourceToParent(drone);

                                break;

                            // Port target reached (reset drone)
                            case 3:
                                ResetResourceDrone(drone);
                                break;
                        }
                    }
                }
                else
                {
                    if (!FindResourceTarget(drone))
                        if ((drone.collectedGold <= 0 && drone.collectedEssence <= 0 && drone.collectedIridium <= 0) || !FindStorageTarget(drone))
                            ReturnResourceToParent(drone);
                }
            }
        }
    }

    public void SetupResourceDrone(ResourceDrone drone)
    {
        var colliders = Physics2D.OverlapCircleAll(drone.port.transform.position, Research.research_resource_range, layer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.name.Contains("Collector")) drone.availableCollectors.Add(collider.GetComponent<Collector>());
            else if (collider.name.Contains("Storage")) drone.availableStorages.Add(collider.GetComponent<Storage>());
        }
    }

    public void ForceUpdateResourceDrones()
    {
        for (int i = 0; i < resourceDrone.Count; i++)
            if (!resourceDrone[i].storagesAvailable)
                resourceDrone[i].storagesAvailable = true;
    }

    public void AddCollectorReference(Collector collector)
    {
        var colliders = Physics2D.OverlapCircleAll(collector.transform.position, Research.research_resource_range, layer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.name == "Drone Port")
            {
                foreach(ResourceDrone drone in resourceDrone)
                {
                    if (drone.port == collider.transform)
                    {
                        drone.availableCollectors.Add(collector);
                        break;
                    }
                }
            }
        }
    }

    public void AddStorageReference(Storage storage)
    {
        var colliders = Physics2D.OverlapCircleAll(storage.transform.position, Research.research_resource_range, layer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.name == "Drone Port")
            {
                foreach (ResourceDrone drone in resourceDrone)
                {
                    if (drone.port == collider.transform)
                    {
                        drone.availableStorages.Add(storage);
                        drone.storagesAvailable = true;
                        break;
                    }
                }
            }
        }
    }

    public bool FindResourceTarget(ResourceDrone drone)
    {
        if (isMenu)
        {
            if (drone.availableCollectors.Count == 0) return false;
            Collector randomizer = drone.availableCollectors[Random.Range(0, drone.availableCollectors.Count)];
            drone.target = randomizer.transform;
            Vector2 lookDirection = new Vector2(drone.target.position.x, drone.target.position.y) - new Vector2(drone.body.position.x, drone.body.position.y);
            drone.body.eulerAngles = new Vector3(0, 0, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f);
            drone.targetScript = randomizer;
            drone.targetType = 1;
            return true;
        }

        Collector holder = null;
        int mostResources = -1;

        for(int i = 0; i < drone.availableCollectors.Count; i++)   
        {
            Collector collector = drone.availableCollectors[i];
            if (collector == null)
            {
                drone.availableCollectors.Remove(collector);
                continue;
            }
            else if (!drone.visitedCollectors.Contains(collector) && collector.collected > mostResources) 
            {
                holder = collector;
                mostResources = collector.collected;
                drone.target = collector.transform;
                Vector2 lookDirection = new Vector2(drone.target.position.x, drone.target.position.y) - new Vector2(drone.body.position.x, drone.body.position.y);
                drone.body.eulerAngles = new Vector3(0, 0, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f);
                drone.targetScript = collector;
                drone.targetType = 1;
            }
        }

        if (holder != null) return true;
        else return false;
    }

    public bool FindStorageTarget(ResourceDrone drone)
    {
        // Keep track of closest target
        float closest = Mathf.Infinity;
        int total = drone.availableStorages.Count;
        int index = -1;
        int type = 1;

        if (drone.visitedGold) type = 1;
        else if (drone.visitedEssence) type = 2;
        else if (drone.visitedIridium) type = 3;

        // Loop through all available storages
        Storage storage;
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

            else if (!storage.isFull && storage.type == type)
            {
                // If storage is not full, grab it's distance from the drones current position 
                float distance = Vector2.Distance(storage.GetPosition().position, drone.body.transform.position);
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
            drone.target = drone.availableStorages[index].GetPosition();
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
    public void CheckConstructionDrones()
    {
        float closest = Mathf.Infinity;
        int[] available = new int[] { -1, -1 };

        for (int a = 0; a < buildingQueue.Count; a++)
        {
            // Check if building queue still available
            if (buildingQueue[a].buildingPos == null)
                buildingQueue.Remove(buildingQueue[a]);

            // Check if adequate resources for a drone to be deployed
            if (!buildingQueue[a].freeBuilding)
            {
                // if (survival.Spawner.htrack >= survival.Spawner.maxHeat && buildingQueue[a].heatCost > 0) continue;
                // if (survival.PowerConsumption >= survival.AvailablePower && buildingQueue[a].powerCost > 0) continue;
                // if (buildingQueue[a].goldCost > survival.gold && buildingQueue[a].goldCost > 0) continue;
            }

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
            // Grab the index values
            int a = available[0];
            int b = available[1];

            // Update resource values
            if(!buildingQueue[a].freeBuilding) ApplyResources(buildingQueue[a].goldCost, buildingQueue[a].powerCost, buildingQueue[a].heatCost);

            // Register the construction drone
            RegisterConstructionDrone(availableConstructionDrones[b].body, buildingQueue[a].buildingPos, buildingQueue[a].building, availableConstructionDrones[b].port, 
                availableConstructionDrones[b].plates, availableConstructionDrones[b].isHubDrone, buildingQueue[a].goldCost, buildingQueue[a].powerCost, 
                buildingQueue[a].heatCost, availableConstructionDrones[b].buildingIcon, buildingQueue[a].freeBuilding);

            // Remove index from the pool
            availableConstructionDrones.Remove(availableConstructionDrones[b]);
            buildingQueue.Remove(buildingQueue[a]);

            // Update the UI
            // survival.UI.updateDronesUI(availableConstructionDrones.Count, availableConstructionDrones.Count + constructionDrones.Count);
        }
    }

    // Reset the drone after it's task is complete
    public void ResetConstructionDrone(ConstructionDrone drone)
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
    public void ResetResourceDrone(ResourceDrone drone)
    {
        if (isMenu)
        {
            ClearResourceDrone(drone);
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
    public void ReturnConstructionToParent(ConstructionDrone drone)
    {
        drone.targetPos = drone.spawnPosition.position;
        drone.target = drone.spawnPosition;
        Vector2 lookDirection = new Vector2(drone.spawnPosition.position.x, drone.spawnPosition.position.y) - new Vector2(drone.body.position.x, drone.body.position.y);
        drone.body.eulerAngles = new Vector3(0, 0, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f);
        drone.droneReturning = true;
    }

    // Set the drone to return back to the parent
    public void ReturnResourceToParent(ResourceDrone drone)
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
                    drone.buildingIcon.sprite = transparent;
                    RegisterAvailableConstructionDrone(drone.body, drone.spawnPosition, drone.plates, drone.isHubDrone, drone.buildingIcon);
                    // survival.UI.updateDronesUI(availableConstructionDrones.Count, availableConstructionDrones.Count + constructionDrones.Count - 1);
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
            // if (survival.gold < survival.goldStorage && drone.storagesAvailable && drone.availableCollectors.Count > 0 && drone.availableStorages.Count > 0)
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
                ClearResourceDrone(drone);
            }
            else return true;
        }
        return false;
    }

    public void ClearResourceDrone(ResourceDrone drone)
    {
        drone.collectedGold = 0;
        drone.collectedEssence = 0;
        drone.collectedIridium = 0;
        drone.targetType = 1;
        drone.targetsLeft = Research.research_resource_amount;
        drone.platesOpening = true;
        drone.platesClosing = false;
        drone.body.position = drone.port.position;
        drone.body.localScale = new Vector2(0.8f, 0.8f);
        drone.visitedCollectors = new List<Collector>();
        drone.target = null;
        drone.targetScript = null;
        drone.check = true;
    }

    // Attempt to place a building
    public void PlaceBuilding(ConstructionDrone drone)
    {
        // Create the new building and remove the ghost version
        var LastObj = Instantiate(drone.targetBuilding, drone.targetPos, Quaternion.Euler(new Vector3(0, 0, 0)));
        LastObj.name = drone.targetBuilding.name;
        // survival.ghostBuildings.Remove(new Vector2(drone.target.position.x, drone.target.position.y));
        Destroy(drone.target.gameObject);
        drone.buildingIcon.sprite = transparent;

        if (tutorial.tutorialStarted)
        {
            if (LastObj.name == "Drone Port" && tutorial.tutorialSlide == 3) tutorial.nextSlide();
            else if (LastObj.name == "Gold Collector" && tutorial.tutorialSlide == 7) tutorial.nextSlide();
            else if (LastObj.name == "Gold Storage" && tutorial.tutorialSlide == 10) tutorial.nextSlide();
            else if (LastObj.name == "Turret" && tutorial.tutorialSlide == 13) tutorial.nextSlide();
        }

        // Create a UI resource popup thing idk lmaooo
        // if (!drone.freeBuilding)
        //     survival.UI.CreateResourcePopup("- " + drone.goldCost, "Gold", drone.targetPos);

        // Play audio
        float audioScale = CameraScroll.getZoom() / 1400f;
        AudioSource.PlayClipAtPoint(placementSound, LastObj.transform.position, Settings.soundVolume - audioScale);

        // Check for nearby enemy buildings
        var colliders = Physics2D.OverlapCircleAll(
            this.gameObject.transform.position,
            100 + Research.research_range,
            1 << LayerMask.NameToLayer("Enemy Defense"));

        //foreach (Collider2D enemyTurret in colliders)
        //{
        //    enemyTurret.gameObject.GetComponent<DefaultTurret>().enabled = true;
        //}

        return;
    }

    public void ForceCheckAvailableDrones()
    {
        for (int i = 0; i < availableConstructionDrones.Count; i++)
        {
            if (availableConstructionDrones[i].body == null)
            {
                availableConstructionDrones.RemoveAt(i);
                i--;
            }
        }
    }

    // Register an active resource drone
    public ResourceDrone RegisterResourceDrone(Transform body, Transform port, Transform[] plates)
    {
        ResourceDrone drone = new ResourceDrone(body, port, plates);
        SetupResourceDrone(drone);
        resourceDrone.Add(drone);
        return drone;
    }

    // Register an active construction drone
    public void RegisterConstructionDrone(Transform body, Transform targetPos, Transform targetBuilding, Transform startingPos, Transform[] plates, bool isHubDrone, int gold, int power, int heat, SpriteRenderer buildingIcon, bool freeBuilding = false)
    {
        constructionDrones.Add(new ConstructionDrone(body, targetPos, targetBuilding, startingPos, plates, isHubDrone, gold, power, heat, buildingIcon, freeBuilding));
    }

    // Register an available construction drone
    public void RegisterAvailableConstructionDrone(Transform body, Transform port, Transform[] plates, bool isHubDrone, SpriteRenderer buildingIcon)
    {
        availableConstructionDrones.Add(new AvailableConstructionDrones(body, port, plates, buildingIcon, isHubDrone));
    }

    // Queue a building to be placed
    public void QueueBuilding(Transform building, Transform ghostBuilding, int gold, int power, int heat)
    {
        // Checks to see if a drone port should be counted as free or not
        if (building.name == "Drone Port" && constructionDrones.Count == 0)

            // Queues the building for free
            buildingQueue.Add(new BuildingQueue(building, ghostBuilding, gold, power, heat, true));

        // Checks to see if a gold storage should be counted as free or not
        else if (building.name == "Gold Storage" && constructionDrones.Count == 0)

            // Queues the building for free
            buildingQueue.Add(new BuildingQueue(building, ghostBuilding, gold, power, heat, true));

        // Checks to see if a gold collector should be counted as free or not
        else if (building.name == "Gold Collector" && constructionDrones.Count == 0)

            // Queues the building for free
            buildingQueue.Add(new BuildingQueue(building, ghostBuilding, gold, power, heat, true));

        // Queues a building as the system normally would
        else buildingQueue.Add(new BuildingQueue(building, ghostBuilding, gold, power, heat, false));
    }

    // Remove a queued building
    public bool DequeueBuilding(Transform ghost)
    {
        // Check the building queue
        for (int i = 0; i < buildingQueue.Count; i++)
        {
            if (buildingQueue[i].buildingPos.position == ghost.position)
            {
                // survival.ghostBuildings.Remove(ghost.position);
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
                RevertResources(constructionDrones[i].goldCost, constructionDrones[i].powerCost, constructionDrones[i].heatCost);

                // survival.ghostBuildings.Remove(ghost.position);
                Destroy(ghost.gameObject);
                ReturnConstructionToParent(constructionDrones[i]);
                return true;
            }
        }

        return false;
    }

    // Forces a UI update
    public void ForceUI()
    {
        // if (!isMenu) survival.UI.updateDronesUI(availableConstructionDrones.Count, availableConstructionDrones.Count + constructionDrones.Count);
    }

    public void ApplyResources(int gold, int power, int heat)
    {
        Resource.Remove(Resource.Currency.Gold, gold);
        Resource.Remove(Resource.Currency.Power, power);
        Resource.Remove(Resource.Currency.Heat, heat);
    }

    public void RevertResources(int gold, int power, int heat)
    {
        Resource.Add(Resource.Currency.Gold, gold);
        Resource.Add(Resource.Currency.Power, power);
        Resource.Add(Resource.Currency.Heat, heat);
    }
}
