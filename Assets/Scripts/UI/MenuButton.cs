using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using TMPro;

public class MenuButton : MonoBehaviour
{
    // Building holder
    public Building building;

    // Button variables
    public new ButtonManagerBasic button;
    public TextMeshProUGUI desc;
    public Image icon;

    // Resource icons
    public GameObject iridiumIcon;
    public GameObject essenceIcon;
    public GameObject goldIcon;
    public GameObject heatIcon;
    public GameObject powerIcon;
}
