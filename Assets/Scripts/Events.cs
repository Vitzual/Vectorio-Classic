using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events active;

    // Start is called before the first frame update
    public void Awake()
    {
        active = this;
    }
    
    // Invoked when a bullet is fired
    public event Action<Bullet> onBulletFired;
    public void BulletFired(Bullet bullet)
    {
        if (onBulletFired != null)
            onBulletFired(bullet);
    }

    // Invoked when a building with a rotating piece is placed
    public event Action<Rotator> onRotatorPlaced;
    public void RotatorPlaced(Rotator rotator)
    {
        if (onRotatorPlaced != null)
            onRotatorPlaced(rotator);
    }

    // Invoked when a building with a collector script is placed
    public event Action<DefaultCollector> onCollectorPlaced;
    public void CollectorPlaced(DefaultCollector collector)
    {
        if (onCollectorPlaced != null)
            onCollectorPlaced(collector);
    }

    // Invoked when a building with a storage script is placed
    public event Action<DefaultStorage> onStoragePlaced;
    public void StoragePlaced(DefaultStorage storage)
    {
        if (onStoragePlaced != null)
            onStoragePlaced(storage);
    }

    // Invoked when a building is placed
    public event Action onBuildingPlaced;
    public void PlaceBuilding()
    {
        if (onBuildingPlaced != null)
            onBuildingPlaced();
    }

    // Invoked when a survival save is loaded
    public event Action<SurvivalData> onSurvivalLoaded;
    public void SurvivalLoaded(SurvivalData data)
    {
        if (onSurvivalLoaded != null)
            onSurvivalLoaded(data);
    }

    // Invoked when a hotbar is set
    public event Action<Tile, int> onHotbarSet;
    public void HotbarSet(Tile tile, int slot)
    {
        if (onHotbarSet != null)
            onHotbarSet(tile, slot);
    }

    // Invoked when input from the keyboard is detected
    public event Action<int, bool> onNumberInput;
    public void NumberInput(int number, bool hotbar = false)
    {
        if (onNumberInput != null)
            onNumberInput(number, hotbar);
    }

    // Initializes entities
    public event Action<string> initEntities;
    public void InitEntities(String path)
    {
        if (initEntities != null)
            initEntities(path);
    }
}