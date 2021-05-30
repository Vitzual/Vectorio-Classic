using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using TMPro;
using System.Collections;

public class Interface : MonoBehaviour
{
    // Survival script
    public Survival main;

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
    public Transform[] InfoPanels;
    public Transform[] HotbarUI;
    public Canvas Overlay;
    public bool MenuOpen;
    public bool BuildingOpen;
    public bool ResearchOpen;
    public bool SettingsOpen;
    public bool ControlsOpen;
    public bool EngineerOpen;
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
    public GameObject BossBar;

    // Selected UI elements
    public Transform gold;
    public Transform essence;
    public Transform iridium;
    public Transform heat;
    public Transform power;

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
        ResourceObject.transform.parent = IngameCanvas.transform;
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
        if (Time.timeScale != 1.0f)
            Time.timeScale = 1f;
    }

    // Set the engineer cooldown
    public void SetEngineerTimer(string a)
    {
        EngineerCooldown.text = a;
    }

    // Activates an engineers UI panel
    public void OpenEngineer(bool isActive)
    {
        EngineerCooldownOverlay.SetActive(isActive);
        EngineerOverlay.SetActive(true);
        EngineerOpen = true;
    }

    // Deactivates an engineers UI panel
    public void CloseEngineer()
    {
        EngineerOverlay.SetActive(false);
        EngineerOpen = false;
    }

    // Enables the research overlay
    public void showResearchUnlock()
    {
        UOLOpen = true;
        ResearchUnlockedWindow.OpenWindow();
    }

    // Enables the research overlay
    public void showEnergizerUnlock()
    {
        UOLOpen = true;
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
        {
            CloseResearchOverlay();
            main.SetHoverObject(null);
        }
        if (EngineerOpen)
            CloseEngineer();

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
    public void UpdateInventoryInfo(Transform obj)
    {
        foreach (Transform panel in InfoPanels)
        {
            if (panel.parent.parent.GetComponent<CanvasGroup>().alpha == 1f)
            {
                if (obj == null)
                {
                    panel.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Undiscovered");
                    panel.Find("Name").GetComponent<TextMeshProUGUI>().text = "???";
                    panel.Find("Description").GetComponent<TextMeshProUGUI>().text = "???";
                    panel.Find("Gold").GetComponent<TextMeshProUGUI>().text = "???";
                    panel.Find("Heat").GetComponent<TextMeshProUGUI>().text = "???";
                    panel.Find("Power").GetComponent<TextMeshProUGUI>().text = "???";
                    break;
                }
                else
                {
                    panel.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + obj.name);
                    if (obj.name.Contains("Storage"))
                        panel.Find("Name").GetComponent<TextMeshProUGUI>().text = "STORAGE";
                    else
                        panel.Find("Name").GetComponent<TextMeshProUGUI>().text = obj.name.ToUpper();
                    panel.Find("Description").GetComponent<TextMeshProUGUI>().text = obj.GetComponent<TileClass>().GetDescription();
                    panel.Find("Gold").GetComponent<TextMeshProUGUI>().text = obj.GetComponent<TileClass>().GetCost().ToString();
                    panel.Find("Heat").GetComponent<TextMeshProUGUI>().text = obj.GetComponent<TileClass>().GetHeat().ToString();
                    panel.Find("Power").GetComponent<TextMeshProUGUI>().text = obj.GetComponent<TileClass>().getConsumption().ToString();
                    break;
                }
            }
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
        Time.timeScale = 0f;
    }

    // Closes research overlay and unpauses game
    public void CloseResearchOverlay()
    {
        if (!Research.ResearchUnlocked) return;
        ResearchOpen = false;
        SetOverlayStatus("Research UI", false);
        Time.timeScale = 1f;
    }

    public GameObject GetBossBar()
    {
        return BossBar;
    }
}
