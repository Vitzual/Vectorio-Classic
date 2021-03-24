using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Research : MonoBehaviour
{

    // Research variables
    public static int bonus_damage = 0;
    public static int bonus_gold = 0;
    public static int bonus_burning = 0;
    public static int bonus_freezing = 0;
    public static int bonus_poisoning = 0;
    public static int bonus_shield = 0;
    public static int bonus_health = 0;
    public static int bonus_pierce = 0;
    public static float bonus_range = 0;
    public static float bonus_firerate = 0;
    public static float bonus_bulletspeed = 1;
    public static bool research_unlocked = false;

    // Research UI stuff
    public TextMeshProUGUI ResearchTitle;
    public TextMeshProUGUI ResearchDescription;
    public TextMeshProUGUI ResearchCost;
    public GameObject ResearchButton;
    public GameObject LockedButton;
    public bool ResearchLocked;
    public int SelectedResearch = 0;

    // Data source scripts
    public Survival SurvivalCS;

    // Research array class
    [System.Serializable]
    public class Researchable
    {
        public string Research; // Name of the research type
        public string Title; // The title to be displayed in the research menu
        [TextArea] public string Description; // The description to be displayed in the research menu
        public int EssenceRequired; // The cost of the research
        public int[] RequiredResearch; // Array of indexs of the required research nodes
        public Image ResearchButtonIcon; // The button icon associated with this research
        public bool IsResearched; // If this researchable is researched
    }

    // Research tracking
    public Researchable[] Researchables;

    // On button click
    public void ResearchTreeButton(int number)
    {
        // Get research data ands tore in temp var
        Researchable research = Researchables[number];

        // Set the locked button to false by default
        ResearchLocked = false;

        // Returns false if one or more required research is not researched
        foreach (int j in research.RequiredResearch)
            if (!Researchables[j].IsResearched) ResearchLocked = true;
        LockedButton.SetActive(ResearchLocked);

        // Check if already researched
        if (research.IsResearched) ResearchButton.SetActive(false);

        // If not yet researched, update with info
        else
        {
            // Set thing true
            ResearchButton.SetActive(true);

            // Set research information
            ResearchTitle.text = research.Title;
            ResearchDescription.text = research.Description;
            ResearchCost.text = research.EssenceRequired.ToString();

            // Set selected research for the thing (idk lmao)
            SelectedResearch = number;

            // Set button status to active if not already active
            if (!ResearchButton.activeSelf) ResearchButton.SetActive(true);
        }
    }

    // Increases a research variable (automatically gets applied to all buildings)
    public void ApplyResearch(string type)
    {
        // Still have to add hub & auto repair tool thing
        if (type.ToLower() == "hub")
        {
            SurvivalCS.increaseAvailablePower(5000); // Increase hub power by 5000
            SurvivalCS.IncreaseAOC(); // Increases area of control by 1
        }
        else if (type.ToLower() == "damage") bonus_damage += 2; // Increases damage of all defenses by 2
        else if (type.ToLower() == "health") bonus_health += 5; // Increases health of all buildings by 5
        else if (type.ToLower() == "gold") bonus_gold += 5; // Increases output of all resources by 1
        else if (type.ToLower() == "burning") bonus_burning += 5; // Adds 3 seconds of burning (5% proc chance)
        else if (type.ToLower() == "freezing") bonus_freezing += 10; // Adds 10 seconds of freezing (5% proc chance)
        else if (type.ToLower() == "poisoning") bonus_poisoning += 3; // Adds 3 seconds of poisoning (5% proc chance)
        else if (type.ToLower() == "shield") bonus_shield += 1; // Adds defensive shield to all buildings
        else if (type.ToLower() == "pierce") bonus_pierce += 1; // Add piercing shots to bullets
        else if (type.ToLower() == "range") bonus_range += 5f; // Increase range by 5 units (1x1 tile)
        else if (type.ToLower() == "firerate") bonus_firerate += 0.1f; // Decrease shoot cooldown by 0.1s
        else Debug.Log("The research type supplied is invalid.");
    }

    // Unlocks a new researchable 
    public void UnlockResearch()
    {
        Researchable research = Researchables[SelectedResearch]; // Get research as a temp variable

        foreach (int j in research.RequiredResearch) // Returns false if one or more required research is not researched
            if (!Researchables[j].IsResearched) return;

        if (research.EssenceRequired > SurvivalCS.essence) return; // Return false if the player does not have the required essence

        SurvivalCS.RemoveEssence(research.EssenceRequired); // Subtract the cost from SurvivalCS

        ApplyResearch(research.Research); // Applies the research once all checks are passed

        research.IsResearched = true; // Set research to researched

        research.ResearchButtonIcon.color = Color.yellow; // Set button to yellow

        ResearchButton.SetActive(false); // Set false for guy button

        Researchables[SelectedResearch] = research; // Store temp variable
    }

    // Save research values when saving data
    public bool[] GetResearchData()
    {
        bool[] researched = new bool[Researchables.Length];
        for (int i = 0; i < Researchables.Length; i++)
            researched[i] = Researchables[i].IsResearched;
        return researched; // Return to caller
    }

    // Set research values when loading data
    public void SetResearchData(bool[] data)
    {
        // In the case previous data changed, system will pass that research itteration
        try
        {
            // Sets the stored research value if true
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i])
                {
                    Researchables[i].IsResearched = true;
                    ApplyResearch(Researchables[i].Research);
                    Researchables[i].ResearchButtonIcon.color = Color.yellow;
                }
            }
        } 
        catch
        {
            // Debugs to console that the research data does not match the current versions research structure
            Debug.Log("Mismatch in save data to research data. Skipping itteration.");
        }
    }
}
