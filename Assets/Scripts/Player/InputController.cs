using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Get the camera
    public Camera cam;
    public GameObject grid;

    // TEMP
    public AudioSource music;
    public GameObject inv;

    public void Update()
    {
        if (!inv.activeSelf)
        {
            if (Input.GetKey(Keybinds.lmb))
                Events.active.LeftMousePressed();
            if (Input.GetKeyUp(Keybinds.rmb))
                Events.active.RightMouseReleased();
            if (Input.GetKey(Keybinds.rmb))
                Events.active.RightMousePressed();
            if (Input.GetKeyDown(Keybinds.escape))
            {
                Debug.Log("Quit game!");
                UIEvents.active.QuitGame();
            }
            if (Input.GetKeyDown(Keybinds.test))
                Events.active.FireHubLaser();
            if (Input.GetKeyDown(Keybinds.debug))
                InstantiationHandler.active.RemoveDebugCircles();
        }
        else
        {
            if (Input.GetKeyDown(Keybinds.escape))
            {
                UIEvents.active.MenuOpened();
                UIEvents.active.DisableHotbar();
            }
        }

        CheckNumberInput();

        if (Input.GetKeyDown(Keybinds.inventory))
            UIEvents.active.MenuOpened();
        if (Input.GetKeyDown(Keybinds.pause))
            Settings.paused = !Settings.paused;

        // TEMP
        if (Input.GetKeyDown(Keybinds.map))
        {
            if (music.volume <= 0f)
                music.volume = 0.5f;
            else music.volume = 0f;
        }
    }

    // Checks if for numeric input
    public void CheckNumberInput()
    {
        if (Input.GetKeyDown(Keybinds.hotbar_1))
            Events.active.NumberInput(0);
        else if (Input.GetKeyDown(Keybinds.hotbar_2))
            Events.active.NumberInput(1);
        else if (Input.GetKeyDown(Keybinds.hotbar_3))
            Events.active.NumberInput(2);
        else if (Input.GetKeyDown(Keybinds.hotbar_4))
            Events.active.NumberInput(3);
        else if (Input.GetKeyDown(Keybinds.hotbar_5))
            Events.active.NumberInput(4);
        else if (Input.GetKeyDown(Keybinds.hotbar_6))
            Events.active.NumberInput(5);
        else if (Input.GetKeyDown(Keybinds.hotbar_7))
            Events.active.NumberInput(6);
        else if (Input.GetKeyDown(Keybinds.hotbar_8))
            Events.active.NumberInput(7);
        else if (Input.GetKeyDown(Keybinds.hotbar_9))
            Events.active.NumberInput(8);
    }
}
