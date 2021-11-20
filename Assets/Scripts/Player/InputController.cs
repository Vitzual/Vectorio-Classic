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
        if (!NewInterface.isOpen)
        {
            if (Input.GetKey(Keybinds.lmb))
                InputEvents.active.LeftMousePressed();
            if (Input.GetKeyUp(Keybinds.lmb))
                InputEvents.active.LeftMouseReleased();
            if (Input.GetKey(Keybinds.rmb))
                InputEvents.active.RightMousePressed();
            if (Input.GetKeyUp(Keybinds.rmb))
                InputEvents.active.RightMouseReleased();
            if (Input.GetKeyDown(Keybinds.inventory))
                InputEvents.active.InventoryPressed();
            if (Input.GetKeyDown(Keybinds.pause))
                Settings.paused = !Settings.paused;
        }

        if (Input.GetKeyDown(Keybinds.sprint))
            InputEvents.active.ShiftPressed();
        if (Input.GetKeyUp(Keybinds.sprint))
            InputEvents.active.ShiftReleased();
        if (Input.GetKeyDown(Keybinds.escape))
            InputEvents.active.EscapePressed();

        CheckNumberInput();
    }

    // Checks if for numeric input
    public void CheckNumberInput()
    {
        if (Input.GetKeyDown(Keybinds.hotbar_1))
            InputEvents.active.NumberInput(0);
        else if (Input.GetKeyDown(Keybinds.hotbar_2))
            InputEvents.active.NumberInput(1);
        else if (Input.GetKeyDown(Keybinds.hotbar_3))
            InputEvents.active.NumberInput(2);
        else if (Input.GetKeyDown(Keybinds.hotbar_4))
            InputEvents.active.NumberInput(3);
        else if (Input.GetKeyDown(Keybinds.hotbar_5))
            InputEvents.active.NumberInput(4);
        else if (Input.GetKeyDown(Keybinds.hotbar_6))
            InputEvents.active.NumberInput(5);
        else if (Input.GetKeyDown(Keybinds.hotbar_7))
            InputEvents.active.NumberInput(6);
        else if (Input.GetKeyDown(Keybinds.hotbar_8))
            InputEvents.active.NumberInput(7);
        else if (Input.GetKeyDown(Keybinds.hotbar_9))
            InputEvents.active.NumberInput(8);
    }
}
