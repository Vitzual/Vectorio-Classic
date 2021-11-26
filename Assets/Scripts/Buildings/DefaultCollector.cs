 using System.Collections;
using UnityEngine;

public class DefaultCollector : ResourceTile
{
    // Declare local object variables
    [HideInInspector] public float cooldown;
    [HideInInspector] public bool enhanced;
    public int collectorStorage = 100;
    public AnimateThenStop animator;

    // Lights
    public new SpriteRenderer light;
    public Material normalLight;
    public Material fullLight;
    public Material enhancedLight;

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
            SetLight();
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
                SetLight();
            }
        }
        else base.OnClick();
    }

    // Set light
    public void SetLight()
    {
        if (isFull) light.material = fullLight;
        else if (enhanced) light.material = enhancedLight;
        else light.material = normalLight;
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

        // See how much is taken
        if (holder > 0) isFull = false;
        SetLight();

        return holder;
    }

    // Enhance collector
    public void EnhanceCollector()
    {
        enhanced = true;
        SetLight();
    }

    // Deenhance collector
    public void DeenhanceCollector()
    {
        enhanced = false;
        SetLight();
    }
}
