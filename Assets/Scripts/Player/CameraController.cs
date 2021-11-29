using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Camera Variable
    protected Camera cam;
    public bool isMenu;

    // Resolution layers
    protected bool mapEnabled = false;
    public LayerMask lowresMap;
    public LayerMask lowResLayers;
    public LayerMask highResLayers;

    // Movement variables
    public float maxRange = 750f;
    public float maxZoom = 350f;
    protected float moveSpeed = 150f;
    protected Rigidbody2D cameraRB;
    protected Vector2 movement;
    protected Vector2 mousePos;
    protected bool allowMovement = true;

    // Zoom variables
    private static float targetZoom;
    public float zoomFactor = 150f;
    public float zoomSpeed = 40;

    // Grid variables
    private bool gridActive = true;
    public GameObject grid;

    // Start is called before the first frame update
    public void Start()
    {
        // Menu
        if (isMenu) return;

        // Get camera components
        cam = GetComponent<Camera>();
        cameraRB = GetComponent<Rigidbody2D>();
        targetZoom = cam.orthographicSize;

        // Set input events
        if (InputEvents.active != null)
        {
            InputEvents.active.onShiftPressed += SetFastSpeed;
            InputEvents.active.onShifReleased += SetNormalSpeed;
            InputEvents.active.onLeftControlPressed += SetSlowSpeed;
            InputEvents.active.onLeftControlReleased += SetNormalSpeed;
            InputEvents.active.onMapPressed += EnableLowresMap;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        // Check if research open
        if (Inventory.isOpen || ResearchUI.isOpen || StatsPanel.isOpen || isMenu) return;

        // Get scroll input
        float scrollData; 
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        // Calculate scorll data
        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, 15f, maxZoom);

        // Calculate orthographic map size
        if (mapEnabled) cam.orthographicSize = targetZoom;
        else cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

        // If target zoom less than 100, activate grid
        if (targetZoom < 100f && gridActive == false)
        {
            EnableAllLayers();
            grid.SetActive(true);
            gridActive = true;
        }

        // If target zoom exceeds 100, deactive grid
        else if (targetZoom >= 100f && gridActive == true)
        {
            if (Settings.experimentalRendering) EnableLowresView();

            grid.SetActive(false);
            gridActive = false;
        }

        // If target zoom lower than 350 and map active, disabel mapview
        else if (targetZoom < 350f && mapEnabled)
        {
            mapEnabled = false;
            maxZoom = 350f;

            if (Settings.experimentalRendering) EnableLowresView();
            else EnableAllLayers();
        }
    }

    // Physics update
    private void FixedUpdate()
    {
        // Check if research open
        if (isMenu) return;

        // Get directional movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Determine if movement should be allowed
        if (movement.x > 0 && cameraRB.position.x + movement.x > Border.east) { allowMovement = false; movement.x = 0; }
        if (movement.x < 0 && cameraRB.position.x + movement.x < Border.west) { allowMovement = false; movement.x = 0; }
        if (movement.y > 0 && cameraRB.position.y + movement.y > Border.north) { allowMovement = false; movement.y = 0; }
        if (movement.y < 0 && cameraRB.position.y + movement.y < Border.south) { allowMovement = false; movement.y = 0; }
        if (allowMovement) cameraRB.MovePosition(cameraRB.position + movement * moveSpeed * Time.fixedDeltaTime);

        // Reset movement variable
        allowMovement = true;
    }

    public void EnableAllLayers()
    {
        cam.cullingMask = 1 | lowResLayers | highResLayers;
    }

    public void EnableLowresView()
    {
        cam.cullingMask = ~(1 | lowResLayers | lowresMap);
    }

    public void EnableLowresMap()
    {
        mapEnabled = true;
        cam.cullingMask = 1 | lowresMap;
        maxZoom = 1000f;
        cam.orthographicSize = 400f;
    }

    public void SetFastSpeed() { moveSpeed = 600f;}
    public void SetNormalSpeed() { moveSpeed = 150f; }
    public void SetSlowSpeed() { moveSpeed = 20f;}
}
