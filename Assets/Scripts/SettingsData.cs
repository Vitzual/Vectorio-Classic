using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SettingsData : MonoBehaviour
{
    // Boolean options
    public bool autoSave;
    public bool disableRotatingObjects;
    public bool disableResourcePopups;

    // Other options
    public Tuple<int, int> resolution;
    public bool screenmode;
    public float music;
    public float sound;
    public int framerate;
}