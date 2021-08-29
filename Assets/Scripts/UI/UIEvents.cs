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
    public event Action onMenuOpened;
    public void MenuOpened()
    {
        if (onMenuOpened != null)
            onMenuOpened();
    }
}
