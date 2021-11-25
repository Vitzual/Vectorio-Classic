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
        cooldown = Research.resource[type].extractionRate;
    }

    // Add resources to collector
    public void AddResources()
    {
        if (isFull) return;

        if (enhanced) amount += Research.resource[type].extractionYield * 4;
        else amount += Research.resource[type].extractionYield;

        cooldown = Research.resource[type].extractionRate;

        if (amount > collectorStorage)
        {
            amount = collectorStorage;
            isFull = true;
        }
    }

    // On click override method
    public override void OnClick()
    {
        if (InputController.shiftHeld)
        {
            if (amount > 0 && Resource.active.GetAmount(type) + amount < Resource.active.GetStorage(type))
            {
                int amount = TakeResource();
                Resource.active.Add(type, amount, true);
                PopupHandler.active.CreatePopup(transform.position, type, "+" + amount);
                isFull = false;
            }
        }
        else base.OnClick();
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
