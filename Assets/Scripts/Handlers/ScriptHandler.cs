using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptHandler : MonoBehaviour
{
    public static BuildingRegistrar buildingRegistrar;

    // Start is called before the first frame update
    void Start()
    {
        buildingRegistrar = GameObject.Find("Building Registrar").GetComponent<BuildingRegistrar>();
    }
}
