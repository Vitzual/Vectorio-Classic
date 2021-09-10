using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public MenuButton buildable;
    public Transform defensesList;
    public Transform logisticsList;

    public void Start()
    {
        // Loads all buildings on record
        try
        {
            List<Building> tiles = Resources.LoadAll("Scriptables", typeof(Building)).Cast<Building>().ToList();
            Debug.Log("Loaded " + tiles.Count + " tiles from Resources/Scriptables");

            foreach (Building building in tiles) 
                CreateBuildable(building);
        }
        catch
        {
            Debug.LogError("The folder Resources/Scriptables/ contains a non-scriptable object. Please remove it!");
        }
    }

    public void CreateBuildable(Building building)
    {
        // Create the new buildable object
        GameObject holder = Instantiate(buildable.obj, new Vector3(0, 0, 0), Quaternion.identity);

        // Set parent
        if (building.isDefensive) holder.transform.SetParent(defensesList);
        else holder.transform.SetParent(logisticsList);
        holder.transform.SetSiblingIndex(building.unlockOrder);
        holder.transform.name = building.name;

        // Adjust size
        RectTransform temp = holder.GetComponent<RectTransform>();
        if (temp != null) temp.localScale = new Vector3(1, 1, 1);

        // Set buildable values
        buildable = holder.GetComponent<MenuButton>();
        buildable.building = building;
        buildable.button.buttonText = "<b>" + building.name.ToUpper() + "</b><size=20> LEVEL " + building.level;
        buildable.icon.sprite = Sprites.GetSprite(building.sprite);

        // Set building unlock value
        if (building.isUnlocked)
            buildable.desc.text = "<b>24 ACTIVE |</b> <size=16>0 power draw, 0 heat generation";
        else
            buildable.desc.text = "<b>LOCKED |</b> <size=16>" + building.unlockDesc;

        // Set buildable resources
        //
        // WIP - Will eventually automatically scan and add every resource
        // so that this script doesn't need to be modified

        foreach (Building.Resources resource in building.resources)
        {
            // Check power
            if (resource.resource == Resource.Currency.Power)
                buildable.essenceIcon.SetActive(true);
            else buildable.essenceIcon.SetActive(false);

            // Check heat
            if (resource.resource == Resource.Currency.Heat)
                buildable.essenceIcon.SetActive(true);
            else buildable.essenceIcon.SetActive(false);

            // Check gold
            if (resource.resource == Resource.Currency.Gold)
                buildable.essenceIcon.SetActive(true);
            else buildable.essenceIcon.SetActive(false);

            // Check essence
            if (resource.resource == Resource.Currency.Essence)
                buildable.essenceIcon.SetActive(true);
            else buildable.essenceIcon.SetActive(false);

            // Check iridium
            if (resource.resource == Resource.Currency.Iridium)
                buildable.iridiumIcon.SetActive(true);
            else buildable.iridiumIcon.SetActive(false);
        }
    }
}
