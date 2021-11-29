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
    public static bool rotatingObjects = true;
    public static bool resourcePopups = true;

    // Experimental values
    public static bool experimentalLOD = true;
    public static bool experimentalRendering = true;

    // Other options
    public static int resolutionX = 1920;
    public static int resolutionY = 1080;
    public static bool screenmode = true;
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
        settingsData.disableRotatingObjects = rotatingObjects;
        settingsData.disableResourcePopups = resourcePopups;
        settingsData.resolutionX = resolutionX;
        settingsData.resolutionY = resolutionY;
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
            rotatingObjects = settingsData.disableRotatingObjects;
            resourcePopups = settingsData.disableResourcePopups;

            // Other options
            resolutionX = settingsData.resolutionX;
            resolutionY = settingsData.resolutionY;
            screenmode = settingsData.screenmode;
            framerate = settingsData.framerate;
            music = settingsData.music;
            sound = settingsData.sound;

            // Apply settings
            ApplySettings();
            UpdateSounds();
        }
    }    

    // Apply settings
    public static void ApplySettings()
    {
        Screen.SetResolution(resolutionX, resolutionY, screenmode, framerate);
    }

    // Update sounds
    public static void UpdateSounds()
    {
        if (Events.active != null)
            Events.active.RefreshSound();
    }
}
