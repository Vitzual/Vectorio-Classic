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

    // Changes the conveyors rotation an re-calculates entry / exit positions 
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
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
