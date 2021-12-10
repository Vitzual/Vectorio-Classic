using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survival : Gamemode
{
    // Hub object
    public Building hub;

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

        // Check stage variable
        if (stage == null)
        {
            Debug.Log("Stage data missing. Setting to default");
            stage = _stage;
        }

        // Setup hub
        Buildable hubBuildable = Buildables.RequestBuildable(hub);
        InstantiationHandler.active.RpcInstantiateBuilding(hubBuildable, Vector2.zero, Quaternion.identity, -1, -1);

        // Initialize gamemode
        InitGamemode();

        if (NewSaveSystem.loadGame && NewSaveSystem.saveData != null)
        {
            Resource.storages = new List<DefaultStorage>();
            NewSaveSystem.LoadGame();
            Border.UpdateStage();
            Events.active.ChangeBorderColor(stage.borderOutline, stage.borderFill);
        }
        else ResearchUI.active.Setup();

        // Setup starting resources
        SetupStartingResources();

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
        InstantiationHandler.active.RpcInstantiateBuilding(hubBuildable, Vector2.zero, Quaternion.identity, -1, -1);

        // Initialize gamemode
        InitGamemode();

        if (NewSaveSystem.loadGame && NewSaveSystem.saveData != null)
        {
            Resource.storages = new List<DefaultStorage>();
            NewSaveSystem.LoadGame();
            Border.UpdateStage();
            Events.active.ChangeBorderColor(stage.borderOutline, stage.borderFill);
        }
        else ResearchUI.active.Setup();

        // Setup starting resources
        SetupStartingResources();

        NewSaveSystem.loadGame = false;

        // Invoke auto saving
        InvokeRepeating("AutoSave", 360f, 360f);
    }
}
