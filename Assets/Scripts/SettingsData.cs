using System;

[Serializable]
public class SettingsData
{
    // Boolean options
    public bool autoSave;
    public bool disableRotatingObjects;
    public bool disableResourcePopups;
    public bool experimentalRendering;

    // Other options
    public int resolutionX;
    public int resolutionY;
    public bool screenmode;
    public float music;
    public float sound;
    public int framerate;
}