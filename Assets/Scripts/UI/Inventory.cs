using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class Inventory : MonoBehaviour
{
    // Inventory variables
    public static Inventory active;
    public MenuButton buildable;
    public List<Transform> lists;

    public static CanvasGroup canvasGroup;
    public static bool isOpen = false;

    public void Awake()
    {
        active = this;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void GenerateEntities(Entity[] entities)
    {
        // Create a new array of holders
        MenuButton[] holders = new MenuButton[entities.Length];

        // Generate buildables
        for(int i = 0; i < entities.Length; i++)
        {
            if (!entities[i].display) return;
            int index = (int)entities[i].inventoryHeader;

            if (index >= 0 && index < lists.Count)
            {
                MenuButton holder = CreateBuildable(entities[i], lists[index]);
                if (holder != null) holders[i] = holder;
                else Debug.Log("Entity " + entities[i].name + "could not be created!");
            }
        }

        // Set order of buildables
        for(int i = 0; i < holders.Length; i++)
        {
            if(holders[i].transform != null)
                holders[i].transform.SetSiblingIndex(holders[i].entity.inventoryIndex);
        }
    }

    public void GenerateBuildings(Building[] buildings)
    {
        // Create a new array of holders
        MenuButton[] holders = new MenuButton[buildings.Length];

        // Generate buildables
        for (int i = 0; i < buildings.Length; i++)
        {
            int index = (int)buildings[i].inventoryHeader;

            if (index >= 0 && index < lists.Count)
            {
                MenuButton holder = CreateBuildable(buildings[i], lists[index]);
                if (holder != null) holders[i] = holder;
                else Debug.Log("Entity " + buildings[i].name + "could not be created!");
            }
        }

        // Set order of buildables twice
        for (int i = 0; i < holders.Length; i++)
        {
            if (holders[i].transform != null)
                holders[i].transform.SetSiblingIndex(holders[i].buildable.building.inventoryIndex);
        }
        for (int i = 0; i < holders.Length; i++)
        {
            if (holders[i].transform != null)
                holders[i].transform.SetSiblingIndex(holders[i].buildable.building.inventoryIndex);
        }
    }

    public MenuButton CreateBuildable(Entity entity, Transform list)
    {
        // Create the new buildable object
        GameObject holder = Instantiate(this.buildable.gameObject, new Vector3(0, 0, 0), Quaternion.identity);

        // Set parent
        holder.transform.SetParent(list);
        holder.transform.name = entity.name;

        // Adjust size
        RectTransform temp = holder.GetComponent<RectTransform>();
        if (temp != null) temp.localScale = new Vector3(1, 1, 1);

        // Set buildable values and retrieve buildable
        MenuButton container = holder.GetComponent<MenuButton>();
        Buildable buildable = Buildables.RequestBuildable(entity);
        if (buildable != null)
        {
            container.SetBuilding(buildable);
            buildable.button = container;
        }
        else container.SetEntity(entity);

        return container;
    }

    // Close UI
    public static void Open()
    {
        isOpen = true;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    // Close UI
    public static void CloseMenu()
    {
        isOpen = false;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
