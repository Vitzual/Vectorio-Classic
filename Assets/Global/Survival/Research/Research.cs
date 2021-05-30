using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Research : MonoBehaviour
{

    // Research variables
    public static int research_damage = 0;
    public static int research_burning = 0;
    public static int research_freezing = 0;
    public static int research_poisoning = 0;
    public static int research_shield = 0;
    public static int research_health = 0;
    public static int research_pierce = 0;
    public static float research_range = 0;
    public static float research_firerate = 0;
    public static float research_bulletspeed = 1;
    public static float research_gold_time = 5f;
    public static int research_gold_yield = 1;
    public static float research_essence_time = 8f;
    public static int research_essence_yield = 1;
    public static float research_iridium_time = 15f;
    public static int research_iridium_yield = 1;
    public static float research_construction_speed = 25f;
    public static float research_combat_speed = 20f;

    // Research UI stuff
    public static int LabsAvailable = 0;
    public static bool ResearchUnlocked = true;

    // Movement stuff
    protected Vector2 movement;
    public float moveSpeed = 60000f;
    public ScrollRect overlay;

    // Data source scripts
    public Survival SurvivalCS;

    // Research array class
    [System.Serializable]
    public class Researchable
    {
        public string Research; // Name of the research type
        public string ResearchType;
        public int[] RequiredResearch; // Array of indexs of the required research nodes
        public int GoldRequired; // The cost of the research
        public int EssenceRequired; // The cost of the research
        public int IridiumRequired; // The cost of the research
        public Button ResearchButton; // The button object associated with this research
        public bool IsResearched; // If this researchable is researched
    }

    // Research tracking
    public Researchable[] Researchables;

    // On start
    public void Start()
    {
        UpdateAllButtons();
        UpdateAllPrices();
    }

    // Movement tracking
    public void Update()
    {
        // Check if space pressed
        if (Input.GetKeyDown(KeyCode.Space))
            overlay.normalizedPosition = new Vector2(0, 0);

        // Get directional movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
            moveSpeed = 400000f;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            moveSpeed = 60000f;
        else if (Input.GetKeyDown(KeyCode.LeftControl))
            moveSpeed = 30000f;
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            moveSpeed = 60000f;

        overlay.velocity = -movement * moveSpeed * Time.fixedDeltaTime;

        if (Input.GetKeyDown(KeyCode.M))
        {
            SurvivalCS.AddGold(100);
            SurvivalCS.AddEssence(100);
            SurvivalCS.AddIridium(100);
        }
    }

    // On button click
    public void ResearchTreeButton(int number)
    {
        // Get research data ands tore in temp var
        Researchable research = Researchables[number];

        // Check if already researched
        if (research.IsResearched) return;

        // Returns false if one or more required research is not researched
        foreach (int j in research.RequiredResearch)
            if (!Researchables[j].IsResearched) return;

        // Check resources
        if (SurvivalCS.gold < research.GoldRequired) return;
        if (SurvivalCS.essence < research.EssenceRequired) return;
        if (SurvivalCS.iridium < research.IridiumRequired) return;

        // If all checks passed, apply research
        ApplyResearch(research);
    }

    // Increases a research variable (automatically gets applied to all buildings)
    public void ApplyResearch(Researchable research)
    {
        // Assign type
        string type = research.ResearchType;

        // Still have to add hub & auto repair tool thing
        if (type.ToLower() == "damage") research_damage += 5; // Increases damage of all defenses by 2
        else if (type.ToLower() == "health") research_health += 10; // Increases health of all buildings by 5
        else if (type.ToLower() == "gold yield") research_gold_yield += 1; // Increases output of all resources by 5
        else if (type.ToLower() == "essence yield") research_essence_yield += 1; // Increases output of all resources by 5
        else if (type.ToLower() == "iridium yield") research_iridium_yield += 1; // Increases output of all resources by 5
        else if (type.ToLower() == "gold time") research_gold_time -= 1f; // Increases output of all resources by 5
        else if (type.ToLower() == "essence time") research_essence_time -= 1f; // Increases output of all resources by 5
        else if (type.ToLower() == "iridium time") research_essence_time -= 1f; // Increases output of all resources by 5
        else if (type.ToLower() == "burning") research_burning += 5; // Adds 3 seconds of burning (5% proc chance)
        else if (type.ToLower() == "freezing") research_freezing += 10; // Adds 10 seconds of freezing (5% proc chance)
        else if (type.ToLower() == "poisoning") research_poisoning += 3; // Adds 3 seconds of poisoning (5% proc chance)
        else if (type.ToLower() == "shield") research_shield += 1; // Adds defensive shield to all buildings
        else if (type.ToLower() == "pierce") research_pierce += 1; // Add piercing shots to bullets
        else if (type.ToLower() == "range") research_range += 5f; // Increase range by 5 units (1x1 tile)
        else if (type.ToLower() == "firerate") research_firerate += 0.1f; // Decrease shoot cooldown by 0.1s
        else { Debug.Log("The research type supplied is invalid."); }

        // Set research boolean
        research.IsResearched = true;
        SetResearchButtonColor(research, "green");
    }

    public void SetResearchButtonColor(Researchable research, string color)
    {
        var colors = research.ResearchButton.colors;
        if (color == "red")
        {
            colors.normalColor = new Color(154, 53, 53, 0.8f);
            colors.highlightedColor = new Color(190, 62, 62, 0.8f);
            colors.pressedColor = new Color(221, 64, 64, 0.8f);
        }
        else if (color == "green")
        {
            colors.normalColor = new Color(53, 154, 73, 0.8f);
            colors.highlightedColor = new Color(62, 190, 73, 0.8f);
            colors.pressedColor = new Color(64, 221, 97, 0.8f);
        }
        else
        {
            colors.normalColor = new Color(34, 34, 34, 0.8f);
            colors.highlightedColor = new Color(51, 50, 50, 0.8f);
            colors.pressedColor = new Color(156, 153, 153, 0.8f);
        }
    }

    public void UpdateAllButtons()
    {
        foreach (Researchable research in Researchables)
        {
            if (research.IsResearched) SetResearchButtonColor(research, "green");
            else
            {
                foreach (int j in research.RequiredResearch)
                {
                    if (!Researchables[j].IsResearched) { SetResearchButtonColor(research, "red"); return; }
                }
                SetResearchButtonColor(research, "default");
            }
        }
    }

    public void UpdateAllPrices()
    {
        foreach (Researchable research in Researchables)
            research.ResearchButton.transform.Find("Gold Amount").GetComponent<TextMeshProUGUI>().text = "x" + research.GoldRequired;
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
                if (data[i]) ApplyResearch(Researchables[i]);
        } 
        catch
        {
            // Debugs to console that the research data does not match the current versions research structure
            Debug.Log("Mismatch in save data to research data. Skipping itteration.");
        }
    }
}
