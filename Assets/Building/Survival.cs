using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;

public class Survival : MonoBehaviour
{
    // Player stats
    public int gold = 0;
    public int essence = 0;
    public int iridium = 0;
    public int PowerConsumption = 0;
    public int AvailablePower = 100;

    // Placement sprites
    private SpriteRenderer Selected;
    private float Adjustment = 1f;
    private int AdjustLimiter = 0;
    private bool AdjustSwitch = false;

    // Object placements
    [SerializeField] private GameObject HubObj;        // No ID
    [SerializeField] private GameObject TurretObj;     // ID = 0
    [SerializeField] private GameObject WallObj;       // ID = 1
    [SerializeField] private GameObject CollectorObj;  // ID = 2
    [SerializeField] private GameObject ShotgunObj;    // ID = 3
    [SerializeField] private GameObject SniperObj;     // ID = 4
    [SerializeField] private GameObject EnhancerObj;   // ID = 5
    [SerializeField] private GameObject SMGObj;        // ID = 6
    [SerializeField] private GameObject BoltObj;       // ID = 7
    [SerializeField] private GameObject ChillerObj;    // ID = 8
    [SerializeField] private GameObject RocketObj;     // ID = 9

    // Object variables
    public GameObject Spawner;
    public GameObject SelectedOverlay;
    private GameObject SelectedObj;
    private GameObject LastObj;
    private float rotation = 0f;
    public bool largerUnit = false;

    // UI Elements
    public Canvas Overlay;
    private bool MenuOpen;
    private bool BuildingOpen;
    private bool ShowingInfo;
    public TextMeshProUGUI GoldAmount;
    public ModalWindowManager UOL;
    public ProgressBar PowerUsageBar;
    public ProgressBar[] UpgradeProgressBars;
    public TextMeshProUGUI UpgradeProgressName;

    // Internal placement variables
    [SerializeField] private LayerMask TileLayer;
    [SerializeField] private LayerMask UILayer;
    private Vector2 MousePos;
    delegate void HotbarItem();
    protected float distance = 10;
    List<HotbarItem> hotbar = new List<HotbarItem>();
    List<GameObject> unlocked = new List<GameObject>();

    // Unlock list
    public int UnlockLvl = 0;
    public bool UnlocksLeft = true;
    [System.Serializable]
    public class Unlockables
    {
        public GameObject Unlock;
        public ButtonManagerBasicIcon InventoryButton;
        public Transform[] Enemy;
        public int[] AmountNeeded;
        public int[] AmountTracked;
    }
    public Unlockables[] UnlockTier;

    private void Start()
    {
        // Assign default variables
        Selected = GetComponent<SpriteRenderer>();
        MenuOpen = false;
        BuildingOpen = false;

        // Default starting unlocks / hotbar
        hotbar.Add(SetTurret);
        hotbar.Add(SetRocket);
        hotbar.Add(SetCollector);
        hotbar.Add(SetShotgun);
        hotbar.Add(SetSniper);
        hotbar.Add(SetEnhancer);
        hotbar.Add(SetSMG);
        hotbar.Add(SetBolt);
        hotbar.Add(SetChiller);
        unlocked.Add(TurretObj);
        unlocked.Add(CollectorObj);
        unlocked.Add(WallObj);
        unlocked.Add(RocketObj);

        // Check for save data on start, and if there is, set values for everything.
        try
        {
            SaveData data = SaveSystem.LoadGame();
            PowerConsumption = data.PowerUsage;
            AvailablePower = data.PowerAvailable;
            gold = data.Gold;
            essence = data.Essence;
            iridium = data.Iridium;
            UnlockLvl = data.UnlockLevel - 1;
            Spawner.GetComponent<WaveSpawner>().increaseHeat(data.HeatUsage);
            Debug.Log("Save data was found and loaded");

            UpdateGui();
            StartNextUnlock();
            UpdateUnlockableGui();
            PlaceSavedBuildings(data.Locations);
            PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
        }
        catch
        {
            Debug.Log("No save data was found, or the save data found was corrupt.");
        }
    }

    private void Update()
    {
        // Get mouse position and round to middle grid coordinate
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!largerUnit) transform.position = new Vector2(5 * Mathf.Round(MousePos.x / 5), 5 * Mathf.Round(MousePos.y / 5));
        else transform.position = new Vector2(5 * Mathf.Round(MousePos.x / 5) - 2.5f, 5 * Mathf.Round(MousePos.y / 5) + 2.5f);

        // Make color flash
        Color tmp = this.GetComponent<SpriteRenderer>().color;
        tmp.a = Adjustment;
        this.GetComponent<SpriteRenderer>().color = tmp;
        AdjustAlphaValue();

        // If user left clicks, place object
        if (Input.GetButton("Fire1") && !BuildingOpen)
        {
            bool ValidTile = true;
            if (SelectedObj == RocketObj)
            {
                // Check for wires and adjust accordingly 
                RaycastHit2D a = Physics2D.Raycast(new Vector2(MousePos.x, MousePos.y), Vector2.zero, Mathf.Infinity, TileLayer);
                RaycastHit2D b = Physics2D.Raycast(new Vector2(MousePos.x - 5f, MousePos.y), Vector2.zero, Mathf.Infinity, TileLayer);
                RaycastHit2D c = Physics2D.Raycast(new Vector2(MousePos.x, MousePos.y + 5f), Vector2.zero, Mathf.Infinity, TileLayer);
                RaycastHit2D d = Physics2D.Raycast(new Vector2(MousePos.x - 5f, MousePos.y + 5f), Vector2.zero, Mathf.Infinity, TileLayer);

                if (a.collider != null || b.collider != null || c.collider != null || d.collider != null) ValidTile = false;
            }

            //if (ValidTile) Debug.Log("Valid placement");
            //else Debug.Log("Invalid placement");

            // Raycast tile to see if there is already a tile placed
            RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

            if (ValidTile && rayHit.collider == null && SelectedObj != null && transform.position.x <= 250 && transform.position.x >= -245 && transform.position.y <= 245 && transform.position.y >= -245)
            {
                int cost = SelectedObj.GetComponent<TileClass>().GetCost();
                int power = SelectedObj.GetComponent<TileClass>().getConsumption();
                if (cost <= gold && PowerConsumption + power <= AvailablePower)
                {
                    gold -= cost;
                    UpdateGui();
                    if (SelectedObj == WallObj)
                    {
                        LastObj = Instantiate(SelectedObj, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                    }
                    else
                    {
                        LastObj = Instantiate(SelectedObj, transform.position, Quaternion.Euler(new Vector3(0, 0, rotation)));
                    }
                    LastObj.name = SelectedObj.name;
                    increasePowerConsumption(LastObj.GetComponent<TileClass>().getConsumption());
                    Spawner.GetComponent<WaveSpawner>().increaseHeat(SelectedObj.GetComponent<TileClass>().GetHeat());
                }
            }
            else if (rayHit.collider != null)
            {
                if (rayHit.collider.name != "Hub")
                {
                    ShowTileInfo(rayHit.collider);
                    ShowingInfo = true;
                    SelectedOverlay.transform.position = rayHit.collider.transform.position;
                    SelectedOverlay.SetActive(true);
                }
            }
        }

        // If user right clicks, remove object
        else if (Input.GetButton("Fire2") && !BuildingOpen)
        {
            //Overlay.transform.Find("Hovering Stats").GetComponent<CanvasGroup>().alpha = 0;
            RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

            // Raycast tile to see if there is already a tile placed
            if (rayHit.collider != null && rayHit.collider.name != "Hub")
            {
                if (rayHit.collider.name == "Wall")
                {
                    RaycastHit2D a = Physics2D.Raycast(new Vector2(transform.position.x + 5f, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
                    RaycastHit2D b = Physics2D.Raycast(new Vector2(transform.position.x - 5f, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
                    RaycastHit2D c = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 5f), Vector2.zero, Mathf.Infinity, TileLayer);
                    RaycastHit2D d = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 5f), Vector2.zero, Mathf.Infinity, TileLayer);
                    if (a.collider != null && a.collider.name == "Wall")
                    {
                        a.collider.GetComponent<WallAI>().UpdateSprite(-1);
                    }
                    if (b.collider != null && b.collider.name == "Wall")
                    {
                        b.collider.GetComponent<WallAI>().UpdateSprite(-3);
                    }
                    if (c.collider != null && c.collider.name == "Wall")
                    {
                        c.collider.GetComponent<WallAI>().UpdateSprite(-2);
                    }
                    if (d.collider != null && d.collider.name == "Wall")
                    {
                        d.collider.GetComponent<WallAI>().UpdateSprite(-4);
                    }
                }

                ResetTileInfo();
                ShowingInfo = false;
                SelectedOverlay.SetActive(false);
                Spawner.GetComponent<WaveSpawner>().decreaseHeat(SelectedObj.GetComponent<TileClass>().GetHeat());
                decreasePowerConsumption(rayHit.collider.gameObject.GetComponent<TileClass>().getConsumption());
                int cost = rayHit.collider.GetComponent<TileClass>().GetCost();
                gold += cost - cost / 5;
                UpdateGui();
                Destroy(rayHit.collider.gameObject);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectHotbar(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectHotbar(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectHotbar(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectHotbar(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectHotbar(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectHotbar(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectHotbar(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SelectHotbar(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SelectHotbar(8);
        }
        else if (Input.GetKeyDown(KeyCode.R) && BuildingOpen == false && MenuOpen == false && SelectedObj != null)
        {
            rotation = rotation -= 90f;
            if (rotation == -360f)
            {
                rotation = 0;
            }
            Selected.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        }
        if (Input.GetKeyDown(KeyCode.E) && BuildingOpen == false)
        {
            BuildingOpen = true;
            Overlay.transform.Find("Survival Menu").GetComponent<CanvasGroup>().alpha = 1;
            Overlay.transform.Find("Survival Menu").GetComponent<CanvasGroup>().interactable = true;
            Overlay.transform.Find("Survival Menu").GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) && BuildingOpen == true)
        {
            BuildingOpen = false;
            Overlay.transform.Find("Survival Menu").GetComponent<CanvasGroup>().alpha = 0;
            Overlay.transform.Find("Survival Menu").GetComponent<CanvasGroup>().interactable = false;
            Overlay.transform.Find("Survival Menu").GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && SelectedObj != null)
        {
            Overlay.transform.Find("Selected").GetComponent<CanvasGroup>().alpha = 0;
            Selected.sprite = null;
            SelectedObj = null;
            rotation = 0;
            ResetTileInfo();
            DisableActiveInfo();
            ShowingInfo = false;
            SelectedOverlay.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && ShowingInfo == true)
        {
            ResetTileInfo();
            ShowingInfo = false;
            SelectedOverlay.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && MenuOpen == false)
        {
            MenuOpen = true;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().alpha = 1;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().blocksRaycasts = true;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().interactable = true;
            Overlay.transform.Find("Return (1)").GetComponent<CanvasGroup>().alpha = 1;
            Overlay.transform.Find("Return (1)").GetComponent<CanvasGroup>().blocksRaycasts = true;
            Overlay.transform.Find("Return (1)").GetComponent<CanvasGroup>().interactable = true;

            Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuOpen = false;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().alpha = 0;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().blocksRaycasts = false;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().interactable = false;
            Overlay.transform.Find("Return (1)").GetComponent<CanvasGroup>().alpha = 0;
            Overlay.transform.Find("Return (1)").GetComponent<CanvasGroup>().blocksRaycasts = false;
            Overlay.transform.Find("Return (1)").GetComponent<CanvasGroup>().interactable = false;

            Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        }
    }

    public void PlaceSavedBuildings(int[,] a)
    {
        for (int i = 0; i < a.GetLength(0); i++)
        {
            GameObject building = GetBuildingWithID(a[i, 0]);
            GameObject obj = Instantiate(building, new Vector3(a[i,2], a[i,3],0), Quaternion.Euler(new Vector3(0, 0, 0)));
            obj.name = building.name;
            obj.GetComponent<TileClass>().SetLevel(a[i, 1]);
        }
    }

    public GameObject GetBuildingWithID(int a)
    {
        for (int i = 0; i < unlocked.Count; i++)
        {
            if (unlocked[i].GetComponent<TileClass>().getID() == a)
            {
                return unlocked[i];
            }
        }
        return TurretObj;
    }

    public void UpdateUnlockableGui()
    {
        for (int i=0; i<UnlockLvl; i++)
        {
            addUnlocked(UnlockTier[i].Unlock);
            UnlockTier[i].InventoryButton.normalIcon.sprite = Resources.Load<Sprite>("Sprites/" + UnlockTier[i].Unlock.transform.name);
        }
    }

    public void UpdateUnlock(Transform a)
    {
        if (UnlocksLeft)
        {
            // Itterate through list and update GUI accordingly
            for (int i = 0; i < UnlockTier[UnlockLvl].Enemy.Length; i++)
            {
                if (UnlockTier[UnlockLvl].Enemy[i].name == a.name)
                {
                    // Increment amount tracked and update GUI
                    UnlockTier[UnlockLvl].AmountTracked[i] += 1;
                    UpdateUnlockGui(i, ((double)UnlockTier[UnlockLvl].AmountTracked[i] / (double)UnlockTier[UnlockLvl].AmountNeeded[i]) * 100);
                }
            }

            // Check if requirements have been met
            bool RequirementsMetCheck = true;
            for (int i = 0; i < UnlockTier[UnlockLvl].Enemy.Length; i++)
            {
                if (UnlockTier[UnlockLvl].AmountTracked[i] < UnlockTier[UnlockLvl].AmountNeeded[i])
                {
                    RequirementsMetCheck = false;
                }
            }

            // If requirements met, unlock and start next unlock
            if (RequirementsMetCheck == true)
            {
                GameObject newUnlock = UnlockTier[UnlockLvl].Unlock;

                if (UnlockLvl == 0)
                {
                    ButtonManagerBasicIcon bm = Overlay.transform.Find("Four").GetComponent<ButtonManagerBasicIcon>();
                    bm.buttonIcon = Resources.Load<Sprite>("Sprites/" + newUnlock.name);
                    bm.UpdateUI();
                }
                else if (UnlockLvl == 1)
                {
                    ButtonManagerBasicIcon bm = Overlay.transform.Find("Five").GetComponent<ButtonManagerBasicIcon>();
                    bm.buttonIcon = Resources.Load<Sprite>("Sprites/" + newUnlock.name);
                    bm.UpdateUI();
                }
                else if (UnlockLvl == 2)
                {
                    ButtonManagerBasicIcon bm = Overlay.transform.Find("Six").GetComponent<ButtonManagerBasicIcon>();
                    bm.buttonIcon = Resources.Load<Sprite>("Sprites/" + newUnlock.name);
                    bm.UpdateUI();
                }
                else if (UnlockLvl == 3)
                {
                    ButtonManagerBasicIcon bm = Overlay.transform.Find("Seven").GetComponent<ButtonManagerBasicIcon>();
                    bm.buttonIcon = Resources.Load<Sprite>("Sprites/" + newUnlock.name);
                    bm.UpdateUI();
                }
                else if (UnlockLvl == 4)
                {
                    ButtonManagerBasicIcon bm = Overlay.transform.Find("Eight").GetComponent<ButtonManagerBasicIcon>();
                    bm.buttonIcon = Resources.Load<Sprite>("Sprites/" + newUnlock.name);
                    bm.UpdateUI();
                }
                else if (UnlockLvl == 5)
                {
                    ButtonManagerBasicIcon bm = Overlay.transform.Find("Nine").GetComponent<ButtonManagerBasicIcon>();
                    bm.buttonIcon = Resources.Load<Sprite>("Sprites/" + newUnlock.name);
                    bm.UpdateUI();
                }

                unlockDefense(newUnlock, UnlockTier[UnlockLvl].InventoryButton, newUnlock.GetComponent<TileClass>().GetDescription());
                StartNextUnlock();
            }
        }
    }

    public void UpdateUnlockGui(int a, double b)
    {
        UpgradeProgressBars[a].currentPercent = (float)b;
    }

    public void StartNextUnlock()
    {
        UnlockLvl += 1;
        Transform c = Overlay.transform.Find("Upgrade");
        try
        {
            int z = UnlockTier[UnlockLvl].Enemy.Length;
        }
        catch
        {
            UnlocksLeft = false;
            c.gameObject.SetActive(false);
        }
        finally
        {
            for (int i = 0; i <= 4; i++)
            {
                UpgradeProgressBars[i].currentPercent = 0;
                try
                {
                    UpgradeProgressBars[i].transform.Find("Type").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + UnlockTier[UnlockLvl].Enemy[i].name);
                }
                catch
                {
                    UpgradeProgressBars[i].transform.Find("Type").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + "Undiscovered");
                }
            }
            UpgradeProgressName.text = UnlockTier[UnlockLvl].Unlock.transform.name;
        }
    }

    void ShowTileInfo(Collider2D a)
    {
        Transform b = Overlay.transform.Find("Prompt");
        b.transform.Find("Health").GetComponent<ProgressBar>().currentPercent = a.GetComponent<TileClass>().GetPercentage();
        b.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = a.name;
        b.transform.Find("Tier").GetComponent<TextMeshProUGUI>().text = a.GetComponent<TileClass>().GetTier();
    }

    void ResetTileInfo()
    {
        Transform b = Overlay.transform.Find("Prompt");
        b.transform.Find("Health").GetComponent<ProgressBar>().currentPercent = HubObj.GetComponent<TileClass>().GetPercentage();
        b.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = "Headquarters";
        b.transform.Find("Tier").GetComponent<TextMeshProUGUI>().text = HubObj.GetComponent<TileClass>().GetTier();
    }

    void ShowSelectedInfo(GameObject a)
    {
        Overlay.transform.Find("Selected").GetComponent<CanvasGroup>().alpha = 1;
        Transform b = Overlay.transform.Find("Selected");
        b.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = a.name + " (" + a.GetComponent<TileClass>().GetTier() + ")";
        b.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = a.GetComponent<TileClass>().GetCost().ToString();
        b.transform.Find("Building").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + a.name);
    }

    public void unlockDefense(GameObject a, ButtonManagerBasicIcon b, string c)
    {
        addUnlocked(a);
        b.normalIcon.sprite = Resources.Load<Sprite>("Sprites/" + a.transform.name);
        UOL.icon = Resources.Load<Sprite>("Sprites/" + a.transform.name);
        UOL.titleText = a.transform.name.ToUpper();
        UOL.descriptionText = c;
        UOL.UpdateUI();
        UOL.OpenWindow();
    }

    public void UpdateGui()
    {
        GoldAmount.text = gold.ToString();
    }

    public void increaseAvailablePower(int a)
    {
        AvailablePower += a;
        PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
    }

    public void decreaseAvailablePower(int a)
    {
        AvailablePower -= a;
        PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
    }

    public int getAvailablePower()
    {
        return AvailablePower;
    }

    public void increasePowerConsumption(int a)
    {
        PowerConsumption += a;
        PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
    }

    public void decreasePowerConsumption(int a)
    {
        PowerConsumption -= a;
        PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
    }

    public int getPowerConsumption()
    {
        return PowerConsumption;
    }

    public void AddGold(int a)
    {
        gold += a;
    }

    public void RemoveGold(int a)
    {
        gold -= a;
    }

    public void AdjustAlphaValue()
    {
        if (AdjustLimiter == 10)
        {
            if (AdjustSwitch == false)
            {
                Adjustment -= 0.1f;
            }
            else if (AdjustSwitch == true)
            {
                Adjustment += 0.1f;
            }
            if (AdjustSwitch == false && Adjustment <= 0f)
            {
                AdjustSwitch = true;
            }
            else if (AdjustSwitch == true && Adjustment >= 1f)
            {
                AdjustSwitch = false;
            }
            AdjustLimiter = 0;
        }
        else
        {
            AdjustLimiter += 1;
        }
    }

    public void SelectHotbar(int index)
    {
        try
        {
            hotbar[index]();
        } catch { return; }
        if (index == 0)
        {
            Overlay.transform.Find("One").GetComponent<Button>().interactable = false;
        }
        else if (index == 1)
        {
            Overlay.transform.Find("Two").GetComponent<Button>().interactable = false;
        }
        else if (index == 2)
        {
            Overlay.transform.Find("Three").GetComponent<Button>().interactable = false;
        }
        else if (index == 3)
        {
            Overlay.transform.Find("Four").GetComponent<Button>().interactable = false;
        }
        else if (index == 4)
        {
            Overlay.transform.Find("Five").GetComponent<Button>().interactable = false;
        }
        else if (index == 5)
        {
            Overlay.transform.Find("Six").GetComponent<Button>().interactable = false;
        }
        else if (index == 6)
        {
            Overlay.transform.Find("Seven").GetComponent<Button>().interactable = false;
        }
        else if (index == 7)
        {
            Overlay.transform.Find("Eight").GetComponent<Button>().interactable = false;
        }
        else
        {
            Overlay.transform.Find("Nine").GetComponent<Button>().interactable = false;
        }
    }

    public void SetTurret()
    {
        if (checkIfUnlocked(TurretObj))
        {
            SelectedObj = TurretObj;
            SwitchObj();
        }
    }

    public void SetShotgun()
    {
        if (checkIfUnlocked(ShotgunObj))
        {
            SelectedObj = ShotgunObj;
            SwitchObj();
        }
    }

    public void SetSniper()
    {
        if (checkIfUnlocked(SniperObj))
        {
            SelectedObj = SniperObj;
            SwitchObj();
        }
    }

    public void SetSMG()
    {
        if (checkIfUnlocked(SMGObj))
        {
            SelectedObj = SMGObj;
            SwitchObj();
        }
    }

    public void SetBolt()
    {
        if (checkIfUnlocked(BoltObj))
        {
            SelectedObj = BoltObj;
            SwitchObj();
        }
    }

    public void SetWall()
    {
        if (checkIfUnlocked(WallObj))
        {
            SelectedObj = WallObj;
            SwitchObj();
        }
    }

    public void SetCollector()
    {
        if (checkIfUnlocked(CollectorObj))
        {
            SelectedObj = CollectorObj;
            SwitchObj();
        }
    }

    public void SetEnhancer()
    {
        if (checkIfUnlocked(EnhancerObj))
        {
            SelectedObj = EnhancerObj;
            SwitchObj();
        }
    }

    public void SetChiller()
    {
        if (checkIfUnlocked(ChillerObj))
        {
            SelectedObj = ChillerObj;
            SwitchObj();
        }
    }

    public void SetRocket()
    {
        if (checkIfUnlocked(RocketObj))
        {
            SelectedObj = RocketObj;
            SwitchObj();
            largerUnit = true;
            transform.localScale = new Vector3(2, 2, 1);
        }
    }

    public void SwitchObj()
    {
        if (largerUnit)
        {
            largerUnit = false;
            transform.localScale = new Vector3(1, 1, 1);
        }
        DisableActiveInfo();
        Adjustment = 1f;
        Selected.sprite = Resources.Load<Sprite>("Sprites/" + SelectedObj.name);
        ShowSelectedInfo(SelectedObj);
    }

    public bool checkIfUnlocked(GameObject a)
    {
        for (int i = 0; i < unlocked.Count; i++)
        {
            if (a.name == unlocked[i].name)
            {
                return true;
            }
        }
        DisableActiveInfo();
        return false;
    }

    public void DisableActiveInfo()
    {
        Overlay.transform.Find("One").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Two").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Three").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Four").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Five").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Six").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Seven").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Eight").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Nine").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Wire").GetComponent<CanvasGroup>().interactable = true;
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Save()
    {
        Debug.Log("Attempting to save data");
        SaveSystem.SaveGame(this, Spawner.GetComponent<WaveSpawner>());
        Debug.Log("Data was saved successfully");
    }

    public int[,] GetLocationData()
    {
        Transform[] allObjects = FindObjectsOfType<Transform>();

        int length = 0;
        for (int i = 0; i < allObjects.Length; i++)
        {
            if (allObjects[i].tag == "Defense") length += 1;
        }

        int[,] data = new int[length, 4];
        length = 0;
        for (int i=0; i<allObjects.Length; i++)
        {
            if (allObjects[i].tag == "Defense")
            {
                data[length, 0] = allObjects[i].GetComponent<TileClass>().getID();
                data[length, 1] = allObjects[i].GetComponent<TileClass>().GetLevel();
                data[length, 2] = (int)allObjects[i].position.x;
                data[length, 3] = (int)allObjects[i].position.y;
                length += 1;
            }
        }

        return data;
    }

    public void addUnlocked(GameObject a)
    {
        unlocked.Add(a);
    }

}