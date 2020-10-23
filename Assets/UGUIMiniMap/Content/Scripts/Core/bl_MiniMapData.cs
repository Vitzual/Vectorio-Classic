using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bl_MiniMapData : ScriptableObject
{
    public GameObject IconPrefab;
    public GameObject ScreenShotPrefab;

    public static bl_MiniMapData _instance;
    public static bl_MiniMapData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = Resources.Load<bl_MiniMapData>("MiniMapData") as bl_MiniMapData;
            }
            return _instance;
        }
    }
}