using System.Collections;
using UnityEngine;

public class DefaultCollector : BaseBuilding
{
    // Declare local object variables
    public int collectorType;
    [HideInInspector] public int collected;
    [HideInInspector] public bool enhanced;
    private AnimateThenStop animator;

    // On start, invoke repeating SendGold() method
    public void Start()
    {
        SetBuildingStats();
        Events.active.CollectorPlaced(this);

        /*
        droneManager = GameObject.Find("Drone Handler").GetComponent<DroneManager>();
        droneManager.updateResourceDrones(transform);

        switch (collectorType) {
            case 1:
                if (Research.research_gold_time >= 1 && !isOffset)
                    InvokeRepeating("IncreaseGold", 0f, Research.research_gold_time);
                else if (!isOffset) InvokeRepeating("IncreaseGold", 0f, 2f);
                return;
            case 2:
                if (Research.research_essence_time >= 1 && !isOffset)
                    InvokeRepeating("IncreaseEssence", 0f, Research.research_essence_time);
                else if (!isOffset) InvokeRepeating("IncreaseEssence", 0f, 2f);
                return;
            case 3:
                if (Research.research_iridium_time >= 1 && !isOffset)
                    InvokeRepeating("IncreaseIridium", 0f, Research.research_iridium_time);
                else if (!isOffset) InvokeRepeating("IncreaseIridium", 0f, 2f);
                return;
        }
        */
    }

    public int GrabResources()
    {
        // Set animation
        animator.resetAnim();
        animator.animEnabled = true;
        animator.enabled = true;

        // Set values
        int holder = collected;
        collected = 0;
        return holder;
    }

    // Enhance collector
    public void EnhanceCollector()
    {
        enhanced = true;
    }

    // Deenhance collector
    public void DeenhanceCollector()
    {
        enhanced = false;
    }
}
