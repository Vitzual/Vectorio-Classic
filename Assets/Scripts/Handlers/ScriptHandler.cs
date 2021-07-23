using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptHandler : MonoBehaviour
{
    public BuildingRegistrar _buildingRegistrar;
    public static BuildingRegistrar buildingRegistrar;

    // Start is called before the first frame update
    void Start()
    {
        buildingRegistrar = _buildingRegistrar;
    }
}
