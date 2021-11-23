using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour
{
    [HideInInspector]
    public SaveData saveData;
    [HideInInspector]
    public string savePath;

    public GameObject obj;
    public bool outdated;
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
        Menu.active.LoadSave(pathNumber, outdated);
    }

    public void DeleteSave()
    {
        Menu.active.deleteIndex = pathNumber;
        Menu.active.deletePath = savePath;
        Menu.active.confirmDelete.OpenWindow();
    }
}
