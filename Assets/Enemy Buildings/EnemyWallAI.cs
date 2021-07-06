using UnityEngine;

public class EnemyWallAI : TileClass
{
    // Tile layer
    [SerializeField]
    private LayerMask TileLayer;

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
        if (a.collider != null && a.collider.name.Contains("Enemy Wall"))
        {
            a.collider.GetComponent<EnemyWallAI>().UpdateSprite(1);
            UpdateSprite(3);
        }
        if (b.collider != null && b.collider.name.Contains("Enemy Wall"))
        {
            b.collider.GetComponent<EnemyWallAI>().UpdateSprite(3);
            UpdateSprite(1);
        }
        if (c.collider != null && c.collider.name.Contains("Enemy Wall"))
        {
            c.collider.GetComponent<EnemyWallAI>().UpdateSprite(2);
            UpdateSprite(4);
        }
        if (d.collider != null && d.collider.name.Contains("Enemy Wall"))
        {
            d.collider.GetComponent<EnemyWallAI>().UpdateSprite(4);
            UpdateSprite(2);
        }
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
        if (total == 0)
        {
            SROBJ.sprite = Resources.Load<Sprite>("Sprites/Wall");
        }
        else if (total == 1)
        {
            SROBJ.sprite = Resources.Load<Sprite>("Sprites/WallDuo");
            if (transform.name != "Heavy Wall")
            {
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
        }
        else if (total == 2)
        {
            if (top == bottom || left == right)
            {
                SROBJ.sprite = Resources.Load<Sprite>("Sprites/WallCross");
                if (transform.name != "Heavy Wall")
                {
                    if (top == 1 && bottom == 1)
                    {
                        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
                    }
                    else
                    {
                        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                }
            }
            else
            {
                SROBJ.sprite = Resources.Load<Sprite>("Sprites/WallCorner");
                if (transform.name != "Heavy Wall")
                {
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
        }
        else if (total == 3)
        {
            SROBJ.sprite = Resources.Load<Sprite>("Sprites/WallTrio");
            if (transform.name != "Heavy Wall")
            {
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
        }
        else if (total == 4)
        {
            SROBJ.sprite = Resources.Load<Sprite>("Sprites/WallQuad");
        }
    }
}
