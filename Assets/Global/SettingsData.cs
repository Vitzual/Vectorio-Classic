using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{
    public int width;
    public int height;
    public float volume;
    public bool fullscreen;
    public int glowMode;

    public SettingsData(int width, int height, float volume, bool fullscreen, int glowMode) 
    {
        this.width = width;
        this.height = height;
        this.volume = volume;
        this.fullscreen = fullscreen;
        this.glowMode = glowMode;
    }
}
