using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    // List of stats
    private List<MenuStat> menuObjects;
    private List<MenuStat> unusedObjects;

    // Panel variables
    public MenuStat menuStat;
    public Transform resourceStats;
    public Transform buildingStats;
    public bool settingHotbar;

    // Panel UI variables
    public new TextMeshProUGUI name;
    public TextMeshProUGUI desc;
    public ButtonManagerBasic hotbar;
    public Image icon;

    // Selection objects
    public HorizontalSelector variantSelector;
    public GameObject configSelector;

    // Building
    public Entity entity;

    public void Start()
    {
        UIEvents.active.onEntityPressed += SetPanel;
        UIEvents.active.onDisableHotbar += DisableHotbar;
        //Events.active.onVariantLoaded += AddVariant;

        menuObjects = new List<MenuStat>();
        unusedObjects = new List<MenuStat>();
    }

    // Toggles the hotbar
    public void ToggleHotbar()
    {
        if (settingHotbar) DisableHotbar();
        else EnableHotbar();
    }

    // Enables setting hotbar
    public void EnableHotbar()
    {
        hotbar.buttonText = "PRESS NUMBER (1-9)";
        hotbar.UpdateUI();
        settingHotbar = true;
    }

    // Disables setting hotbar
    public void DisableHotbar()
    {
        hotbar.buttonText = "ASSIGN TO HOTBAR";
        hotbar.UpdateUI();
        settingHotbar = false;
    }

    // Sets the panel information
    public void SetPanel(Entity entity, bool isEnemy)
    {
        // Toggle variant selection
        variantSelector.gameObject.SetActive(isEnemy);

        // Grab building
        this.entity = entity;

        // Set panel description
        name.text = entity.name.ToUpper();
        desc.text = entity.description;
        icon.sprite = Sprites.GetSprite(entity.name);

        // Create stats for the building
        SetUnused();
        entity.CreateStats(this);
        ResetUnused();
    }

    // Reset unused objects
    public void SetUnused()
    {
        unusedObjects = new List<MenuStat>(menuObjects);
    }

    // Creates a menu stat
    public void CreateStat(Stat stat)
    {
        // Already created
        MenuStat holder = null;
        bool isInstantiated = false;

        // Check previous stats to see if it exists
        foreach(MenuStat previousStat in menuObjects)
        {
            if (previousStat.name == stat.name)
            {
                unusedObjects.Remove(previousStat);
                holder = previousStat;
                isInstantiated = true;
                break;
            }
        }

        // Create object
        if (!isInstantiated)
        {
            // Instantiate the object
            GameObject obj = Instantiate(menuStat.obj, new Vector3(0, 0, 0), Quaternion.identity);
            obj.name = stat.name;
            
            // Add to list
            holder = obj.GetComponent<MenuStat>();
            menuObjects.Add(holder);
            holder.name = stat.name;

            // Set parent transform
            if (stat.isResource)
                obj.transform.SetParent(resourceStats);
            else
                obj.transform.SetParent(buildingStats);

            // Adjust size
            RectTransform temp = obj.GetComponent<RectTransform>();
            if (temp != null) temp.localScale = new Vector3(1, 1, 1);
        }

        // Create modifier variable
        string modifier = "";

        // Set modifier string if not 0
        if (stat.modifier > 0)
            modifier = "<color=green>(+" + stat.modifier + ")";
        else if (stat.modifier < 0)
            modifier = "<color=red>(+" + stat.modifier + ")";

        // Set the values
        holder.text.text = "<b>" + stat.name + ":</b> " + (stat.value + stat.modifier) + " " + modifier;
        holder.icon.sprite = stat.icon;
    }

    /* Adds a variant to the variant list
    public void AddVariant(Variant variant)
    {
        variantSelector.CreateNewItem(variant.name);
        variantSelector.SetupSelector();
    }
    */

    // Resets all the menu stats
    public void ResetUnused()
    {
        foreach (MenuStat stat in unusedObjects)
        {
            menuObjects.Remove(stat);
            Recycler.AddRecyclable(stat.transform);
        }
        unusedObjects.Clear();
    }
}
