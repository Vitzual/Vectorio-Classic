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
    public Dictionary<ResearchTech, ResearchButton> researchButtons;

    // UI lists
    public List<Transform> listTypes;
    public static CanvasGroup canvasGroup;

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
        canvasGroup = GetComponent<CanvasGroup>();
        researchButtons = new Dictionary<ResearchTech, ResearchButton>();
    }

    public void Setup(List<string> unlocked = null)
    {
        // Iterate through research techs
        Debug.Log("Research handler grabbing research boosts");
        Research.techs = new Dictionary<ResearchTech, Research.Tech>();
        foreach (KeyValuePair<string, ResearchTech> tech in ScriptableLoader.researchTechs)
        {
            ResearchButton button = Instantiate(researchButton, new Vector2(0, 0), Quaternion.identity).GetComponent<ResearchButton>();
            button.transform.SetParent(listTypes[(int)tech.Value.indexList]);
            button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            button.Setup(tech.Value);
            researchButtons.Add(tech.Value, button);
            Research.techs.Add(tech.Value, new Research.Tech());

            if (unlocked != null && unlocked.Contains(button.researchTech.InternalID))
                button.UnlockResearch();
        }

        // Set sibling index
        foreach(KeyValuePair<ResearchTech, ResearchButton> newButton in researchButtons)
            newButton.Value.transform.SetSiblingIndex(newButton.Key.indexNumber);

        // Parse on lab clicked event
        Events.active.onLabClicked += OnLabClicked;
        Events.active.onResearchButtonClicked += SetPanel;
    }

    // Set lab information
    // 
    // This is currently hardcoded but soon will be generated using
    // a ResearchPanelStat class.
    public void SetPanel(ResearchTech type)
    {
        if (type == null || selectedLab == null) return;

        bool preview = type != selectedLab.researchTech;

        Research.Tech tech = Research.techs[type];
        selectedTech = type;

        techIcon.sprite = type.icon;
        techName.text = type.name;
        techDescription.text = type.description;

        stats.text = "";
        consumption.text = "";
        usage.text = "";

        if (preview)
        {
            stats.text += "<b>ACTIVE LABS:</b> " + tech.totalLabs + " researching <color=green>(+1))</color>\n";
            stats.text += "<b>TOTAL EFFECT:</b> " + tech.totalEffect + "x effect <color=green>(+" + type.amount + ")</color>\n";
            stats.text += "<b>BREAKDOWNS:</b> " + tech.totalBooms + " breakdowns\n";

            // Get gold amount
            if (tech.costs.ContainsKey(Resource.CurrencyType.Gold))
                consumption.text += "<b>GOLD:</b> " + tech.costs[Resource.CurrencyType.Gold] + " / second <color=red>(+" + type.GetCost(Resource.CurrencyType.Gold) + ")</color>\n";
            else consumption.text += "<b>GOLD:</b> 0 / second <color=red>(+" + type.GetCost(Resource.CurrencyType.Gold) + ")</color>\n";

            if (tech.costs.ContainsKey(Resource.CurrencyType.Essence))
                consumption.text += "<b>ESSENCE:</b> " + tech.costs[Resource.CurrencyType.Essence] + " / second <color=red>(+" + type.GetCost(Resource.CurrencyType.Essence) + ")</color>\n";
            else consumption.text += "<b>ESSENCE:</b> 0 / second <color=red>(+" + type.GetCost(Resource.CurrencyType.Essence) + ")</color>\n";

            if (tech.costs.ContainsKey(Resource.CurrencyType.Iridium))
                consumption.text += "<b>IRIDUM:</b> " + tech.costs[Resource.CurrencyType.Iridium] + " / second <color=red>(+" + type.GetCost(Resource.CurrencyType.Iridium) + ")</color>\n";
            else consumption.text += "<b>IRIDUM:</b> 0 / second <color=red>(+" + type.GetCost(Resource.CurrencyType.Iridium) + ")</color>\n";

            if (tech.costs.ContainsKey(Resource.CurrencyType.Power))
                usage.text += "<b>POWER:</b> " + tech.costs[Resource.CurrencyType.Power] + " input <color=red>(+" + type.GetCost(Resource.CurrencyType.Power) + ")</color>\n";
            else usage.text += "<b>POWER:</b> 0 input <color=red>(+" + type.GetCost(Resource.CurrencyType.Power) + ")</color>\n";

            if (tech.costs.ContainsKey(Resource.CurrencyType.Heat))
                usage.text += "<b>HEAT:</b> " + tech.costs[Resource.CurrencyType.Heat] + " output <color=red>(+" + type.GetCost(Resource.CurrencyType.Heat) + ")</color>\n";
            else usage.text += "<b>HEAT:</b> 0 output <color=red>(+" + type.GetCost(Resource.CurrencyType.Heat) + ")</color>\n";

            applyButton.buttonText = "INITIATE RESEARCH";
            applyButton.UpdateUI();
        }
        else
        {
            stats.text += "<b>ACTIVE LABS:</b> " + tech.totalLabs + " researching\n";
            stats.text += "<b>TOTAL EFFECT:</b> " + tech.totalEffect + "x effect\n";
            stats.text += "<b>BREAKDOWNS:</b> " + tech.totalBooms + " breakdowns\n";

            if (tech.costs.ContainsKey(Resource.CurrencyType.Gold))
                consumption.text += "<b>GOLD:</b> " + tech.costs[Resource.CurrencyType.Gold] + " / second\n";
            else consumption.text += "<b>GOLD:</b> 0 / second\n";

            if (tech.costs.ContainsKey(Resource.CurrencyType.Essence))
                consumption.text += "<b>ESSENCE:</b> " + tech.costs[Resource.CurrencyType.Essence] + " / second\n";
            else consumption.text += "<b>ESSENCE:</b> 0 / second\n";

            if (tech.costs.ContainsKey(Resource.CurrencyType.Iridium))
                consumption.text += "<b>IRIDIUM:</b> " + tech.costs[Resource.CurrencyType.Iridium] + " / second\n";
            else consumption.text += "<b>IRIDIUM:</b> 0 / second\n";

            if (tech.costs.ContainsKey(Resource.CurrencyType.Power))
                usage.text += "<b>POWER:</b> " + tech.costs[Resource.CurrencyType.Power] + " input\n";
            else usage.text += "<b>POWER:</b> 0 input\n";

            if (tech.costs.ContainsKey(Resource.CurrencyType.Heat))
                usage.text += "<b>HEAT:</b> " + tech.costs[Resource.CurrencyType.Heat] + " output\n";
            else usage.text += "<b>HEAT:</b> 0 output\n";

            applyButton.buttonText = "CANCEL RESEARCH";
            applyButton.UpdateUI();
        }
    }

    // On research button clicked
    public void ApplyResarch()
    {
        if (selectedTech == null)
        {
            Debug.Log("No tech selected");
        }

        else if (selectedTech == selectedLab.researchTech)
        {
            Debug.Log("Cancelling research");
            ResearchTech tech = selectedLab.researchTech;
            selectedLab.CancelResearch();
            researchButtons[tech].UpdateButton();
            SetPanel(selectedTech);
        }

        else if (selectedLab != null)
        {
            Debug.Log("Checking lab costs");

            if (!Resource.active.CheckOutputsOnly(selectedTech.cost.ToArray())) return;

            Debug.Log("Cost check passed, applying research");

            if (selectedLab.researchTech != null)
            {
                ResearchTech tech = selectedLab.researchTech;
                selectedLab.CancelResearch();
                researchButtons[tech].UpdateButton();
            }

            Communicator.active.SyncMetadata(selectedLab.runtimeID, selectedTech.metadataID);
        }
    }

    // On lab clicked
    public void OnLabClicked(ResearchLab lab)
    {
        // Update all research
        int heat = Resource.active.GetAmount(Resource.CurrencyType.Heat);
        Debug.Log("Lab opened with " + heat + " heat");
        foreach (KeyValuePair<ResearchTech, ResearchButton> button in researchButtons)
            if (!button.Value.isUnlocked && button.Value.researchTech.heatUnlockCost <= heat)
                button.Value.UnlockResearch();

        // Set research panel true
        selectedLab = lab;
        isOpen = true;
        OpenResearch();
    }

    // Close UI
    public static void OpenResearch()
    {
        isOpen = true;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    // Close UI
    public static void CloseMenu()
    {
        isOpen = false;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
