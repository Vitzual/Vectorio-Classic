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
using System.Collections.Generic;
using System;

public class Survival : MonoBehaviour
{
    // Manager
    public GameObject manager;
    
    // Technology script
    public Technology tech;

    // Interface script
    public Interface UI;

    public Tutorial tutorial;

    // Difficulties script
    private Difficulties difficulties;

    // Research object
    public Research rsrch;

    // The game object used to spawn enemies
    public Spawner Spawner;

    // Camera zoom object
    public CameraScroll cameraScroll;

    // EnemyHandler object
    public DroneManager droneManager;

    // Save stats
    public int Playtime = 0;
    public int AutoSaveInterval = 300;
    public bool firstAuto = true;

    // Music audio source
    public AudioSource music;
    public AudioClip placementSound;
    public AudioClip pipetteSound;

    // layers
    public LayerMask GhostLayer;
    public LayerMask EnemyLayer;
    public LayerMask EnemyBuildingLayer;
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
    public int goldStorage = 0;
    public int essenceStorage = 0;
    public int iridiumStorage = 0;
    public int PowerConsumption = 0;
    public int AvailablePower = 0;
    public static bool Blackout = false;
    public Notify[] elements;

    // Resource UI popups
    public GameObject BuildingStats;
    public GameObject popup;

    // Add additional cost
    public float additionalCost = 1f;

    // Sprite of the selected object
    private SpriteRenderer Selected;

    // Flashing effect variables
    private float Adjustment = 1f;
    private int AdjustLimiter = 0;
    private bool AdjustSwitch = false;

    // Objects that have additional mechanics in Survival
    [SerializeField] private Transform HubObj;             // No ID
    [SerializeField] private Transform TurretObj;          // ID = 0
    [SerializeField] private Transform WallObj;            // ID = 1
    [SerializeField] private Transform CollectorObj;       // ID = 2
    [SerializeField] private Transform DroneObj;
    [SerializeField] private Transform EssenceObj;         // ID = 10
    [SerializeField] private Transform PowerObj;           // ID = 13
    [SerializeField] private Transform EngineerObj;        // ID = 16
    [SerializeField] private Transform Iridium;            // ID = 23
    [SerializeField] private Transform GoldStorage;        // ID = 25
    [SerializeField] private Transform EssenceStorage;     // ID = 26
    [SerializeField] private Transform IridiumStorage;     // ID = 27
    
    [SerializeField] private Transform EnemyTurretDual;    // ID = 201
    [SerializeField] private Transform EnemyTurretSMG;     // ID = 200
    [SerializeField] private Transform EnemyTurretRanger;  // ID = 202
    [SerializeField] private Transform EnemyStaticWall;    // ID = 205
    [SerializeField] private Transform EnemyStaticMine;    // ID = 204

    // Holds the last building that was hit
    public Vector3 lastHit;

    // Holds the most recent engineer
    public Transform EngineerHolder;
    public int EngineerModID = 0;
    public string EngineerName = "Default";

    // The square that appears around buildings when clicked
    public GameObject SelectedOverlay;

    // The stats of the building clicked 
    public CanvasGroup PromptOverlay;

    // The radius of the selected building
    public GameObject SelectedRadius;
    public GameObject SquareRadius;

    // The ghost building when placing an object
    public Transform GhostBuilding;

    // The building you currently have selected
    public Transform SelectedObj;

    // The hotbar building you last hovered over
    public Transform HoveredObj;

    // Stores information about previously selected object
    private Transform LastObj;

    // If the object selected is 2x2 instead of 1x1
    public bool largerUnit = false;

    // Holds the AOC level
    public int AOC_Level = 1;

    // The area of control size
    private int AOC_Size = 750;

    // The AOC border game object;
    public Transform AOC_Object;

    public Transform lastDronePort;
    public GameObject Locked1;
    public GameObject Locked2;
    public GameObject Drone1;
    public GameObject Drone2;

    // Internal placement variables
    [SerializeField] private LayerMask ResourceLayer;
    [SerializeField] private LayerMask TileLayer;
    [SerializeField] private LayerMask UILayer;
    private Vector2 MousePos;
    private Vector3 LastPos;
    private bool isObjectNull;
    protected float distance = 10;
    public Transform[] hotbar = new Transform[9];

    bool isMenu = false;
    public bool SettingHotbar = false;
    public int panelType = 0;

    // Holds a list of all ghost objects 
    public List<Vector2> ghostBuildings;

    private void Start()
    {
        difficulties = GameObject.Find("Difficulty").GetComponent<Difficulties>();

        // Make sure it's not the menu
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            isMenu = true;
            return;
        }

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
        tech.unlocked.Add(GoldStorage);
        tech.unlocked.Add(WallObj);
        tech.unlocked.Add(DroneObj);

        // Load save data to file
        SaveData data = SaveSystem.LoadGame();

        // Load settings
        manager.GetComponent<Settings>().LoadSettings();

        if (data != null)
        {

            // Check to see if there's a difficulty saved
            //try
            //{
                // Set world data
                Difficulties.world = data.WorldName;
                Difficulties.mode = data.WorldMode;
                Difficulties.seed = data.WorldSeed;
                Difficulties.version = data.WorldVersion;

                // Set resources
                Difficulties.goldMulti = data.GoldAmount;
                Difficulties.essenceMulti = data.EssenceAmount;
                Difficulties.iridiumMulti = data.IridiumAmount;

                // Set enemies
                Difficulties.enemyAmountMulti = data.EnemyAmountMulti;
                Difficulties.enemyHealthMulti = data.EnemyHealthMulti;
                Difficulties.enemyDamageMulti = data.EnemyDamageMulti;
                Difficulties.enemySpeedMulti = data.EnemySpeedMulti;
                Difficulties.enemyWaves = data.EnemyGroups;
                Difficulties.enemyOutposts = data.EnemyOutposts;
                Difficulties.enemyGuardians = data.EnemyGuaridans;
            //}
            //catch
            //{
                Debug.Log("Save file contains obsolete data! This may cause errors while attempting to load this save...");
            //}

            try { Playtime = data.time; }
            catch { Debug.Log("Save file does not contain new time tracking data!"); Playtime = 0; }

            // Set power usage
            UI.PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
            UI.AvailablePower.text = AvailablePower.ToString() + " MAX";

            // Attempt to load save data 
            //try
            //{
                // Force tech tree update
                try { tech.LoadSaveData(data.UnlockIDs); }
                catch (Exception e) { Debug.Log("Save file does not contain new unlock data!\nStack: "+e); }

                // Generate world data
                GameObject.Find("OnSpawn").GetComponent<OnSpawn>().GenerateWorldData(Difficulties.seed, true);

                // Get research save data
                rsrch.SetResearchData(data.ResearchedTiers);

                // Attempt to place saved buildings
                float soundHolder = manager.GetComponent<Settings>().GetSound();
                manager.GetComponent<Settings>().SetSound(0);
                PlaceSavedBuildings(data);

                // Update bosses
                //try
                //{
                    Spawner.updateBosses(data.bossesDefeated);
                //}
                //catch
                //{
                    Debug.Log("Save file does not contain new Guardian data!");
                //}

                // Set power usage
                UI.PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
                UI.AvailablePower.text = AvailablePower.ToString() + " MAX";

                // Place saved enemies 
                //try
                //{
                    PlaceSavedEnemies(data.Enemies);
                //}
                //catch
                //{
                    Debug.Log("Save file does not contain new enemy data!");
                //}

                // Update hotbar with saved ID's
                //try
                //{
                    SetHotbarData(data.hotbar);
                //}
                //catch
                //{
                    Debug.Log("Save file does not contain new hotbar data!");
                //}

                manager.GetComponent<Settings>().SetSound(soundHolder);
            //}
            //catch (System.Exception e)
            //{
                Spawner.loadingSave = false;
                //Debug.Log("The save data found was corrupt.\n\nStacktrace: " + e.Message + "\n" + e.StackTrace);
                GameObject.Find("OnSpawn").GetComponent<OnSpawn>().GenerateWorldData(Difficulties.seed, false);
            //}

        }
        else
        {
            // New save
            tutorial.enableTutorial();
            Spawner.loadingSave = false;
            Debug.LogError("No save data found. Starting new save.");
            GameObject.Find("OnSpawn").GetComponent<OnSpawn>().GenerateWorldData(Difficulties.seed, false);
        }

        // Start auto saving
        InvokeRepeating("AutoSave", 0f, AutoSaveInterval);
        InvokeRepeating("IncreaseTime", 0f, 1f);
        increaseAvailablePower(0);
        increasePowerConsumption(0);
    }

    // Gets called once every frame
    private void Update()
    {
        if (isMenu) return;
        if (UI.MenuOpen || UI.SettingsOpen) { CheckSettings(); return; }

        // Get mouse position and round to middle grid coordinate
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check if the selected object is null
        if (SelectedObj == null) isObjectNull = true;
        else isObjectNull = false;

        // Update tile spacing
        CenterTransform();

        // Check if object selected
        if (!isObjectNull)
        {
            // Check unit size and make flash
            AdjustAlphaValue();
            if (AOCB.activeSelf) AOCB.transform.position = new Vector3(transform.position.x, transform.position.y, 5);
        }

        // Check if user left clicks
        if (Input.GetButton("Fire1") && !UI.BuildingOpen && !UI.ResearchOpen && !UI.DroneOpen && !UI.BossInfoOpen && !UI.UOLOpen && Input.mousePosition.y >= 200)
        {
            // Check if valid placement
            bool ValidTile = CheckPlacement(SelectedObj);

            // Raycast tile to see if there is already a tile placed
            RaycastHit2D[] rayHit = Physics2D.RaycastAll(MousePos, Vector2.zero, Mathf.Infinity, TileLayer | EnemyBuildingLayer);
            Transform RayTarget = CheckRaycast(rayHit);

            // Check if placement is within AOC
            if (ValidTile && RayTarget == null && !isObjectNull && transform.position.x <= AOC_Size && transform.position.x >= -AOC_Size + 5 && transform.position.y <= AOC_Size && transform.position.y >= -AOC_Size + 5)
            {
                if (tutorial.disableBuilding) return;

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
                TileClass ObjectComponent = SelectedObj.GetComponent<TileClass>();

                // Place the building and register as a ghost variant and queue it in the drone network
                LastObj = Instantiate(GhostBuilding, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                LastObj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + SelectedObj.name);
                if (largerUnit) LastObj.GetComponent<BoxCollider2D>().size = new Vector2(10f, 10f);
                LastObj.name = SelectedObj.name;
                droneManager.queueBuilding(SelectedObj, LastObj, ObjectComponent.GetCost(), ObjectComponent.getConsumption(), ObjectComponent.GetHeat());
                ghostBuildings.Add(new Vector2(transform.position.x, transform.position.y));
            }
            else if (RayTarget != null)
            {
                if (RayTarget.name == "Drone Port" && isObjectNull)
                {
                    if (tutorial.tutorialSlide == 4) tutorial.nextSlide();

                    if (Research.research_fixer_drones)
                    {
                        Locked1.SetActive(false);
                        Drone1.SetActive(true);
                    }
                    if (Research.research_combat_drones)
                    {
                        Locked2.SetActive(false);
                        Drone2.SetActive(true);
                    }
                    lastDronePort = RayTarget.transform;
                    UI.OpenDronePort();
                }
                else if (RayTarget.name != "Hub" && isObjectNull)
                {
                    UI.ShowTileInfo(RayTarget);
                    UI.ShowingInfo = true;
                    SelectedOverlay.SetActive(true);
                    SelectedOverlay.transform.position = RayTarget.position;
                    PromptOverlay.alpha = 1;
                }
            }
        }

        // If user right clicks, remove object
        else if (Input.GetButton("Fire2") && !UI.BuildingOpen && !UI.DroneOpen && !tutorial.disableBuilding)
        {
            //Overlay.transform.Find("Hovering Stats").GetComponent<CanvasGroup>().alpha = 0;
            RaycastHit2D[] rayHit = Physics2D.RaycastAll(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);
            Transform RayTarget = CheckRaycast(rayHit);

            // Raycast tile to see if there is already a tile placed
            if (RayTarget != null && RayTarget.name != "Hub")
            {
                TileClass rayScript = RayTarget.GetComponent<TileClass>();
                UI.ShowingInfo = false;
                SelectedOverlay.SetActive(false);

                // Check if tile is an essential resource and the amount is not less then 2
                if ((RayTarget.name == "Drone Port" || RayTarget.name == "Gold Collector" || RayTarget.name == "Gold Storage") &&
                    (!BuildingHandler.buildingAmount.ContainsKey(RayTarget.name) || BuildingHandler.buildingAmount[RayTarget.name] <= 1))
                {
                    rayScript.DestroyTile(false);
                }
                else
                {
                    int amount = rayScript.GetCost() - rayScript.GetCost() / 5;
                    AddGold(amount, true);
                    UI.CreateResourcePopup("+ " + amount, "Gold", RayTarget.position);
                    rayScript.DestroyTile(true);
                }
            }
            else
            {
                RaycastHit2D rayGhost = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, GhostLayer);
                bool holder;
                if (rayGhost.collider != null)
                {
                    holder = droneManager.dequeueBuilding(rayGhost.collider.transform);
                    if (!holder) Destroy(rayGhost.collider.gameObject);
                }
            }
        }

        // Check hotbar thing        
        CheckNumberInput();

        if (tutorial.disableMenus) return;

        // Pipette a building
        if (Input.GetKeyDown(KeyCode.Mouse2) && !UI.BuildingOpen && !UI.DroneOpen && !UI.UOLOpen)
        {
            // Attempt a raycast on the tile survival is in
            RaycastHit2D[] rayHits = Physics2D.RaycastAll(MousePos, Vector2.zero, Mathf.Infinity, TileLayer | GhostLayer);

            // Set the selected object to the collider if not null
            foreach (RaycastHit2D rayHit in rayHits)
            {
                if (Vector3.Distance(rayHit.collider.transform.position, transform.position) < 5)
                {
                    SelectObject(tech.FindTechBuildingWithName(rayHit.collider.name));
                    AudioSource.PlayClipAtPoint(pipetteSound, rayHit.collider.transform.position, Settings.soundVolume);
                    UI.CreatePippeteSquare(rayHit.collider.transform.position);
                }
            }
        }

        // Opens the building menu if E is pressed.
        if (Input.GetKeyDown(KeyCode.E) && UI.BuildingOpen == false && UI.UOLOpen == false && !UI.SettingsOpen)
        {
            if (tutorial.tutorialSlide == 8) tutorial.nextSlide();
            UI.OpenSurvivalMenu();
        }

        // If T pressed, open research menu
        else if (Input.GetKeyDown(KeyCode.T) && UI.MenuOpen == false && UI.ResearchOpen == false && UI.UOLOpen == false)
        {
            if (tutorial.tutorialSlide == 6) tutorial.nextSlide();
            UI.closeResearchUnlock();
            if (UI.DroneOpen)
                UI.CloseDronePort();
            UI.OpenResearchOverlay();
        }

        // If T pressed and research menu open, close it
        else if (Input.GetKeyDown(KeyCode.T) && UI.MenuOpen == false && UI.ResearchOpen == true)
        {
            UI.closeResearchUnlock();
            UI.CloseResearchOverlay();
        }

        // If escape pressed and boss panel is open, close it
        else if (Input.GetKeyDown(KeyCode.Escape) && UI.EndScreenOpen == true)
        {
            UI.CloseEndWindow();
        }

        // If escape pressed and boss panel is open, close it
        else if (Input.GetKeyDown(KeyCode.Escape) && UI.BossInfoOpen == true)
        {
            Spawner.closeBossInfo();
        }

        // If escape pressed and new info is open, close it
        else if (Input.GetKeyDown(KeyCode.Escape) && UI.UOLOpen == true)
        {
            UI.AdjustTimescale();
        }

        // If escape pressed and engineer open, close it
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) && UI.DroneOpen == true)
        {
            if (tutorial.tutorialSlide == 6) tutorial.nextSlide();
            UI.CloseDronePort();
        }

        // If escape pressed and building menu open, close it
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) && UI.BuildingOpen == true)
        {
            UI.OpenSurvivalMenu();
        }

        // If escape pressed and research open, close it.
        else if ((Input.GetKeyDown(KeyCode.Escape) && UI.ResearchOpen == true))
        {
            UI.CloseResearchOverlay();
        }

        // If escape pressed and selected object isn't null, unselect it
        else if (Input.GetKeyDown(KeyCode.Escape) && !isObjectNull)
        {
            Selected.sprite = null;
            SelectedObj = null;
            UI.DisableActiveInfo();
            UI.ShowingInfo = false;
            BuildingStats.SetActive(false);
            SelectedRadius.SetActive(false);
            SquareRadius.SetActive(false);

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

        else CheckSettings();
    }

    public void IncreaseTime()
    {
        Playtime++;
    }

    public void CheckSettings()
    {
        // If escape pressed and menu not open, open it
        if (Input.GetKeyDown(KeyCode.Escape) && UI.MenuOpen == false && UI.SettingsOpen == false && UI.UOLOpen == false)
        {
            UI.SaveButton.GetComponent<CanvasGroup>().interactable = true;
            UI.SaveButton.buttonText = "SAVE";
            UI.SaveButton.UpdateUI();

            UI.MenuOpen = true;
            UI.SetOverlayStatus("Paused", true);

            Time.timeScale = 0f;
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

        // If escape pressed and settings open, close it and open menu
        else if (Input.GetKeyDown(KeyCode.Escape) && UI.ControlsOpen == true)
        {
            UI.ControlsOpen = false;
            UI.SettingsOpen = true;
            UI.SetOverlayStatus("Controls", false);
            UI.SetOverlayStatus("Settings", true);
        }

        // If escape pressed and menu open, close it
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI.MenuOpen = false;
            UI.SetOverlayStatus("Paused", false);

            Time.timeScale = 1f;
        }
    }

    public void SetDronePort(int type)
    {
        lastDronePort.GetComponent<Dronehub>().changeDroneType(type);
        UI.OpenDronePort();
    }

    // Updates the gold storage
    public void UpdateGoldStorage(int amount)
    {
        RemoveGold(amount);
        goldStorage -= Research.research_gold_storage;
        UI.GoldStorage.text = goldStorage + " MAX";
    }

    // Updates the essence storage
    public void UpdateEssenceStorage(int amount)
    {
        RemoveEssence(amount);
        essenceStorage -= Research.research_essence_storage;
        UI.EssenceStorage.text = essenceStorage + " MAX";
    }

    // Updates the iridium storage
    public void UpdateIridiumStorage(int amount)
    {
        RemoveIridium(amount);
        iridiumStorage -= Research.research_iridium_storage;
        UI.IridiumStorage.text = iridiumStorage + " MAX";
    }

    // Set the last object that was hit
    public void SetLastHit(Vector3 pos)
    {
        lastHit = pos;
        UI.EnableWarning();
    }

    // Go to the position of the last hit object
    public void MoveToLastHit()
    {
        MainCamera.transform.position = new Vector3(lastHit.x, lastHit.y, MainCamera.transform.position.z);
    }

    // Checks unit size
    private void CenterTransform()
    {
        if (isObjectNull || !largerUnit) transform.position = new Vector2(5 * Mathf.Round(MousePos.x / 5), 5 * Mathf.Round(MousePos.y / 5));
        else transform.position = new Vector2(5 * Mathf.Round(MousePos.x / 5) - 2.5f, 5 * Mathf.Round(MousePos.y / 5) + 2.5f);
    }

    public void SetHorbar(int number)
    {
        SettingHotbar = true;
        panelType = number;
        UI.InfoPanels[panelType].hotbarButton.buttonText = "PRESS 1-9 TO SET";
        UI.InfoPanels[panelType].hotbarButton.UpdateUI();
    }

    // Checks if for numeric input
    public void CheckNumberInput()
    {
        if (tutorial.disableBuilding) return;

        if (SettingHotbar)
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
            return;
        }
        else
        {
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
    }

    // Checks if a unit can be placed
    public bool CheckPlacement(Transform obj)
    {
        if (!isObjectNull)
        {
            // Check the AOCB
            if (SelectedObj.name == "Energizer")
            {
                var colliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y), new Vector2(60, 60), 0, AOCBLayer);
                if (colliders.Length == 0) return false;
            }
            else
            {
                RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, AOCBLayer);
                if (rayHit.collider == null) return false;
                if (largerUnit)
                {
                    rayHit = Physics2D.Raycast(new Vector2(MousePos.x - 5f, MousePos.y), Vector2.zero, Mathf.Infinity, AOCBLayer);
                    if (rayHit.collider == null) return false;
                    rayHit = Physics2D.Raycast(new Vector2(MousePos.x - 5f, MousePos.y + 5f), Vector2.zero, Mathf.Infinity, AOCBLayer);
                    if (rayHit.collider == null) return false;
                    rayHit = Physics2D.Raycast(new Vector2(MousePos.x, MousePos.y + 5f), Vector2.zero, Mathf.Infinity, AOCBLayer);
                    if (rayHit.collider == null) return false;
                }
            }

            // Check for and adjust accordingly 
            RaycastHit2D[] rayHits;
            rayHits = Physics2D.RaycastAll(new Vector2(MousePos.x, MousePos.y), Vector2.zero, Mathf.Infinity, TileLayer | EnemyBuildingLayer | GhostLayer);
            if (CheckRaycast(rayHits) != null) return false;
            if (largerUnit)
            {
                rayHits = Physics2D.RaycastAll(new Vector2(MousePos.x - 5f, MousePos.y), Vector2.zero, Mathf.Infinity, TileLayer | EnemyBuildingLayer | GhostLayer);
                if (CheckRaycast(rayHits) != null) return false;
                rayHits = Physics2D.RaycastAll(new Vector2(MousePos.x, MousePos.y + 5f), Vector2.zero, Mathf.Infinity, TileLayer | EnemyBuildingLayer | GhostLayer);
                if (CheckRaycast(rayHits) != null) return false;
                rayHits = Physics2D.RaycastAll(new Vector2(MousePos.x - 5f, MousePos.y + 5f), Vector2.zero, Mathf.Infinity, TileLayer | EnemyBuildingLayer | GhostLayer);
                if (CheckRaycast(rayHits) != null) return false;
            }
        }
        return true;
    }

    public Transform CheckRaycast(RaycastHit2D[] ray)
    {
        foreach (RaycastHit2D hit in ray)
            if (hit.collider is BoxCollider2D) return hit.collider.transform;
        return null;
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


    // Place building loaded from a save file
    public void PlaceSavedBuildings(SaveData data)
    {
        bool metadata = true;
        int[,] a = data.Locations;

        for (int i = 0; i < a.GetLength(0); i++)
        {
            Transform building = GetBuildingWithID(a[i, 0]);
            if (building == null) continue;

            float x = a[i, 2];
            float y = a[i, 3];
            Vector2 position;

            if (building.GetComponent<TileClass>().isBig)
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
            BuildingHandler.addBuilding(obj);
            try { obj.GetComponent<AnimateThenStop>().animEnabled = false; } catch { }

            // Resource offset
            if (obj.name.Contains("Collector")) StartCoroutine(obj.GetComponent<CollectorAI>().OffsetStart());

            // Attempt to apply metadata for storages
            else if (metadata && obj.name.Contains("Storage"))
            {
                try 
                {
                    // Attempt to grab the attached script
                    StorageAI storage = obj.GetComponent<StorageAI>();
                    int holder = storage.addResources(a[i, 5], true);

                    // Add resources depedent on collector type
                    if (storage.type == 1) AddGold(a[i, 5], false, true);
                    else if (storage.type == 2) AddEssence(a[i, 5], false, true);
                    else if(storage.type == 3) AddIridium(a[i, 5], false, true);
                }
                catch
                {
                    // No meta data
                    metadata = false;
                    Debug.Log("Save does not contain metadata support. Save the game again to update the save structure automatically.");

                    // Reset values and add what is saved on record. Do note this can cause offsets between storages and internal values
                    gold = 0;
                    essence = 0;
                    iridium = 0;
                    AddGold(data.Gold);
                    AddEssence(data.Essence);
                    AddIridium(data.Iridium);
                }
            }

            // Attempt to apply metadata for drone ports
            else if (metadata && obj.name == "Drone Port")
            {
                try 
                {
                    Dronehub drone = obj.GetComponent<Dronehub>();
                    drone.droneType = a[i, 5];
                    drone.offsetStart = true;
                }
                catch
                {
                    metadata = false;
                    Debug.Log("Save does not contain metadata support. Save the game again to update the save structure automatically.");
                }
            }

            // Set survival
            increasePowerConsumption(building.GetComponent<TileClass>().getConsumption());
            Spawner.GetComponent<Spawner>().increaseHeat(building.GetComponent<TileClass>().GetHeat());
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

        return tech.FindTechBuilding(a);
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
        if (AvailablePower >= 1000000) UI.AvailablePower.text = string.Concat(AvailablePower / 100000, "M") + " MAX";
        else if (AvailablePower >= 10000) UI.AvailablePower.text = string.Concat(AvailablePower / 1000, "K") + " MAX";
        else UI.AvailablePower.text = AvailablePower + " MAX";
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

        tech.UpdateUnlock("Power");
    }

    // Decrease power consumption by x amount
    public void decreasePowerConsumption(int a)
    {
        PowerConsumption -= a;
        UI.PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
        UI.PowerUsage.text = PowerConsumption.ToString();
    }

    // Add x gold to player
    public void AddGold(int a, bool addToStorages = false, bool fromSave = false)
    {
        if (!fromSave && a + gold >= goldStorage)
            gold = goldStorage;
        else gold += a;
        UI.GoldAmount.text = gold.ToString();

        // This will force input resources into the storages
        if (addToStorages)
        {
            for (int i = 0; i < BuildingHandler.storages.Count; i++)
            {
                if (BuildingHandler.storages[i].icon == null)
                {
                    BuildingHandler.storages.RemoveAt(i);
                    i--;
                    continue;
                }
                else if (BuildingHandler.storages[i].type == 1)
                {
                    a = BuildingHandler.storages[i].addResources(a, true);
                    if (a == 0) return;
                }
            }
        }
    }

    // Remove x gold from player
    public void RemoveGold(int a)
    {
        gold -= a;
        if (gold < 0) gold = 0;
        BuildingHandler.removeStorageResources(a, 1);
        UI.GoldAmount.text = gold.ToString();
    }

    // Add x essence to player
    public void AddEssence(int a, bool addToStorages = false, bool fromSave = false)
    {
        if (!fromSave && a + essence >= essenceStorage)
            essence = essenceStorage;
        else essence += a;
        UI.EssenceAmount.text = essence.ToString();

        // This will force input resources into the storages
        if (addToStorages)
        {
            for (int i = 0; i < BuildingHandler.storages.Count; i++)
            {
                if (BuildingHandler.storages[i].icon == null)
                {
                    BuildingHandler.storages.RemoveAt(i);
                    i--;
                    continue;
                }
                else if (BuildingHandler.storages[i].type == 2)
                {
                    a = BuildingHandler.storages[i].addResources(a, true);
                    if (a == 0) return;
                }
            }
        }
    }

    // Remove x essence from player
    public void RemoveEssence(int a)
    {
        essence -= a;
        if (essence < 0) essence = 0;
        BuildingHandler.removeStorageResources(a, 2);
        UI.EssenceAmount.text = essence.ToString();
    }

    // Add x iridium to player
    public void AddIridium(int a, bool addToStorages = false, bool fromSave = false)
    {
        if (!fromSave && a + iridium >= iridiumStorage)
            iridium = iridiumStorage;
        else iridium += a;
        UI.IridiumAmount.text = iridium.ToString();

        // This will force input resources into the storages
        if (addToStorages)
        {
            for (int i = 0; i < BuildingHandler.storages.Count; i++)
            {
                if (BuildingHandler.storages[i].icon == null)
                {
                    BuildingHandler.storages.RemoveAt(i);
                    i--;
                    continue;
                }
                else if (BuildingHandler.storages[i].type == 3)
                {
                    a = BuildingHandler.storages[i].addResources(a, true);
                    if (a == 0) return;
                }
            }
        }
    }

    // Remove x essence from player
    public void RemoveIridium(int a)
    {
        iridium -= a;
        if (iridium < 0) iridium = 0;
        BuildingHandler.removeStorageResources(a, 3);
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
        hotbar[3] = DroneObj;
        UI.UpdateHotbar();
    }

    // Select object on hotbar
    public void SelectHotbar(int index)
    {
        if (UI.MenuOpen || UI.SettingsOpen || UI.ControlsOpen) return;

        Transform holder = SelectedObj;
        try
        {
            // Disable selected overlay
            UI.ShowingInfo = false;
            PromptOverlay.alpha = 0;
            SelectedOverlay.SetActive(false);

            // Set hotbar transform
            holder = SelectedObj;
            SelectedObj = hotbar[index];
            SwitchObj();
            UI.SetSelectedHotbar(index);
        }
        catch { Debug.Log("Error selecting object at index " + index); SelectedObj = holder; }
    }


    // Changes the object that the player has selected (pass null to deselect)
    public void SelectObject(Transform obj)
    {
        // Disable selected overlay
        UI.ShowingInfo = false;
        PromptOverlay.alpha = 0;
        SelectedOverlay.SetActive(false);

        SelectedObj = obj;
        if (obj != null && !tech.CheckIfUnlocked(obj)) return;
        SwitchObj();
    }

    // Changes the stored object for hotbar changing
    public void SetChosenObj(Transform obj)
    {
        // Tutorial related, ignore
        if (tutorial.tutorialSlide == 9 && obj != null && obj.name == "Gold Storage") tutorial.nextSlide();

        // Check if the object is unlocked
        if (obj != null && !tech.CheckIfUnlocked(obj))
        {
            UI.UpdateInfoPanel(null);
            return;
        }
        else
        {
            SelectObject(obj);
            UI.UpdateInfoPanel(obj);
            HoveredObj = obj;
        }
    }

    // THIS IS THE CULPRIT 
    //
    // Changes the object stored in a hotbar slot
    public void SetHotbarSlot(int slot, Transform obj)
    {
        SettingHotbar = false;
        UI.InfoPanels[panelType].hotbarButton.buttonText = "ASSIGN TO HOTBAR";
        UI.InfoPanels[panelType].hotbarButton.UpdateUI();
        if (!tech.CheckIfUnlocked(obj)) return;
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
        if (SelectedObj.GetComponent<TileClass>().isBig) largerUnit = true;
        else largerUnit = false;

        // Disable any active info not relative to selected object
        UI.DisableActiveInfo();
        Adjustment = 1f;
        Selected.sprite = Resources.Load<Sprite>("Sprites/" + SelectedObj.name);
        UI.ShowSelectedInfo(SelectedObj);

        // Set radius dimensions if selected object is defense
        if (SelectedObj.tag == "Defense" && SelectedObj.name != "Wall" && SelectedObj.name != "Drone Port")
        {
            float range = SelectedObj.GetComponent<TurretClass>().range * 2 + Research.research_range;
            SelectedRadius.transform.localScale = new Vector3(range, 1, range);
            SelectedRadius.SetActive(true);
            SquareRadius.SetActive(false);
        }
        else if (SelectedObj.name == "Enhancer" || SelectedObj.name == "Engineer")
        {
            SquareRadius.SetActive(true);
            SquareRadius.transform.localScale = new Vector3(15, 15, 15);
            SelectedRadius.SetActive(false);
        } 
        else if (SelectedObj.name == "Drone Port")
        {
            float rng = (Research.research_resource_range * 2) + 5f;
            SquareRadius.SetActive(true);
            SquareRadius.transform.localScale = new Vector3(rng, rng, rng);
            SelectedRadius.SetActive(false);
        }
        else
        {
            SquareRadius.SetActive(false);
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
        Destroy(difficulties.gameObject);
        SceneManager.LoadScene("Menu");
    }

    public void AutoSave()
    {
        if (!firstAuto) {
            UI.DisplayAutosave();
            Save();
        } else firstAuto = false;
    }

    // Sends all data to the save script
    public void Save()
    {
        if (!Spawner.bossSpawned)
        {
            Debug.Log("Attempting to save data");
            SaveSystem.SaveGame(this, tech, Spawner, rsrch, Playtime, Spawner.htrack);
            Debug.Log("Data was saved successfully");

            UI.SaveButton.buttonText = "SAVED";
            UI.SaveButton.GetComponent<CanvasGroup>().interactable = false;
            UI.SaveButton.UpdateUI();
        }
        else
        {
            UI.SaveButton.buttonText = "DISABLED";
            UI.SaveButton.UpdateUI();
        }
    }

    public void SaveAndQuit()
    {
        Save();
        Quit();
    }

    // Returns the ID's of all buildings on the hotbar
    public int[] GetHotbarData()
    {
        int[] hotbarIDs = new int[9];
        for(int i = 0; i < hotbar.Length; i++)
        {
            if (hotbar[i] != null)
                hotbarIDs[i] = hotbar[i].GetComponent<TileClass>().ID;
            else hotbarIDs[i] = -1;
        }
        return hotbarIDs;
    }

    // Set hotbar data from the ID's on the save file
    public void SetHotbarData(int[] data)
    {
        for (int i = 0; i < hotbar.Length; i++)
            if (data[i] != -1) SetHotbarSlot(i, GetBuildingWithID(data[i]));
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

        int[,] data = new int[length, 6];
        length = 0;
        for (int i = 0; i < allObjects.Length; i++)
        {
            if (allObjects[i].tag == "Defense" || allObjects[i].tag == "Production" || allObjects[i].tag == "Enemy Defense")
            {
                try
                {
                    // ID of the building to save
                    data[length, 0] = allObjects[i].GetComponent<TileClass>().getID();

                    // Health of the building being saved
                    data[length, 1] = allObjects[i].GetComponent<TileClass>().GetHealth();

                    // Coordinates of the building
                    data[length, 2] = (int)allObjects[i].position.x;
                    data[length, 3] = (int)allObjects[i].position.y;
                    data[length, 4] = -1; // No engineer modifications on this unit

                    // Meta data that should be saved 
                    if (allObjects[i].name == "Drone Port") data[length, 5] = allObjects[i].GetComponent<Dronehub>().droneType;
                    else if (allObjects[i].name.Contains("Storage")) data[length, 5] = allObjects[i].GetComponent<StorageAI>().amount;
                    else data[length, 5] = -1; // No meta data to apply

                    // Increment length
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
            Transform enemy = Spawner.GetEnemyWithID((int)a[i, 0]);
            if (enemy == null) continue;

            float x = a[i, 2];
            float y = a[i, 3];
            Vector2 position = new Vector2(x, y);

            Transform obj = Instantiate(enemy, position, Quaternion.Euler(new Vector3(0, 0, 0)));

            obj.name = enemy.name;
            enemy.gameObject.GetComponent<EnemyClass>().SetHealth((int)a[i, 1]);

            //Debug.Log("Placed " + obj.name + " at " + a[i, 2] + " " + a[i, 3]);
        }
    }

    public Transform GetEssenceObj()
    {
        return EssenceObj;
    }
}