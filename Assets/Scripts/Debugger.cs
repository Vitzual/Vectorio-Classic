using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    // Active instance
    public static Debugger active;

    [Header("Debug Settings")]
    public bool drawTargetLines;
    public bool ignoreSpawnValues;

    public void Start()
    {
        active = this;
    }
}
