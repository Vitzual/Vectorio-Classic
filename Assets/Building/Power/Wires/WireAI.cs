using UnityEngine;

public class WireAI : TileClass
{

    // Tile layer
    [SerializeField]
    private LayerMask TileLayer;
    public bool powered = false;

    // Wall auto place variables
    int top = 0;
    int right = 0;
    int bottom = 0;
    int left = 0;
    int total = 0;

    public void Start()
    {
        RaycastHit2D a = Physics2D.Raycast(new Vector2(transform.position.x + 5f, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D b = Physics2D.Raycast(new Vector2(transform.position.x - 5f, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D c = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 5f), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D d = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 5f), Vector2.zero, Mathf.Infinity, TileLayer);

        // Hub checks
        if (a.collider != null && a.collider.name == "Hub")
        {
            if (transform.position.y == 0)
            {
                powered = true;
                UpdateSprite(3);
            }
        }
        if (b.collider != null && b.collider.name == "Hub")
        {
            if (transform.position.y == 0)
            {
                powered = true;
                UpdateSprite(1);
            }
        }
        if (c.collider != null && c.collider.name == "Hub")
        {
            if (transform.position.y == 0)
            {
                powered = true;
                UpdateSprite(4);
            }
        }
        if (d.collider != null && d.collider.name == "Hub")
        {
            if (transform.position.y == 0)
            {
                powered = true;
                UpdateSprite(2);
            }
        }

        // Wire checks
        if (a.collider != null && a.collider.name != "Wall")
        {
            if (a.collider.name == "Wire" && a.collider.GetComponent<WireAI>().getPowered())
            {
                powered = true;
            }
            UpdateSprite(3);
        }
        if (b.collider != null && b.collider.name != "Wall")
        {
            if (b.collider.name == "Wire" && b.collider.GetComponent<WireAI>().getPowered())
            {
                powered = true;
            }
            UpdateSprite(1);
        }
        if (c.collider != null && c.collider.name != "Wall")
        {
            if (c.collider.name == "Wire" && c.collider.GetComponent<WireAI>().getPowered())
            {
                powered = true;
            }
            UpdateSprite(4);
        }
        if (d.collider != null && d.collider.name != "Wall")
        {
            if (d.collider.name == "Wire" && d.collider.GetComponent<WireAI>().getPowered())
            {
                powered = true;
            }
            UpdateSprite(2);
        }
    }

    public override void DestroyTile()
    {
        RaycastHit2D a = Physics2D.Raycast(new Vector2(transform.position.x + 5f, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D b = Physics2D.Raycast(new Vector2(transform.position.x - 5f, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D c = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 5f), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D d = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 5f), Vector2.zero, Mathf.Infinity, TileLayer);
        if (a.collider != null && a.collider.name == "Wire")
        {
            a.collider.GetComponent<WireAI>().UpdateSprite(-1);
        }
        if (b.collider != null && b.collider.name == "Wire")
        {
            b.collider.GetComponent<WireAI>().UpdateSprite(-3);
        }
        if (c.collider != null && c.collider.name == "Wire")
        {
            c.collider.GetComponent<WireAI>().UpdateSprite(-2);
        }
        if (d.collider != null && d.collider.name == "Wire")
        {
            d.collider.GetComponent<WireAI>().UpdateSprite(-4);
        }

        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public int CheckTotal()
    {
        return top + right + bottom + left;
    }

    public void UpdateVertices(int a)
    {
        if (a == 4)
        {
            top = 1;
        }
        else if (a == 3)
        {
            right = 1;
        }
        else if (a == 2)
        {
            bottom = 1;
        }
        else if (a == 1)
        {
            left = 1;
        }
        else if (a == -1)
        {
            left = 0;
        }
        else if (a == -2)
        {
            bottom = 0;
        }
        else if (a == -3)
        {
            right = 0;
        }
        else if (a == -4)
        {
            top = 0;
        }
    }

    public void UpdateSprite(int a)
    {
        UpdateVertices(a);
        total = CheckTotal();
        SpriteRenderer SROBJ = gameObject.GetComponent<SpriteRenderer>();
        if (total == 1)
        {
            SROBJ.sprite = Resources.Load<Sprite>("Sprites/WireEnd");
            if (bottom == 1)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
            }
            else if (right == 1)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180f));
            } 
            else if (top == 1)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90f));
            }
        }
        else if (total == 2)
        {
            if (top == bottom || left == right)
            {
                SROBJ.sprite = Resources.Load<Sprite>("Sprites/WireOne");
                if (top == 1 && bottom == 1)
                {
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
            }
            else
            {
                SROBJ.sprite = Resources.Load<Sprite>("Sprites/WireTwo");
                if (left == 1 && bottom == 1) 
                {
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90f));
                }
                else if (bottom == 1 && right == 1)
                {
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0f));
                }
                else if (right == 1 && top == 1)
                {
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
                }
                else if (top == 1 && left == 1)
                {
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180f));
                }
            }
        }
        else if (total == 3)
        {
            SROBJ.sprite = Resources.Load<Sprite>("Sprites/WireThree");
            if (top == 0)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0f));
            }
            else if (right == 0)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90f));
            }
            else if (bottom == 0)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -180f));
            } 
            else
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
            }
        }
        else if (total == 4)
        {
            SROBJ.sprite = Resources.Load<Sprite>("Sprites/WireFour");
        }
    }

    public bool getPowered()
    {
        return powered;
    }
}
