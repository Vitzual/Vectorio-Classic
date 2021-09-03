using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public static UIEvents active;

    // Start is called before the first frame update
    public void Awake()
    {
        active = this;
    }

    // Invoked when a bullet is fired
    public event Action onBuildingMenuPressed;
    public void MenuOpened()
    {
        if (onBuildingMenuPressed != null)
            onBuildingMenuPressed();
    }

    // Invoked when a building is clicked
    public event Action onBuildingPressed;
    public void BuildingPressed()
    {
        if (onBuildingPressed != null)
            onBuildingPressed();
    }

    // Invoked when a building creates a custom stat
    public event Action<Stat> onCreateStat;
    public  void CreateStat(Stat stat)
    {
        if (onCreateStat != null)
            onCreateStat(stat);
    }
}
