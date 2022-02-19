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

    // Display host loading
    public event Action<bool> onHostLoading;
    public void LoadHost(bool enabled)
    {
        if (onHostLoading != null)
            onHostLoading(enabled);
    }

    // Display player joining
    public event Action<float> onUpdateLoadProgress;
    public void UpdateLoadPractice(float percentage)
    {
        if (onUpdateLoadProgress != null)
            onUpdateLoadProgress(percentage);
    }

    // Display player joining
    public event Action<bool> onPlayerJoin;
    public void PlayerJoin(bool enabled)
    {
        if (onPlayerJoin != null)
            onPlayerJoin(enabled);
    }

    // Display player joining
    public event Action<float> onUpdateJoinProgress;
    public void UpdateJoinProgress(float percentage)
    {
        if (onUpdateJoinProgress != null)
            onUpdateJoinProgress(percentage);
    }

    // On add resource
    public event Action<CollectedBlueprint> onApplyBlueprint;
    public void ApplyBlueprint(CollectedBlueprint blueprint)
    {
        if (onApplyBlueprint != null)
            onApplyBlueprint(blueprint);
    }

    // On add resource
    public event Action<Resource.Type, int> onAddResource;
    public void AddResource(Resource.Type currency, int amount)
    {
        if (onAddResource != null)
            onAddResource(currency, amount);
    }

    // Invoked when a building is clicked
    public event Action<Entity, int> onEntityPressed;
    public void EntityPressed(Entity entity, int metadata)
    {
        if (onEntityPressed != null)
            onEntityPressed(entity, metadata);
    }

    // Invoked when a building is clicked
    public event Action<Building, int> onBuildingPressed;
    public void BuildablePressed(Building building, int metadata)
    {
        if (onBuildingPressed != null)
            onBuildingPressed(building, metadata);
    }

    // Invoked when a building is clicked
    public event Action<Buildable, int> onBuildablePressed;
    public void BuildablePressed(Buildable buildable, int metadata)
    {
        if (onBuildablePressed != null)
            onBuildablePressed(buildable, metadata);
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
