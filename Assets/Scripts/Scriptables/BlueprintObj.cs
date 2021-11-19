using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This whole class will eventually be handled through
// the blueprint enumerators. Just limited on time when
// making this, so went with easiest route (to me at least).

public class BlueprintObj : MonoBehaviour
{
    // Models
    [System.Serializable]
    public class BlueprintModel
    {
        public Blueprint.RarityType rarity;
        public Sprite icon;
        public ParticleSystem particle;
    }
    public BlueprintModel[] blueprintModels;

    // Blueprint icon value
    public EngineerTooltip tooltip;
    public TooltipManager tooltipManager;
    public SpriteRenderer blueprintModel;
    public SpriteRenderer blueprintIcon;

    // Internal values
    private Blueprint blueprint;
    private Blueprint.Rarity rarity;

    public void Setup(Blueprint blueprint, Blueprint.Rarity rarity)
    {
        // Set internal values
        this.blueprint = blueprint;
        this.rarity = rarity;

        // Set sprite
        blueprintIcon.sprite = blueprint.icon;

        // Set correct particle and model (brain dead from studies when making this)
        foreach(BlueprintModel model in blueprintModels)
        {
            if (model.rarity == rarity.rarity)
            {
                blueprintModel.sprite = model.icon;
                model.particle.gameObject.SetActive(true);
            }
        }

        tooltip.SetTooltip(blueprint, rarity);
    }

    public void Collect()
    {
        Events.active.BlueprintCollected(blueprint, rarity);
        Destroy(gameObject);
    }

    public void Recycle()
    {
        Resource.active.Add(rarity.applicationCost.resource, rarity.applicationCost.amount / 10);
        Destroy(gameObject);
    }
}
