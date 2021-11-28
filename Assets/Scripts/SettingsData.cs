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
    public int resolutionX;
    public int resolutionY;
    public bool screenmode;
    public float music;
    public float sound;
    public int framerate;
}