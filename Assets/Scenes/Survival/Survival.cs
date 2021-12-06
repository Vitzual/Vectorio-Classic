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
        // Generate all scriptables
        ScriptableLoader.GenerateAllScriptables();
        LowresMap.active.Setup();

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
        InstantiationHandler.active.RpcInstantiateBuilding(hubBuildable, Vector2.zero, Quaternion.identity);

        // Initialize gamemode
        InitGamemode();

        // Load game 
        try
        {
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
        }
        catch { Debug.Log("Game ran into problem while loading!"); }

        NewSaveSystem.loadGame = false;

        // Invoke auto saving
        InvokeRepeating("AutoSave", 360f, 360f);
    }
}
