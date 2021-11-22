using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderDrone : Drone
{
    public bool buildingPlaced = false;
    public SpriteRenderer buildingIcon;

    public override void ExitPort()
    {
        buildingIcon.sortingOrder = 0;
        buildingPlaced = false;
        base.ExitPort();
    }

    public override void StartRoute()
    {
        buildingIcon.sortingOrder = 2;
        base.StartRoute();
    }

    public override void SetTarget(BaseTile tile)
    {
        buildingIcon.sprite = Sprites.GetSprite(tile.name);
        base.SetTarget(tile);
    }

    public override void TargetReached()
    {
        if (!buildingPlaced)
        {
            buildingIcon.sprite = Sprites.GetSprite("Transparent");
            GhostTile tile = target.GetComponent<GhostTile>();
            if (tile != null) tile.CreateBuilding();
            buildingPlaced = true;//
        }
        base.TargetReached();
    }

    public override void AddTarget(BaseTile tile)
    {
        if (tile == null) return;
        else if (tile.GetComponent<GhostTile>() != null) nearbyTargets.Add(tile);
    }

    public override void Destroy()
    {
        if (!buildingPlaced && target != null)
        {
            DroneManager.active.ghostTiles.Add(target.GetComponent<GhostTile>());
            Resource.active.RevertResources(target.GetComponent<GhostTile>().buildable);
        }
        base.Destroy();
    }
}
