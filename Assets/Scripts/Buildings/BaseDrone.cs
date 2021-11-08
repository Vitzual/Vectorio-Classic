using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseDrone : BaseTile
{
    // Holds a reference to scriptable object
    public Building building;

    // Holds the drone type and object
    public Drone drone;
    public GameObject _droneObj;

    // Nearby targets
    [HideInInspector] public List<BaseEntity> nearbyTargets;

    // Side panels 
    public Transform leftPanel;
    public Transform rightPanel;

    // Locate nearby buildings for drone
    public override void Setup()
    {
        // Reset nearby targets
        nearbyTargets = new List<BaseEntity>();

        // Loop through all nearby drone ports
        int adjustment = Research.research_drone_coverage * 5;
        int xTile = (int)transform.position.x;
        int yTile = (int)transform.position.y;

        // Loop through all tiles and try to find drones
        for (int x = xTile - adjustment; x <= xTile + adjustment; x += 5)
            for (int y = yTile - adjustment; y <= yTile + adjustment; y += 5)
                AddTarget(InstantiationHandler.active.TryGetBuilding(new Vector2(x, y)));

        // Set material
        material = building.material;
    }

    // Creates the drone
    public virtual void AddTarget(BaseTile tile) { }

    // Switch drone
    public void SwitchDrone()
    {

    }

    // Destroy entity
    public override void DestroyEntity()
    {
        Recycler.AddRecyclable(drone.transform);
        base.DestroyEntity();
    }
}
