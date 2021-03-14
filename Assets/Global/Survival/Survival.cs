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
    // Manager
    public GameObject manager;
    
    // Technology script
    public Technology tech;

    // Interface script
    public Interface UI;

    // Difficulties script
    private Difficulties difficulties;

    // Research object
    public Research rsrch;

    // Gold script
    public GoldAI GoldScript;

    // Music audio source
    public AudioSource music;

    // Tutorial Elements (Survival Exclusive)
    public GameObject TutorialOverlay;
    public GameObject[] TutorialSlides;
    public bool showingTutorial;
    public int tutorialNumber = 0;

    // layers
    public LayerMask EnemyLayer;
    public LayerMask AOCBLayer;

    // Area of control border
    public GameObject AOCB;

    // Camera thing
    public Camera MainCamera;
    Color CameraColor;

    // Resource stats
    public int gold = 0;
    public int essence = 0;
    public int iridium = 0;
    public int goldStorage = 1000;
    public int essenceStorage = 1000;
    public int iridiumStorage = 1000;
    public int PowerConsumption = 0;
    public int AvailablePower = 1000;

    // Add additional cost
    public float additionalCost = 1f;

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
    [SerializeField] private Transform PowerObj;         // ID = 13
    [SerializeField] private Transform FlamethrowerObj;  // ID = 14
    [SerializeField] private Transform MinigunObj;       // ID = 15
    [SerializeField] private Transform EngineerObj;      // ID = 16
    [SerializeField] private Transform TurretMK2;        // ID = 17
    [SerializeField] private Transform ShotgunMK2;       // ID = 18
    [SerializeField] private Transform SniperMK2;        // ID = 19
    [SerializeField] private Transform SunbeamObj;       // ID = 20
    [SerializeField] private Transform TurretMK3;        // ID = 21
    [SerializeField] private Transform AreaCoolerObj;    // ID = 22
    [SerializeField] private Transform Iridium;          // ID = 23
    [SerializeField] private Transform Solar;            // ID = 24

    [SerializeField] private Transform EnemyTurretDual;  // ID = 201
    [SerializeField] private Transform EnemyTurretSMG;   // ID = 200
    [SerializeField] private Transform EnemyTurretRanger;// ID = 202
    [SerializeField] private Transform EnemyStaticWall;  // ID = 205
    [SerializeField] private Transform EnemyStaticMine;  // ID = 204

    // Holds the most recent engineer
    public Transform EngineerHolder;
    public int EngineerModID = 0;
    public string EngineerName = "Default";

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
    private int AOC_Size = 750;

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
        CameraColor = new Color();

        // Set color
        ColorUtility.TryParseHtmlString("#333333", out CameraColor);
        MainCamera.backgroundColor = CameraColor;

        // Default starting unlocks / hotbar
        PopulateHotbar();
        tech.unlocked.Add(TurretObj);
        tech.unlocked.Add(CollectorObj);
        tech.unlocked.Add(WallObj);

        // Load save data to file
        SaveData data = SaveSystem.LoadGame();

        // Get difficulty object
        difficulties = GameObject.Find("Difficulty").GetComponent<Difficulties>();
        int holder = difficulties.GetDifficulty();

        // Check to see if there's a difficulty saved
        try
        {
            difficulties.SetDifficulty(0);
            difficulties.SetStartingGold(data.startingGold);
            difficulties.SetStartingPower(data.startingPower);
            difficulties.SetAdditionalCost(data.additionalCost);
            difficulties.SetEnemyHP(data.enemyHP);
            difficulties.SetEnemyDMG(data.enemyDMG);
            difficulties.SetDefenseHP(data.defenseHP);
            difficulties.SetGold(data.goldSpawn);
            difficulties.SetEssence(data.essenceSpawn);
            difficulties.SetIridium(data.iridiumSpawn);
            difficulties.SetBases(data.enemyBases);
        }
        catch
        {
            difficulties.SetDifficulty(holder);
        }

        // Get default stats
        gold = difficulties.GetStartingGold();
        goldStorage = gold * 4;
        AvailablePower = difficulties.GetStartingPower();
        additionalCost = difficulties.GetAdditionalCost();
        UI.GoldAmount.text = gold.ToString();

        // Set power usage
        UI.PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
        UI.AvailablePower.text = AvailablePower.ToString() + " MAX";

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

            // Attempt to place saved buildings
            try
            {
                PlaceSavedBuildings(data.Locations);
            }
            catch
            {
                Debug.Log("Save file contains obsolete data!");
            }

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
                Debug.Log("Save file contains obsolete data!");
            }
        }
        catch
        {
            // New save
            Debug.Log("No save data was found, or the save data found was corrupt.");
            seed = Random.Range(1000000, 10000000);
            GameObject.Find("OnSpawn").GetComponent<OnSpawn>().GenerateWorldData(seed, false);

            // Show tutorial UI
            TutorialOverlay.SetActive(true);
            showingTutorial = true;
            music.Stop();
            Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        }

        // Load settings
        manager.GetComponent<Settings>().LoadSettings();

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
            if (AOCB.activeSelf) AOCB.transform.position = new Vector3(transform.position.x, transform.position.y, 5);
        }

        // Check if user left clicks
        if (Input.GetButton("Fire1") && !UI.BuildingOpen && !UI.ResearchOpen && !UI.EngineerOpen && Input.mousePosition.y >= 200)
        {

            // Check if valid placement
            bool ValidTile = CheckPlacement(SelectedObj);

            // Raycast tile to see if there is an enemy occupying the tile
            RaycastHit2D enemyHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, EnemyLayer);

            // Raycast tile to see if there is already a tile placed
            RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

            // Raycast tile to see if it is within the AOCB
            RaycastHit2D aocbHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, AOCBLayer);

            // Check the AOCB
            if (SelectedObj != null && SelectedObj.name == "Energizer")
            {
                var colliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y), new Vector2(60, 60), 0, 1 << LayerMask.NameToLayer("AOCB"));
                if (colliders.Length == 0) return;
            }
            else if (SelectedObj != null)
                if (aocbHit.collider == null) return;

            // Check if placement is within AOC
            if (ValidTile && enemyHit.collider == null && rayHit.collider == null && SelectedObj != null && transform.position.x <= AOC_Size && transform.position.x >= -AOC_Size+5 && transform.position.y <= AOC_Size && transform.position.y >= -AOC_Size+5)
            {
                if (SelectedObj == CollectorObj)
                {
                    RaycastHit2D resourceCheck = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, ResourceLayer);
                    if (resourceCheck.collider == null || resourceCheck.collider.name != "Goldtile") return;
                }
                else if (SelectedObj == EssenceObj)
                {
                    RaycastHit2D resourceCheck = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, ResourceLayer);
                    if (resourceCheck.collider == null || resourceCheck.collider.name != "Essencetile") return;
                }
                else if (SelectedObj == Iridium)
                {
                    RaycastHit2D resourceCheck = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, ResourceLayer);
                    if (resourceCheck.collider == null || resourceCheck.collider.name != "Iridiumtile") return;
                }
                int cost = SelectedObj.GetComponent<TileClass>().GetCost();
                int power = SelectedObj.GetComponent<TileClass>().getConsumption();
                if (cost <= gold && PowerConsumption + power <= AvailablePower)
                {
                    RemoveGold(cost);

                    // If it is a wall, dont use rotation
                    if (SelectedObj.name == "Wall")
                        LastObj = Instantiate(SelectedObj, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                    else LastObj = Instantiate(SelectedObj, transform.position, Quaternion.Euler(new Vector3(0, 0, rotation)));

                    LastObj.name = SelectedObj.name;
                    LastObj.GetComponent<TileClass>().IncreaseHealth();
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

                    // Check to see if the object is an engineer
                    if (rayHit.collider.name == "Engineer")
                    {
                        EngineerHolder = rayHit.collider.transform;
                        rayHit.collider.GetComponent<Engineer>().CheckAdjacentTiles();
                        UI.OpenEngineer(EngineerHolder.GetComponent<Engineer>().applyingModifications);
                    }
                }
            }
        }

        // If user right clicks, remove object
        else if (Input.GetButton("Fire2") && !UI.BuildingOpen && !UI.EngineerOpen)
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

                else if (rayHit.collider.name.Contains("Energizer"))
                {
                    rayHit.collider.GetComponent<Distributor>().DestroyTile();
                    return;
                }

                else if (rayHit.collider.name.Contains("Enhancer"))
                {
                    var colliders = Physics2D.OverlapBoxAll(rayHit.collider.transform.position, new Vector2(7, 7), 1 << LayerMask.NameToLayer("Building"));
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        if (colliders[i].name == "Collector")
                        {
                            colliders[i].GetComponent<CollectorAI>().enhanceCollector();
                        }
                        else if (colliders[i].name == "Essence Drill")
                        {
                            colliders[i].GetComponent<EssenceAI>().enhanceCollector();
                        }
                        else if (colliders[i].name == "Iridium Mine")
                        {
                            colliders[i].GetComponent<IridiumAI>().enhanceCollector();
                        }
                    }
                }

                else if (rayHit.collider.name.Contains("Area Cooler"))
                {
                    var colliders = Physics2D.OverlapBoxAll(rayHit.collider.transform.position, new Vector2(7, 7), 1 << LayerMask.NameToLayer("Building"));
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        if (colliders[i].name != "Hub" && colliders[i].name != "Area Cooler" && colliders[i].name != "AOCB")
                        {
                            Debug.Log("Trying to decool: " + colliders[i].name);
                            try
                            {
                                colliders[i].GetComponent<TileClass>().IncreaseHeat();
                            }
                            catch
                            {
                                Debug.Log("Cant decool: "+colliders[i].name);
                            }
                        }
                    }
                }

                else if (rayHit.collider.name == "Turbine")
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

        // Iterates through the tutorial sequence if enabled
        if (showingTutorial && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space)))
        {
            if (tutorialNumber == 0 && Input.GetKeyDown(KeyCode.Escape))
            {
                TutorialOverlay.SetActive(false);
                showingTutorial = false;
                Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
                music.Play();
            }
            else if (tutorialNumber == 5 && Input.GetKeyDown(KeyCode.Escape))
            {
                TutorialSlides[tutorialNumber].SetActive(false);
                tutorialNumber = 0;
                TutorialSlides[tutorialNumber].SetActive(true);
            }
            else if (tutorialNumber == 5)
            {
                TutorialSlides[tutorialNumber].GetComponent<AudioSource>().Play();
                TutorialOverlay.SetActive(false);
                showingTutorial = false;
                Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
                music.Play();
            }
            else
            {
                // Move slides
                TutorialSlides[tutorialNumber].SetActive(false);
                TutorialSlides[tutorialNumber + 1].SetActive(true);
                TutorialSlides[tutorialNumber + 1].GetComponent<AudioSource>().Play();
                tutorialNumber++;
            }
            return;
        }
        else if (showingTutorial) return;

        // Check hotbar thing        
        CheckNumberInput();

        // Rotates object if no menus open
        if (Input.GetKeyDown(KeyCode.R) && !UI.BuildingOpen && !UI.MenuOpen && !UI.EngineerOpen && SelectedObj != null)
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
            if (UI.EngineerOpen)
                UI.CloseEngineer();
            UI.BuildingOpen = true;
            UI.SetOverlayStatus("Survival Menu", true);
        }

        // If T pressed, open research menu
        else if (Input.GetKeyDown(KeyCode.T) && UI.MenuOpen == false && UI.ResearchOpen == false)
        {
            if (UI.EngineerOpen)
                UI.CloseEngineer();
            UI.OpenResearchOverlay();
        }

        // If T pressed and research menu open, close it
        else if (Input.GetKeyDown(KeyCode.T) && UI.MenuOpen == false && UI.ResearchOpen == true)
        {
            UI.CloseResearchOverlay();
        }

        // If escape pressed and engineer open, close it
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) && UI.EngineerOpen == true)
        {
            UI.CloseEngineer();
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

            // Set color
            // ColorUtility.TryParseHtmlString("#444444", out CameraColor);
            MainCamera.backgroundColor = CameraColor;
            AOCB.SetActive(false);
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

            // Save user settings
            manager.GetComponent<Settings>().SaveSettings();
        }

        // If escape pressed and menu open, close it
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI.MenuOpen = false;
            UI.SetOverlayStatus("Paused", false);

            Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        }
    }

    // Let's put Engineer holder here as a "temporary" solution haha TEMPORARY that's a good one there bud
    public void StartEngineer() { if (EngineerHolder != null) StartCoroutine(EngineerHolder.GetComponent<Engineer>().StartEngineer(EngineerName, EngineerModID)); }
    public void SetEngineerName(string a) { EngineerName = a; }
    public void SetEngineerID(int a) { EngineerModID = a; }
    public void SetEngineerButton(Sprite Icon)
    {
        foreach(Transform building in EngineerHolder.GetComponent<Engineer>().availableBuildings)
        {
            if (building.name == EngineerName)
            {
                TileClass selected = building.GetComponent<TileClass>();
                UI.EngineerIcon.sprite = Icon;
                UI.EngineerTitle.text = selected.EngineerModifications[EngineerModID].title.ToUpper();
                UI.EngineerDescription.text = selected.EngineerModifications[EngineerModID].description;
                UI.EngineerTime.text = selected.EngineerModifications[EngineerModID].upgradeTime + " seconds";
                UI.EngineerChance.text = selected.EngineerModifications[EngineerModID].successRate + "% success rate";
                UI.EngineerCost.text = selected.EngineerModifications[EngineerModID].iridiumCost.ToString();
                return;
            }
        }
        UI.EngineerIcon.sprite = Resources.Load<Sprite>("Undiscovered");
        UI.EngineerTitle.text = "???";
        UI.EngineerDescription.text = "???";
        UI.EngineerTime.text = "???";
        UI.EngineerChance.text = "???";
        UI.EngineerCost.text = "???";
    }

    // Checks unit size
    private void CheckSize()
    {
        if (!largerUnit) transform.position = new Vector2(5 * Mathf.Round(MousePos.x / 5), 5 * Mathf.Round(MousePos.y / 5));
        else transform.position = new Vector2(5 * Mathf.Round(MousePos.x / 5) - 2.5f, 5 * Mathf.Round(MousePos.y / 5) + 2.5f);
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
        if (obj != null && (obj.name == "Rocket Pod" || obj.name == "Turbine" || obj.name == "Sunbeam"))
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
        //AOC_Size += 60;
        //AOC_Object.localScale = new Vector2(AOC_Object.localScale.x + .394f, AOC_Object.localScale.y + .394f);
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

            if (building.name == "Rocket Pod" || building.name == "Turbine" || building.name == "Sunbeam")
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
            obj.GetComponent<TileClass>().SetHealth(a[i, 1]);
            obj.name = building.name;

            increasePowerConsumption(building.GetComponent<TileClass>().getConsumption());
            Spawner.GetComponent<WaveSpawner>().increaseHeat(building.GetComponent<TileClass>().GetHeat());

            // Set engineering
            try
            {
                if (a[i, 4] != -1)
                {
                    try
                    {
                        obj.GetComponent<TileClass>().ApplyModification(a[i, 4]);
                        obj.GetComponent<TileClass>().isEngineered = true;
                    }
                    catch
                    {
                        Debug.Log("A modification on " + obj.name + " has become obsolete, and was removed.");
                    }
                }
            } 
            catch
            {
                Debug.Log("This save file does not contain engineering data and may become unstable.\nGenerate a new save file to fix this issue");
            }

            Debug.Log("Placed " + obj.name + " at " + a[i, 2] + " " + a[i, 3]);
        }
    }

    // Returns a buildings ID if unlocked
    public Transform GetBuildingWithID(int a)
    {
        switch (a)
        {
            case 200:
                return EnemyTurretSMG;
            case 201:
                return EnemyTurretDual;
            case 202:
                return EnemyTurretRanger;
            case 204:
                return EnemyStaticMine;
            case 205:
                return EnemyStaticWall;
        }

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
        if (a + gold >= goldStorage)
            gold = goldStorage;
        else gold += a;
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
        if (a + essence >= essenceStorage)
            essence = essenceStorage;
        else essence += a;
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
        if (a + iridium >= iridiumStorage)
            iridium = iridiumStorage;
        else iridium += a;
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
        if (!tech.checkIfUnlocked(obj))
        {
            // Change to some default shit
            UI.UpdateInventoryInfo(null);
            return;
        }
        // Change to some good shit oh fyck ya
        UI.UpdateInventoryInfo(obj);
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
        // Set color
        // ColorUtility.TryParseHtmlString("#333333", out CameraColor);
        MainCamera.backgroundColor = CameraColor;

        // If unit is larger then 1x1, change selected obj accordingly
        if (SelectedObj.name == "Rocket Pod" || SelectedObj.name == "Turbine" || SelectedObj.name == "Sunbeam")
        {
            largerUnit = true;
            if (SelectedObj.name != "Turbine")
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

        // Set AOCB if required
        if (SelectedObj != null && SelectedObj == PowerObj)
            AOCB.SetActive(true);
        else
            AOCB.SetActive(false);
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
        SaveSystem.SaveGame(this, tech, Spawner.GetComponent<WaveSpawner>(), rsrch, difficulties);
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
            if (allObjects[i].tag == "Defense" || allObjects[i].tag == "Production" || allObjects[i].tag == "Enemy Defense") length += 1;
        }

        int[,] data = new int[length, 5];
        length = 0;
        for (int i = 0; i < allObjects.Length; i++)
        {
            if (allObjects[i].tag == "Defense" || allObjects[i].tag == "Production" || allObjects[i].tag == "Enemy Defense")
            {
                try
                {
                    Debug.Log(allObjects[i].name);
                    data[length, 0] = allObjects[i].GetComponent<TileClass>().getID();
                    data[length, 1] = allObjects[i].GetComponent<TileClass>().GetHealth();
                    data[length, 2] = (int)allObjects[i].position.x;
                    data[length, 3] = (int)allObjects[i].position.y;
                    if (allObjects[i].GetComponent<TileClass>().AppliedModification.Count > 0)
                        data[length, 4] = allObjects[i].GetComponent<TileClass>().AppliedModification[0];
                    else
                        data[length, 4] = -1; // No engineer modifications on this unit
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