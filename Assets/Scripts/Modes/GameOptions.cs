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

    // Resource modifiers
    public SliderManager goldSpawnModifier;
    public SliderManager essenceSpawnModifier;
    public SliderManager iridiumSpawnModifier;

    // Start resources
    public SliderManager startingGold;
    public SliderManager startingPower;

    // Cost modifiers
    public SliderManager buildingCostModifier;
    public SliderManager buildingHealthModifier;
    public SliderManager buildingDamageModifier;

    // Enemy modifiers
    public SliderManager enemyHealthModifier;
    public SliderManager enemyDamageModifier;
    public SliderManager enemySpeedModifier;
    public SliderManager enemySpawnrateModifier;

    // Start thing
    public void GetDifficulties()
    {
        List<Difficulty> difficulties = Resources.LoadAll("Scriptables/Difficulties", typeof(Difficulty)).Cast<Difficulty>().ToList();

        foreach(Difficulty difficulty in difficulties)
        {
            presets.CreateNewItem(difficulty.name);
            presets.UpdateUI();
        }
    }

    // Set difficulty data
    public void CreateGame()
    {
        NewSaveSystem.saveName = name.text;

        if (mode != "Creative")
        {
            Gamemode.seed = seed.text;

            DifficultyData data = new DifficultyData();

            data.enableInstaPlace = enableInstaPlace.isOn;
            data.naturalHeatGrowth = naturalHeatGrowth.isOn;

            data.goldSpawnModifier = goldSpawnModifier.mainSlider.value;
            data.essenceSpawnModifier = essenceSpawnModifier.mainSlider.value;
            data.iridiumSpawnModifier = iridiumSpawnModifier.mainSlider.value;

            data.startingGold = (int)startingGold.mainSlider.value;
            data.startingPower = (int)startingPower.mainSlider.value;

            data.buildingCostModifier = buildingCostModifier.mainSlider.value;
            data.buildingHealthModifier = buildingHealthModifier.mainSlider.value;
            data.buildingDamageModifier = buildingDamageModifier.mainSlider.value;

            data.enemyHealthModifier = enemyHealthModifier.mainSlider.value;
            data.enemyDamageModifier = enemyDamageModifier.mainSlider.value;
            data.enemySpeedModifier = enemySpeedModifier.mainSlider.value;
            data.enemySpawnrateModifier = enemySpawnrateModifier.mainSlider.value;

            Gamemode.difficulty = data;
        }

        Menu.active.StartGame(mode);
    }
}
