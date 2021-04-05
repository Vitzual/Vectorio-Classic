// This script handles all active drones each frame

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    [System.Serializable]
    public class FixerDrones
    {
        public FixerDrones(Transform body, float droneSpeed, float droneHealth, float fixAmount, float fixTime)
        {
            this.body = body;
            this.droneSpeed = droneSpeed;
            this.droneHealth = droneHealth;
            this.fixAmount = fixAmount;
            this.fixTime = fixTime;

            targetClass = body.GetComponent<TileClass>();
            isFixing = false;
            isAnimating = false;
            isMoving = false;
            finderCheck = Random.Range(0,10);
        }

        public TileClass targetClass;
        public Transform body;
        public Transform target;
        public float droneSpeed;
        public float droneHealth;
        public float fixAmount;
        public float fixTime;
        public bool isMoving;
        public bool isFixing;
        public bool isAnimating;
        public int finderCheck;
    }
    List<FixerDrones> fixerDrones;

    // Update is called once per frame
    void Update()
    {
        foreach(FixerDrones drone in fixerDrones)
        {
            if (drone.target != null)
            {

            }
            else if (drone.finderCheck == 10)
            {
                drone.finderCheck = 0;
                findFixerTarget(drone.body.position);
            }
            else drone.finderCheck++;
        }
    }

    public void registerFixerDrone(Transform body, float droneSpeed, float droneHealth, float fixAmount, float fixTime)
    {
        fixerDrones.Add(new FixerDrones(body, droneSpeed, droneHealth, fixAmount, fixTime));
    }

    protected void findFixerTarget(Vector3 pos)
    {

    }
}
