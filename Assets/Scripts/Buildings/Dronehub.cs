using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dronehub : BaseTile
{
    // Nearby targets
    public List<BaseEntity> nearbyTargets;

    // Lights
    public GameObject BlueLight;
    public GameObject YellowLight;

    // Side panels 
    public Transform leftPanel;
    public Transform rightPanel;

    // Locate nearby buildings for drone
    public override void Setup()
    {
        
    }

}
