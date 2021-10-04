using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using TMPro;

public class MenuButton : MonoBehaviour
{
    // Building holder
    [HideInInspector]
    public Entity entity;

    // Button variables
    public GameObject obj;
    public ButtonManagerBasic button;
    public TextMeshProUGUI desc;
    public Image icon;

    // Resource icons
    public GameObject iridiumIcon;
    public GameObject essenceIcon;
    public GameObject goldIcon;
    public GameObject heatIcon;
    public GameObject powerIcon;

    // Show stats
    public void DisplayStats()
    {
        if (BuildingSystem.active != null)
            BuildingSystem.active.SetBuilding(entity);
        else Debug.LogError("Cannot set building without building system!");

        DefaultEnemy enemy = entity.obj.GetComponent<DefaultEnemy>();
        UIEvents.active.BuildingPressed(entity, enemy != null);
    }
}
