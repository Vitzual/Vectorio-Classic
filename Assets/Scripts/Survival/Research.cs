using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using TMPro;

public class Research : MonoBehaviour
{
    // Research variables
    public static int research_damage;
    public static int research_burning;
    public static int research_freezing;
    public static int research_poisoning;
    public static int research_shield;
    public static int research_health;
    public static int research_wall_health;
    public static int research_pierce;
    public static float research_range;
    public static float research_firerate;
    public static float research_bulletspeed;
    public static float research_gold_time;
    public static int research_gold_yield;
    public static int research_gold_storage;
    public static float research_essence_time;
    public static int research_essence_yield;
    public static int research_essence_storage;
    public static float research_iridium_time;
    public static int research_iridium_yield;
    public static int research_iridium_storage;
    public static float research_construction_speed;
    public static int research_construction_placements;
    public static float research_resource_speed;
    public static int research_resource_collections;
    public static int research_resource_amount;
    public static float research_resource_range;
    public static float research_combat_speed;
    public static float research_fixer_speed;
    public static int research_fixer_amount;
    public static int research_combat_targets;
    public static bool research_explosive_storages;
    public static bool research_explosive_defenses;
    public static bool research_explosive_collectors;
    public static int research_research_speed;
    public static bool research_fixer_drones;
    public static bool research_combat_drones;

    // Research UI stuff
    public Researchable Researching;
    public GameObject ResearchBackground;
    public ProgressBar ResearchBar;
    public static int LabsAvailable;
    public static bool ResearchUnlocked;
    public static bool ResearchActive;
    public int resourcesNeeded;
    public int resourcesTracked;
    public int goldNeeded;
    public int goldTracked;
    public int essenceNeeded;
    public int essenceTracked;
    public int iridiumNeeded;
    public int iridiumTracked;
    public float researchSpeed;
    public bool isMenu;
    public static int amountResearched;

    // Movement stuff
    protected Vector2 movement;
    public float moveSpeed = 60000f;
    public ScrollRect overlay;

    // Data source scripts
    public Survival SurvivalCS;
    public Technology TechnologyCS;

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
        public bool needsGold; // Bool that tracks if gold is required
        public bool needsEssence; // Bool that tracks if essence is required
        public bool needsIridium; // Bool that tracks if iridium is required
        public Button ResearchButton; // The button object associated with this research
        public bool IsResearched; // If this researchable is researched
        public TextMeshProUGUI goldText;
        public TextMeshProUGUI essenceText;
        public TextMeshProUGUI iridiumText;
    }

    // Research tracking
    public Researchable[] Researchables;

    // On start
    public void Start()
    {
        ResetResearchData();
        if (isMenu) return;
        UpdateAllButtons();
        UpdateAllPrices();
        ResearchBackground.SetActive(false);
    }

    // Movement trackingoverlay.normalizedPosition = new Vector2(0, 1);
    public void Update()
    {
        // Check if research open
        if (isMenu || !SurvivalCS.UI.ResearchOpen) return;

        // Check if space pressed
        if (Input.GetKeyDown(KeyCode.Space)) ScreenSet();

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
    }

    public void ScreenSet()
    {
        overlay.normalizedPosition = new Vector2(0, 1);
    }

    public void UpdateAvailable()
    {
        CancelInvoke("UpdateResearch");
        InvokeRepeating("UpdateResearch", 1f, researchSpeed);
    }

    public void SetResearchUI(Transform research, bool status)
    {
        if (status)
        {
            ResearchBar.currentPercent = (float)goldTracked / (float)goldNeeded * 100;
            ResearchBar.UpdateUI();
            ResearchBackground.transform.SetParent(research);
            ResearchBackground.transform.position = research.position;
            ResearchBackground.SetActive(true);
        }
        else ResearchBackground.SetActive(false);
    }

    public void UpdateResource(int type)
    {
        switch (type)
        {
            case 1:
                //SurvivalCS.RemoveGold(research_research_speed);
                goldTracked += research_research_speed;
                resourcesTracked += research_research_speed;
                if (goldTracked >= goldNeeded)
                {
                    Researching.needsGold = false;
                    if (Researching.goldText != null) Researching.goldText.text = "x0";
                }
                else if (Researching.goldText != null) Researching.goldText.text = "x" + (goldNeeded - goldTracked);
                break;
            case 2:
                //SurvivalCS.RemoveEssence(research_research_speed);
                essenceTracked += research_research_speed;
                resourcesTracked += research_research_speed;
                if (essenceTracked >= essenceNeeded)
                {
                    Researching.needsEssence = false;
                    if (Researching.essenceText != null) Researching.essenceText.text = "x0";
                }
                else if (Researching.essenceText != null) Researching.essenceText.text = "x" + (essenceNeeded - essenceTracked);
                break;
            case 3:
                //SurvivalCS.RemoveIridium(research_research_speed);
                iridiumTracked += research_research_speed;
                resourcesTracked += research_research_speed;
                if (iridiumTracked >= iridiumNeeded)
                {
                    Researching.needsIridium = false;
                    if (Researching.iridiumText != null) Researching.iridiumText.text = "x0";
                }
                else if (Researching.iridiumText != null) Researching.iridiumText.text = "x" + (iridiumNeeded - iridiumTracked);
                break;
        }
    }

    public void UpdateResearch()
    {
        // Update all resources
        // if (Researching.needsGold && SurvivalCS.gold >= research_research_speed) UpdateResource(1);
        // if (Researching.needsEssence && SurvivalCS.essence >= research_research_speed) UpdateResource(2);
        // if (Researching.needsIridium && SurvivalCS.iridium >= research_research_speed) UpdateResource(3);

        // Update the UI elements
        ResearchBar.currentPercent = (float)resourcesTracked / (float)resourcesNeeded * 100;
        ResearchBar.UpdateUI();

        // Check if resources still left
        if (!Researching.needsGold && !Researching.needsEssence && !Researching.needsIridium)
        {
            SetResearchUI(Researchables[0].ResearchButton.transform, false);
            ApplyResearch(Researching);
            CancelInvoke("UpdateResearch");
        }
    }

    // On button click
    public void ResearchTreeButton(int number)
    {
        // Get research data ands tore in temp var
        Researchable research = Researchables[number];

        // Check if at least one research lab available
        if (LabsAvailable < 1) return;

        // Check if being research
        if (research == Researching) return;

        // Check if already researched
        if (research.IsResearched) return;

        // Returns false if one or more required research is not researched
        foreach (int j in research.RequiredResearch)
            if (!Researchables[j].IsResearched) return;

        // Set resource tracking
        resourcesNeeded = research.GoldRequired + research.EssenceRequired + research.IridiumRequired;
        resourcesTracked = 0;

        // Set gold tracking
        if (research.GoldRequired > 0)
        {
            research.goldText = research.ResearchButton.transform.Find("Gold Amount").GetComponent<TextMeshProUGUI>();
            research.needsGold = true;
        }
        else research.needsGold = false;
        goldNeeded = research.GoldRequired;
        goldTracked = 0;

        // Set essence tracking
        if (research.EssenceRequired > 0)
        {
            research.essenceText = research.ResearchButton.transform.Find("Essence Amount").GetComponent<TextMeshProUGUI>();
            research.needsEssence = true;
        }
        else research.needsEssence = false;
        essenceNeeded = research.EssenceRequired;
        essenceTracked = 0;

        // Set iridium tracking
        if (research.IridiumRequired > 0)
        {
            research.iridiumText = research.ResearchButton.transform.Find("Iridium Amount").GetComponent<TextMeshProUGUI>();
            research.needsIridium = true;
        }
        else research.needsIridium = false;
        iridiumNeeded = research.IridiumRequired;
        iridiumTracked = 0;

        // If all checks passed, apply research
        Researching = research;
        SetResearchUI(research.ResearchButton.transform, true);
        ResearchActive = true;

        // Start research updating
        researchSpeed = 1f / LabsAvailable;
        CancelInvoke("UpdateResearch");
        InvokeRepeating("UpdateResearch", 1f, researchSpeed);
    }

    // Increases a research variable (automatically gets applied to all buildings)
    public void ApplyResearch(Researchable research)
    {
        // Assign type
        string type = research.ResearchType;
        amountResearched += 1;
        TechnologyCS.UpdateUnlock("Research");

        // Apply the research
        switch (type)
        {
            case "damage":
                research_damage += 5;
                break;
            case "health":
                research_health += 50;
                research_wall_health += 150;
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
                research_gold_yield += 5;
                break;
            case "gold time":
                research_gold_time -= 0.5f;
                DefaultCollector[] allGoldCollectors = FindObjectsOfType<DefaultCollector>();
                foreach (DefaultCollector collector in allGoldCollectors)
                    collector.UpdateCollector();
                break;
            case "gold storage":

                // Update variables and set main gold storage to 0
                research_gold_storage += 2500;
                //SurvivalCS.goldStorage = 0;

                break;
            case "essence yield":
                research_essence_yield += 5;
                break;
            case "essence time":
                research_essence_time -= 1f;
                DefaultCollector[] allEssenceCollectors = FindObjectsOfType<DefaultCollector>();
                foreach (DefaultCollector collector in allEssenceCollectors)
                    collector.UpdateCollector();
                break;
            case "essence storage":

                // Update variables and set main gold storage to 0
                research_essence_storage += 500;
                //SurvivalCS.essenceStorage = 0;

                break;
            case "iridium yield":
                research_iridium_yield += 5;
                break;
            case "iridium time":
                research_iridium_time -= 1f;
                DefaultCollector[] allIridiumCollectors = FindObjectsOfType<DefaultCollector>();
                foreach (DefaultCollector collector in allIridiumCollectors)
                    collector.UpdateCollector();
                break;
            case "iridium storage":

                // Update variables and set main gold storage to 0
                research_iridium_storage += 100;
                //SurvivalCS.iridiumStorage = 0;

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
            case "explosive defenses":
                research_explosive_defenses = true;
                break;
            case "explosive collectors":
                research_explosive_collectors = true;
                break;
            case "fixer drones":
                research_fixer_drones = true;
                break;
            case "combat drones":
                research_combat_drones = true;
                break;
            case "construction speed":
                research_construction_speed += 5f;
                break;
            case "combat speed":
                research_combat_speed += 5f;
                break;
            case "fixer speed":
                research_fixer_speed += 5f;
                break;
            case "resource speed":
                research_resource_speed += 5f;
                break;
            case "resource targets":
                research_resource_amount += 1;
                break;
            case "research":
                research_research_speed += 1;
                break;
            case "piercing":
                research_pierce += 1;
                break;
            default:
                Debug.Log("The research type " + type + " is invalid.");
                break;
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
            ColorUtility.TryParseHtmlString("#A6232399", out colorHolder);
            colors.normalColor = colorHolder;
            ColorUtility.TryParseHtmlString("#BC181899", out colorHolder);
            colors.highlightedColor = colorHolder;
            ColorUtility.TryParseHtmlString("#F31D1D99", out colorHolder);
            colors.pressedColor = colorHolder;
        }
        else if (color == "green")
        {
            ColorUtility.TryParseHtmlString("#259F3B99", out colorHolder);
            colors.normalColor = colorHolder;
            ColorUtility.TryParseHtmlString("#24B93F99", out colorHolder);
            colors.highlightedColor = colorHolder;
            ColorUtility.TryParseHtmlString("#1FD13F99", out colorHolder);
            colors.pressedColor = colorHolder;
        }
        else
        {
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
        {
            research.goldText = research.ResearchButton.transform.Find("Gold Amount").GetComponent<TextMeshProUGUI>();
            research.goldText.text = "x" + research.GoldRequired;
            if (research.EssenceRequired > 0)
            {
                research.essenceText = research.ResearchButton.transform.Find("Essence Amount").GetComponent<TextMeshProUGUI>();
                research.essenceText.text = "x" + research.EssenceRequired;
            }
            if (research.IridiumRequired > 0)
            {
                research.iridiumText = research.ResearchButton.transform.Find("Iridium Amount").GetComponent<TextMeshProUGUI>();
                research.iridiumText.text = "x" + research.IridiumRequired;
            }
        }
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
                if (data[i]) { ApplyResearch(Researchables[i]); };
        } 
        catch
        {
            // Debugs to console that the research data does not match the current versions research structure
            Debug.Log("Mismatch in save data to research data. Skipping itteration.");
        }
    }

    // Set research values when loading data
    public void ResetResearchData()
    {
        LabsAvailable = 0;
        ResearchUnlocked = false;
        ResearchActive = false;
        resourcesNeeded = 0;
        resourcesTracked = 0;
        goldNeeded = 0;
        goldTracked = 0;
        essenceNeeded = 0;
        essenceTracked = 0;
        iridiumNeeded = 0;
        iridiumTracked = 0;
        researchSpeed = 1f;

        research_damage = 0;
        research_burning = 0;
        research_freezing = 0;
        research_poisoning = 0;
        research_shield = 0;
        research_health = 0;
        research_wall_health = 0;
        research_pierce = 0;
        research_range = 0;
        research_firerate = 0;
        research_bulletspeed = 1;
        research_gold_time = 2f;
        research_gold_yield = 20;
        research_gold_storage = 1000;
        research_essence_time = 8f;
        research_essence_yield = 25;
        research_essence_storage = 500;
        research_iridium_time = 12f;
        research_iridium_yield = 10;
        research_iridium_storage = 100;
        research_construction_speed = 35f;
        research_construction_placements = 1;
        research_resource_speed = 25f;
        research_resource_collections = 5;
        research_resource_amount = 3;
        research_resource_range = 50f;
        research_combat_speed = 20f;
        research_fixer_speed = 25f;
        research_fixer_amount = 50;
        research_combat_targets = 10;
        research_explosive_storages = false;
        research_explosive_defenses = false;
        research_explosive_collectors = false;
        research_research_speed = 1;
        research_fixer_drones = false;
        research_combat_drones = false;
    }
}
