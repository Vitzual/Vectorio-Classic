using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkEvents : MonoBehaviour
{
    public static NetworkEvents active;

    // Start is called before the first frame update
    public void Awake()
    {
        active = this;
    }

    
}
