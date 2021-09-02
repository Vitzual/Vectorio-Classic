using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    private List<Transform> menuObjects;

    public MenuStat menuStat;
    public Transform resourceStats;
    public Transform buildingStats;

    public void Start()
    {
        
    }

    public void SetResourceStats(Building building)
    {
        // Create resources
        foreach (Building.Resources resource in building.resources)
            CreateStat(new Stat(resource.ToString(), resource.amount, 0, resource.icon, true));
    }

    // Creates a menu stat
    public void CreateStat(Stat stat)
    {
        // Create object
        GameObject obj = Instantiate(menuStat.obj, new Vector3(0, 0, 0), Quaternion.identity);
        menuObjects.Add(obj.transform);

        // Create modifier variable
        string modifier = "";

        // Set modifier string if not 0
        if (stat.modifier > 0)
            modifier = "<color=green>(+" + stat.modifier + ")";
        else if (stat.modifier < 0)
            modifier = "<color=red>(+" + stat.modifier + ")";

        // Set the values
        MenuStat holder = obj.GetComponent<MenuStat>();
        holder.text.text = "<b>" + name + ":</b> " + (stat.value + stat.modifier) + " " + modifier;
        holder.icon.sprite = stat.icon;

        // Set parent transform
        if (stat.isResource)
            obj.transform.parent = resourceStats;
        else
            obj.transform.parent = buildingStats;
    }

    // Resets all the menu stats
    public void ResetStats()
    {
        List<Transform> objs = menuObjects;
        menuObjects.Clear();

        foreach (Transform transform in objs)
            Recycler.AddRecyclable(transform);
    }

    
}
