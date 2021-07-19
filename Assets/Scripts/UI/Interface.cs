using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using TMPro;
using System.Collections;

public class Interface : MonoBehaviour
{
    // Survival script
    public Survival main;
    public DroneManager drone;

    // Interace Elements
    public Canvas IngameCanvas;
    public GameObject ResourcePopup;
    public GameObject PipettePopup;
    public GameObject ResearchOverlay;
    public GameObject EngineerOverlay;
    public Transform EngineerList;
    public GameObject EngineerCooldownOverlay;
    public TextMeshProUGUI EngineerCooldown;
    public Image EngineerIcon;
    public TextMeshProUGUI EngineerTitle;
    public TextMeshProUGUI EngineerDescription;
    public TextMeshProUGUI EngineerTime;
    public TextMeshProUGUI EngineerChance;
    public TextMeshProUGUI EngineerCost;
    public Transform[] HotbarUI;
    public Canvas Overlay;
    public bool MenuOpen;
    public bool BuildingOpen;
    public bool ResearchOpen;
    public bool SettingsOpen;
    public bool ControlsOpen;
    public bool DroneOpen;
    public bool BossInfoOpen;
    public bool ShowingInfo;
    public bool UOLOpen;
    public bool EndScreenOpen;
    public TextMeshProUGUI GoldAmount;
    public TextMeshProUGUI EssenceAmount;
    public TextMeshProUGUI IridiumAmount;
    public TextMeshProUGUI GoldStorage;
    public TextMeshProUGUI EssenceStorage;
    public TextMeshProUGUI IridiumStorage;
    public TextMeshProUGUI PowerUsage;
    public TextMeshProUGUI AvailablePower;
    public TextMeshProUGUI GPS;
    public TextMeshProUGUI EPS;
    public TextMeshProUGUI IPS;
    public ModalWindowManager UOL;
    public ModalWindowManager ResearchUnlockedWindow;
    public ModalWindowManager EnergizerUnlockedWindow;
    public ModalWindowManager EndOfEarlyAccessWindow;
    public ModalWindowManager EndOfAlphaWindow;
    public NotificationManager AutosaveComplete;
    public NotificationManager BigWaveIncoming;
    public ProgressBar PowerUsageBar;
    public ProgressBar[] UpgradeProgressBars;
    public TextMeshProUGUI UpgradeProgressName;
    public ButtonManagerBasic SaveButton;
    public ButtonManagerBasicIcon ResearchButton;
    public ButtonManagerBasicIcon[] hotbarButtons;
    public GameObject WarningButton;
    public int ActiveWarnings = 0;
    public GameObject[] BossBar;
    public GameObject dronePortUI;
    public TextMeshProUGUI builderDronesActive;
    public TextMeshProUGUI resourceDronesActive;

    // Selected UI elements
    public Transform gold;
    public Transform essence;
    public Transform iridium;
    public Transform heat;
    public Transform power;

    // Engineer mods
    [System.Serializable]
    public class InfoPanelClass
    {
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        public Image icon;
        public TextMeshProUGUI cost;
        public TextMeshProUGUI heat;
        public TextMeshProUGUI power;
        public TextMeshProUGUI health;
        public TextMeshProUGUI damage;
        public TextMeshProUGUI range;
        public TextMeshProUGUI firerate;
        public TextMeshProUGUI pierce;
        public TextMeshProUGUI rotation;
        public TextMeshProUGUI engineer;
        public ButtonManagerBasic hotbarButton;
        public ButtonManagerBasic engineerButton;
    }
    public InfoPanelClass[] InfoPanels;

    // Drones
    public ProgressBar dronesBar;
    public TextMeshProUGUI dronesText;

    // Start is called before the first frame update
    private void Start()
    {
        // Set default booleans
        MenuOpen = false;
        ResearchOpen = false;
        BuildingOpen = false;
        BossInfoOpen = false;
    }

    public void updateDronesUI(int a, int b)
    {
        dronesBar.currentPercent = a;
        dronesBar.maxValue = b;
        dronesBar.UpdateUI();
        dronesText.text = a + "/" + b;
    }

    public void CreateResourcePopup(string amount, string name, Vector3 position)
    {
        GameObject ResourceObject = Instantiate(ResourcePopup, new Vector3(position.x, position.y + 2.5f, position.z), Quaternion.Euler(new Vector3(0, 0, 0)));
        ResourceObject.transform.parent = IngameCanvas.transform;
        ResourceObject.GetComponent<Popup>().SetPopup(amount, name);
    }

    public void CreatePippeteSquare(Vector3 position)
    {
        GameObject ResourceObject = Instantiate(PipettePopup, new Vector3(position.x, position.y, position.z), Quaternion.Euler(new Vector3(0, 0, 0)));
        ResourceObject.transform.SetParent(IngameCanvas.transform);
    }

    public void DisplayAutosave()
    {
        AutosaveComplete.OpenNotification();
    }

    public void DisplayGroupComing(string a)
    {
        BigWaveIncoming.description = a;
        BigWaveIncoming.UpdateUI();
        BigWaveIncoming.OpenNotification();
        AudioSource sfx = BigWaveIncoming.GetComponent<AudioSource>();
        float volume = GameObject.Find("Manager").GetComponent<Settings>().GetSound();
        if (volume == 0) sfx.volume = 0;
        else
        {
            volume -= 0.4f;
            if (volume < 0.2f) sfx.volume = 0.2f;
            else sfx.volume = volume;
        }
        sfx.Play();
    }

    public void EnableWarning()
    {
        ActiveWarnings++;
        WarningButton.SetActive(true);
        StartCoroutine(DisableWarning(5));
    }

    // Disabled the active warning after 5 seconds
    private IEnumerator DisableWarning(float time)
    {
        yield return new WaitForSeconds(time);
        ActiveWarnings--;
        if (ActiveWarnings == 0) WarningButton.SetActive(false);
    }

    public void OpenEndWindow()
    {
        EndOfEarlyAccessWindow.OpenWindow();
        Time.timeScale = 0f;
        EndScreenOpen = true;
    }

    public void OpenAlphaWindow()
    {
        EndOfAlphaWindow.OpenWindow();
        Time.timeScale = 0f;
        EndScreenOpen = true;
    }

    public void CloseEndWindow()
    {
        EndOfEarlyAccessWindow.CloseWindow();
        Time.timeScale = 1f;
        EndScreenOpen = false;
    }

    public void AdjustTimescale()
    {
        UOLOpen = false;
        if (UOL.isOn) UOL.CloseWindow();
        if (ResearchUnlockedWindow.isOn) ResearchUnlockedWindow.CloseWindow();
        if (EnergizerUnlockedWindow.isOn) EnergizerUnlockedWindow.CloseWindow();
        if (Time.timeScale != 1f)
            Time.timeScale = 1f;
    }

    // Set the engineer cooldown
    public void SetEngineerTimer(string a)
    {
        EngineerCooldown.text = a;
    }

    // Activates an engineers UI panel
    public void OpenDronePort()
    {
        builderDronesActive.text = drone.constructionDrones.Count + drone.availableConstructionDrones.Count + " Currently Active";
        resourceDronesActive.text = drone.resourceDrone.Count + " Currently Active";
        dronePortUI.SetActive(true);
        DroneOpen = true;
    }

    // Deactivates an engineers UI panel
    public void CloseDronePort()
    {
        dronePortUI.SetActive(false);
        DroneOpen = false;
    }

    // Enables the research overlay
    public void showResearchUnlock()
    {
        UOLOpen = true;
        ResearchUnlockedWindow.OpenWindow();
        Research.ResearchUnlocked = true;
    }

    // Enables the research overlay
    public void showEnergizerUnlock()
    {
        EnergizerUnlockedWindow.OpenWindow();
    }

    // Disables the research overlay
    public void closeResearchUnlock()
    {
        UOLOpen = false;
        ResearchUnlockedWindow.CloseWindow();
        Time.timeScale = 1f;
    }

    // Disables the research overlay
    public void closeEnergizerUnlocks()
    {
        UOLOpen = false;
        EnergizerUnlockedWindow.CloseWindow();
        Time.timeScale = 1f;
    }

    // Enable cooling overlay
    public void EnableCooldown()
    {
        EngineerCooldownOverlay.SetActive(true);

    }

    // Disable cooling overlay
    public void DisableCooldown()
    {
        EngineerCooldownOverlay.SetActive(false);
    }

    public void OpenSurvivalMenu()
    {
        // Check if other menus open, if so, close them
        if (ResearchOpen)
            CloseResearchOverlay();
        if (DroneOpen)
            CloseDronePort();

        // Toggle menu
        if (!BuildingOpen)
        {
            BuildingOpen = true;
            SetOverlayStatus("Survival Menu", true);
        }
        else
        {
            BuildingOpen = false;
            SetOverlayStatus("Survival Menu", false);
        }
    }

    // Set the status of an overlay. 
    // a = name of the overlay
    // b = activate or deactive overlay
    public void SetOverlayStatus(string a, bool b)
    {
        if (b)
        {
            Overlay.transform.Find(a).GetComponent<CanvasGroup>().alpha = 1;
            Overlay.transform.Find(a).GetComponent<CanvasGroup>().interactable = true;
            Overlay.transform.Find(a).GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            Overlay.transform.Find(a).GetComponent<CanvasGroup>().alpha = 0;
            Overlay.transform.Find(a).GetComponent<CanvasGroup>().interactable = false;
            Overlay.transform.Find(a).GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    // plz dont ask me how this works :(
    public void UpdateInfoPanel(Transform obj)
    {
        // If null, not unlocked
        if (obj == null) return;

        // Get unlockable
        Technology.Unlockable unlockable = main.tech.GetUnlockableWithBuilding(obj);
        string type;

        if (unlockable == null)
        {
            if (obj.name == "Turret" || obj.name == "Wall") type = "Defenses";
            else if (obj.name == "Drone Port" || obj.name == "Gold Collector" || obj.name == "Gold Storage") type = "Logistics";
            else return;
        }
        else type = unlockable.InvType;

        // Get some good shit
        TileClass tileInfo = obj.GetComponent<TileClass>();

        // Iterate through
        switch (type)
        {
            case "Defenses":
                TurretClass turret = obj.GetComponent<TurretClass>();
                InfoPanels[0].title.text = obj.name.ToUpper();
                InfoPanels[0].description.text = tileInfo.description;
                InfoPanels[0].icon.sprite = Resources.Load<Sprite>("Sprites/" + obj.name);
                InfoPanels[0].cost.text = tileInfo.cost.ToString();
                InfoPanels[0].heat.text = tileInfo.heat.ToString();
                InfoPanels[0].power.text = tileInfo.power.ToString();
                InfoPanels[0].health.text = tileInfo.health + "hp";
                if (obj.name != "Wall")
                {
                    InfoPanels[0].damage.text = turret.bulletDamage + "/ps";
                    InfoPanels[0].range.text = (turret.range / 5) + " tiles";
                    InfoPanels[0].firerate.text = turret.fireRate + "s";
                    InfoPanels[0].pierce.text = turret.bulletPierces + "/ps";
                    InfoPanels[0].rotation.text = turret.rotationSpeed + "v";
                }
                else
                {
                    InfoPanels[0].damage.text = "-";
                    InfoPanels[0].range.text = "-";
                    InfoPanels[0].firerate.text = "-";
                    InfoPanels[0].pierce.text = "-";
                    InfoPanels[0].rotation.text = "-";
                }
                return;
            case "Logistics":
                InfoPanels[1].title.text = obj.name.ToUpper();
                InfoPanels[1].description.text = tileInfo.description;
                InfoPanels[1].icon.sprite = Resources.Load<Sprite>("Sprites/" + obj.name);
                InfoPanels[1].cost.text = tileInfo.cost.ToString();
                InfoPanels[1].heat.text = tileInfo.heat.ToString();
                InfoPanels[1].power.text = tileInfo.power.ToString();
                InfoPanels[1].health.text = tileInfo.health + "hp";
                return;
            case "Power":
                InfoPanels[2].title.text = obj.name.ToUpper();
                InfoPanels[2].description.text = tileInfo.description;
                InfoPanels[2].icon.sprite = Resources.Load<Sprite>("Sprites/" + obj.name);
                InfoPanels[2].cost.text = tileInfo.cost.ToString();
                InfoPanels[2].heat.text = tileInfo.heat.ToString();
                InfoPanels[2].power.text = tileInfo.power.ToString();
                InfoPanels[2].health.text = tileInfo.health + "hp";
                return;
            case "Heat":
                InfoPanels[3].title.text = obj.name.ToUpper();
                InfoPanels[3].description.text = tileInfo.description;
                InfoPanels[3].icon.sprite = Resources.Load<Sprite>("Sprites/" + obj.name);
                InfoPanels[3].cost.text = tileInfo.cost.ToString();
                InfoPanels[3].heat.text = tileInfo.heat.ToString();
                InfoPanels[3].power.text = tileInfo.power.ToString();
                InfoPanels[3].health.text = tileInfo.health + "hp";
                return;
            default:
                Debug.Log("Could not find a valid info panel");
                return;
        }
    }

    public void ShowTileInfo(Transform a)
    {
        // TODO: Fix this bullshit
        Transform b = Overlay.transform.Find("Prompt");
        TileClass c = a.GetComponent<TileClass>();
        b.transform.Find("Health").GetComponent<ProgressBar>().currentPercent = c.GetPercentage();
        b.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = a.name;
        b.transform.Find("Building").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + a.name);
        b.transform.Find("Gold Amount").GetComponent<TextMeshProUGUI>().text = (c.GetCost() - c.GetCost()/5).ToString();
        b.transform.Find("Power Amount").GetComponent<TextMeshProUGUI>().text = c.getConsumption().ToString();
        b.transform.Find("Heat Amount").GetComponent<TextMeshProUGUI>().text = c.GetHeat().ToString();
    }

    public void ShowSelectedInfo(Transform a)
    {
        if(!main.BuildingStats.activeSelf)
            main.BuildingStats.SetActive(true);

        string goldUI = a.GetComponent<TileClass>().GetCost().ToString();
        string powerUI = a.GetComponent<TileClass>().getConsumption().ToString();
        string heatUI = a.GetComponent<TileClass>().GetHeat().ToString();

        gold.GetChild(0).GetComponent<TextMeshProUGUI>().text = goldUI;
        gold.GetChild(1).GetComponent<TextMeshProUGUI>().text = goldUI;
        power.GetChild(0).GetComponent<TextMeshProUGUI>().text = powerUI;
        power.GetChild(1).GetComponent<TextMeshProUGUI>().text = powerUI;
        heat.GetChild(0).GetComponent<TextMeshProUGUI>().text = heatUI;
        heat.GetChild(1).GetComponent<TextMeshProUGUI>().text = heatUI;
    }

    public void SetSelectedHotbar(int index)
    {
        HotbarUI[index].GetComponent<Button>().interactable = false;
    }

    // why, why is u not wokr
    public void UpdateHotbar()
    {
        for (int i = 0; i < main.hotbar.Length; i++)
        {
            if (main.hotbar[i] != null)
            {
                if (main.hotbar[i].name == "Gold Storage")
                    hotbarButtons[i].buttonIcon = Resources.Load<Sprite>("Sprites/Gold Storage Small");
                else hotbarButtons[i].buttonIcon = Resources.Load<Sprite>("Sprites/" + main.hotbar[i].name);
            }
            else
            {
                hotbarButtons[i].buttonIcon = Resources.Load<Sprite>("Sprites/Undiscovered");
            }
            hotbarButtons[i].UpdateUI();
        }
    }

    // Disables active information
    public void DisableActiveInfo()
    {
        for (int i = 0; i < 9; i++)
            HotbarUI[i].GetComponent<Button>().interactable = true;
    }

    // Opens research overlay and pauses game
    public void OpenResearchOverlay()
    {
        if (!Research.ResearchUnlocked) return;
        ResearchOpen = true;
        SetOverlayStatus("Research UI", true);
    }

    // Closes research overlay and unpauses game
    public void CloseResearchOverlay()
    {
        if (!Research.ResearchUnlocked) return;
        ResearchOpen = false;
        SetOverlayStatus("Research UI", false);
    }

    public GameObject GetBossBar(int a)
    {
        return BossBar[a];
    }
}
