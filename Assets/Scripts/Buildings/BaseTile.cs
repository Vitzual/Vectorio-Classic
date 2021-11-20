using UnityEngine;
//using Mirror;
using System.Collections.Generic;

public class BaseTile : BaseEntity
{
    [HideInInspector] public List<Vector2Int> cells;
    [HideInInspector] public Buildable buildable;
    public bool saveBuilding = true;

    public override void Setup()
    {
        DroneManager.active.UpdateNearbyPorts(this, transform.position);
        Buildables.UpdateEntityUnlockables(Unlockable.UnlockType.PlaceBuildingAmount, buildable.building, 1);
    }

    public virtual void OnClick()
    {
        // Override this method for on click behaviour
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
        Resource.active.ApplyResources(buildable, true);

        // Create particle and destroy
        if (particle != null)
            Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
