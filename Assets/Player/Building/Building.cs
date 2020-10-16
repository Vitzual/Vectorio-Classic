using UnityEngine;

public class Building : Player
{

    [SerializeField]
    private GameObject placeObject;
    [SerializeField]
    private LayerMask tileLayer;
    private Vector2 mousePos;
    protected float placementRate = 0.1f;
    protected float nextPlacement = -1f;

    void Update()
    {
        // Get mouse position and round to middle grid coordinate
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));

        // If user left clicks, place object
        if (Input.GetButtonDown("Fire1"))
        {

            Vector2 mouseRay = Camera.main.ScreenToWorldPoint(transform.position);
            RaycastHit2D rayHit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, tileLayer);

            // Raycast tile to see if there is already a tile placed
            if (rayHit.collider == null)
            {
                Instantiate(placeObject, transform.position, Quaternion.identity);
            }
        }

        // If user right clicks, place object
        if (Input.GetButtonDown("Fire2"))
        {

            Vector2 mouseRay = Camera.main.ScreenToWorldPoint(transform.position);
            RaycastHit2D rayHit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, tileLayer);

            // Raycast tile to see if there is already a tile placed
            if (rayHit.collider != null)
            {
                Destroy(rayHit.collider.gameObject);
            }
        }
    }
}
