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
        cooldown = Random.Range(0, Research.resource[type].extractionRate);
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
    
    // Override resource thing
    public override int AddResources(int amountToAdd, bool show)
    {
        amount = amountToAdd;
        return 0;
    }

    // Override button click
    public override void ButtonClicked()
    {
        if (amount > 0 && Resource.active.GetAmount(type) + amount < Resource.active.GetStorage(type))
        {
            Events.active.CollectorHarvested(type, amount);
            CollectResources();
        }
    }

    // On click override method
    public override void OnClick()
    {
        if (InputController.shiftHeld)
        {
            if (amount > 0 && Resource.active.GetAmount(type) + amount < Resource.active.GetStorage(type))
            {
                Events.active.CollectorHarvested(type, amount);
                CollectResources();
            }
        }
        else base.OnClick();
    }

    // Collect resource
    public void CollectResources() 
    {
        if (amount > 0)
            Communicator.active.SyncCollector(runtimeID, TakeResource());
    }

    // Sync resource
    public override void SyncEntity(int amount)
    {
        this.amount = 0;
        Resource.active.Apply(type, amount, true);
        Effects.CreateTempText("+" + amount, transform.position, Color.white);
        isFull = false;
        SetLight();
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
        animator.enabled = true;
        animator.ResetAnim();

        // Set values
        int holder = amount;
        amount = 0;

        // See how much is taken
        isFull = false;
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
