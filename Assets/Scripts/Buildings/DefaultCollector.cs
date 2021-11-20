 using System.Collections;
using UnityEngine;

public class DefaultCollector : ResourceTile
{
    // Declare local object variables
    [HideInInspector] public float cooldown;
    [HideInInspector] public bool enhanced;
    public int collectorStorage = 500;
    public AnimateThenStop animator;

    // On start, invoke repeating SendGold() method
    public void Start()
    {
        Events.active.CollectorPlaced(this);
        cooldown = Research.gold_time;
    }

    // Add resources to collector
    public void AddResources()
    {
        if (isFull) return;

        if (enhanced) amount += Research.gold_yield * 4;
        else amount += Research.gold_yield;
        cooldown = Research.gold_time;

        if (amount > collectorStorage)
        {
            amount = collectorStorage;
            isFull = true;
        }
    }

    // On click override method
    public override void OnClick()
    {
        if (Resource.active.GetAmount(type) + amount < Resource.active.GetStorage(type))
        {
            if (amount > 0)
            {
                CollectorHandler.active.TransferResources(TakeResource(), type);
                isFull = false;
            }
        }
    }

    public override int TakeResource()
    {
        // Set animation
        animator.resetAnim();
        animator.animEnabled = true;
        animator.enabled = true;

        // Set values
        int holder = amount;
        amount = 0;
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
