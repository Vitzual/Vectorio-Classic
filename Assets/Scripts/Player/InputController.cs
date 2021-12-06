using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    // Shift held
    public static bool shiftHeld;

    // Get the camera
    public Camera cam;
    public GameObject grid;

    // TEMP
    public AudioSource music;
    public GameObject inv;

    public void Update()
    {
        if (!Inventory.isOpen && !ResearchUI.isOpen && !StatsPanel.isOpen && !CameraController.mapEnabled)
        {
            if (Input.GetKeyDown(Keybinds.construct) && !InterfaceCheck())
                InputEvents.active.LeftMouseTapped();
            if (Input.GetKey(Keybinds.construct) && !InterfaceCheck())
                InputEvents.active.LeftMousePressed();
            if (Input.GetKeyUp(Keybinds.construct))
                InputEvents.active.LeftMouseReleased();
            if (Input.GetKey(Keybinds.delete))
                InputEvents.active.RightMousePressed();
            if (Input.GetKeyUp(Keybinds.delete))
                InputEvents.active.RightMouseReleased();
            if (Input.GetKeyDown(Keybinds.pipette))
                InputEvents.active.PipettePressed();
            if (Input.GetKeyDown(Keybinds.inventory))
                InputEvents.active.InventoryPressed();
            if (Input.GetKeyDown(Keybinds.pause))
                Settings.paused = !Settings.paused;
            if (Input.GetKeyDown(Keybinds.map))
                InputEvents.active.MapPressed();
        }

        if (Input.GetKeyDown(Keybinds.sprint))
        {
            shiftHeld = true;
            InputEvents.active.ShiftPressed();
        }
        if (Input.GetKeyUp(Keybinds.sprint))
        {
            shiftHeld = false;
            InputEvents.active.ShiftReleased();
        }
        if (Input.GetKeyDown(Keybinds.escape))
            InputEvents.active.EscapePressed();

        CheckNumberInput();
    }

    // Check UI elements 
    private bool InterfaceCheck()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
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
