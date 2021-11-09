using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Camera Variable
    protected Camera cam;
    public TextMeshProUGUI fps;

    // Movement variables
    public float maxRange = 750f;
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
    void Start()
    {
        // Get camera components
        cam = GetComponent<Camera>();
        cameraRB = GetComponent<Rigidbody2D>();
        targetZoom = cam.orthographicSize;

        // Determine if FPS should be calculated
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 1000;
        if (fps != null) InvokeRepeating("CalculateFPS", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {        
        // Check if research open
        if (Interface.researchOpen) return;

        // Get scroll input
        float scrollData; 
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        // Calculate scorll data
        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, 15f, 350f);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

        // Determine if grid should be active
        if (targetZoom >= 100f && gridActive == true)
        {
            grid.SetActive(false);
            gridActive = false;
        }
        else if (targetZoom < 100f && gridActive == false)
        {
            grid.SetActive(true);
            gridActive = true;
        }
    }

    // Physics update
    private void FixedUpdate()
    {
        // Check if research open
        if (Interface.researchOpen) return;

        // Get directional movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Calculate move speed
        if (Input.GetKeyDown(KeyCode.LeftShift))
            moveSpeed = 600f;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            moveSpeed = 150f;
        else if (Input.GetKeyDown(KeyCode.LeftControl))
            moveSpeed = 20f;
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            moveSpeed = 150f;

        // Determine if movement should be allowed
        if (movement.x > 0 && cameraRB.position.x + movement.x > Border.east) { allowMovement = false; movement.x = 0; }
        if (movement.x < 0 && cameraRB.position.x + movement.x < Border.west) { allowMovement = false; movement.x = 0; }
        if (movement.y > 0 && cameraRB.position.y + movement.y > Border.north) { allowMovement = false; movement.y = 0; }
        if (movement.y < 0 && cameraRB.position.y + movement.y < Border.south) { allowMovement = false; movement.y = 0; }
        if (allowMovement) cameraRB.MovePosition(cameraRB.position + movement * moveSpeed * Time.fixedDeltaTime);

        // Reset movement variable
        allowMovement = true;
    }

    public void CalculateFPS()
    {
        fps.text = (int)(1f / Time.unscaledDeltaTime) + " fps";
    }
}
