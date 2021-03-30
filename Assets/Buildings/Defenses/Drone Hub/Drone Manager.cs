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

            isFixing = false;
            isAnimating = false;
        }

        public Transform body;
        public Transform target;
        public float droneSpeed;
        public float droneHealth;
        public float fixAmount;
        public float fixTime;
        public bool isFixing;
        public bool isAnimating;
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
            else findTarget(drone.body.position);
        }
    }

    public void registerFixerDrone(Transform body, float droneSpeed, float droneHealth, float fixAmount, float fixTime)
    {
        fixerDrones.Add(new FixerDrones(body, droneSpeed, droneHealth, fixAmount, fixTime));
    }

    protected void findTarget(Vector3 pos)
    {

    }
}
