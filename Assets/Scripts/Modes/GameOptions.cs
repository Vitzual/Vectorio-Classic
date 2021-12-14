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
    //public HorizontalSelector presets;

    // Default threshold division
    public float resourceDivider = 2500f;

    // Difficulty values
    public TMP_InputField seedInput;
    public new TextMeshProUGUI name;
    public TextMeshProUGUI seed;

    // Gameplay modifiers
    public Toggle useDroneConstruction;
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

    // Set random seed on start
    public void Start()
    {
        SetRandomSeed();
    }

    // Start thing
    public void SetDifficulty(Difficulty difficulty)
    {
        // Gameplay modifiers
        useDroneConstruction.isOn = difficulty.useDroneConstruction;
        naturalHeatGrowth.isOn = difficulty.naturalHeatGrowth;

        // Gameplay settings
        enemySpawnrateModifier.mainSlider.value = difficulty.enemySpawnrateModifier * 100;
        enemyHealthModifier.mainSlider.value = difficulty.enemyHealthModifier * 100;
        enemySpeedModifier.mainSlider.value = difficulty.enemySpeedModifier * 100;
        enemyGroupSpawnrate.mainSlider.value = difficulty.enemyGroupSpawnrate * 100;
        enemyGroupSpawnsize.mainSlider.value = difficulty.enemyGroupSpawnsize * 100;

        // Difficulty modifiers
        goldSpawnModifier.mainSlider.value = difficulty.goldSpawnModifier * 100;
        essenceSpawnModifier.mainSlider.value = difficulty.essenceSpawnModifier * 100;
        iridiumSpawnModifier.mainSlider.value = difficulty.iridiumSpawnModifier * 100;
        vectoriumSpawnModifier.mainSlider.value = difficulty.vectoriumSpawnModifier * 100;

        // Update all UI elements
        enemySpawnrateModifier.UpdateUI();
        enemyHealthModifier.UpdateUI();
        enemySpeedModifier.UpdateUI();
        enemyGroupSpawnrate.UpdateUI();
        enemyGroupSpawnsize.UpdateUI();
        goldSpawnModifier.UpdateUI();
        essenceSpawnModifier.UpdateUI();
        iridiumSpawnModifier.UpdateUI();
        vectoriumSpawnModifier.UpdateUI();
    }

    // Set random seed
    public void SetRandomSeed()
    {
        string random = Random.Range(100000000, 999999999).ToString();
        seedInput.text = random;
    }

    // Set difficulty data
    public void CreateGame()
    {
        // Set save name
        if (name.text == "") name.text = "Unnamed Save";
        NewSaveSystem.saveName = name.text;

        // Set save seed
        Gamemode.seed = seed.text;

        // Create data
        DifficultyData difficultyData = new DifficultyData();
        OnlineData onlineData = new OnlineData();

        // Gameplay modifiers
        difficultyData.useDroneConstruction = useDroneConstruction.isOn;
        difficultyData.naturalHeatGrowth = naturalHeatGrowth.isOn;

        // Online settings
        int maxPlayers = 1;
        if (enableOnlineGame.isOn) maxPlayers = 10;
        onlineData.maxConnections = maxPlayers;
        onlineData.listAsLobby = false;
        onlineData.privateSession = false;

        // Gameplay settings
        difficultyData.enemySpawnrateModifier = enemySpawnrateModifier.mainSlider.value / 100;
        difficultyData.enemyHealthModifier = enemyHealthModifier.mainSlider.value / 100;
        difficultyData.enemySpeedModifier = enemySpeedModifier.mainSlider.value / 100;
        difficultyData.enemyGroupSpawnrate = enemyGroupSpawnrate.mainSlider.value / 100;
        difficultyData.enemyGroupSpawnsize = enemyGroupSpawnsize.mainSlider.value / 100;

        // Difficulty modifiers
        difficultyData.goldSpawnModifier = goldSpawnModifier.mainSlider.value / resourceDivider;
        difficultyData.essenceSpawnModifier = essenceSpawnModifier.mainSlider.value / resourceDivider;
        difficultyData.iridiumSpawnModifier = iridiumSpawnModifier.mainSlider.value / resourceDivider;
        difficultyData.vectoriumSpawnModifier = vectoriumSpawnModifier.mainSlider.value / resourceDivider;

        // Set gamemode and start
        Gamemode.difficulty = difficultyData;
        Gamemode.online = onlineData;
        Menu.active.StartSurvivalGame(maxPlayers);
    }
}
