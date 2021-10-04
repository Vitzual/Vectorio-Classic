using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class Inventory : MonoBehaviour
{
    public MenuButton buildable;
    public List<Transform> lists;

    public void Start()
    {
        Events.active.initBuildables += GenerateEntities;
        gameObject.SetActive(false);
    }

    public void GenerateVariants(string path)
    {
        List<Variant> entities = Resources.LoadAll(path, typeof(Variant)).Cast<Variant>().ToList();
        Debug.Log("Loaded " + entities.Count + " variants from " + path);
    }

    public void GenerateEntities(string path)
    {
        List<Entity> entities = Resources.LoadAll(path, typeof(Entity)).Cast<Entity>().ToList();
        Debug.Log("Loaded " + entities.Count + " entities from " + path);
        MenuButton[] holders = new MenuButton[entities.Count];

        // Generate buildables
        for(int i = 0; i < entities.Count; i++)
        {
            Debug.Log("Setting up " + entities[i].name);

            if (entities[i].invIndex >= 0 && entities[i].invIndex < lists.Count)
            {
                Debug.Log("Creating entity at " + entities[i].invIndex + " " + entities[i].invOrder);

                MenuButton holder = CreateEntity(entities[i], lists[entities[i].invIndex]);
                if (holder != null) holders[i] = holder;
                else Debug.Log("Error");
            }
        }

        // Set order of buildables
        for(int i = 0; i < holders.Length; i++)
        {
            if(holders[i].transform != null)
                holders[i].transform.SetSiblingIndex(holders[i].entity.invOrder);
        }
    }

    public MenuButton CreateEntity(Entity entity, Transform list, List<Building.Resources> resources = null)
    {
        // Create the new buildable object
        GameObject holder = Instantiate(buildable.obj, new Vector3(0, 0, 0), Quaternion.identity);

        // Set parent
        holder.transform.SetParent(list);
        holder.transform.name = entity.name;

        // Adjust size
        RectTransform temp = holder.GetComponent<RectTransform>();
        if (temp != null) temp.localScale = new Vector3(1, 1, 1);

        // Set buildable values
        buildable = holder.GetComponent<MenuButton>();
        buildable.entity = entity;

        // Set building level
        if (entity.level > 0)
            buildable.button.buttonText = "<b>" + entity.name.ToUpper() + "</b><size=20> LEVEL " + entity.level;
        else
        {
            buildable.button.buttonText = "<b>" + entity.name.ToUpper() + "</b>";
        }

        // Set building icon
        buildable.icon.sprite = Sprites.GetSprite(entity.name);

        // Set building unlock value
        buildable.desc.text = "<b>" + entity.active + " ACTIVE |</b> <size=16>Click for more details!";

        // Add resource icons
        if (resources != null) {
            foreach (Building.Resources resource in resources)
            {
                if (resource.amount > 0)
                {
                    if (resource.resource == Resource.Currency.Power)
                        buildable.powerIcon.SetActive(true);
                    else if (resource.resource == Resource.Currency.Heat)
                        buildable.heatIcon.SetActive(true);
                    else if (resource.resource == Resource.Currency.Gold)
                        buildable.goldIcon.SetActive(true);
                    else if (resource.resource == Resource.Currency.Essence)
                        buildable.essenceIcon.SetActive(true);
                    else if (resource.resource == Resource.Currency.Iridium)
                        buildable.iridiumIcon.SetActive(true);
                }
            }
        }

        return buildable;
    }
}
