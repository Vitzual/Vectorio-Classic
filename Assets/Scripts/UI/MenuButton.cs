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
    public Building building;

    // Button variables
    public bool unlocked;
    public TextMeshProUGUI desc;
    public Image icon;

    // Resource icons
    public GameObject iridiumIcon;
    public GameObject essenceIcon;
    public GameObject goldIcon;
    public GameObject heatIcon;
    public GameObject powerIcon;

    // Set entity
    public void SetEntity(Entity entity)
    {
        // Set scriptables
        this.entity = entity;
        building = null;

        // Determine if building is unlocked
        unlocked = Gamemode.active.unlockEverything || entity.unlockable.unlockedByDefault;
        SetVariables(entity);
    }

    // Set building
    public void SetBuilding(Building building)
    {
        // Set scriptables
        entity = null;
        this.building = building;

        // Determine if building is unlocked
        unlocked = Gamemode.active.unlockEverything || entity.unlockable.unlockedByDefault;
        SetVariables(building);

        // Set resources (if unlocked)
        if (unlocked)
        {
            foreach (Building.Resources resource in building.resources)
            {
                if (resource.resource == Resource.CurrencyType.Power)
                    powerIcon.SetActive(true);
                else if (resource.resource == Resource.CurrencyType.Heat)
                    heatIcon.SetActive(true);
                else if (resource.resource == Resource.CurrencyType.Gold)
                    goldIcon.SetActive(true);
                else if (resource.resource == Resource.CurrencyType.Essence)
                    essenceIcon.SetActive(true);
                else if (resource.resource == Resource.CurrencyType.Iridium)
                    iridiumIcon.SetActive(true);
            }
        }
    }

    // Set vairable stats
    public void SetVariables(Entity entity)
    {
        if (unlocked)
        {
            GetComponent<ButtonManagerBasic>().buttonText = entity.name;
            desc.text = "<b>" + 0 + " ACTIVE |</b> <size=16>Click for more details!";
            icon.sprite = Sprites.GetSprite(entity.name);
        }
        else
        {
            GetComponent<ButtonManagerBasic>().buttonText = "LOCKED";
            desc.text = "<size=16>" + entity.unlockable.unlockDescription;
            icon.sprite = Sprites.GetSprite("Locked");
        }
    }


    // Show stats
    public void DisplayStats()
    {
        if (unlocked)
        {
            if (entity != null) UIEvents.active.EntityPressed(entity);
            else if (building != null) UIEvents.active.BuildingPressed(building);
        }
    }
}
