using UnityEngine;
//using Mirror;
using System.Collections.Generic;

public class BaseTile : BaseEntity
{
    [HideInInspector] public List<Vector2Int> cells;
    [HideInInspector] public Buildable buildable;
    public bool saveBuilding = true;
    public bool isSellable = true;

    public override void Setup()
    {
        // Update nearby ports and unlockables
        DroneManager.active.UpdateNearbyPorts(this, transform.position);
        Buildables.UpdateEntityUnlockables(Unlockable.UnlockType.PlaceBuildingAmount, buildable.building, 1);
    }

    public void CheckNearbyEnergizers()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, InstantiationHandler.active.aocbLayer);
        if (hit.collider == null) DestroyEntity();
    }

    public override void DestroyEntity()
    {
        // Update unlockables
        if(buildable != null) Buildables.UpdateEntityUnlockables(Unlockable.UnlockType.PlaceBuildingAmount, buildable.building, -1);

        // Remove cells
        if (InstantiationHandler.active != null)
        {
            foreach (Vector2Int cell in cells)
                InstantiationHandler.active.tileGrid.RemoveCell(cell);
        }

        // Refund cost
        if (buildable != null)
            Resource.active.ApplyResources(buildable, true);

        // Update damage handler
        Events.active.BuildingDestroyed(this);

        // Create particle and destroy
        if (particle != null)
            Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
