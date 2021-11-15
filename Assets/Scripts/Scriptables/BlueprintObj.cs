using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintObj : MonoBehaviour
{
    // Icon value
    public SpriteRenderer blueprintIcon;
    public Blueprint.RarityType rarity;

    // Internal values
    private Blueprint blueprint;

    public void Setup(Blueprint blueprint)
    {
        // Set internal values
        this.blueprint = blueprint;
        blueprintIcon.sprite = blueprint.icon;
    }
}
