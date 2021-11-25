using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchHandler : MonoBehaviour
{
    // Active instance
    public static ResearchHandler active;
    public static bool isOpen = false;

    // All research boosts
    public List<ResearchBoost> researchBoosts = new List<ResearchBoost>();
    public List<ResearchButton> researchButtons = new List<ResearchButton>();

    // List of all techs
    public GameObject researchPanel;
    public Lab selectedLab;
    public ResearchButton button;
    public Transform techs;

    // Set active instance
    public void Awake() { active = this; }

    public void Setup()
    {
        // Iterate through research techs
        Debug.Log("Research handler grabbing research boosts");
        foreach (KeyValuePair<string, ResearchBoost> tech in ScriptableLoader.researchTechs)
        {
            ResearchButton button = Instantiate(this.button, techs.position, Quaternion.identity).GetComponent<ResearchButton>();
            button.transform.SetParent(techs);
            button.transform.SetSiblingIndex(tech.Value.organization);
            button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            button.Setup(tech.Value);
            researchBoosts.Add(tech.Value);
        }

        // Parse on lab clicked event
        Events.active.onLabClicked += OnLabClicked;
        InputEvents.active.onEscapePressed += CloseResearch;
    }

    // On lab clicked
    public void OnLabClicked(Lab lab)
    {
        // Update research 
        foreach (ResearchButton button in researchButtons)
            button.UpdateResearch();

        // Set research panel true
        researchPanel.SetActive(true);
        selectedLab = lab;
        isOpen = true;
    }


    // Close UI
    public void CloseResearch()
    {
        if (isOpen)
        {
            isOpen = false;
            researchPanel.SetActive(false);
        }
    }

    // On research button clicked
    public void OnResearchButtonClicked(ResearchBoost boost)
    {
        if (selectedLab != null)
        {
            selectedLab.ApplyResearch(boost);
            CloseResearch();
        }
    }
}
