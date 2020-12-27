using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;

public class Research : MonoBehaviour
{

    // Research UI stuff
    public WindowManager ResearchUI;
    public ButtonManagerBasicIcon ResearchUIButton;
    public TextMeshProUGUI ResearchDescriptionBox;
    public TextMeshProUGUI ResearchTitleBox;
    public TextMeshProUGUI ResearchCostBox;
    public Image ResearchImage;
    public int ResearchLevel;

    // Data source scripts
    public Survival SurvivalCS;

    // Research array class
    [System.Serializable]
    public class Researchable
    {
        public GameObject ResearchObject;
        public int EssenceRequired; // The cost of the research
        public int[] RequiredResearch; // Array of indexs of the required research nodes
        public string Title; // The title to be displayed in the research menu
        [TextArea] public string Description; // The description to be displayed in the research menu
        public ButtonManagerBasicIcon ResearchButton; // The button associated with this research
        public ButtonManagerBasicIcon ResearchInvButton;
        public bool IsResearched; // If this researchable is researched
    }

    // Research tracking
    public Researchable[] Researchables;

    public bool UnlockResearch(int i)
    {
        Researchable research = Researchables[i]; // Get research as a temp variable

        foreach (int j in research.RequiredResearch) // Returns false if one or more required research is not researched
            if (!Researchables[j].IsResearched) return false;

        if (research.EssenceRequired < SurvivalCS.essence) return false; // Return false if the player does not have the required essence

        SurvivalCS.essence -= research.EssenceRequired; // Subtract the cost from SurvivalCS
        research.IsResearched = true; // Set research to researched

        Researchables[i] = research; // Store temp variable

        return true;
    }

    public void ShowResearchButton()
    {
        ResearchUIButton.buttonIcon = Resources.Load<Sprite>("Sprites/Research");
        ResearchUIButton.UpdateUI();
    }

    
}
