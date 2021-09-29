using UnityEngine;

public class DefaultWall : BaseTile
{

    // Tile layer
    [SerializeField]
    private LayerMask TileLayer;

    // Wall auto place variables
    public int top = 0;
    public int right = 0;
    public int bottom = 0;
    public int left = 0;
    public int total = 0;

    public override void Setup()
    {
        // Set holder variables
        RaycastHit2D[] rayHits;

        // Check for walls on the right
        rayHits = Physics2D.RaycastAll(new Vector2(transform.position.x + 5f, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
        foreach (RaycastHit2D rayHit in rayHits)
            if (rayHit.collider != null && rayHit.collider.name.Contains("Wall"))
                SetWallStatus(3, 1, rayHit.collider.GetComponent<DefaultWall>());

        // Check for walls on the left
        rayHits = Physics2D.RaycastAll(new Vector2(transform.position.x - 5f, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
        foreach (RaycastHit2D rayHit in rayHits)
            if (rayHit.collider != null && rayHit.collider.name.Contains("Wall"))
                SetWallStatus(1, 3, rayHit.collider.GetComponent<DefaultWall>());

        // Check for walls on the top
        rayHits = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y +5f), Vector2.zero, Mathf.Infinity, TileLayer);
        foreach (RaycastHit2D rayHit in rayHits)
            if (rayHit.collider != null && rayHit.collider.name.Contains("Wall"))
                SetWallStatus(4, 2, rayHit.collider.GetComponent<DefaultWall>());

        // Check for walls on the bottom
        rayHits = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y - 5f), Vector2.zero, Mathf.Infinity, TileLayer);
        foreach (RaycastHit2D rayHit in rayHits)
            if (rayHit.collider != null && rayHit.collider.name.Contains("Wall"))
                SetWallStatus(2, 4, rayHit.collider.GetComponent<DefaultWall>());
    }

    public void SetWallStatus(int thisWallID, int otherWallID, DefaultWall otherWallScript)
    {
        if (thisWallID != 0) UpdateSprite(thisWallID);
        otherWallScript.UpdateSprite(otherWallID);
    }

    public override void DestroyEntity()
    {
        // Set holder variables
        RaycastHit2D[] rayHits;

        // Check for walls on the right
        rayHits = Physics2D.RaycastAll(new Vector2(transform.position.x + 5f, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
        foreach (RaycastHit2D rayHit in rayHits)
            if (rayHit.collider != null && rayHit.collider.name.Contains("Wall"))
                SetWallStatus(0, -1, rayHit.collider.GetComponent<DefaultWall>());

        // Check for walls on the left
        rayHits = Physics2D.RaycastAll(new Vector2(transform.position.x - 5f, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
        foreach (RaycastHit2D rayHit in rayHits)
            if (rayHit.collider != null && rayHit.collider.name.Contains("Wall"))
                SetWallStatus(0, -3, rayHit.collider.GetComponent<DefaultWall>());

        // Check for walls on the top
        rayHits = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y + 5f), Vector2.zero, Mathf.Infinity, TileLayer);
        foreach (RaycastHit2D rayHit in rayHits)
            if (rayHit.collider != null && rayHit.collider.name.Contains("Wall"))
                SetWallStatus(0, -2, rayHit.collider.GetComponent<DefaultWall>());

        // Check for walls on the bottom
        rayHits = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y - 5f), Vector2.zero, Mathf.Infinity, TileLayer);
        foreach (RaycastHit2D rayHit in rayHits)
            if (rayHit.collider != null && rayHit.collider.name.Contains("Wall"))
                SetWallStatus(0, -4, rayHit.collider.GetComponent<DefaultWall>());

        base.DestroyEntity();
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
            if (bottom == 1)
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
            else if (right == 1)
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180f));
            else if (top == 1)
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90f));
        }
        else if (total == 2)
        {
            if (top == bottom || left == right)
            {
                SROBJ.sprite = Resources.Load<Sprite>("Sprites/WallCross");
                if (top == 1 && bottom == 1)
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
                else
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else
            {
                SROBJ.sprite = Resources.Load<Sprite>("Sprites/WallCorner");
                if (left == 1 && bottom == 1)
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90f));
                else if (bottom == 1 && right == 1)
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0f));
                else if (right == 1 && top == 1)
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
                else if (top == 1 && left == 1)
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180f));
            }
        }
        else if (total == 3)
        {
            SROBJ.sprite = Resources.Load<Sprite>("Sprites/WallTrio");
            if (top == 0)
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0f));
            else if (right == 0)
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90f));
            else if (bottom == 0)
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -180f));
            else
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
        }
        else if (total == 4)
        {
            SROBJ.sprite = Resources.Load<Sprite>("Sprites/WallQuad");
        }
    }
}
