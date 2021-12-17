using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticButton : MonoBehaviour
{
    // Variables
    public ButtonManagerBasic button;
    public TextMeshProUGUI description;
    public Image buttonIcon;
    public Cosmetic cosmetic;
    public Buildable buildable;
    public GameObject locked;
    public TextMeshProUGUI lockedText;

    public void Setup(Cosmetic cosmetic, Buildable buildable)
    {
        this.cosmetic = cosmetic;
        this.buildable = buildable;

        button.buttonText = cosmetic.name.ToUpper();
        button.UpdateUI();
        description.text = cosmetic.description;
        buttonIcon.sprite = cosmetic.hologram;
    }

    public void ApplyCosmetic()
    {
        if (cosmetic.validateLocalApplication())
        {
            Debug.Log("Validated ownership of " + cosmetic.name + ", applying!");
            buildable.ApplyCosmetic(cosmetic);
            Panel.active.ToggleArmory(false);
            Panel.active.SetPanel(buildable);
            Events.active.CosmeticApplied(cosmetic);
        }
        else if (cosmetic.dlc != null) cosmetic.dlc.OpenStore();
    }
}
