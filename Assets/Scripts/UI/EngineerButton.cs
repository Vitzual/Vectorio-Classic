using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EngineerButton : MonoBehaviour
{
    // Engineer variables
    private ButtonManagerBasicIcon button;
    public Image blueprintIcon;

    // Internal values
    public Blueprint blueprint;
    public Blueprint.Rarity rarity;

    // Get BMBI reference
    public void Awake()
    {
        button = GetComponent<ButtonManagerBasicIcon>();
    }

    // Set button
    public void SetButton(Blueprint blueprint, Blueprint.Rarity rarity)
    {
        this.blueprint = blueprint;
        this.rarity = rarity;

        blueprintIcon.sprite = blueprint.icon;
    }

    // Apply blueprint
    public void ApplyBlueprint()
    {

    }
}
