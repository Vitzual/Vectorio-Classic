using UnityEngine;
using Mirror;
using System.Collections.Generic;

[HideInInspector]
public class DefaultBuilding : DefaultEntity
{
    public override void DestroyEntity()
    {
        BuildingSystem.active.tileGrid.RemoveCell(Vector2Int.RoundToInt(transform.position));

        if (particle != null)
            Instantiate(particle, transform.position, transform.rotation);
        Recycler.AddRecyclable(transform);
    }
}
