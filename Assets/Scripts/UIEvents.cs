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

    // On add resource
    public event Action<Resource.CurrencyType, int> onAddResource;
    public void AddResource(Resource.CurrencyType currency, int amount)
    {
        if (onAddResource != null)
            onAddResource(currency, amount);
    }

    // Invoked when a building is clicked
    public event Action<Entity> onEntityPressed;
    public void EntityPressed(Entity entity)
    {
        if (onEntityPressed != null)
            onEntityPressed(entity);
    }

    // Invoked when a building is clicked
    public event Action<Building> onBuildingPressed;
    public void BuildablePressed(Building building)
    {
        if (onBuildingPressed != null)
            onBuildingPressed(building);
    }

    // Invoked when a building is clicked
    public event Action<Buildable> onBuildablePressed;
    public void BuildablePressed(Buildable buildable)
    {
        if (onBuildablePressed != null)
            onBuildablePressed(buildable);
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
