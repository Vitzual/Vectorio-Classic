using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using TMPro;

public class MenuButton : MonoBehaviour
{
    // Building holder
    public Entity entity;
    public Buildable buildable;

    // Button variables
    public TextMeshProUGUI desc;
    public Image icon;

    // Resource icons
    public GameObject iridiumIcon;
    public GameObject essenceIcon;
    public GameObject goldIcon;
    public GameObject heatIcon;
    public GameObject powerIcon;

    // Progress
    public ProgressBar progress;
    
    // Set entity
    public void SetEntity(Entity entity)
    {
        // Set scriptables
        this.entity = entity;
        buildable = null;

        // Determine if building is unlocked
        SetVariables(entity);
    }

    // Set building
    public void SetBuilding(Buildable buildable)
    {
        // Set scriptables
        entity = null;
        this.buildable = buildable;

        // Determine if building is unlocked
        SetVariables(buildable.building);
        
        // Set resources (if unlocked)
        if (buildable.isUnlocked)
        {
            foreach (Cost resource in buildable.resources)
            {
                if (resource.storage) continue;

                if (resource.type == Resource.CurrencyType.Power)
                    powerIcon.SetActive(true);
                else if (resource.type == Resource.CurrencyType.Heat)
                    heatIcon.SetActive(true);
                else if (resource.type == Resource.CurrencyType.Gold)
                    goldIcon.SetActive(true);
                else if (resource.type == Resource.CurrencyType.Essence)
                    essenceIcon.SetActive(true);
                else if (resource.type == Resource.CurrencyType.Iridium)
                    iridiumIcon.SetActive(true);
            }
        }

        // Get active events
        Events.active.onBuildingPlaced += IncreaseBuildingPlaced;
        Events.active.onBuildingDestroyed += DecreasedBuildingPlaced;
    }

    // Set vairable stats
    public void SetVariables(Entity entity)
    {
        if (buildable != null && buildable.isUnlocked)
        {
            ButtonManagerBasic button = GetComponent<ButtonManagerBasic>();
            button.buttonText = entity.name.ToUpper();
            button.UpdateUI();
            desc.text = "<b>" + buildable.tracked + " ACTIVE |</b> <size=16>Click for more details!";
            icon.sprite = Sprites.GetSprite(entity.name);
            progress.gameObject.SetActive(false);
        }
        else
        {
            GetComponent<ButtonManagerBasic>().buttonText = "LOCKED";
            desc.text = entity.unlockable.description;
            icon.sprite = Sprites.GetSprite("Locked");
        }
    }

    // Update button
    public void IncreaseBuildingPlaced(BaseTile tile)
    {
        if (tile.buildable != null && buildable != null && tile.buildable == buildable)
        {
            tile.buildable.UpdateActiveAmount(true, tile.transform.position);
            desc.text = "<b>" + tile.buildable.tracked + " ACTIVE |</b> <size=16>Click for more details!";
        }
    }

    // Update button
    public void DecreasedBuildingPlaced(BaseTile tile)
    {
        if (tile.buildable != null && buildable != null && tile.buildable == buildable)
        {
            tile.buildable.UpdateActiveAmount(false, tile.transform.position);
            desc.text = "<b>" + tile.buildable.tracked + " ACTIVE |</b> <size=16>Click for more details!";
        }
    }

    // Show stats
    public void DisplayStats()
    {
        if (buildable.isUnlocked)
        {
            if (entity != null) UIEvents.active.EntityPressed(entity, -1);
            else if (buildable != null) UIEvents.active.BuildablePressed(buildable, -1);
        }
    }
}
