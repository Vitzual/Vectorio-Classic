using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDrone : Drone
{
    // This will be switched out for Resource
    public Dictionary<Resource.CurrencyType, int> collected = new Dictionary<Resource.CurrencyType, int>();

    // Drone variables
    public bool storagesAvailable = false;
    public bool collecting = true;
    public int maxTrips = 3;

    // Add a new resource target
    public override void AddTarget(BaseTile tile)
    {
        if (tile == null) return;
        else if (tile.GetComponent<ResourceTile>() != null)
        {
            if (tile.GetComponent<ResourceTile>().isStorage)
                storagesAvailable = true;
            nearbyTargets.Add(tile);
        }
    }

    // Get a new target
    public override bool FindTarget()
    {
        // Update storage availability
        if (stage == Stage.ReadyToDeploy)
            return CheckDeploymentConditions();

        // Set target to null
        ResourceTile holder = null;
        float amount = 0;
        target = null;

        // Iterate through nearby targets
        for (int b = 0; b < nearbyTargets.Count; b++)
        {
            if (nearbyTargets[b] != null)
            {
                if (!visitedTargets.Contains(nearbyTargets[b]))
                {
                    ResourceTile resourceTile = nearbyTargets[b].GetComponent<ResourceTile>();
                    if (collecting)
                    {
                        if (!resourceTile.isStorage)
                        {
                            if (!resourceTile.hasAssignedDrone && resourceTile.amount > amount)
                            {
                                amount = resourceTile.amount;
                                target = resourceTile;
                            }
                        }
                        else if (holder == null && !resourceTile.isFull && collected.ContainsKey(resourceTile.type))
                        {
                            holder = resourceTile;
                        }
                    }
                    else 
                    {
                        if (resourceTile.isStorage && !resourceTile.isFull && collected.ContainsKey(resourceTile.type))
                        {
                            target = resourceTile;
                            RotateToTarget();
                            return true;
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

        // Check target with most resources
        if (target != null)
        {
            target.GetComponent<ResourceTile>().hasAssignedDrone = true;
            RotateToTarget();
            return true;
        }
        else if (collecting && holder != null)
        {
            collecting = false;
            target = holder;
            RotateToTarget();
            return true;
        }
        else
        {
            ReturnHome();
            return true;
        }
    }

    // Check targets
    public bool CheckDeploymentConditions()
    {
        // Setup storages available boolean
        if (!storagesAvailable) return false;
        else storagesAvailable = false;

        // Set target to null
        float amount = 0;
        target = null;

        // Iterate through nearby targets
        for (int b = 0; b < nearbyTargets.Count; b++)
        {
            if (nearbyTargets[b] != null)
            {
                if (!visitedTargets.Contains(nearbyTargets[b]))
                {
                    ResourceTile resourceTile = nearbyTargets[b].GetComponent<ResourceTile>();
                    if (!resourceTile.isStorage)
                    {
                        if (!resourceTile.hasAssignedDrone && resourceTile.amount > amount)
                        {
                            amount = resourceTile.amount;
                            target = resourceTile;
                            continue;
                        }
                    }
                    else if (resourceTile.isStorage && !resourceTile.isFull)
                    {
                        storagesAvailable = true;
                    }
                }
            }
            else
            {
                nearbyTargets.RemoveAt(b);
                b--;
            }
        }

        // If storages available, deploy
        if (target != null && storagesAvailable)
        {
            target.GetComponent<ResourceTile>().hasAssignedDrone = true;
            RotateToTarget();
            ExitPort();
            return true;
        }
        else return false;
    }

    // Collects or store resources
    public override void TargetReached()
    {
        if (target == home)
        {
            EnterPort();
            return;
        }

        visitedTargets.Add(target);
        ResourceTile tile = target.GetComponent<ResourceTile>();

        if (collecting)
        {
            if (collected.ContainsKey(tile.type))
                collected[tile.type] += tile.TakeResource();
            else collected.Add(tile.type, tile.TakeResource());
            tile.hasAssignedDrone = false;
            maxTrips -= 1;

            if (maxTrips <= 0) collecting = false;
            FindTarget();
        }
        else
        {
            if (collected.ContainsKey(tile.type))
            {
                collected[tile.type] = tile.AddResources(collected[tile.type]);
                if (collected[tile.type] <= 0)
                {
                    collected.Remove(tile.type);
                    if (collected.Count <= 0) ReturnHome();
                }
            }
            FindTarget();
        }
    }

    // Reset variables
    public override void FinishRoute()
    {
        collected = new Dictionary<Resource.CurrencyType, int>();
        visitedTargets = new List<BaseEntity>();
        storagesAvailable = true;
        collecting = true;
        maxTrips = 3;

        base.FinishRoute();
    }
}
