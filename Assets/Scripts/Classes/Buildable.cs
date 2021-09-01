using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine.UI;

public class Buildable : MonoBehaviour
{
    // Building variables
    public string Name;
    public Building tile;

    // GameObject interface variables
    public TextMeshProUGUI invName;
    public Image invIcon;
    public Button invButton;

    // Set panel method
    public void SetPanel()
    {

    }
}
