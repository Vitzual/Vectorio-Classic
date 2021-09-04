using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Get the camera
    public Camera cam;
    public GameObject grid;

    public float closeLOD;
    public float farLOD;

    public float cameraZoomSpeed;
    public float cameraZoomFactor;
    public float cameraMinZoom;
    public float cameraMaxZoom;

    private float targetZoom;

    [HideInInspector]
    public bool SettingHotbar = false;

    public void Start()
    {
        targetZoom = cam.orthographicSize;
    }

    public void Update()
    {
        CheckScrollInput();

        if (Input.GetKeyDown(Keybinds.inventory))
            UIEvents.active.MenuOpened();
        if (Input.GetKey(Keybinds.select))
            Events.active.PlaceBuilding();
    }

    // Checks if for numeric input
    public void CheckNumberInput()
    {
        if (Input.GetKeyDown(Keybinds.hotbar_1))
            Events.active.NumberInput(1, SettingHotbar);
        else if (Input.GetKeyDown(Keybinds.hotbar_2))
            Events.active.NumberInput(2, SettingHotbar);
        else if (Input.GetKeyDown(Keybinds.hotbar_3))
            Events.active.NumberInput(3, SettingHotbar);
        else if (Input.GetKeyDown(Keybinds.hotbar_4))
            Events.active.NumberInput(4, SettingHotbar);
        else if (Input.GetKeyDown(Keybinds.hotbar_5))
            Events.active.NumberInput(5, SettingHotbar);
        else if (Input.GetKeyDown(Keybinds.hotbar_6))
            Events.active.NumberInput(6, SettingHotbar);
        else if (Input.GetKeyDown(Keybinds.hotbar_7))
            Events.active.NumberInput(7, SettingHotbar);
        else if (Input.GetKeyDown(Keybinds.hotbar_8))
            Events.active.NumberInput(8, SettingHotbar);
        else if (Input.GetKeyDown(Keybinds.hotbar_9))
            Events.active.NumberInput(9, SettingHotbar);
    }

    // Check scroll input
    public void CheckScrollInput()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollData * cameraZoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, cameraMinZoom, cameraMaxZoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * cameraZoomSpeed);

        if (grid != null)
        {
            if (cam.orthographicSize > 120 && grid.activeSelf) grid.SetActive(false);
            else if (cam.orthographicSize <= 120 && !grid.activeSelf) grid.SetActive(true);
        }

        /*
        if (Detail.active != null)
        {
            if (cam.orthographicSize > closeLOD && Detail.active.closeEnabled || cam.orthographicSize <= closeLOD && !Detail.active.closeEnabled) Detail.active.ToggleClose();
            else if (cam.orthographicSize > farLOD && Detail.active.farEnabled || cam.orthographicSize <= farLOD && !Detail.active.farEnabled) Detail.active.ToggleFar();
        }
        */
    }
}
