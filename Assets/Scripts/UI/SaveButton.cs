using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour
{
    [HideInInspector]
    public SaveData saveData;

    public GameObject obj;
    public RectTransform rect;
    public Image icon;
    public new TextMeshProUGUI name;
    public TextMeshProUGUI timeAndVersion;
    public TextMeshProUGUI worldMode;

    [HideInInspector]
    public int pathNumber;
    [HideInInspector]
    public string seed;
    [HideInInspector]
    public string mode;

    public void LoadSave()
    {
        Menu.active.LoadSave(pathNumber);
    }
}
