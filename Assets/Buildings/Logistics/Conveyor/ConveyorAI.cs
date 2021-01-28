using UnityEngine;

public class ConveyorAI : TileClass
{
    public int Rotation;

    // Conveyor variables
    public Vector3 EntranceDestination;
    public Vector3 ExitDestination;
    public bool EntranceOccupied;
    public bool ExitOccupied;

    // Tile layer
    [SerializeField]
    private LayerMask TileLayer;

    // On start, calculate destination based on rotation
    private void Start()
    {
        // Set default points
        EntranceOccupied = false;
        ExitOccupied = false;
    }

    // Changes the conveyors rotation and re-calculates entry / exit positions 
    public void ChangeRotation(float rotation)
    {
        transform.localEulerAngles = new Vector3(0, 0, rotation);

        // Get destination
        if (transform.localEulerAngles.z >= 270f)
        {
            EntranceDestination = new Vector3(transform.position.x, transform.position.y + 1.25f, 0);
            ExitDestination = new Vector3(transform.position.x, transform.position.y - 1.25f, 0);
            Rotation = 3;
        }
        else if (transform.localEulerAngles.z >= 180f)
        {
            EntranceDestination = new Vector3(transform.position.x + 1.25f, transform.position.y, 0);
            ExitDestination = new Vector3(transform.position.x - 1.25f, transform.position.y, 0);
            Rotation = 4;
        }
        else if (transform.localEulerAngles.z >= 90f)
        {
            EntranceDestination = new Vector3(transform.position.x, transform.position.y - 1.25f, 0);
            ExitDestination = new Vector3(transform.position.x, transform.position.y + 1.25f, 0);
            Rotation = 1;
        }
        else
        {
            EntranceDestination = new Vector3(transform.position.x - 1.25f, transform.position.y, 0);
            ExitDestination = new Vector3(transform.position.x + 1.25f, transform.position.y, 0);
            Rotation = 2;
        }
    }

    // Calculate conveyor corner logic 
    public void CalculateCorner(Transform LastConveyor, Transform CurrentConveyor)
    {
        RaycastHit2D top = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 5), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D right = Physics2D.Raycast(new Vector2(transform.position.x + 5, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D down = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 5), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D left = Physics2D.Raycast(new Vector2(transform.position.x - 5, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);



        // Conveyor on top and right
        if (top.collider != null && right.collider != null && top.collider.name == "Conveyor" && right.collider.name == "Conveyor")
        {
            // Check to make sure conveyors are recent
            if ((top.collider.transform == LastConveyor || top.collider.transform == CurrentConveyor)
                && (right.collider.transform == LastConveyor || right.collider.transform == CurrentConveyor))
            {
                // Conveyor above is an input conveyor
                if (top.collider.GetComponent<ConveyorAI>().GetDirection() == 3)
                {
                    transform.localEulerAngles = new Vector3(180, 0, 90);
                    EntranceDestination = new Vector3(transform.position.x, transform.position.y + 1.25f, 0);
                    ExitDestination = new Vector3(transform.position.x + 1.25f, transform.position.y, 0);
                    Rotation = 2;
                }

                // Conveyor above is an output conveyor
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, 180);
                    EntranceDestination = new Vector3(transform.position.x + 1.25f, transform.position.y, 0);
                    ExitDestination = new Vector3(transform.position.x, transform.position.y + 1.25f, 0);
                    Rotation = 1;
                }
                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ConveyorCorner");
                return;
            }
        }

        // Conveyor on right and bottom
        if (right.collider != null && down.collider != null && right.collider.name == "Conveyor" && down.collider.name == "Conveyor")
        {
            // Check to make sure conveyors are recent
            if ((right.collider.transform == LastConveyor || right.collider.transform == CurrentConveyor)
                && (down.collider.transform == LastConveyor || down.collider.transform == CurrentConveyor))
            {
                // Conveyor right is an input conveyor
                if (right.collider.GetComponent<ConveyorAI>().GetDirection() == 4)
                {
                    transform.localEulerAngles = new Vector3(180, 0, 180);
                    EntranceDestination = new Vector3(transform.position.x + 1.25f, transform.position.y, 0);
                    ExitDestination = new Vector3(transform.position.x, transform.position.y - 1.25f, 0);
                    Rotation = 3;
                }

                // Conveyor right is an output conveyor
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, 90);
                    EntranceDestination = new Vector3(transform.position.x, transform.position.y - 1.25f, 0);
                    ExitDestination = new Vector3(transform.position.x + 1.25f, transform.position.y, 0);
                    Rotation = 2;
                }

                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ConveyorCorner");
                return;
            }
        }

        // Conveyor on bottom and left
        if (down.collider != null && left.collider != null && down.collider.name == "Conveyor" && left.collider.name == "Conveyor")
        {
            // Check to make sure conveyors are recent
            if ((down.collider.transform == LastConveyor || down.collider.transform == CurrentConveyor)
                && (left.collider.transform == LastConveyor || left.collider.transform == CurrentConveyor))
            {
                // Conveyor bottom is an input conveyor
                if (down.collider.GetComponent<ConveyorAI>().GetDirection() == 1)
                {
                    transform.localEulerAngles = new Vector3(180, 0, 270);
                    EntranceDestination = new Vector3(transform.position.x, transform.position.y - 1.25f, 0);
                    ExitDestination = new Vector3(transform.position.x - 1.25f, transform.position.y, 0);
                    Rotation = 4;
                }

                // Conveyor bottom is an output conveyor
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                    EntranceDestination = new Vector3(transform.position.x - 1.25f, transform.position.y, 0);
                    ExitDestination = new Vector3(transform.position.x, transform.position.y - 1.25f, 0);
                    Rotation = 3;
                }

                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ConveyorCorner");
                return;
            }
        }

        // Conveyor on left and top
        if (left.collider != null && top.collider != null && left.collider.name == "Conveyor" && top.collider.name == "Conveyor")
        {
            // Check to make sure conveyors are recent
            if ((left.collider.transform == LastConveyor || left.collider.transform == CurrentConveyor)
                && (top.collider.transform == LastConveyor || top.collider.transform == CurrentConveyor))
            {
                // Conveyor left is an input conveyor
                if (left.collider.GetComponent<ConveyorAI>().GetDirection() == 2)
                {
                    transform.localEulerAngles = new Vector3(180, 0, 0);
                    EntranceDestination = new Vector3(transform.position.x - 1.25f, transform.position.y, 0);
                    ExitDestination = new Vector3(transform.position.x, transform.position.y + 1.25f, 0);
                    Rotation = 1;
                }

                // Conveyor left is an output conveyor
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, 270);
                    EntranceDestination = new Vector3(transform.position.x, transform.position.y + 1.25f, 0);
                    ExitDestination = new Vector3(transform.position.x - 1.25f, transform.position.y, 0);
                    Rotation = 4;
                }

                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ConveyorCorner");
                return;
            }
        }
    }

    public bool ValidRotation(int a) { return true; /*return Rotation == a;*/ }
    public int GetDirection() { return Rotation; }
    public void SetEntranceStatus(bool a) { EntranceOccupied = a; }
    public void SetExitStatus(bool a) { ExitOccupied = a; }
    public bool IsEntranceOccupied() { return EntranceOccupied; }
    public bool IsExitOccupied() { return ExitOccupied; }
    public Vector3 GetEntranceLocation() { return EntranceDestination; }
    public Vector3 GetExitLocation() { return ExitDestination; }

    public override void DestroyTile()
    {
        GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
