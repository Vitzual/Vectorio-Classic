//////////////////////// IMPORTANT ///////////////////////////
// This script is an absolute mess and we've given up     
// all hope at actually making it even somewhat readable.   
// If you're having troubles finding something contact one  
// of us on Discord. All other scripts are organized and    
// layed out in a much more organized fashion. This was the 
// first script we wrote and at the time knew NOTHING about 
// Unity or C# so it became a flaming pile of garbage.

// Import required libraries
using UnityEngine;
using UnityEngine.SceneManagement;

public class Survival : MonoBehaviour
{
    // Technology script
    public Technology tech;

    // Interface script
    public Interface UI;

    // Research object
    public Research rsrch;

    // Enemy layer
    public LayerMask EnemyLayer;

    // Resource stats
    public int gold = 0;
    public int essence = 0;
    public int iridium = 0;
    public int PowerConsumption = 0;
    public int AvailablePower = 1000;

    // Resource per second variables
    public int GPS = 0;
    public int EPS = 0;
    public int IPS = 0;
    public int GPSP = 0;
    public int EPSP = 0;
    public int IPSP = 0;

    // Sprite of the selected object
    private SpriteRenderer Selected;

    // Flashing effect variables
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
    [SerializeField] private Transform TeslaObj;         // ID = 12
    

    // Enemy list
    public Transform[] enemies;

    // The seed the word is set to
    public int seed;

    // The game object used to spawn enemies
    public GameObject Spawner;

    // The square that appears around buildings when clicked
    public GameObject SelectedOverlay;

    // The stats of the building clicked 
    public CanvasGroup PromptOverlay;

    // The radius of the selected building
    public GameObject SelectedRadius;

    // The building you currently have selected
    private Transform SelectedObj;

    // The hotbar building you last hovered over
    public Transform HoveredObj;

    // Stores information about previously selected object
    private Transform LastObj;

    // The rotation of the selected object
    private float rotation = 0f;

    // If the object selected is 2x2 instead of 1x1
    public bool largerUnit = false;

    // Holds the AOC level
    public int AOC_Level = 1;

    // The area of control size
    private int AOC_Size = 150;

    // The AOC border game object;
    public Transform AOC_Object;

    // Internal placement variables
    [SerializeField] private LayerMask ResourceLayer;
    [SerializeField] private LayerMask TileLayer;
    [SerializeField] private LayerMask UILayer;
    private Vector2 MousePos;
    protected float distance = 10;
    public Transform[] hotbar = new Transform[9];

    private void Start()
    {
        // Assign default variables
        Selected = GetComponent<SpriteRenderer>();

        // Default starting unlocks / hotbar
        PopulateHotbar();
        tech.unlocked.Add(TurretObj);
        tech.unlocked.Add(CollectorObj);
        tech.unlocked.Add(WallObj);

        // Load save data to file
        SaveData data = SaveSystem.LoadGame();

        // Attempt to load save data 
        try
        {
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

            // Force tech tree update
            tech.ForceUpdateCheck();
            GameObject.Find("OnSpawn").GetComponent<OnSpawn>().GenerateWorldData(seed, true);
            PlaceSavedBuildings(data.Locations);

            // Set power usage
            UI.PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
            UI.AvailablePower.text = AvailablePower.ToString() + " MAX";

            // Get research save data
            rsrch.SetResearchData(data.ResearchedTiers);

            // Place saved enemies 
            try
            {
                PlaceSavedEnemies(data.Enemies);
            }
            catch
            {
                Debug.Log("Save file doesn't contain enemy tracking");
            }
        }
        catch
        {
            Debug.Log("No save data was found, or the save data found was corrupt.");
            seed = Random.Range(1000000, 10000000);
            GameObject.Find("OnSpawn").GetComponent<OnSpawn>().GenerateWorldData(seed, false);
        }

        // Start repeating PS function
        InvokeRepeating("UpdatePerSecond", 0f, 1f);
    }

    // Gets called once every frame
    private void Update()
    {
        // Get mouse position and round to middle grid coordinate
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check if object selected
        if (SelectedObj != null)
        {
            // Check unit size and make flash
            CheckSize();
            AdjustAlphaValue();
        }

        // Check if user left clicks
        if (Input.GetButton("Fire1") && !UI.BuildingOpen && !UI.ResearchOpen && Input.mousePosition.y >= 200)
        {

            // Check if valid placement
            bool ValidTile = CheckPlacement(SelectedObj);

            // Raycast tile to see if there is an enemy occupying the tile
            RaycastHit2D enemyHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, EnemyLayer);

            // Raycast tile to see if there is already a tile placed
            RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

            // Check if placement is within AOC
            if (ValidTile && enemyHit.collider == null && rayHit.collider == null && SelectedObj != null && transform.position.x <= AOC_Size && transform.position.x >= -AOC_Size && transform.position.y <= AOC_Size && transform.position.y >= -AOC_Size)
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
                    if (SelectedObj.name == "Wall")
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
                if (rayHit.collider.name != "Hub" && SelectedObj == null)
                {
                    UI.ShowTileInfo(rayHit.collider);
                    UI.ShowingInfo = true;
                    SelectedOverlay.SetActive(true);
                    SelectedOverlay.transform.position = rayHit.collider.transform.position;
                    PromptOverlay.alpha = 1;
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
                if (rayHit.collider.name.Contains("Wall"))
                {
                    UpdateWallRemoved();
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

                if (rayHit.collider.name == "Turbine")
                {
                    decreaseAvailablePower(100);
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

        // Check if hotbar selected
        CheckNumberInput();

        // Rotates object if no menus open
        if (Input.GetKeyDown(KeyCode.R) && UI.BuildingOpen == false && UI.MenuOpen == false && SelectedObj != null)
        {
            rotation = rotation -= 90f;
            if (rotation == -360f)
            {
                rotation = 0;
            }
            Selected.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        }

        // Opens the building menu if E is pressed.
        if (Input.GetKeyDown(KeyCode.E) && UI.BuildingOpen == false)
        {
            if (UI.ResearchOpen)
            {
                UI.CloseResearchOverlay();
                SetHoverObject(null);
            }
            UI.BuildingOpen = true;
            UI.SetOverlayStatus("Survival Menu", true);
        }

        // If T pressed, open research menu
        else if (Input.GetKeyDown(KeyCode.T) && UI.MenuOpen == false && UI.ResearchOpen == false)
        {
            UI.OpenResearchOverlay();
        }

        // If T pressed and research menu open, close it
        else if (Input.GetKeyDown(KeyCode.T) && UI.MenuOpen == false && UI.ResearchOpen == true)
        {
            UI.CloseResearchOverlay();
        }

        // If escape pressed and building menu open, close it
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) && UI.BuildingOpen == true)
        {
            UI.BuildingOpen = false;
            UI.SetOverlayStatus("Survival Menu", false);
        }

        // If escape pressed and research open, close it.
        else if ((Input.GetKeyDown(KeyCode.Escape) && UI.ResearchOpen == true))
        {
            UI.CloseResearchOverlay();
        }

        // If escape pressed and selected object isn't null, unselect it
        else if (Input.GetKeyDown(KeyCode.Escape) && SelectedObj != null)
        {
            UI.Overlay.transform.Find("Selected").GetComponent<CanvasGroup>().alpha = 0;
            Selected.sprite = null;
            SelectedObj = null;
            rotation = 0;
            UI.DisableActiveInfo();
            UI.ShowingInfo = false;
            SelectedRadius.SetActive(false);
        }

        // If a placed building is selected, un select it
        else if (Input.GetKeyDown(KeyCode.Escape) && UI.ShowingInfo)
        {
            UI.ShowingInfo = false;
            PromptOverlay.alpha = 0;
            SelectedOverlay.SetActive(false);
        }

        // If escape pressed and menu not open, open it
        else if (Input.GetKeyDown(KeyCode.Escape) && UI.MenuOpen == false && UI.SettingsOpen == false)
        {
            UI.SaveButton.GetComponent<CanvasGroup>().interactable = true;
            UI.SaveButton.buttonText = "SAVE";
            UI.SaveButton.UpdateUI();

            UI.MenuOpen = true;
            UI.SetOverlayStatus("Paused", true);

            Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        }

        // If escape pressed and settings open, close it and open menu
        else if (Input.GetKeyDown(KeyCode.Escape) && UI.SettingsOpen == true)
        {
            UI.SettingsOpen = false;
            UI.SetOverlayStatus("Settings", false);
            UI.SetOverlayStatus("Paused", true);
        }

        // If escape pressed and menu open, close it
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI.MenuOpen = false;
            UI.SetOverlayStatus("Paused", false);

            Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        }

        else if (Input.GetKeyDown(KeyCode.G)) IncreaseAOC();
    }

    // Checks unit size
    private void CheckSize()
    {
        if (!largerUnit) transform.position = new Vector2(5 * Mathf.Round(MousePos.x / 5), 5 * Mathf.Round(MousePos.y / 5));
        else transform.position = new Vector2(5 * Mathf.Round(MousePos.x / 5) - 2.5f, 5 * Mathf.Round(MousePos.y / 5) + 2.5f);
    }

    // Update per second variables
    public void UpdatePerSecond()
    {
        GPS = gold - GPSP;
        EPS = essence - EPSP;
        IPS = iridium - IPSP;
        UI.GPS.text = GPS.ToString() + " / SEC";
        UI.EPS.text = EPS.ToString() + " / SEC";
        UI.IPS.text = IPS.ToString() + " / SEC";
        GPSP = gold;
        EPSP = essence;
        IPSP = iridium;
    }

    // Checks if for numeric input
    public void CheckNumberInput()
    {
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
    }

    // Checks if a unit can be placed
    public bool CheckPlacement(Transform obj)
    {
        if (obj != null && (obj.name == "Rocket Pod" || obj.name == "Turbine"))
        {
            // Check for wires and adjust accordingly 
            RaycastHit2D a = Physics2D.Raycast(new Vector2(MousePos.x, MousePos.y), Vector2.zero, Mathf.Infinity, TileLayer);
            RaycastHit2D b = Physics2D.Raycast(new Vector2(MousePos.x - 5f, MousePos.y), Vector2.zero, Mathf.Infinity, TileLayer);
            RaycastHit2D c = Physics2D.Raycast(new Vector2(MousePos.x, MousePos.y + 5f), Vector2.zero, Mathf.Infinity, TileLayer);
            RaycastHit2D d = Physics2D.Raycast(new Vector2(MousePos.x - 5f, MousePos.y + 5f), Vector2.zero, Mathf.Infinity, TileLayer);

            if (a.collider != null || b.collider != null || c.collider != null || d.collider != null) return false;
            else return true;
        }
        return true;
    }

    // If object removed is a wall, update surrounding walls
    public void UpdateWallRemoved()
    {
        RaycastHit2D a = Physics2D.Raycast(new Vector2(transform.position.x + 5f, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D b = Physics2D.Raycast(new Vector2(transform.position.x - 5f, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D c = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 5f), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D d = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 5f), Vector2.zero, Mathf.Infinity, TileLayer);
        if (a.collider != null && a.collider.name.Contains("Wall"))
        {
            if (a.collider.name == "Heavy Wall")
                a.collider.transform.GetChild(0).GetComponent<WallAI>().UpdateSprite(-1);
            a.collider.GetComponent<WallAI>().UpdateSprite(-1);
        }
        if (b.collider != null && b.collider.name.Contains("Wall"))
        {
            if (b.collider.name == "Heavy Wall")
                b.collider.transform.GetChild(0).GetComponent<WallAI>().UpdateSprite(-3);
            b.collider.GetComponent<WallAI>().UpdateSprite(-3);
        }
        if (c.collider != null && c.collider.name.Contains("Wall"))
        {
            if (c.collider.name == "Heavy Wall")
                c.collider.transform.GetChild(0).GetComponent<WallAI>().UpdateSprite(-2);
            c.collider.GetComponent<WallAI>().UpdateSprite(-2);
        }
        if (d.collider != null && d.collider.name.Contains("Wall"))
        {
            if (d.collider.name == "Heavy Wall")
                d.collider.transform.GetChild(0).GetComponent<WallAI>().UpdateSprite(-4);
            d.collider.GetComponent<WallAI>().UpdateSprite(-4);
        }
    }

    // Increase the AOC size
    public void IncreaseAOC()
    {
        AOC_Level += 1;
        AOC_Size += 60;
        AOC_Object.localScale = new Vector2(AOC_Object.localScale.x + .394f, AOC_Object.localScale.y + .394f);
    }
    
    // Returns the AOC size
    public int GetAOC()
    {
        return AOC_Size;
    }

    // Set the games playback speed
    public void SetGameSpeed(int a)
    {
        Time.timeScale = a;
    }

    // Place building loaded from a save file
    public void PlaceSavedBuildings(int[,] a)
    {
        for (int i = 0; i < a.GetLength(0); i++)
        {
            Transform building = GetBuildingWithID(a[i, 0]);

            float x = a[i, 2];
            float y = a[i, 3];
            Vector2 position;

            if (building.name == "Rocket Pod" || building.name == "Turbine")
            {
                // Check the x coordinate
                if (x >= 0)
                    x += 0.5f;
                else
                    x -= 0.5f;

                // Check the y coordinate
                if (y >= 0)
                    y += 0.5f;
                else
                    y -= 0.5f;

                position = new Vector2(x, y);
            }
            else
                position = new Vector2(x, y);

            Transform obj = Instantiate(building, position, Quaternion.Euler(new Vector3(0, 0, 0)));

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
        UI.AvailablePower.text = AvailablePower.ToString() + " MAX";
    }

    // Decreases available power by x amount
    public void decreaseAvailablePower(int a)
    {
        AvailablePower -= a;
        UI.PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
        UI.AvailablePower.text = AvailablePower.ToString() + " MAX";
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
        UI.PowerUsage.text = PowerConsumption.ToString();
    }

    // Decrease power consumption by x amount
    public void decreasePowerConsumption(int a)
    {
        PowerConsumption -= a;
        UI.PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
        UI.PowerUsage.text = PowerConsumption.ToString();
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
        Color tmp = this.GetComponent<SpriteRenderer>().color;
        tmp.a = Adjustment;
        this.GetComponent<SpriteRenderer>().color = tmp;

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
            // Disable selected overlay
            UI.ShowingInfo = false;
            PromptOverlay.alpha = 0;
            SelectedOverlay.SetActive(false);

            // Set hotbar transform
            SelectedObj = hotbar[index];
            SwitchObj();
        }
        catch { return; }
        UI.SetSelectedHotbar(index);
    }

    // Changes the object that the player has selected (pass null to deselect)
    public void SelectObject(Transform obj)
    {
        // Disable selected overlay
        UI.ShowingInfo = false;
        PromptOverlay.alpha = 0;
        SelectedOverlay.SetActive(false);

        SelectedObj = obj;
        if (obj != null && !tech.checkIfUnlocked(obj)) return;
        SwitchObj();
    }

    // Changes the stored object for hotbar changing
    public void SetHoverObject(Transform obj)
    {
        if (!tech.checkIfUnlocked(obj)) return;
        HoveredObj = obj;
    }

    // THIS IS THE CULPRIT 
    //
    // Changes the object stored in a hotbar slot
    public void SetHotbarSlot(int slot, Transform obj)
    {
        Debug.Log("3 > " + obj.name);

        if (!tech.checkIfUnlocked(obj)) return;
        if (slot < 0 || slot > hotbar.Length) return;
        hotbar[slot] = obj;
        UI.UpdateHotbar();
    }

    // Switch the selected object
    public void SwitchObj()
    {
        // If unit is larger then 1x1, change selected obj accordingly
        if (SelectedObj.name == "Rocket Pod" || SelectedObj.name == "Turbine")
        {
            largerUnit = true;
            if (SelectedObj.name == "Rocket Pod") 
                transform.localScale = new Vector2(2, 2);
        }
        else
        {
            largerUnit = false;
            transform.localScale = new Vector2(1, 1);
        }

        // Disable any active info not relative to selected object
        UI.DisableActiveInfo();
        Adjustment = 1f;
        Selected.sprite = Resources.Load<Sprite>("Sprites/" + SelectedObj.name);
        UI.ShowSelectedInfo(SelectedObj);

        // Set radius dimensions if selected object is defense
        if (SelectedObj.tag == "Defense" && SelectedObj.name != "Wall")
        {
            float range = SelectedObj.GetComponent<TurretClass>().range * 2 + Research.bonus_range;
            SelectedRadius.transform.localScale = new Vector3(range, 1, range);
            SelectedRadius.SetActive(true);
        }
        else
        {
            SelectedRadius.SetActive(false);
        }
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
        SaveSystem.SaveGame(this, tech, Spawner.GetComponent<WaveSpawner>(), rsrch);
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
            if (allObjects[i].tag == "Defense" || allObjects[i].tag == "Production") length += 1;
        }

        int[,] data = new int[length, 4];
        length = 0;
        for (int i = 0; i < allObjects.Length; i++)
        {
            if (allObjects[i].tag == "Defense" || allObjects[i].tag == "Production")
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

    // Returns all building locations when saving
    public float[,] GetEnemyData()
    {
        Transform[] allObjects = FindObjectsOfType<Transform>();

        int length = 0;
        for (int i = 0; i < allObjects.Length; i++)
        {
            if (allObjects[i].tag == "Enemy") length += 1;
        }

        float[,] data = new float[length, 4];
        length = 0;
        for (int i = 0; i < allObjects.Length; i++)
        {
            if (allObjects[i].tag == "Enemy")
            {
                try
                {
                    Debug.Log(allObjects[i].name);
                    data[length, 0] = allObjects[i].GetComponent<EnemyClass>().GetID();
                    data[length, 1] = allObjects[i].GetComponent<EnemyClass>().GetHealth();
                    data[length, 2] = allObjects[i].position.x;
                    data[length, 3] = allObjects[i].position.y;
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

    // Place building loaded from a save file
    public void PlaceSavedEnemies(float[,] a)
    {
        for (int i = 0; i < a.GetLength(0); i++)
        {
            Transform enemy = GetEnemyWithID((int)a[i, 0]);

            float x = a[i, 2];
            float y = a[i, 3];
            Vector2 position = new Vector2(x, y);

            Transform obj = Instantiate(enemy, position, Quaternion.Euler(new Vector3(0, 0, 0)));

            obj.name = enemy.name;
            enemy.gameObject.GetComponent<EnemyClass>().SetHealth((int)a[i, 1]);

            Debug.Log("Placed " + obj.name + " at " + a[i, 2] + " " + a[i, 3]);
        }
    }

    // Returns a buildings ID if unlocked
    public Transform GetEnemyWithID(int a)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<EnemyClass>().GetID() == a)
            {
                return enemies[i];
            }
        }
        return null;
    }

    public Transform GetEssenceObj()
    {
        return EssenceObj;
    }
}