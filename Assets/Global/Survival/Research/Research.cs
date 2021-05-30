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
    public static int research_wall_health = 0;
    public static int research_pierce = 0;
    public static float research_range = 0;
    public static float research_firerate = 0;
    public static float research_bulletspeed = 1;
    public static float research_gold_time = 5f;
    public static int research_gold_yield = 1;
    public static int research_gold_storage = 0;
    public static float research_essence_time = 8f;
    public static int research_essence_yield = 1;
    public static int research_essence_storage = 0;
    public static float research_iridium_time = 12f;
    public static int research_iridium_yield = 1;
    public static int research_iridium_storage = 0;
    public static float research_construction_speed = 25f;
    public static float research_combat_speed = 20f;
    public static bool research_explosive_storages = false;

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
            SurvivalCS.AddGold(10000);
            SurvivalCS.AddEssence(10000);
            SurvivalCS.AddIridium(10000);
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


        // Apply the research
        switch (type)
        {
            case "disrupt":
                SurvivalCS.Spawner.maxHeat = 25000;
                SurvivalCS.Spawner.maxHeatAmount.text = 25000 + " MAX";
                SurvivalCS.Spawner.increaseHeat(0);
                break;
            case "damage":
                research_damage += 5;
                break;
            case "health":
                research_health += 10;
                research_wall_health += 50;
                break;
            case "range":
                research_range += 5f;
                break;
            case "firerate":
                research_firerate += 0.1f;
                break;
            case "pierce":
                research_pierce += 1;
                break;
            case "gold yield":
                research_gold_yield += 1;
                break;
            case "gold time":
                research_gold_time -= 1f;
                CollectorAI[] allGoldCollectors = FindObjectsOfType<CollectorAI>();
                foreach (CollectorAI collector in allGoldCollectors)
                    collector.OffsetStart();
                break;
            case "gold storage":
                research_gold_storage += 500;
                GoldStorageAI[] allGoldStorage = FindObjectsOfType<GoldStorageAI>();
                SurvivalCS.goldStorage += allGoldStorage.Length * 500;
                break;
            case "essence yield":
                research_essence_yield += 1;
                break;
            case "essence time":
                research_essence_time -= 1f;
                EssenceAI[] allEssenceCollectors = FindObjectsOfType<EssenceAI>();
                foreach (EssenceAI collector in allEssenceCollectors)
                    collector.OffsetStart();
                break;
            case "essence storage":
                research_essence_storage += 250;
                EssenceStorageAI[] allEssenceStorages = FindObjectsOfType<EssenceStorageAI>();
                SurvivalCS.essenceStorage += allEssenceStorages.Length * 250;
                break;
            case "iridium yield":
                research_iridium_yield += 1;
                break;
            case "iridium time":
                research_iridium_time -= 1f;
                IridiumAI[] allIridiumCollectors = FindObjectsOfType<IridiumAI>();
                foreach (IridiumAI collector in allIridiumCollectors)
                    collector.OffsetStart();
                break;
            case "iridium storage":
                research_iridium_storage += 100;
                IridiumStorageAI[] allIridiumStorages = FindObjectsOfType<IridiumStorageAI>();
                SurvivalCS.iridiumStorage += allIridiumStorages.Length * 100;
                break;
            case "burning":
                research_burning += 5;
                break;
            case "freezing":
                research_freezing += 10;
                break;
            case "poisoning":
                research_poisoning += 3;
                break;
            case "explosive storages":
                research_explosive_storages = true;
                break;
            case "construction speed":
                research_construction_speed += 5f;
                break;
            case "combat speed":
                research_combat_speed += 5f;
                break;
            default:
                Debug.Log("The research type supplied was invalid");
                return;
        }

        // Set research boolean
        research.IsResearched = true;
        UpdateAllButtons();
    }

    public void SetResearchButtonColor(Researchable research, string color)
    {
        var colors = research.ResearchButton.colors;
        Color colorHolder;
        if (color == "red")
        {
            Debug.Log("I got called red!");
            ColorUtility.TryParseHtmlString("#A6232399", out colorHolder);
            colors.normalColor = colorHolder;
            ColorUtility.TryParseHtmlString("#BC181899", out colorHolder);
            colors.highlightedColor = colorHolder;
            ColorUtility.TryParseHtmlString("#F31D1D99", out colorHolder);
            colors.pressedColor = colorHolder;
        }
        else if (color == "green")
        {
            Debug.Log("I got called green!");
            ColorUtility.TryParseHtmlString("#259F3B99", out colorHolder);
            colors.normalColor = colorHolder;
            ColorUtility.TryParseHtmlString("#24B93F99", out colorHolder);
            colors.highlightedColor = colorHolder;
            ColorUtility.TryParseHtmlString("#1FD13F99", out colorHolder);
            colors.pressedColor = colorHolder;
        }
        else
        {
            Debug.Log("I got called normal!");
            ColorUtility.TryParseHtmlString("#22222299", out colorHolder);
            colors.normalColor = colorHolder;
            ColorUtility.TryParseHtmlString("#33323299", out colorHolder);
            colors.highlightedColor = colorHolder;
            ColorUtility.TryParseHtmlString("#9C999999", out colorHolder);
            colors.pressedColor = colorHolder;
        }

        research.ResearchButton.colors = colors;
    }

    public void UpdateAllButtons()
    {
        foreach (Researchable research in Researchables)
        {
            if (research.IsResearched) SetResearchButtonColor(research, "green");
            else
            {
                SetResearchButtonColor(research, "default");
                foreach (int j in research.RequiredResearch)
                    if (!Researchables[j].IsResearched) SetResearchButtonColor(research, "red");
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
