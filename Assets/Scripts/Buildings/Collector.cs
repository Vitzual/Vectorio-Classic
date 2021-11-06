 using System.Collections;
using UnityEngine;

public class Collector : BaseTile
{
    // Declare local object variables
    public Resource.CurrencyType collectorType;
    [HideInInspector] public float cooldown;
    [HideInInspector] public int collected;
    [HideInInspector] public bool enhanced;
    [HideInInspector] public bool isFull;
    public int collectorStorage = 500;
    public AnimateThenStop animator;

    // On start, invoke repeating SendGold() method
    public void Start()
    {
        Events.active.CollectorPlaced(this);
        cooldown = Research.research_gold_time;
    }

    // Add resources to collector
    public void AddResources()
    {
        if (isFull) return;

        if (enhanced) collected += Research.research_gold_yield * 4;
        else collected += Research.research_gold_yield;
        cooldown = Research.research_gold_time;

        if (collected > collectorStorage)
        {
            collected = collectorStorage;
            isFull = true;
        }
    }

    // On click override method
    public override void OnClick()
    {
        if (Resource.active.GetAmount(collectorType) + collected < Resource.active.GetStorage(collectorType))
        {
            if (collected > 0)
            {
                Resource.active.Add(collectorType, GrabResources());
                isFull = false;
            }
        }
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
