using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    // Active instance (temporary)
    public static Panel active;
    public Armory armory;

    // List of stats
    private List<MenuStat> menuObjects;
    private List<MenuStat> unusedObjects;

    // Panel variables
    public CanvasGroup canvasGroup;
    public MenuStat menuStat;
    public Transform resourceStats;
    public Transform buildingStats;
    public bool settingHotbar;

    // Panel UI variables
    public new TextMeshProUGUI name;
    public TextMeshProUGUI levelFront;
    public TextMeshProUGUI levelBack;
    public TextMeshProUGUI desc;
    public ButtonManagerBasic hotbar;
    public Image icon;

    // Selection objects
    public HorizontalSelector variantSelector;
    public GameObject configSelector;

    // Building
    public Buildable buildable;
    public Entity entity;

    // Active instance (temporary solution to engineering problem)
    public void Awake() { active = this; }

    public void Start()
    {
        UIEvents.active.onEntityPressed += SetPanel;
        UIEvents.active.onBuildablePressed += SetPanel;

        menuObjects = new List<MenuStat>();
        unusedObjects = new List<MenuStat>();
    }

    // Open the armory
    public void ToggleArmory(bool toggle)
    {
        armory.gameObject.SetActive(toggle);

        if (toggle)
        {
            armory.SetupArmory(buildable);
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
        else
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
    }

    // Applies a blueprint to the active entity
    public bool ApplyBlueprint(CollectedBlueprint blueprint)
    {
        if (entity != null)
        {
            Buildable buildable = Buildables.RequestBuildable(entity);
            return buildable.ApplyBlueprint(blueprint);
        }
        return false;
    }

    // Sets the panel information based on entity data
    public void SetPanel(Entity entity, int metadata = -1)
    {
        // Toggle armory off
        ToggleArmory(false);

        // Grab entity
        this.entity = entity;

        // Try get buildable
        if (entity != null)
            buildable = Buildables.RequestBuildable(entity);

        // Set panel description
        name.text = entity.name.ToUpper();
        desc.text = entity.description;
        icon.sprite = Sprites.GetSprite(entity.name);
        
        // Create stats for the building
        SetUnused();
        entity.CreateStats(this);
        ResetUnused();
    }

    // Sets the panel information based on buildable data
    public void SetPanel(Buildable buildable, int metadata = -1)
    {
        // Toggle armory off
        ToggleArmory(false);

        // Grab entity
        entity = buildable.building;

        // Try get buildable
        this.buildable = buildable;

        // Set panel description
        if (buildable.cosmetic == null)
        {
            name.text = buildable.building.name.ToUpper();
            desc.text = buildable.building.description;
            icon.sprite = Sprites.GetSprite(entity.name);
        }
        else
        {
            name.text = buildable.cosmetic.name.ToUpper();
            desc.text = buildable.cosmetic.description;
            icon.sprite = buildable.cosmetic.hologram;
        }

        // Set the level
        levelFront.text = "LVL " + buildable.level;
        levelBack.text = levelFront.text;

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

        // Calculate modifier value
        float value = stat.GetModifier();

        // Set modifier string if not 0
        if (value > 0)
            modifier = "<color=green>(+" + stat.modifier + ")";
        else if (value < 0)
            modifier = "<color=red>(+" + stat.modifier + ")";

        // Set the values
        holder.text.text = "<b>" + stat.name + ":</b> " + (stat.value + stat.modifier) + " " + modifier;
        holder.icon.sprite = stat.icon;
    }

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
