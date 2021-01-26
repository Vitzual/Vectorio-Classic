using UnityEngine;
using MK.Glow.Legacy;

public class Settings : MonoBehaviour
{
    public AudioSource music;
    public MKGlowLite glowing;
    public Interface ui;

    public void SetMusic(float a)
    {
        music.volume = a;
    }

    public void SetResolution(int a)
    {
        if (a == 1) Screen.SetResolution(1280, 720, Screen.fullScreen);
        else if (a == 2) Screen.SetResolution(1280, 800, Screen.fullScreen);
        else if (a == 3) Screen.SetResolution(1366, 768, Screen.fullScreen);
        else if (a == 4) Screen.SetResolution(1440, 900, Screen.fullScreen);
        else if (a == 5) Screen.SetResolution(1600, 900, Screen.fullScreen);
        else if (a == 6) Screen.SetResolution(1680, 1050, Screen.fullScreen);
        else if (a == 7) Screen.SetResolution(1920, 1080, Screen.fullScreen);
        else if (a == 8) Screen.SetResolution(2560, 1440, Screen.fullScreen);
    }

    public void SetScreenmode(bool a)
    {
        Screen.fullScreen = a;
    }

    public void SetShaderMode(int a)
    {
        if (a == 1) glowing.enabled = false;
        else glowing.enabled = true;
    }

    public void EnableMenuAndPaused()
    {
        ui.SettingsOpen = true;
        ui.SetOverlayStatus("Settings", true);
        ui.SetOverlayStatus("Paused", false);
    }

    public void DisableMenuAndPaused()
    {
        ui.SettingsOpen = false;
        ui.SetOverlayStatus("Settings", false);
        ui.SetOverlayStatus("Paused", true);
    }

    public void EnableMenu()
    {
        CanvasGroup cg = transform.GetComponent<CanvasGroup>();
        cg.interactable = true;
        cg.blocksRaycasts = true;
        cg.alpha = 1f;
    }

    public void DisableMenu()
    {
        CanvasGroup cg = transform.GetComponent<CanvasGroup>();
        cg.interactable = false;
        cg.blocksRaycasts = false;
        cg.alpha = 0f;
    }
}
