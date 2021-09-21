using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Get the camera
    public Camera cam;
    public GameObject grid;

    // Camera variables
    public float cameraZoomSpeed;
    public float cameraZoomFactor;
    public float cameraMinZoom;
    public float cameraMaxZoom;
    private float targetZoom;

    // TEMP
    public AudioSource music;
    public GameObject inv;
    private float timer = 0;
    private float fpsRefreshRate = 1f;
    public TextMeshProUGUI fpsCurrent;

    public void Start()
    {
        targetZoom = cam.orthographicSize;
    }

    public void Update()
    {
        if (Time.unscaledTime > timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsCurrent.text = fps+"fps";
            timer = Time.unscaledTime + fpsRefreshRate;
        }

        if (!inv.activeSelf)
        {
            CheckScrollInput();

            if (Input.GetKey(Keybinds.lmb))
                Events.active.LeftMousePressed();
            if (Input.GetKeyUp(Keybinds.rmb))
                Events.active.RightMouseReleased();
            if (Input.GetKey(Keybinds.rmb))
                Events.active.RightMousePressed();
            if (Input.GetKeyDown(Keybinds.escape))
                Application.Quit();
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

        // TEMP
        if (Input.GetKeyDown(Keybinds.map))
        {
            if (music.volume <= 0f)
                music.volume = 0f;
            else music.volume = 0.5f;
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
