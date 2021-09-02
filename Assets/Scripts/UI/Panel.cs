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

    public void SetBuildingStats(Building building)
    {
        //foreach (Building.Resources resource in building.resources)
            //CreateStat();
    }

    public void SetTurretStats(Turret turret)
    {

    }

    // Creates a menu stat
    public void CreateStat(string name, int value, Sprite icon, bool isResource)
    {
        // Create object
        GameObject obj = Instantiate(menuStat.obj, new Vector3(0, 0, 0), Quaternion.identity);
        menuObjects.Add(obj.transform);

        // Set the values
        MenuStat holder = obj.GetComponent<MenuStat>();
        holder.text.text = "<b>" + name + ":</b> " + value;
        holder.icon = icon;

        // Set parent transform
        if (isResource)
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
