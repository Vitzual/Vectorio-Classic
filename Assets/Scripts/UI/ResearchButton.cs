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
    public TextMeshProUGUI level;
    public Image researchIcon;
    public TextMeshProUGUI description;
    public TextMeshProUGUI essence;
    public TextMeshProUGUI heat;
    public TextMeshProUGUI power;

    // Research holder
    public ResearchBoost boost;

    // Setup research 
    public void Setup(ResearchBoost boost)
    {
        this.boost = boost;
        button.buttonText = boost.name;
        researchIcon.sprite = boost.icon;
        description.text = boost.description;
        essence.text = boost.essenceRequirement.ToString() + "/s";
        heat.text = boost.heatRequirement.ToString();
        power.text = boost.powerRequirement.ToString();
        UpdateResearch();
    }

    // Update research 
    public void UpdateResearch()
    {
        level.text = Research.GetAmount(boost.type);
    }

    // Set on click
    public void OnClick()
    {
        foreach (Cost cost in boost.cost)
        {
            if (cost.storage)
            {
                Resource.Currency currency = Resource.active.currencies[cost.resource];
                if (cost.add) if (currency.amount + cost.amount > currency.storage) return;
                else if (currency.amount - cost.amount < currency.storage) return;
            }
        }

        Events.active.ResearchButtonClicked(boost);
    }
}
