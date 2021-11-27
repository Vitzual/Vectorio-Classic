using UnityEngine;
//using MK.Glow.URP;
using Michsky.UI.ModernUIPack;
using System;
using System.IO;

public class Settings : MonoBehaviour
{
    // Toggleable options
    public static bool paused = false;
    public static bool autoSave = true;
    public static bool disableRotatingObjects = false;
    public static bool disableResourcePopups = false;

    // Other options
    public static Tuple<int, int> resolution;
    public static bool screenmode;
    public static float music = 0.5f;
    public static float sound = 0.5f;
    public static int framerate = 999;

    // Save settings
    public static void SaveSettings()
    {
        // Create new save data instance
        SettingsData settingsData = new SettingsData();

        // Set settings data
        settingsData.autoSave = autoSave;
        settingsData.disableRotatingObjects = disableRotatingObjects;
        settingsData.disableResourcePopups = disableResourcePopups;
        settingsData.resolution = resolution;
        settingsData.screenmode = screenmode;
        settingsData.framerate = framerate;
        settingsData.music = music;
        settingsData.sound = sound;

        // Convert to json and save
        string data = JsonUtility.ToJson(settingsData);
        File.WriteAllText(Application.persistentDataPath + "/settings.vectorio", data);
    }

    // Load settings
    public static void LoadSettings()
    {
        // Get path
        string path = Application.persistentDataPath + "/settings.vectorio";

        // Check if file exists
        if (File.Exists(path))
        {
            // Load json file
            string data = File.ReadAllText(path);
            SettingsData settingsData = JsonUtility.FromJson<SettingsData>(data);

            // Apply settings
            autoSave = settingsData.autoSave;
            disableRotatingObjects = settingsData.disableRotatingObjects;
            disableResourcePopups = settingsData.disableResourcePopups;

            // Other options
            resolution = settingsData.resolution;
            screenmode = settingsData.screenmode;
            framerate = settingsData.framerate;
            music = settingsData.music;
            sound = settingsData.sound;

            // Apply settings
            ApplySettings();
        }
    }    

    // Apply settings
    public static void ApplySettings()
    {
        Screen.SetResolution(resolution.Item1, resolution.Item2, screenmode, framerate);
        Events.active.RefreshSound();
    }
}
