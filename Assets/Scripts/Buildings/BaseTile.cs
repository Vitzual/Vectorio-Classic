using UnityEngine;
//using Mirror;
using System.Collections.Generic;

public class BaseTile : BaseEntity
{
    [HideInInspector] public List<Vector2Int> cells;
    [HideInInspector] public Buildable buildable;

    public override void Setup()
    {
        buildable = Buildables.RequestBuildable(name);
        if (buildable == null)
        {
            Debug.Log("The buildable for this object could not be retrieved!");
            DestroyEntity();
        }
        DroneManager.active.UpdateNearbyPorts(this, transform.position);

        // Update unlockables
        Buildables.UpdateEntityUnlockables(Unlockable.UnlockType.PlaceBuildingAmount, buildable.building, 1);
    }

    public virtual void OnClick()
    {
        // Override this method for on click behaviour
    }

    public override void DestroyEntity()
    {
        // Update unlockables
        Buildables.UpdateEntityUnlockables(Unlockable.UnlockType.PlaceBuildingAmount, buildable.building, -1);

        if (InstantiationHandler.active != null)
        {
            foreach (Vector2Int cell in cells)
                InstantiationHandler.active.tileGrid.RemoveCell(cell);
        }

        if (particle != null)
            Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
