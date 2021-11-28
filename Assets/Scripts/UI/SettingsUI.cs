using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    // Active
    public static SettingsUI active;
    public static bool isOpen;

    // Variables
    public SliderManager music;
    public SliderManager sound;
    public SwitchManager autoSave;
    public SwitchManager rotatingObjects;
    public SwitchManager resourcePopups;

    // Get stuff
    public GameObject pausedMenu;

    // Canvas group
    public CanvasGroup canvasGroup;

    // Get active instance
    public void Awake()
    {
        active = this;
    }

    // On start
    public void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Settings.LoadSettings();
    }

    // Set auto spinning
    public void AutoSaving(bool enabled)
    {
        Settings.autoSave = enabled;
    }

    // Set auto spinning
    public void ResourcePopups(bool enabled)
    {
        Settings.resourcePopups = enabled;
    }

    // Set auto spinning
    public void RotatingObjects(bool enabled)
    {
        Settings.rotatingObjects = enabled;
    }

    // Set music
    public void SetMusic(float volume)
    {
        Settings.music = volume;
        Settings.UpdateSounds();
    }

    // Set music
    public void SetSoundeffects(float volume)
    {
        Settings.sound = volume;
        Settings.UpdateSounds();
    }

    // Switch resolution
    public void SwitchScreenmode(bool fullscreen)
    {
        Settings.screenmode = fullscreen;
        Settings.ApplySettings();
    }

    // Switch resolution
    public void SwitchFramerate(int amount)
    {
        Settings.framerate = amount;
        Application.targetFrameRate = amount;
        Settings.ApplySettings();
    }

    // Switch resolution
    public void SwitchResolution(int option)
    {
        switch(option)
        {
            case 0:
                Settings.resolutionX = 640;
                Settings.resolutionY = 480;
                break;
            case 1:
                Settings.resolutionX = 1280;
                Settings.resolutionY = 720;
                break;
            case 2:
                Settings.resolutionX = 1366;
                Settings.resolutionY = 768;
                break;
            case 3:
                Settings.resolutionX = 1440;
                Settings.resolutionY = 900;
                break;
            case 4:
                Settings.resolutionX = 1920;
                Settings.resolutionY = 1080;
                break;
            case 5:
                Settings.resolutionX = 2560;
                Settings.resolutionY = 1080;
                break;
            case 6:
                Settings.resolutionX = 2560;
                Settings.resolutionY = 1440;
                break;
            case 7:
                Settings.resolutionX = 3440;
                Settings.resolutionY = 1440;
                break;
        }
        Settings.ApplySettings();
    }

    // Go back
    public void Enable()
    {
        // Set values
        music.mainSlider.value = Settings.music;
        sound.mainSlider.value = Settings.sound;
        autoSave.isOn = Settings.autoSave;
        rotatingObjects.isOn = Settings.rotatingObjects;
        resourcePopups.isOn = Settings.resourcePopups;

        // Update values
        music.UpdateUI();
        sound.UpdateUI();
        autoSave.UpdateUI();
        rotatingObjects.UpdateUI();
        resourcePopups.UpdateUI();

        // Update canvas group
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Set to open
        isOpen = true;

        // Close paused menu if it exists
        if (pausedMenu != null)
            pausedMenu.SetActive(false);
    }

    // Go back
    public void Back()
    {   
        // Update canvas group
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Set to open
        isOpen = false;
        Settings.SaveSettings();

        // Open paused menu if it exists
        if (pausedMenu != null)
            pausedMenu.SetActive(true);
    }
}
