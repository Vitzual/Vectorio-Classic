using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInterface : MonoBehaviour
{
    public GameObject buildingMenu;
    private bool generatedBuildables = false;

    public void Start()
    {
        UIEvents.active.onBuildingMenuPressed += ToggleBuildingMenu;
    }

    public void ToggleBuildingMenu()
    {
        if (!generatedBuildables)
        {
            Events.active.SetupBuildables();
            generatedBuildables = true;
        }

        buildingMenu.SetActive(!buildingMenu.activeInHierarchy);
    }
}
