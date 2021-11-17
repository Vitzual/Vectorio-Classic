using UnityEngine;
//using Mirror;
using System.Collections.Generic;

[HideInInspector]
public class BaseTile : BaseEntity
{
    [HideInInspector]
    public List<Vector2Int> cells;

    public override void Setup()
    {
        DroneManager.active.UpdateNearbyPorts(this, transform.position);
    }

    public virtual void OnClick()
    {
        // Override this method for on click behaviour
    }


    public override void DestroyEntity()
    {
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
