using UnityEngine;
using Mirror;
using System.Collections.Generic;

[HideInInspector]
public class BaseTile : BaseEntity
{
    [HideInInspector]
    public List<Vector2Int> cells;

    public override void Setup()
    {
        Debug.LogError("This object has a BaseTile script attached to it!\n" +
            "Please use a default script that inherits from BaseTile instead.");
    }

    public override void DestroyEntity()
    {
        if (BuildingHandler.active != null)
        {
            foreach (Vector2Int cell in cells)
                BuildingHandler.active.tileGrid.RemoveCell(cell);
        }

        if (particle != null)
            Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
