using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    private Camera cam;
    private float targetZoom;
    private float zoomFactor = 150f;
    [SerializeField]
    private float zoomSpeed = 40;
    private bool gridActive = true;
    public GameObject grid;
    public Interface UI;

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

    public float getZoom()
    {
        return targetZoom;
    }
}
