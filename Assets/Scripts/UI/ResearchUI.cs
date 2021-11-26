using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Michsky.UI.ModernUIPack;

public class ResearchUI : MonoBehaviour
{
    // Active instance
    public static ResearchUI active;

    // Index lists
    public enum IndexLists
    {
        Defensive,
        Logistics
    }

    // Is open
    public static bool isOpen = false;

    // Selected lab
    public ResearchLab selectedLab;
    public ResearchTech selectedTech;

    // Buttons
    public ResearchButton researchButton;
    public List<ResearchButton> researchButtons = new List<ResearchButton>();

    // UI lists
    public List<Transform> listTypes;
    public CanvasGroup canvasGroup;

    // All UI elements
    public Image techIcon;
    public TextMeshProUGUI techName;
    public TextMeshProUGUI techDescription;
    public TextMeshProUGUI stats;
    public TextMeshProUGUI consumption;
    public TextMeshProUGUI usage;
    public ButtonManagerBasic applyButton;

    public void Awake()
    {
        active = this;
    }

    public void Setup(List<string> unlocked = null)
    {
        // Iterate through research techs
        Debug.Log("Research handler grabbing research boosts");
        foreach (KeyValuePair<string, ResearchTech> tech in ScriptableLoader.researchTechs)
        {
            ResearchButton button = Instantiate(researchButton, new Vector2(0, 0), Quaternion.identity).GetComponent<ResearchButton>();
            button.transform.SetParent(listTypes[(int)tech.Value.indexList]);
            button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            button.Setup(tech.Value);
            researchButtons.Add(button);
            Research.techs.Add(tech.Value, new Research.Tech());

            if (unlocked != null && unlocked.Contains(button.researchTech.InternalID))
                button.UnlockResearch();
        }

        // Set sibling index
        foreach(ResearchButton newButton in researchButtons)
            newButton.transform.SetSiblingIndex(newButton.researchTech.indexNumber);

        // Parse on lab clicked event
        Events.active.onLabClicked += OnLabClicked;
        Events.active.onResearchButtonClicked += SetPanel;
    }

    // Set lab information
    public void SetPanel(ResearchTech type)
    {
        bool preview = type != selectedLab.researchTech;

        Research.Tech tech = Research.techs[type];
        selectedTech = type;

        techIcon.sprite = type.icon;
        techName.text = type.name;
        techDescription.text = type.description;

        stats.text = "";
        consumption.text = "";
        usage.text = "";

        // I HATE THIS. DONT LOOK AT IT, I WILL REDO IT

        if (preview)
        {
            stats.text += "<b>ACTIVE LABS:</b> " + tech.totalLabs + " researching <color=green>(+1))</color>\n";
            stats.text += "<b>TOTAL EFFECT:</b> " + tech.totalEffect + "% effect <color=green>(+" + type.amount + ")</color>\n";
            stats.text += "<b>BREAKDOWNS:</b> " + tech.totalBooms + " breakdowns\n";

            // Get gold amount
            if (tech.costs.ContainsKey(Resource.CurrencyType.Gold))
                consumption.text += "<b>GOLD:</b> " + tech.costs[Resource.CurrencyType.Gold] + " / second <color=red>(+" + type.GetCost(Resource.CurrencyType.Gold) + ")</color>\n";
            else consumption.text += "0 / second <color=red>(+" + type.GetCost(Resource.CurrencyType.Gold) + ")</color>\n";

            if (tech.costs.ContainsKey(Resource.CurrencyType.Essence))
                consumption.text += "<b>ESSENCE:</b> " + tech.costs[Resource.CurrencyType.Essence] + " / second <color=red>(+" + type.GetCost(Resource.CurrencyType.Essence) + ")</color>\n";
            else consumption.text += "0 / second <color=red>(+" + type.GetCost(Resource.CurrencyType.Essence) + ")</color>\n";

            if (tech.costs.ContainsKey(Resource.CurrencyType.Iridium))
                consumption.text += "<b>IRIDUM:</b> " + tech.costs[Resource.CurrencyType.Iridium] + " / second <color=red>(+" + type.GetCost(Resource.CurrencyType.Iridium) + ")</color>\n";
            else consumption.text += "0 / second <color=red>(+" + type.GetCost(Resource.CurrencyType.Iridium) + ")</color>\n";

            if (tech.costs.ContainsKey(Resource.CurrencyType.Power))
                usage.text += "<b>POWER:</b> " + tech.costs[Resource.CurrencyType.Power] + " input <color=red>(+" + type.GetCost(Resource.CurrencyType.Power) + ")</color>\n";
            else usage.text += "0 / second <color=red>(+" + type.GetCost(Resource.CurrencyType.Power) + ")</color>\n";

            if (tech.costs.ContainsKey(Resource.CurrencyType.Heat))
                usage.text += "<b>HEAT:</b> " + tech.costs[Resource.CurrencyType.Heat] + " output <color=red>(+" + type.GetCost(Resource.CurrencyType.Heat) + ")</color>\n";
            else usage.text += "<b>HEAT:</b> 0 / second <color=red>(+" + type.GetCost(Resource.CurrencyType.Heat) + ")</color>\n";

            applyButton.buttonText = "INITIATE RESEARCH";
        }
        else
        {
            stats.text += tech.totalLabs + " researching\n";
            stats.text += tech.totalEffect + "% effect\n";
            stats.text += tech.totalBooms + " breakdowns\n";
            if (tech.costs.ContainsKey(Resource.CurrencyType.Gold))
                consumption.text += tech.costs[Resource.CurrencyType.Gold] + " / second\n";
            if (tech.costs.ContainsKey(Resource.CurrencyType.Essence))
                consumption.text += tech.costs[Resource.CurrencyType.Essence] + " / second\n";
            if (tech.costs.ContainsKey(Resource.CurrencyType.Iridium))
                consumption.text += tech.costs[Resource.CurrencyType.Iridium] + " / second\n";
            if (tech.costs.ContainsKey(Resource.CurrencyType.Power))
                usage.text += tech.costs[Resource.CurrencyType.Power] + " input\n";
            if (tech.costs.ContainsKey(Resource.CurrencyType.Heat))
                usage.text += tech.costs[Resource.CurrencyType.Heat] + " output\n";

            applyButton.buttonText = "CANCEL RESEARCH";
        }
    }
    

    // On lab clicked
    public void OnLabClicked(ResearchLab lab)
    {
        // Update all research
        int heat = Resource.active.GetAmount(Resource.CurrencyType.Heat);
        foreach (ResearchButton button in researchButtons)
            if (!button.isUnlocked && button.researchTech.heatUnlockCost >= heat)
                button.UnlockResearch();

        // Set research panel true
        selectedLab = lab;
        isOpen = true;
        OpenResearch();
    }

    // Close UI
    public void OpenResearch()
    {
        isOpen = true;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    // Close UI
    public void CloseResearch()
    {
        isOpen = false;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    // On research button clicked
    public void ApplyResarch()
    {
        if (selectedTech == null) return;

        if (selectedTech == selectedLab.researchTech)
        {
            selectedLab.CancelResearch();
            SetPanel(selectedTech);
        }

        if (selectedLab != null)
        {
            foreach (Cost cost in selectedTech.cost)
                if (!Resource.active.CheckResource(cost)) return;

            selectedLab.ApplyResearch(selectedTech);
            CloseResearch();
        }
    }
}
