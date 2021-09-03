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
    public Building building;

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
        UIEvents.active.BuildingPressed(building);
    }
}
