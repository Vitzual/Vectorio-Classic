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

    // Research array class
    [System.Serializable]
    public class Researchables
    {
        public GameObject ResearchObject;
        public int EssenceRequired;
        public int ResearchNeeded;
        public string ResearchTitle;
        [TextArea] public string ResearchDescription;
        public ButtonManagerBasicIcon ResearchButton;
        public ButtonManagerBasicIcon ResearchInvButton;
    }

    // Research status tracking
    public Researchables[] ResearchTier;
    public bool[] Researched;

    public void ShowResearchButton()
    {
        ResearchUIButton.buttonIcon = Resources.Load<Sprite>("Sprites/Research");
        ResearchUIButton.UpdateUI();
    }

    
}
