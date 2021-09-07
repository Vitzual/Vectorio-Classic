using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultTurret : BaseTurret
{
    // The targetting mode of the turret
    public LayerMask layer;

    // Sets stats and registers itself under the turret handler
    public void Start()
    {
        SetBuildingStats();
    }

    
}
