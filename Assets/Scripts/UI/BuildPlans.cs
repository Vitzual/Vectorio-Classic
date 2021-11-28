using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// This is another slightly hardcoded file that needs
// to be updated to work with ALL currencies. (UI related)

public class BuildPlans : MonoBehaviour
{
    // Canvas group
    public CanvasGroup canvasGroup;

    // Amount tracked
    public Dictionary<Resource.CurrencyType, BuildCost> tracked;
    public TextMeshProUGUI totalQueued;
    public TextMeshProUGUI dronesAvailable;
    public List<BuildCost> buildCosts;

    // Create new dictionary
    public void Awake()
    {
        tracked = new Dictionary<Resource.CurrencyType, BuildCost>();

        foreach (BuildCost cost in buildCosts)
            tracked.Add(cost.resource, cost);
    }

    // Set events
    public void Start()
    {
        Events.active.onGhostPlaced += AddCosts;
        Events.active.onGhostDestroyed += RemoveCosts;
    }

    // Update plans (Add)
    public void AddCosts(GhostTile ghost)
    {
        foreach(Cost cost in ghost.buildable.building.resources)
            if (!cost.storage && tracked.ContainsKey(cost.resource))
                tracked[cost.resource].Add(cost.amount);

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        totalQueued.text = "<b>QUEUED:</b> " + DroneManager.active.ghostTiles.Count;
        dronesAvailable.text = "<b>DRONES:</b> " + DroneManager.active.builderDrones.Count;
    }

    // Update plans (Remove)
    public void RemoveCosts(GhostTile ghost)
    {
        foreach (Cost cost in ghost.buildable.building.resources)
            if (!cost.storage && tracked.ContainsKey(cost.resource))
                tracked[cost.resource].Remove(cost.amount);

        if (DroneManager.active.ghostTiles.Count == 0)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        totalQueued.text = "<b>QUEUED:</b> " + DroneManager.active.ghostTiles.Count;
        dronesAvailable.text = "<b>DRONES:</b> " + DroneManager.active.builderDrones.Count;
    }
}
