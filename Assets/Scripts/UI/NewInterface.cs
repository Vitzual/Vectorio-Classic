using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInterface : MonoBehaviour
{
    public GameObject buildingMenu;

    public void Start()
    {
        UIEvents.active.onBuildingMenuPressed += ToggleBuildingMenu;
    }

    public void ToggleBuildingMenu()
    {
        buildingMenu.SetActive(!buildingMenu.activeInHierarchy);
    }
}
