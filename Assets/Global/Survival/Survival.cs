using UnityEngine;
using UnityEngine.SceneManagement;

public class Survival : MonoBehaviour
{
    // Technology script
    private Technology tech;

    // Interface script
    private Interface UI;

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
    [SerializeField] private Transform HubObj;           // No ID
    [SerializeField] private Transform TurretObj;        // ID = 0
    [SerializeField] private Transform WallObj;          // ID = 1
    [SerializeField] private Transform CollectorObj;     // ID = 2
    [SerializeField] private Transform ShotgunObj;       // ID = 3
    [SerializeField] private Transform SniperObj;        // ID = 4
    [SerializeField] private Transform EnhancerObj;      // ID = 5
    [SerializeField] private Transform SMGObj;           // ID = 6
    [SerializeField] private Transform BoltObj;          // ID = 7
    [SerializeField] private Transform ChillerObj;       // ID = 8
    [SerializeField] private Transform RocketObj;        // ID = 9
    [SerializeField] private Transform EssenceObj;       // ID = 10
    [SerializeField] private Transform TurbineObj;       // ID = 11

    // Object variables
    public int seed;
    public GameObject Spawner;
    public GameObject SelectedOverlay;
    private Transform SelectedObj;
    private Transform HoveredObj;
    private Transform LastObj;
    private float rotation = 0f;
    public bool largerUnit = false;

    // Internal placement variables
    [SerializeField] private LayerMask ResourceLayer;
    [SerializeField] private LayerMask TileLayer;
    [SerializeField] private LayerMask UILayer;
    private Vector2 MousePos;
    protected float distance = 10;
    public Transform[] hotbar = new Transform[9];

    private void Start()
    {
        // Assign technology script
        tech = gameObject.GetComponent<Technology>();

        // Assign interface script
        UI = gameObject.GetComponent<Interface>();

        // Assign default variables
        Selected = GetComponent<SpriteRenderer>();

        // Default starting unlocks / hotbar
        PopulateHotbar();
        tech.unlocked.Add(TurretObj);
        tech.unlocked.Add(CollectorObj);
        tech.unlocked.Add(WallObj);

        // Check for save data on start, and if there is, set values for everything.
        try
        {
            // Load save data to file
            SaveData data = SaveSystem.LoadGame();

            // Set resource amounts
            gold = data.Gold;
            essence = data.Essence;
            iridium = data.Iridium;
            UI.GoldAmount.text = gold.ToString();
            UI.EssenceAmount.text = essence.ToString();
            UI.IridiumAmount.text = iridium.ToString();

            // Update unlock level and research
            tech.SetUnlock(data.UnlockLevel - 1);
            seed = data.WorldSeed;

            tech.ForceUpdateCheck();
            GameObject.Find("OnSpawn").GetComponent<OnSpawn>().GenerateWorldData(seed);
            PlaceSavedBuildings(data.Locations);

            try
            {
                if (data.UnlockProgress[0] >= 0)
                {
                    tech.SetProgress(data.UnlockProgress);
                }
            }
            catch { Debug.Log("Save file does not contain tracking progress"); }

            UI.PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
        }
        catch
        {
            Debug.Log("No save data was found, or the save data found was corrupt.");
            seed = Random.Range(1000000, 10000000);
            GameObject.Find("OnSpawn").GetComponent<OnSpawn>().GenerateWorldData(seed);
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
        if (Input.GetButton("Fire1") && !UI.BuildingOpen && !UI.ResearchOpen && Input.mousePosition.y >= 200)
        {
            bool ValidTile = true;
            if (SelectedObj != null && (SelectedObj.name == "Rocket Pod" || SelectedObj.name == "Turbine"))
            {
                // Check for wires and adjust accordingly 
                RaycastHit2D a = Physics2D.Raycast(new Vector2(MousePos.x, MousePos.y), Vector2.zero, Mathf.Infinity, TileLayer);
                RaycastHit2D b = Physics2D.Raycast(new Vector2(MousePos.x - 5f, MousePos.y), Vector2.zero, Mathf.Infinity, TileLayer);
                RaycastHit2D c = Physics2D.Raycast(new Vector2(MousePos.x, MousePos.y + 5f), Vector2.zero, Mathf.Infinity, TileLayer);
                RaycastHit2D d = Physics2D.Raycast(new Vector2(MousePos.x - 5f, MousePos.y + 5f), Vector2.zero, Mathf.Infinity, TileLayer);

                if (a.collider != null || b.collider != null || c.collider != null || d.collider != null) ValidTile = false;
            }

            // Raycast tile to see if there is already a tile placed
            RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

            if (ValidTile && rayHit.collider == null && SelectedObj != null && transform.position.x <= 250 && transform.position.x >= -245 && transform.position.y <= 245 && transform.position.y >= -245)
            {
                if (SelectedObj == EssenceObj)
                {
                    RaycastHit2D resourceCheck = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, ResourceLayer);
                    if (resourceCheck.collider == null || resourceCheck.collider.name != "Essencetile") return;
                }
                int cost = SelectedObj.GetComponent<TileClass>().GetCost();
                int power = SelectedObj.GetComponent<TileClass>().getConsumption();
                if (cost <= gold && PowerConsumption + power <= AvailablePower)
                {
                    RemoveGold(cost);
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
                    Spawner.GetComponent<WaveSpawner>().increaseHeat(LastObj.GetComponent<TileClass>().GetHeat());
                }
            }
            else if (rayHit.collider != null)
            {
                if (rayHit.collider.name != "Hub")
                {
                    UI.ShowTileInfo(rayHit.collider);
                    UI.ShowingInfo = true;
                    SelectedOverlay.transform.position = rayHit.collider.transform.position;
                    SelectedOverlay.SetActive(true);
                }
            }
        }

        // If user right clicks, remove object
        else if (Input.GetButton("Fire2") && !UI.BuildingOpen)
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

                if (rayHit.collider.name.Contains("Enhancer"))
                {
                    var colliders = Physics2D.OverlapBoxAll(rayHit.collider.transform.position, new Vector2(7, 7), 1 << LayerMask.NameToLayer("Building"));
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        if (colliders[i].name.Contains("Collector"))
                        {
                            colliders[i].GetComponent<CollectorAI>().deenhanceCollector();
                        }
                    }
                }
                UI.ShowingInfo = false;
                SelectedOverlay.SetActive(false);
                Spawner.GetComponent<WaveSpawner>().decreaseHeat(rayHit.collider.GetComponent<TileClass>().GetHeat());
                decreasePowerConsumption(rayHit.collider.gameObject.GetComponent<TileClass>().getConsumption());
                int cost = rayHit.collider.GetComponent<TileClass>().GetCost();
                AddGold(cost - cost / 5);
                Destroy(rayHit.collider.gameObject);
            }
        }

        if (UI.BuildingOpen)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetHotbarSlot(0, HoveredObj);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetHotbarSlot(1, HoveredObj);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetHotbarSlot(2, HoveredObj);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetHotbarSlot(3, HoveredObj);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SetHotbarSlot(4, HoveredObj);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SetHotbarSlot(5, HoveredObj);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                SetHotbarSlot(6, HoveredObj);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                SetHotbarSlot(7, HoveredObj);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                SetHotbarSlot(8, HoveredObj);
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
        else if (Input.GetKeyDown(KeyCode.R) && UI.BuildingOpen == false && UI.MenuOpen == false && SelectedObj != null)
        {
            rotation = rotation -= 90f;
            if (rotation == -360f)
            {
                rotation = 0;
            }
            Selected.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        }

        if (Input.GetKeyDown(KeyCode.E) && UI.BuildingOpen == false)
        {
            if (UI.ResearchOpen)
            {
                UI.ResearchOpen = false;
                // CLOSE RESEARCH MENU HERE
            }
            UI.BuildingOpen = true;
            UI.SetOverlayStatus("Survival Menu", true);
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) && UI.BuildingOpen == true)
        {
            UI.BuildingOpen = false;
            UI.SetOverlayStatus("Survival Menu", false);
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) && UI.ResearchOpen == true))
        {
            UI.ResearchOpen = false;
            // CLOSE RESEARCH MENU HERE
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && SelectedObj != null)
        {
            UI.Overlay.transform.Find("Selected").GetComponent<CanvasGroup>().alpha = 0;
            Selected.sprite = null;
            SelectedObj = null;
            rotation = 0;
            UI.DisableActiveInfo();
            UI.ShowingInfo = false;
            SelectedOverlay.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && UI.ShowingInfo == true)
        {
            UI.ShowingInfo = false;
            SelectedOverlay.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && UI.MenuOpen == false)
        {
            UI.SaveButton.GetComponent<CanvasGroup>().interactable = true;
            UI.SaveButton.buttonText = "SAVE";
            UI.SaveButton.UpdateUI();

            UI.MenuOpen = true;
            UI.SetOverlayStatus("Paused", true);

            Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI.MenuOpen = false;
            UI.SetOverlayStatus("Paused", false);

            Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        }
    }

    // Set the games playback speed
    public void setGameSpeed(int a)
    {
        Time.timeScale = a;
    }

    // Place building loaded from a save file
    public void PlaceSavedBuildings(int[,] a)
    {
        for (int i = 0; i < a.GetLength(0); i++)
        {
            Transform building = GetBuildingWithID(a[i, 0]);
            Transform obj = Instantiate(building, new Vector3(a[i, 2], a[i, 3], 0), Quaternion.Euler(new Vector3(0, 0, 0)));
            obj.name = building.name;

            increasePowerConsumption(building.GetComponent<TileClass>().getConsumption());
            Spawner.GetComponent<WaveSpawner>().increaseHeat(building.GetComponent<TileClass>().GetHeat());

            Debug.Log("Placed " + obj.name + " at " + a[i, 2] + " " + a[i, 3]);
        }
    }

    // Returns a buildings ID if unlocked
    public Transform GetBuildingWithID(int a)
    {
        for (int i = 0; i < tech.unlocked.Count; i++)
        {
            if (tech.unlocked[i].GetComponent<TileClass>().getID() == a)
            {
                return tech.unlocked[i];
            }
        }
        return null;
    }

    // Returns available power 
    public int getAvailablePower()
    {
        return AvailablePower;
    }

    // Increases available power by x amount
    public void increaseAvailablePower(int a)
    {
        AvailablePower += a;
        UI.PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
    }

    // Decreases available power by x amount
    public void decreaseAvailablePower(int a)
    {
        AvailablePower -= a;
        UI.PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
    }

    // Returns power consumption
    public int getPowerConsumption()
    {
        return PowerConsumption;
    }

    // Increase power consumption by x amount
    public void increasePowerConsumption(int a)
    {
        PowerConsumption += a;
        UI.PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
    }

    // Decrease power consumption by x amount
    public void decreasePowerConsumption(int a)
    {
        PowerConsumption -= a;
        UI.PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
    }

    // Add x gold to player
    public void AddGold(int a)
    {
        gold += a;
        UI.GoldAmount.text = gold.ToString();
    }

    // Remove x gold from player
    public void RemoveGold(int a)
    {
        gold -= a;
        UI.GoldAmount.text = gold.ToString();
    }

    // Add x essence to player
    public void AddEssence(int a)
    {
        essence += a;
        UI.EssenceAmount.text = essence.ToString();
    }

    // Remove x essence from player
    public void RemoveEssence(int a)
    {
        essence -= a;
        UI.EssenceAmount.text = essence.ToString();
    }

    // Add x iridium to player
    public void AddIridium(int a)
    {
        iridium += a;
        UI.IridiumAmount.text = iridium.ToString();
    }

    // Remove x essence from player
    public void RemoveIridium(int a)
    {
        iridium -= a;
        UI.IridiumAmount.text = iridium.ToString();
    }

    // Adjust the selected overlay transparency by 0.1f
    public void AdjustAlphaValue()
    {
        if (AdjustLimiter == 5)
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

    // Set default hotbar variables
    private void PopulateHotbar()
    {
        hotbar[0] = TurretObj;
        hotbar[1] = WallObj;
        hotbar[2] = CollectorObj;
        UI.UpdateHotbar();
    }

    // Select object on hotbar
    public void SelectHotbar(int index)
    {
        try
        {
            SelectedObj = hotbar[index];
            SwitchObj();
            if (SelectedObj.name == "Rocket Pod" || SelectedObj.name == "Turbine")
            {
                largerUnit = true;
                transform.localScale = new Vector3(2, 2, 1);
            }
        }
        catch { return; }
        UI.SetSelectedHotbar(index);
    }

    // Changes the object that the player has selected (pass null to deselect)
    public void SelectObject(Transform obj)
    {
        SelectedObj = obj;
        if (obj != null && !tech.checkIfUnlocked(obj)) return;
        SwitchObj();
        if (SelectedObj.name == "Rocket Pod" || SelectedObj.name == "Turbine")
        {
            largerUnit = true;
            transform.localScale = new Vector3(2, 2, 1);
        }
    }

    // Changes the stored object for hotbar changing
    public void SetHoverObject(Transform obj)
    {
        if (!tech.checkIfUnlocked(obj)) return;
        HoveredObj = obj;
    }

    // Changes the object stored in a hotbar slot
    public void SetHotbarSlot(int slot, Transform obj)
    {
        if (!tech.checkIfUnlocked(obj)) return;
        if (slot < 0 || slot > hotbar.Length) return;
        hotbar[slot] = obj;
        UI.UpdateHotbar();
    }

    // Switch the selected object
    public void SwitchObj()
    {
        if (largerUnit)
        {
            largerUnit = false;
            transform.localScale = new Vector3(1, 1, 1);
        }
        UI.DisableActiveInfo();
        Adjustment = 1f;
        Selected.sprite = Resources.Load<Sprite>("Sprites/" + SelectedObj.name);
        UI.ShowSelectedInfo(SelectedObj);
    }

    // Loads the menu scene
    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    // Sends all data to the save script
    public void Save()
    {
        Debug.Log("Attempting to save data");
        SaveSystem.SaveGame(this, tech, Spawner.GetComponent<WaveSpawner>());
        Debug.Log("Data was saved successfully");

        UI.SaveButton.buttonText = "SAVED";
        UI.SaveButton.GetComponent<CanvasGroup>().interactable = false;
        UI.SaveButton.UpdateUI();
    }

    // Returns all building locations when saving
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
        for (int i = 0; i < allObjects.Length; i++)
        {
            if (allObjects[i].tag == "Defense")
            {
                try
                {
                    Debug.Log(allObjects[i].name);
                    data[length, 0] = allObjects[i].GetComponent<TileClass>().getID();
                    data[length, 1] = allObjects[i].GetComponent<TileClass>().GetLevel();
                    data[length, 2] = (int)allObjects[i].position.x;
                    data[length, 3] = (int)allObjects[i].position.y;
                    length += 1;
                }
                catch
                {
                    Debug.Log("Error saving " + allObjects[i].name);
                }
            }
        }

        return data;
    }

    public Transform GetEssenceObj()
    {
        return EssenceObj;
    }
}