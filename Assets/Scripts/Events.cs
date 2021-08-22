using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events active;

    // Start is called before the first frame update
    private void Awake()
    {
        active = this;
    }

    public event Action<Bullet> onBulletFired;
    public void BulletFired(Bullet bullet)
    {
        if (onBulletFired != null)
            onBulletFired(bullet);
    }

    public event Action<Rotator> onRotatorPlaced;
    public void RotatorPlaced(Rotator rotator)
    {
        if (onRotatorPlaced != null)
            onRotatorPlaced(rotator);
    }

    public event Action<DefaultCollector> onCollectorPlaced;
    public void CollectorPlaced(DefaultCollector collector)
    {
        if (onCollectorPlaced != null)
            onCollectorPlaced(collector);
    }

    public event Action<DefaultStorage> onStoragePlaced;
    public void StoragePlaced(DefaultStorage storage)
    {
        if (onStoragePlaced != null)
            onStoragePlaced(storage);
    }
}