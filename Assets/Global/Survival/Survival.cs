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

    // The game object used to spawn enemies
    public WaveSpawner Spawner;

    // Camera zoom object
    public CameraScroll cameraScroll;

    public int Playtime = 0;
    public int AutoSaveInterval = 300;
    public bool firstAuto = true;

    // Music audio source
    public AudioSource music;
    public AudioClip placementSound;
    public AudioClip pipetteSound;

    // Tutorial Elements (Survival Exclusive)
    public GameObject TutorialOverlay;
    public GameObject TutorialSlides;
    public GameObject[] Slides;
    public bool showingTutorial = false;
    public int tutorialNumber = 0;
    public int tutorialCollectors = 0;
    public Transform[] TutorialEnemies = new Transform[3];
    public GameObject TutorialHolder;

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
    public int iridiumStorage = 0;
    public int PowerConsumption = 0;
    public int AvailablePower = 0;

    public Notify[] elements;

    // Resource UI popups
    public GameObject popup;

    // Add additional cost
    public float additionalCost = 1f;

    // Sprite of the selected object
    private SpriteRenderer Selected;

    // Flashing effect variables
    private float Adjustment = 1f;
    private int AdjustLimiter = 0;
    private bool AdjustSwitch = false;

    // Object placements
    [SerializeField] private Transform HubObj;             // No ID
    [SerializeField] private Transform TurretObj;          // ID = 0
    [SerializeField] private Transform WallObj;            // ID = 1
    [SerializeField] private Transform CollectorObj;       // ID = 2
    // [SerializeField] private Transform ShotgunObj;      // ID = 3
    // [SerializeField] private Transform SniperObj;       // ID = 4
    // [SerializeField] private Transform EnhancerObj;     // ID = 5
    // [SerializeField] private Transform SMGObj;          // ID = 6
    // [SerializeField] private Transform BoltObj;         // ID = 7
    // [SerializeField] private Transform ChillerObj;      // ID = 8
    // [SerializeField] private Transform RocketObj;       // ID = 9
    [SerializeField] private Transform EssenceObj;         // ID = 10
    // [SerializeField] private Transform TurbineObj;      // ID = 11
    // [SerializeField] private Transform TeslaObj;        // ID = 12
    [SerializeField] private Transform PowerObj;           // ID = 13
    //[SerializeField] private Transform FlamethrowerObj;  // ID = 14
    //[SerializeField] private Transform MinigunObj;       // ID = 15
    [SerializeField] private Transform EngineerObj;        // ID = 16
    //[SerializeField] private Transform SunbeamObj;       // ID = 20
    //[SerializeField] private Transform AreaCoolerObj;    // ID = 22
    [SerializeField] private Transform Iridium;            // ID = 23
    //[SerializeField] private Transform Solar;            // ID = 24
    [SerializeField] private Transform GoldStorage;        // ID = 25
    [SerializeField] private Transform EssenceStorage;     // ID = 26
    [SerializeField] private Transform IridiumStorage;     // ID = 27
    //[SerializeField] private Transform ArtilleryObj;     // ID = 28
    

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

    // Enemy list
    public Transform[] enemies;

    // The seed the word is set to
    public int seed;

    // The square that appears around buildings when clicked
    public GameObject SelectedOverlay;

    // The stats of the building clicked 
    public CanvasGroup PromptOverlay;

    // The radius of the selected building
    public GameObject SelectedRadius;
    public GameObject SquareRadius;

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

    bool isMenu = false;

    private void Start()
    {
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
        tech.unlocked.Add(WallObj);
        tech.unlocked.Add(GoldStorage);

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

            try
            {
                difficulties.SetGroupSpawns(data.groupSpawnEvery);
                if (difficulties.GetGroupSpawns() == 0) difficulties.SetGroupSpawns(300);
            }
            catch
            {
                Debug.Log("File does not contain group spawning");
                difficulties.SetGroupSpawns(300);
            }

            try
            {
                difficulties.SetSaveName(data.name);
                difficulties.SetModeName(data.mode);
            }
            catch
            {
                difficulties.SetSaveName("OLD SAVE");
                difficulties.SetModeName("CUSTOM");
            }
        }
        catch
        {
            Debug.Log("Difficulty settings could not be loaded!");
            difficulties.SetDifficulty(holder);
        }

        try { Playtime = data.time; }
        catch { Debug.Log("Save file does not contain time play tracking"); Playtime = 0; }

        // Get default stats
        gold = difficulties.GetStartingGold();
        goldStorage = gold * 4;
        UI.GoldStorage.text = goldStorage + " MAX";
        if (AvailablePower == 0)
            AvailablePower = difficulties.GetStartingPower();
        else
            AvailablePower += difficulties.GetStartingPower();
        additionalCost = difficulties.GetAdditionalCost();
        UI.GoldAmount.text = gold.ToString();

        // Set group spawn
        Spawner.SetSpawnAmount(difficulties.GetGroupSpawns());

        // Set power usage
        UI.PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
        UI.AvailablePower.text = AvailablePower.ToString() + " MAX";

        // Load settings
        manager.GetComponent<Settings>().LoadSettings();

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
            seed = data.WorldSeed;

            // Force tech tree update
            tech.loadSaveData(data.UnlockLevel);

            // Generate world data
            GameObject.Find("OnSpawn").GetComponent<OnSpawn>().GenerateWorldData(seed, true);

            // Attempt to place saved buildings
            float soundHolder = manager.GetComponent<Settings>().GetSound();
            manager.GetComponent<Settings>().SetSound(0);

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
                Debug.Log("Save file contains obsolete enemy data!");
            }
            manager.GetComponent<Settings>().SetSound(soundHolder);
        }
        catch (System.Exception e)
        {
            // New save
            Debug.Log("No save data was found, or the save data found was corrupt.\nStacktrace: " + e.Message + "\n" + e.StackTrace);
            seed = Random.Range(1000000, 10000000);
            GameObject.Find("OnSpawn").GetComponent<OnSpawn>().GenerateWorldData(seed, false);

            // Show tutorial UI
            TutorialOverlay.SetActive(true);
            showingTutorial = true;
        }

        // Update bosses
        Spawner.updateBosses();

        // Start auto saving
        InvokeRepeating("AutoSave", 0f, AutoSaveInterval);
        InvokeRepeating("IncreaseTime", 0f, 1f);
    }

    // Gets called once every frame
    private void Update()
    {
        if (isMenu) return;

        // Iterates through the tutorial sequence if enabled
        if (showingTutorial && tutorialNumber != 2 && tutorialNumber != 4)
        {
            if (tutorialNumber == 5 && (TutorialEnemies[0] != null || TutorialEnemies[1] != null || TutorialEnemies[2] != null))
            {
                return;
            }
            else if (tutorialNumber == 5)
            {
                Slides[4].SetActive(false);
                Slides[5].SetActive(true);
            }

            // Main slide (1)
            if (Input.GetKeyDown(KeyCode.Escape) && tutorialNumber == 0)
            {
                TutorialOverlay.SetActive(false);
                showingTutorial = false;
            }

            // Second slide - Welcome to Vectorio!
            else if (Input.GetKeyDown(KeyCode.Space) && tutorialNumber == 0)
            {
                Slides[0].SetActive(false);
                Slides[1].SetActive(true);
                tutorialNumber = 1;
            }

            // Third slide - Place gold mines
            else if (Input.GetKeyDown(KeyCode.Space) && tutorialNumber == 1)
            {
                Slides[1].SetActive(false);
                Slides[2].SetActive(true);
                tutorialNumber = 2;
            }

            // Fourth side - Heat 
            else if (Input.GetKeyDown(KeyCode.Space) && tutorialNumber == 3)
            {
                Slides[3].SetActive(false);
                Slides[4].SetActive(true);
                tutorialNumber = 4;
                TutorialHolder.SetActive(true);
            }

            // Fourth side - Heat 
            else if (Input.GetKeyDown(KeyCode.Space) && tutorialNumber == 5)
            {
                Slides[5].SetActive(false);
                Slides[6].SetActive(true);
                tutorialNumber = 6;
            }

            // Fourth side - Heat 
            else if (Input.GetKeyDown(KeyCode.Space) && tutorialNumber == 6)
            {
                TutorialOverlay.SetActive(false);
                showingTutorial = false;
            }

            return;
        }


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
        if (Input.GetButton("Fire1") && !UI.BuildingOpen && !UI.ResearchOpen && !UI.EngineerOpen && !UI.BossInfoOpen && !UI.UOLOpen&& Input.mousePosition.y >= 200)
        {
            if (showingTutorial)
            {
                if (tutorialNumber == 2)
                {
                    if (SelectedObj != CollectorObj)
                    {
                        return;
                    }
                }
                else if (tutorialNumber == 4)
                {
                    if (SelectedObj == TurretObj)
                    {
                        if (!(transform.position.x == -20 && transform.position.y == 35) &&
                            !(transform.position.x == 0 && transform.position.y == 35) &&
                            !(transform.position.x == 20 && transform.position.y == 35)) { return; }
                    }
                    else return;
                }
            }


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
                TileClass ObjectComponent = SelectedObj.GetComponent<TileClass>();

                bool validHeat = false;
                if (ObjectComponent.GetHeat() <= 0) validHeat = true;
                else if (Spawner.htrack < Spawner.maxHeat) validHeat = true;

                if (ObjectComponent.GetCost() <= gold && PowerConsumption + ObjectComponent.getConsumption() <= AvailablePower && validHeat)
                {
                    // Remove gold from player once confirmed
                    RemoveGold(ObjectComponent.GetCost());

                    // If it is a wall, dont use rotation
                    if (SelectedObj.name == "Wall")
                        LastObj = Instantiate(SelectedObj, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                    else LastObj = Instantiate(SelectedObj, transform.position, Quaternion.Euler(new Vector3(0, 0, rotation)));

                    // Create a UI resource popup thing idk lmaooo
                    UI.CreateResourcePopup("- " + ObjectComponent.GetCost(), "Gold", LastObj.position);

                    // Tutorial only
                    if (showingTutorial)
                    {
                        if (tutorialNumber == 2)
                        {
                            Debug.Log(tutorialCollectors);
                            tutorialCollectors++;
                            if (tutorialCollectors == 5)
                            {
                                Slides[2].SetActive(false);
                                Slides[3].SetActive(true);
                                tutorialNumber = 3;
                                tutorialCollectors = 0;
                            }
                        }
                        else if (tutorialNumber == 4 && tutorialCollectors != 3)
                        {
                            tutorialCollectors++;
                            if (tutorialCollectors == 3)
                            {
                                UI.Overlay.transform.Find("Selected").GetComponent<CanvasGroup>().alpha = 0;
                                Selected.sprite = null;
                                SelectedObj = null;
                                rotation = 0;
                                UI.DisableActiveInfo();
                                UI.ShowingInfo = false;
                                SelectedRadius.SetActive(false);

                                TutorialEnemies[0] = Instantiate(enemies[0], new Vector3(5, 85), Quaternion.Euler(new Vector3(0, 0, 0)));
                                TutorialEnemies[1] = Instantiate(enemies[0], new Vector3(10, 95), Quaternion.Euler(new Vector3(0, 0, 0)));
                                TutorialEnemies[2] = Instantiate(enemies[0], new Vector3(-5, 90), Quaternion.Euler(new Vector3(0, 0, 0)));

                                tutorialNumber = 5;
                                TutorialHolder.SetActive(false);
                            }
                        }
                    }

                    // Instantiate the price floating thing
                    // GameObject PriceHolder = Instantiate(popup, new Vector3(LastObj.position.x, LastObj.position.y + 5f, LastObj.position.z), Quaternion.Euler(new Vector3(0, 0, 0)));
                    // PriceHolder.GetComponent<Popup>().SetPopup("- " + ObjectComponent.GetCost());

                    // Set component values
                    LastObj.name = SelectedObj.name;
                    LastObj.GetComponent<TileClass>().IncreaseHealth();
                    increasePowerConsumption(LastObj.GetComponent<TileClass>().getConsumption());
                    Spawner.increaseHeat(LastObj.GetComponent<TileClass>().GetHeat());

                    // Play placement sound
                    float audioScale = cameraScroll.getZoom() / 1400f;
                    AudioSource.PlayClipAtPoint(placementSound, LastObj.transform.position, Settings.soundVolume - audioScale);
                }
                else
                {
                    if (!(ObjectComponent.GetCost() <= gold)) elements[0].startFlash();
                    else if (!(PowerConsumption + ObjectComponent.getConsumption() <= AvailablePower)) elements[1].startFlash();
                    else if (!validHeat) elements[2].startFlash();
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
                TileClass rayScript = rayHit.collider.gameObject.GetComponent<TileClass>();
                UI.ShowingInfo = false;
                SelectedOverlay.SetActive(false);
                int amount = rayScript.GetCost() - rayScript.GetCost() / 5;
                AddGold(amount);
                rayScript.DestroyTile();

                // Create a UI resource popup thing idk lmaooo
                UI.CreateResourcePopup("+ " + amount, "Gold", rayHit.collider.transform.position);
            }
        }

        // Check hotbar thing        
        CheckNumberInput();

        // Pipette a building
        if (Input.GetKeyDown(KeyCode.Mouse2) && !UI.BuildingOpen && !UI.MenuOpen && !UI.EngineerOpen && !UI.UOLOpen)
        {
            // Attempt a raycast on the tile survival is in
            RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

            // Set the selected object to the collider if not null
            if (rayHit.collider != null)
            {
                SelectObject(tech.FindTechBuilding(rayHit.collider.GetComponent<TileClass>().getID()));
                if (rayHit.collider.name != "Energizer") rayHit.collider.GetComponent<AnimateThenStop>().enabled = true;
                AudioSource.PlayClipAtPoint(pipetteSound, rayHit.collider.transform.position, Settings.soundVolume);
                UI.CreatePippeteSquare(rayHit.collider.transform.position);
            }
        }

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
        if (Input.GetKeyDown(KeyCode.E) && UI.BuildingOpen == false && UI.UOLOpen == false)
        {
            UI.OpenSurvivalMenu();
        }

        // If T pressed, open research menu
        else if (Input.GetKeyDown(KeyCode.T) && UI.MenuOpen == false && UI.ResearchOpen == false && UI.UOLOpen == false)
        {
            UI.closeResearchUnlock();
            if (UI.EngineerOpen)
                UI.CloseEngineer();
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
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) && UI.EngineerOpen == true)
        {
            UI.CloseEngineer();
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
        else if (Input.GetKeyDown(KeyCode.Escape) && SelectedObj != null)
        {
            UI.Overlay.transform.Find("Selected").GetComponent<CanvasGroup>().alpha = 0;
            Selected.sprite = null;
            SelectedObj = null;
            rotation = 0;
            UI.DisableActiveInfo();
            UI.ShowingInfo = false;
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

        // If escape pressed and menu not open, open it
        else if (Input.GetKeyDown(KeyCode.Escape) && UI.MenuOpen == false && UI.SettingsOpen == false && UI.UOLOpen == false)
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

    public void IncreaseTime()
    {
        Playtime++;
    }

    // Updates the gold storage
    public void UpdateGoldStorage(int amount)
    {
        goldStorage -= amount;
        if (gold > goldStorage)
            gold = goldStorage;
        UI.GoldStorage.text = goldStorage + " MAX";
        UI.GoldAmount.text = gold.ToString();
    }

    // Updates the essence storage
    public void UpdateEssenceStorage(int amount)
    {
        essenceStorage -= amount;
        if (essence > essenceStorage)
            essence = essenceStorage;
        UI.EssenceStorage.text = essenceStorage + " MAX";
        UI.EssenceAmount.text = essence.ToString();
    }

    // Updates the iridium storage
    public void UpdateIridiumStorage(int amount)
    {
        iridiumStorage -= amount;
        if (iridium > iridiumStorage)
            iridium = iridiumStorage;
        UI.IridiumStorage.text = iridiumStorage + " MAX";
        UI.IridiumAmount.text = iridium.ToString();
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
            if (building == null) continue;

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
            try { obj.GetComponent<AnimateThenStop>().animEnabled = false; } catch { }

            // Resource offset
            if (building.name == "Collector") StartCoroutine(obj.GetComponent<CollectorAI>().OffsetStart());
            else if (building.name == "Essence Drill") StartCoroutine(obj.GetComponent<EssenceAI>().OffsetStart());
            else if (building.name == "Iridium Mine") StartCoroutine(obj.GetComponent<IridiumAI>().OffsetStart());

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

            //Debug.Log("Placed " + obj.name + " at " + a[i, 2] + " " + a[i, 3]);
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
        hotbar[3] = GoldStorage;
        UI.UpdateHotbar();
    }

    // Select object on hotbar
    public void SelectHotbar(int index)
    {
        if (showingTutorial && tutorialNumber != 2 && tutorialNumber != 4) return;

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
        catch { SelectedObj = holder; }
    }

    // Coming soon to a Vectorio near you
    public void SelectTurret(int a)
    {

    }

    public void SelectBuilding(int a)
    {

    }

    public void SelectSpecial(int a)
    {

    }


    // Changes the object that the player has selected (pass null to deselect)
    public void SelectObject(Transform obj)
    {
        if (showingTutorial && tutorialNumber != 2 && tutorialNumber != 4) return;

        // Disable selected overlay
        UI.ShowingInfo = false;
        PromptOverlay.alpha = 0;
        SelectedOverlay.SetActive(false);

        SelectedObj = obj;
        if (obj != null && !tech.checkIfUnlocked(obj)) return;
        SwitchObj();
        if (UI.BuildingOpen)
        {
            UI.BuildingOpen = false;
            UI.SetOverlayStatus("Survival Menu", false);
        }
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
            SquareRadius.SetActive(false);
        }
        else if (SelectedObj.name == "Enhancer" || SelectedObj.name == "Engineer")
        {
            SquareRadius.SetActive(true);
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
            SaveSystem.SaveGame(this, tech, Spawner, rsrch, difficulties, Playtime, Spawner.htrack, difficulties.GetGroupSpawns());
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