using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchButton : MonoBehaviour
{
    // UI elements
    public ButtonManagerBasic button;
    public Image researchIcon;
    public TextMeshProUGUI description;
    public TextMeshProUGUI level;
    public TextMeshProUGUI heat;
    public TextMeshProUGUI heatTitle;
    public Image heatIcon;
    public bool isUnlocked = false;

    // Research holder
    public ResearchTech researchTech;

    // Setup research 
    public void Setup(ResearchTech boost)
    {
        this.researchTech = boost;
        heat.text = Resource.FormatNumber(boost.heatUnlockCost).ToString();
    }

    // Check if unlocked
    public void CheckUnlock(int amount)
    {
        if (amount >= researchTech.heatUnlockCost)
            UnlockResearch();
    }

    // Unlock research
    public void UnlockResearch()
    {
        if (!isUnlocked)
        {
            isUnlocked = true;

            researchIcon.sprite = researchTech.icon;
            button.buttonText = researchTech.name;
            description.text = researchTech.description;

            heat.gameObject.SetActive(false);
            heatTitle.gameObject.SetActive(false);
            heatIcon.gameObject.SetActive(false);

            level.text = Research.GetAmount(researchTech);

            Events.active.ResearchUnlocked(researchTech);
        }
    }

    // Set on click
    public void OnClick()
    {
        if (isUnlocked) Events.active.ResearchButtonClicked(researchTech);
    }
}
