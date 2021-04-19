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
        public ConstructionDrone(Transform body, Transform targetPos, Transform targetBuilding, float droneSpeed)
        {
            this.body = body;
            this.targetPos = targetPos;
            this.targetBuilding = targetBuilding;
            this.droneSpeed = droneSpeed;

            isAnimating = false;
            isMoving = false;
            finderCheck = Random.Range(0, 10);
        }

        public Transform body;
        public Transform targetPos;
        public Transform targetBuilding;
        public float droneSpeed;
        public bool isMoving;
        public bool isAnimating;
        public int finderCheck;
    }
    List<ConstructionDrone> constructionDrones;

    // Update is called once per frame
    void Update()
    {
        foreach(ConstructionDrone drone in constructionDrones)
        {
            if (drone.isAnimating)
            {
                if (drone.body.localScale.x < 1f)
                {
                    drone.body.localScale = new Vector2(drone.body.localScale.x + 0.1f, drone.body.localScale.y + 0.1f);
                }
                else drone.isAnimating = false;
            }
            else
            {

            }
        }
    }

    public void registerConstructionDrone(Transform body, Transform targetPos, Transform targetBuilding)
    {
        constructionDrones.Add(new ConstructionDrone(body, targetPos, targetBuilding, 1f));
    }
}
