using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDrone : Drone
{
    // This will be switched out for Resource
    public Dictionary<Resource.CurrencyType, int> collected;

    // Drone variables
    public bool collecting = true;
    public int maxTrips = 3;

    // Add a new resource target
    public override void AddTarget(BaseTile tile)
    {
        if (tile == null) return;
        else if (tile.GetComponent<ResourceTile>() != null) nearbyTargets.Add(tile);
    }

    // Get a new target
    public override bool FindTarget()
    {
        target = null;
        bool storagesAvailable = false;

        for (int b = 0; b < nearbyTargets.Count; b++)
        {
            if (nearbyTargets[b] != null)
            {
                if (!visitedTargets.Contains(nearbyTargets[b]))
                {
                    ResourceTile resourceTile = nearbyTargets[b].GetComponent<ResourceTile>();
                    if (!resourceTile.isStorage && collecting)
                    {
                        target = resourceTile;
                        resourceTile.hasAssignedDrone = true;
                    }
                    else if (resourceTile.isStorage && !resourceTile.isFull)
                    {
                        storagesAvailable = true;
                        if (!collecting && collected.ContainsKey(resourceTile.type))
                        {
                            target = resourceTile;
                        }
                    }
                }
            }
            else
            {
                nearbyTargets.RemoveAt(b);
                b--;
            }
        }

        if (storagesAvailable && stage == Stage.ReadyToDeploy)
        {
            ExitPort();
            return true;
        }
        else if (target == null && stage == Stage.MovingToTarget)
        {
            ReturnHome();
            return true;
        }
        else return false;
    }

    // Collects or store resources
    public override void TargetReached()
    {
        if (target == home) EnterPort();

        ResourceTile tile = target.GetComponent<ResourceTile>();

        if (collecting)
        {
            if (collected.ContainsKey(tile.type))
                collected[tile.type] += tile.amount;
            else collected.Add(tile.type, tile.amount);
            tile.hasAssignedDrone = false;

            maxTrips -= 1;
        }
        else
        {
            collected[tile.type] = tile.AddResources(collected[tile.type]);
            if (collected[tile.type] <= 0)
            {
                collected.Remove(tile.type);
                if (collected.Count <= 0) ReturnHome();
            }
            FindTarget();
        }

        if (maxTrips <= 0)
        {
            collecting = false;
            FindTarget();
        }
    }

    public override void FinishRoute()
    {
        collected = new Dictionary<Resource.CurrencyType, int>();
        collecting = true;
        maxTrips = 3;

        base.FinishRoute();
    }
}
