using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using BayatGames.SaveGameFree;
using TMPro;

public class Survival : MonoBehaviour
{
    // Player stats
    public int gold = 0;
    protected int essence = 0;
    protected int iridium = 0;

    // Placement sprites
    private SpriteRenderer Selected;
    private float Adjustment = 1f;
    private int AdjustLimiter = 0;
    private bool AdjustSwitch = false;

    // Object placements
    [SerializeField] private GameObject HubObj;
    [SerializeField] private GameObject SniperObj;
    [SerializeField] private GameObject TurretObj;
    [SerializeField] private GameObject BoltObj;
    [SerializeField] private GameObject ShotgunObj;
    [SerializeField] private GameObject WallObj;
    [SerializeField] private GameObject SMGObj;
    [SerializeField] private GameObject MineObj;
    [SerializeField] private GameObject ConveyorObj;
    [SerializeField] private GameObject CollectorObj;
    [SerializeField] private GameObject WireObj;
    [SerializeField] private GameObject ProjectorObj;
    [SerializeField] private GameObject VoidripperObj;

    // Object variables
    public Transform[] ObjectsToSave;
    public GameObject Spawner;
    public GameObject SelectedOverlay;
    private GameObject SelectedObj;
    private GameObject LastObj;
    private float rotation = 0f;

    // UI Elements
    public Canvas Overlay;
    private bool MenuOpen;
    private bool BuildingOpen;
    private bool ShowingInfo;
    public TextMeshProUGUI GoldAmount;
    public ModalWindowManager UOL;
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
    private int UnlockLvl = 0;
    private bool UnlocksLeft = true;
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
        hotbar.Add(SetWall);
        hotbar.Add(SetMine);
        hotbar.Add(SetShotgun);
        hotbar.Add(SetSniper);
        hotbar.Add(SetCollector);
        hotbar.Add(SetSMG);
        hotbar.Add(SetBolt);
        hotbar.Add(SetVoidripper);
        unlocked.Add(TurretObj);
        unlocked.Add(WallObj);
        unlocked.Add(MineObj);
        unlocked.Add(WireObj);
    }

    private void Update()
    {
        // Get mouse position and round to middle grid coordinate
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(5*Mathf.Round(MousePos.x/5), 5*Mathf.Round(MousePos.y/5));

        // Make color flash
        Color tmp = this.GetComponent<SpriteRenderer>().color;
        tmp.a = Adjustment;
        this.GetComponent<SpriteRenderer>().color = tmp;
        AdjustAlphaValue();

        // If user left clicks, place object
        if (Input.GetButton("Fire1") && !BuildingOpen)
        {
            //Overlay.transform.Find("Hovering Stats").GetComponent<CanvasGroup>().alpha = 0;
            RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

            // Raycast tile to see if there is already a tile placed
            if (rayHit.collider == null && transform.position.x <= 245 && transform.position.x >= -245 && transform.position.y <= 245 && transform.position.y >= -245)
            {
                int cost = SelectedObj.GetComponent<TileClass>().GetCost();
                if (cost <= gold)
                {
                    gold -= cost;
                    UpdateGui();
                    if (SelectedObj == WallObj || SelectedObj == WireObj)
                    {
                        LastObj = Instantiate(SelectedObj, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                    }
                    else
                    {
                        LastObj = Instantiate(SelectedObj, transform.position, Quaternion.Euler(new Vector3(0, 0, rotation)));
                    }
                    LastObj.name = SelectedObj.name;
                    Spawner.GetComponent<WaveSpawner>().increaseHeat(SelectedObj.GetComponent<TileClass>().GetHeat());

                    if (SelectedObj != WallObj)
                    {
                        // Check for wires and adjust accordingly 
                        RaycastHit2D a = Physics2D.Raycast(new Vector2(LastObj.transform.position.x + 5f, LastObj.transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
                        RaycastHit2D b = Physics2D.Raycast(new Vector2(LastObj.transform.position.x - 5f, LastObj.transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
                        RaycastHit2D c = Physics2D.Raycast(new Vector2(LastObj.transform.position.x, LastObj.transform.position.y + 5f), Vector2.zero, Mathf.Infinity, TileLayer);
                        RaycastHit2D d = Physics2D.Raycast(new Vector2(LastObj.transform.position.x, LastObj.transform.position.y - 5f), Vector2.zero, Mathf.Infinity, TileLayer);

                        if (a.collider != null && a.collider.name == "Wire")
                        {
                            a.collider.GetComponent<WireAI>().UpdateSprite(1);
                        }
                        if (b.collider != null && b.collider.name == "Wire")
                        {
                            b.collider.GetComponent<WireAI>().UpdateSprite(3);
                        }
                        if (c.collider != null && c.collider.name == "Wire")
                        {
                            c.collider.GetComponent<WireAI>().UpdateSprite(2);
                        }
                        if (d.collider != null && d.collider.name == "Wire")
                        {
                            d.collider.GetComponent<WireAI>().UpdateSprite(4);
                        }
                    }
                }
            } 
            else
            {
                if(rayHit.collider.name != "Hub")
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
                ResetTileInfo();
                ShowingInfo = false;
                SelectedOverlay.SetActive(false);
                Spawner.GetComponent<WaveSpawner>().decreaseHeat(SelectedObj.GetComponent<TileClass>().GetHeat());
                int cost = rayHit.collider.GetComponent<TileClass>().GetCost();
                gold += cost - cost / 5;
                UpdateGui();
                Destroy(rayHit.collider.gameObject);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SelectHotbar(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SelectHotbar(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SelectHotbar(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            SelectHotbar(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            SelectHotbar(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            SelectHotbar(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7)) {
            SelectHotbar(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8)) {
            SelectHotbar(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9)) {
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
                    UpdateUnlockGui(i, ((double)UnlockTier[UnlockLvl].AmountTracked[i] / (double)UnlockTier[UnlockLvl].AmountNeeded[i])*100);
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
        b.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = a.name + " ("+ a.GetComponent<TileClass>().GetTier() + ")";
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

    public void SetMine()
    {
        if (checkIfUnlocked(MineObj))
        {
            SelectedObj = MineObj;
            SwitchObj();
        }
    }

    public void SetProjector()
    {
        if (checkIfUnlocked(ProjectorObj))
        {
            SelectedObj = ProjectorObj;
            SwitchObj();
        }
    }

    public void SetWire()
    {
        if (checkIfUnlocked(WireObj))
        {
            SelectedObj = WireObj;
            SwitchObj();
        }
    }

    public void SetConveyor()
    {
        if (checkIfUnlocked(ConveyorObj))
        {
            SelectedObj = ConveyorObj;
            SwitchObj();
        }
    }

    public void SetVoidripper()
    {
        if (checkIfUnlocked(VoidripperObj))
        {
            SelectedObj = VoidripperObj;
            SwitchObj();
        }
    }

    public void SwitchObj()
    {
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
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Save()
    {
        Debug.Log("Saving...");
        Transform[] allObjects = FindObjectsOfType<Transform>();
        SaveGame.Save<Transform[]>("save.txt", allObjects);
        Debug.Log("Saved!");
    }

    public void addUnlocked(GameObject a)
    {
        unlocked.Add(a);
    }

}