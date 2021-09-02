using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    public MenuStat menuStat;
    public Transform resourceStats;
    public Transform buildingStats;

    public void SetBuildingStats(Building building)
    {
        
    }

    public void SetTurretStats(Turret turret)
    {

    }

    public void CreateStat(Stat stat)
    {
        // Create object
        GameObject obj = Instantiate(menuStat.obj, new Vector3(0, 0, 0), Quaternion.identity);
        obj.name = stat.name;

        // Set parent transform
        if (stat.isResource)
            obj.transform.parent = resourceStats;
        else
            obj.transform.parent = buildingStats;
    }

    public void ResetStats()
    {

    }

    
}
