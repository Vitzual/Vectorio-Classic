using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBuilding : BaseBuilding
{
    public Rotator[] rotators;

    public void Start()
    {
        SetBuildingStats();
    }
}
