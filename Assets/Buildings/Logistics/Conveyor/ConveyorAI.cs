using UnityEngine;

public class ConveyorAI : TileClass
{
    public int Rotation;
    public int Nearby = 0;
    public bool IsCorner = false;
    public Transform LastCheck;

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

    // Checks if leading into a nearby conveyor belt
    public  void CheckForConveyors(Transform Conveyor)
    {
        // Check if a call is being made twice
        if (Conveyor == LastCheck)
            return;
        else
            LastCheck = Conveyor;

        // Check if nearby conveyors
        Vector2 RayLoc;
        switch (Rotation)
        {
            case 1:
                RayLoc = new Vector2(transform.position.x, transform.position.y + 5);
                break;
            case 2:
                RayLoc = new Vector2(transform.position.x + 5, transform.position.y);
                break;
            case 3:
                RayLoc = new Vector2(transform.position.x, transform.position.y - 5);
                break;
            default:
                RayLoc = new Vector2(transform.position.x - 5, transform.position.y);
                break;
        }
        RaycastHit2D Target = Physics2D.Raycast(RayLoc, Vector2.zero, Mathf.Infinity, TileLayer);

        if (Target.transform != null && Target.transform.name == "Conveyor")
        {
            ConveyorAI ConveyorScript = Target.transform.GetComponent<ConveyorAI>();
            if (ConveyorScript.GetDirection() != Rotation && !ConveyorScript.IsCorner)
            {
                Debug.Log("Checking for conveyors");
                ConveyorScript.IncreaseNearby(1);
                int OtherNearby = ConveyorScript.GetNearbyAmount();
                if (OtherNearby == 1)
                {
                    Target.transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ConveyorTrio");
                    if ((Rotation == 3 || Rotation == 4) && ConveyorScript.GetDirection() != 4)
                        Target.transform.localEulerAngles = new Vector3(Target.transform.localEulerAngles.x + 180f, Target.transform.localEulerAngles.y, Target.transform.localEulerAngles.z);
                }
                else if (OtherNearby == 2)
                {
                    Target.transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ConveyorCross");
                }
            }
        }
    }

    // Checks if nearby a selling input
    public void CheckForSeller(Transform LastConveyor)
    {
        Debug.Log("Checking for a seller");
        RaycastHit2D top = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 5), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D right = Physics2D.Raycast(new Vector2(transform.position.x + 5, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D down = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 5), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D left = Physics2D.Raycast(new Vector2(transform.position.x - 5, transform.position.y), Vector2.zero, Mathf.Infinity, TileLayer);

        // If there is a seller above this tile
        if (top.collider != null && top.collider.name == "Hub" && transform.position.x == 0)
        {
            // If there is a conveyor to the right of this tile
            if (right.collider != null && right.collider.name == "Conveyor" && LastConveyor == right.collider.transform && right.collider.GetComponent<ConveyorAI>().GetDirection() == 4)
            {
                transform.localEulerAngles = new Vector3(0, 0, 180);
                EntranceDestination = new Vector3(transform.position.x + 1.25f, transform.position.y, 0);
                ExitDestination = new Vector3(transform.position.x, transform.position.y + 1.25f, 0);
                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ConveyorCorner");
                IsCorner = true;
                Rotation = 1;
                return;
            }

            // If there is a conevyor to the left of this tile
            else if (left.collider != null && left.collider.name == "Conveyor" && LastConveyor == left.collider.transform && left.collider.GetComponent<ConveyorAI>().GetDirection() == 2)
            {
                transform.localEulerAngles = new Vector3(180, 0, 0);
                EntranceDestination = new Vector3(transform.position.x - 1.25f, transform.position.y, 0);
                ExitDestination = new Vector3(transform.position.x, transform.position.y + 1.25f, 0);
                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ConveyorCorner");
                IsCorner = true;
                Rotation = 1;
                return;
            }
        }

        // If there is a seller to the right of this tile
        else if (right.collider != null && right.collider.name == "Hub" && transform.position.y == 0)
        {
            // If there is a conveyor above of this tile
            if (top.collider != null && top.collider.name == "Conveyor" && LastConveyor == top.collider.transform && top.collider.GetComponent<ConveyorAI>().GetDirection() == 3)
            {
                transform.localEulerAngles = new Vector3(180, 0, 90);
                EntranceDestination = new Vector3(transform.position.x, transform.position.y + 1.25f, 0);
                ExitDestination = new Vector3(transform.position.x + 1.25f, transform.position.y, 0);
                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ConveyorCorner");
                IsCorner = true;
                Rotation = 2;
                return;
            }

            // If there is a conveyor below this tile
            else if (down.collider != null && down.collider.name == "Conveyor" && LastConveyor == down.collider.transform && down.collider.GetComponent<ConveyorAI>().GetDirection() == 1)
            {
                transform.localEulerAngles = new Vector3(0, 0, 90);
                EntranceDestination = new Vector3(transform.position.x, transform.position.y - 1.25f, 0);
                ExitDestination = new Vector3(transform.position.x + 1.25f, transform.position.y, 0);
                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ConveyorCorner");
                IsCorner = true;
                Rotation = 2;
                return;
            }
        }

        // If there is a seller to the bottom of this tile
        else if (down.collider != null && down.collider.name == "Hub" && transform.position.x == 0)
        {

            // If there is a conveyor to the right of this tile
            if (right.collider != null && right.collider.name == "Conveyor" && LastConveyor == right.collider.transform && right.collider.GetComponent<ConveyorAI>().GetDirection() == 4)
            {
                transform.localEulerAngles = new Vector3(180, 0, 180);
                EntranceDestination = new Vector3(transform.position.x + 1.25f, transform.position.y, 0);
                ExitDestination = new Vector3(transform.position.x, transform.position.y - 1.25f, 0);
                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ConveyorCorner");
                IsCorner = true;
                Rotation = 3;
                return;
            }

            // If there is a conevyor to the left of this tile
            else if (left.collider != null && left.collider.name == "Conveyor" && LastConveyor == left.collider.transform && left.collider.GetComponent<ConveyorAI>().GetDirection() == 2)
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
                EntranceDestination = new Vector3(transform.position.x - 1.25f, transform.position.y, 0);
                ExitDestination = new Vector3(transform.position.x, transform.position.y - 1.25f, 0);
                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ConveyorCorner");
                IsCorner = true;
                Rotation = 2;
                return;
            }
        }

        // If there is a seller to the left of this tile
        else if (left.collider != null && left.collider.name == "Hub" && transform.position.y == 0)
        {
            Debug.Log("Found hub input to left of this tile");
            // If there is a conveyor above this tile
            if (top.collider != null && top.collider.name == "Conveyor" && LastConveyor == top.collider.transform && top.collider.GetComponent<ConveyorAI>().GetDirection() == 3)
            {
                transform.localEulerAngles = new Vector3(0, 0, 270);
                EntranceDestination = new Vector3(transform.position.x, transform.position.y + 1.25f, 0);
                ExitDestination = new Vector3(transform.position.x - 1.25f, transform.position.y, 0);
                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ConveyorCorner");
                IsCorner = true;
                Rotation = 4;
                return;
            }

            // If there is a conevyor below this tile
            else if (down.collider != null && down.collider.name == "Conveyor" && LastConveyor == down.collider.transform && down.collider.GetComponent<ConveyorAI>().GetDirection() == 1)
            {
                transform.localEulerAngles = new Vector3(180, 0, 270);
                EntranceDestination = new Vector3(transform.position.x, transform.position.y - 1.25f, 0);
                ExitDestination = new Vector3(transform.position.x - 1.25f, transform.position.y, 0);
                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ConveyorCorner");
                IsCorner = true;
                Rotation = 4;
                return;
            }
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
        if (top.collider != null && right.collider != null)
        {

            // If both raycasts are conveyors, calculate the corner
            if (top.collider.name == "Conveyor" && right.collider.name == "Conveyor")
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
                    IsCorner = true;
                    return;
                }
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
                IsCorner = true;
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
                IsCorner = true;
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
                IsCorner = true;
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
    public void IncreaseNearby(int a) { Nearby += a; }
    public int GetNearbyAmount() { return Nearby; }

    public override void DestroyTile()
    {
        GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
