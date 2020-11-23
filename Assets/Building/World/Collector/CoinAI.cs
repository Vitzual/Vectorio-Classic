using UnityEngine;

public class CoinAI : MonoBehaviour
{
    // Tile layer
    [SerializeField] private LayerMask TileLayer;
    [SerializeField] private GameObject conveyors;
    [SerializeField] private int value = 1;
    private bool isMoving = false;
    private Transform target;
    private Transform lastTarget;
    private GameObject SRVSC;

    private void Start()
    {
        Physics2D.IgnoreCollision(conveyors.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        SRVSC = GameObject.Find("Survival");
    }

    // Start updating
    private void Update()
    {
        if (isMoving == false)
        {
            target = checkConveyors();
            if (target != null && target != lastTarget)
            {
                isMoving = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // If target gets removed, delete coin
            if (target == null)
            {
                Destroy(gameObject);
            }

            // Move coin position a step closer to the target.
            transform.position = Vector3.MoveTowards(transform.position, target.position, 5f * Time.deltaTime);
            if (Vector3.Distance(transform.position, target.position) < 0.001f)
            {
                if (target.name == "Hub")
                {
                    SRVSC.GetComponent<Survival>().AddGold(value);
                    Destroy(gameObject);
                }

                isMoving = false;
                lastTarget = target;
            }
        }
    }

    // Check surroundings for conveyor
    private Transform checkConveyors()
    {
        // Raycast tiles beside coin
        RaycastHit2D a = Physics2D.Raycast(new Vector2(transform.position.x + 5f, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D b = Physics2D.Raycast(new Vector2(transform.position.x - 5f, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D c = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 5f), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D d = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 5f), Vector2.zero, Mathf.Infinity, TileLayer);

        // Return a tile if it's a coneyor
        if (a.collider != null && (a.collider.name == "Conveyor" || a.collider.name == "Hub")) {return a.transform;}
        else if (b.collider != null && (b.collider.name == "Conveyor" || b.collider.name == "Hub")) {return b.transform;}
        else if (c.collider != null && (c.collider.name == "Conveyor" || c.collider.name == "Hub")) {return c.transform;}
        else if (d.collider != null && (d.collider.name == "Conveyor" || d.collider.name == "Hub")) {return d.transform;}
        return null;
    }
}
