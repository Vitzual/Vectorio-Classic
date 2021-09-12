using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public MenuButton buildable;
    public List<Transform> lists;

    public void Start()
    {
        Events.active.initEntities += GenerateEntities;
    }

    public void GenerateEntities(string path)
    {
        List<MenuButton> holders = new List<MenuButton>();
        List<Entity> entities = Resources.LoadAll(path, typeof(Entity)).Cast<Entity>().ToList();
        Debug.Log("Loaded " + entities.Count + " entities from " + path);

        // Generate buildables
        foreach (Entity entity in entities)
        {
            if (entity.invIndex >= 0 && entity.invIndex < lists.Count)
            {
                MenuButton holder = CreateEntity(entity, lists[entity.invIndex]);
                if (holder != null) holders.Add(holder);
            }
        }

        // Set order of buildables
        foreach(MenuButton holder in holders)
        {
            holder.transform.SetSiblingIndex(holder.entity.invOrder);
        }
    }

    public MenuButton CreateEntity(Entity entity, Transform list)
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
            buildable.button.buttonText = "<b>" + entity.name.ToUpper() + "</b>";

        // Set building icon
        buildable.icon.sprite = Sprites.GetSprite(entity.name);

        // Set building unlock value
        buildable.desc.text = "<b>" + entity.active + " ACTIVE |</b> <size=16>Click for more details!";

        return buildable;

        /*
        foreach (Building.Resources resource in building.resources)
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
        */
    }
}
