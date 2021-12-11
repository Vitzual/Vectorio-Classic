using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    // Mode name
    public string mode;

    // Presets
    public HorizontalSelector presets;

    // Difficulty values
    public new TextMeshProUGUI name;
    public TextMeshProUGUI seed;

    // Gameplay modifiers
    public Toggle enableInstaPlace;
    public Toggle naturalHeatGrowth;

    // Online modifiers
    public Toggle enableOnlineGame;

    // Enemy modifiers
    public SliderManager enemySpawnrateModifier;
    public SliderManager enemyHealthModifier;
    public SliderManager enemySpeedModifier;
    public SliderManager enemyGroupSpawnrate;
    public SliderManager enemyGroupSpawnsize;

    // Resource modifiers
    public SliderManager goldSpawnModifier;
    public SliderManager essenceSpawnModifier;
    public SliderManager iridiumSpawnModifier;
    public SliderManager vectoriumSpawnModifier;

    // Start thing
    public void GetDifficulties()
    {
        //List<Difficulty> difficulties = Resources.LoadAll("Scriptables/Difficulties", typeof(Difficulty)).Cast<Difficulty>().ToList();
        //
        //foreach(Difficulty difficulty in difficulties)
        //{
        //    presets.CreateNewItem(difficulty.name);
        //    presets.UpdateUI();
        //}
    }

    // Set difficulty data
    public void CreateGame()
    {
        // Set save name
        if (name.text == "") name.text = "Unnamed Save";
        NewSaveSystem.saveName = name.text;

        // Set save seed
        if (seed.text == "") seed.text = Random.Range(100000000, 999999999).ToString();
        Gamemode.seed = seed.text;

        // Create data
        DifficultyData difficultyData = new DifficultyData();
        OnlineData onlineData = new OnlineData();

        // Gameplay modifiers
        difficultyData.enableInstaPlace = enableInstaPlace.isOn;
        difficultyData.naturalHeatGrowth = naturalHeatGrowth.isOn;

        // Online settings
        int maxPlayers = 1;
        if (enableOnlineGame.isOn) maxPlayers = 10;
        onlineData.maxConnections = maxPlayers;
        onlineData.listAsLobby = false;
        onlineData.privateSession = false;

        // Gameplay settings
        difficultyData.enemySpawnrateModifier = enemySpawnrateModifier.mainSlider.value;
        difficultyData.enemyHealthModifier = enemyHealthModifier.mainSlider.value;
        difficultyData.enemySpeedModifier = enemySpeedModifier.mainSlider.value;
        difficultyData.enemyGroupSpawnrate = enemyGroupSpawnrate.mainSlider.value;
        difficultyData.enemyGroupSpawnsize = enemyGroupSpawnsize.mainSlider.value;

        // Difficulty modifiers
        difficultyData.goldSpawnModifier = goldSpawnModifier.mainSlider.value;
        difficultyData.essenceSpawnModifier = essenceSpawnModifier.mainSlider.value;
        difficultyData.iridiumSpawnModifier = iridiumSpawnModifier.mainSlider.value;
        difficultyData.vectoriumSpawnModifier = vectoriumSpawnModifier.mainSlider.value;

        // Set gamemode and start
        Gamemode.difficulty = difficultyData;
        Gamemode.online = onlineData;
        Menu.active.StartSurvivalGame(maxPlayers);
    }
}
