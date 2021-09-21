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
    public event Action<Entity> onBuildingPressed;
    public void BuildingPressed(Entity entity)
    {
        if (onBuildingPressed != null)
            onBuildingPressed(entity);
    }

    public event Action<int> onHotbarPressed;
    public void HotbarPressed(int index)
    {
        if (onHotbarPressed != null)
            onHotbarPressed(index);
    }

    public event Action onDisableHotbar;
    public void DisableHotbar()
    {
        if (onDisableHotbar != null)
            onDisableHotbar();
    }
}
