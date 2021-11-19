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
    public CollectedBlueprint blueprint;

    // Get BMBI reference
    public void Awake()
    {
        button = GetComponent<ButtonManagerBasicIcon>();
    }

    // Set button
    public void SetButton(CollectedBlueprint blueprint)
    {
        this.blueprint = blueprint;
        blueprintIcon.sprite = blueprint.blueprint.icon;
    }

    // Clear button
    public void ClearButton()
    {
        blueprint = null;
        Sprite emptySprite = Sprites.GetSprite("Transparent");
        blueprintIcon.sprite = emptySprite;
        button.buttonIcon = emptySprite;
    }

    // Apply blueprint
    public void ApplyBlueprint()
    {
        if (Panel.active.ApplyBlueprint(blueprint)) ClearButton();
    }
}
