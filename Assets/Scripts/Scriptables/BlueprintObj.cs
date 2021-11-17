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
    public TooltipManager tooltipManager;
    public SpriteRenderer blueprintModel;
    public SpriteRenderer blueprintIcon;
    public Blueprint.RarityType rarity;

    // Internal values
    private Blueprint blueprint;

    public void Setup(Blueprint blueprint, Blueprint.RarityType rarity)
    {
        // Set internal values
        this.blueprint = blueprint;
        this.rarity = rarity;
        blueprintIcon.sprite = blueprint.icon;

        // Set correct particle and model (brain dead from studies when making this)
        foreach(BlueprintModel model in blueprintModels)
        {
            if (model.rarity == rarity)
            {
                blueprintModel.sprite = model.icon;
                model.particle.gameObject.SetActive(true);
            }
        }
    }

    public void SetTooltip()
    {

    }
}
