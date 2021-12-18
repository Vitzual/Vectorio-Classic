using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survival : Gamemode
{
    // Hub object
    public Building hub;
    public GameObject welcome;
    public Hotbar hotbarWindow;

    // TEMP FOR BUG
    public Stage _TEMP_BUGFIX_STAGE;
    public Entity _TEMP_BUGFIX_TRIDENT;

    // Instantiate hub
    public override void Setup()
    {
        // Debug
        Debug.Log("[SURVIVAL] Setting up game!");

        // Check difficulty variable
        if (difficulty == null)
        {
            Debug.Log("Difficulty data missing. Creating new one");
            difficulty = _difficulty.SetData(new DifficultyData());
        }

        // Check difficulty variable
        if (online == null)
        {
            Debug.Log("Difficulty data missing. Creating new one");
            online = new OnlineData();
            online.maxConnections = 10;
            online.listAsLobby = false;
            online.privateSession = false;
            NetworkManagerSF.active.maxConnections = Gamemode.online.maxConnections;
        }

        // Check stage variable
        if (stage == null)
        {
            Debug.Log("Stage data missing. Setting to default");
            stage = _stage;
        }

        // Set max connections
        NetworkManagerSF.active.maxConnections = online.maxConnections;

        // Setup hub
        Buildable hubBuildable = Buildables.RequestBuildable(hub);
        InstantiationHandler.active.RpcInstantiateBuilding(hubBuildable, "", Vector2.zero, Quaternion.identity, -1, -1);

        // Initialize gamemode
        InitGamemode();

        if (NewSaveSystem.loadGame && NewSaveSystem.saveData != null)
        {
            // Load game
            Resource.storages = new List<DefaultStorage>();
            NewSaveSystem.LoadGame();
            Border.UpdateStage();
            Events.active.ChangeBorderColor(stage.borderOutline, stage.borderFill);

            // Check for guardian mission bug
            if (stage.heat > _TEMP_BUGFIX_STAGE.heat)
                Buildables.UnlockBuildable(Buildables.RequestBuildable(_TEMP_BUGFIX_TRIDENT));
        }
        else
        {
            welcome.SetActive(true);
            ResearchUI.active.Setup();

            if (hotbarWindow != null)
                hotbarWindow.SetDefaultSlots();
        }

        // Set difficulty stuff
        Events.active.SetEnemyDifficulty(difficulty.enemyGroupSpawnrate, difficulty.enemyGroupSpawnsize);

        // Setup starting resources
        SetupStartingResources();

        // Stop loading game
        NewSaveSystem.loadGame = false;

        // Invoke auto saving
        InvokeRepeating("AutoSave", 360f, 360f);
    }

    // Instantiate hub
    public override void SyncSetup()
    {
        // Debug
        Debug.Log("[SURVIVAL] Syncing new client to game!");

        // Check difficulty variable
        if (difficulty == null)
        {
            Debug.Log("Difficulty data missing. Creating new one");
            difficulty = _difficulty.SetData(new DifficultyData());
        }

        // Check stage variable
        if (stage == null)
        {
            Debug.Log("Stage data missing. Setting to default");
            stage = _stage;
        }

        // Setup hub
        Buildable hubBuildable = Buildables.RequestBuildable(hub);
        InstantiationHandler.active.RpcInstantiateBuilding(hubBuildable, "", Vector2.zero, Quaternion.identity, -1, -1);

        // Initialize gamemode
        InitGamemode();
        Resource.storages = new List<DefaultStorage>();
        ResearchUI.active.Setup();

        // Invoke auto saving
        // InvokeRepeating("AutoSave", 360f, 360f);
    }

    // Calls upper class update methods
    public override void UpdateMethod()
    {
        // Increment time
        time += Time.deltaTime;

        // Check heat growth
        if (naturalHeatGrowth)
        {
            naturalHeatTimer -= Time.deltaTime;
            if (naturalHeatTimer <= 0)
            {
                Resource.active.Apply(Resource.CurrencyType.Heat, 1, false);
                difficulty.heatTracked += 1;
                naturalHeatTimer = 5f;
            }
        }
    }
}
