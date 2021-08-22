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

    public event Action<Rotator> onRegisterRotator;
    public void RegisterRotator(Rotator rotator)
    {
        if (onRegisterRotator != null)
            onRegisterRotator(rotator);
    }
}