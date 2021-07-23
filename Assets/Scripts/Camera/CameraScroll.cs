using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    private Camera cam;
    private static float targetZoom;
    private float zoomFactor = 150f;
    [SerializeField]
    private float zoomSpeed = 40;
    private bool gridActive = true;
    public GameObject grid;
    public Interface UI;
    public Tutorial tutorial;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
    }


    // Update is called once per frame
    void Update()
    {
        if (UI.BuildingOpen) return;
        else if (UI.ResearchOpen) return;
        else if (tutorial.disableMoving) return;

        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, 15f, 350f);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

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

    public static float getZoom()
    {
        return targetZoom;
    }
}
