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
        // Loop through all nearby drone ports
        int adjustment = Research.drone_tile_coverage * 5;
        int xTile = (int)transform.position.x;
        int yTile = (int)transform.position.y;

        // Loop through all tiles and try to find drones
        for (int x = xTile - adjustment; x <= xTile + adjustment; x += 5) 
        {
            for (int y = yTile - adjustment; y <= yTile + adjustment; y += 5) 
            {
                BaseTile holder = InstantiationHandler.active.TryGetBuilding(new Vector2(x, y));
                if (holder != null)
                {
                    Droneport droneport = holder.GetComponent<Droneport>();
                    if (droneport != null) droneport.AddTarget(this);
                }
            }
        } 
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
