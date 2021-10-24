//using Mirror;
using UnityEngine;

// This script is ported from Automa.
// https://github.com/Vitzual/Automa

public class BuildingController : MonoBehaviour
{
    // Selected tile
    public Transform hologram;
    private Building building;

    // Sprite values
    private SpriteRenderer spriteRenderer;
    private float alphaAdjust = 0.005f;
    private float alphaHolder;

    public void Start()
    {
        // Confirm user has authority
        //if (!hasAuthority) return;

        // Grab sprite renderer component
        spriteRenderer = hologram.GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        // Confirm user has authority
        //if (!hasAuthority || Tablet.active) return;

        // Update position and sprite transparency
        UpdatePosition();
        AdjustTransparency();
        CheckInput();
    }

    //[Client]
    public void CheckInput()
    {
        // Clicking input check
        if (Input.GetKey(Keybinds.lmb))
        {
            if (building != null) CmdCreateBuilding();
            else
            {
                BaseTile holder = BuildingHandler.active.TryGetBuilding(hologram.position);
                //if (holder != null) Events.active.BuildingClicked(holder);
            }
        }
        else if (Input.GetKey(Keybinds.rmb)) CmdDestroyBuilding();
        else if (Input.GetKeyDown(Keybinds.rotate)) RotatePosition();
        else if (Input.GetKeyDown(Keybinds.rmb)
            || Input.GetKeyDown(Keybinds.escape)) SetBuilding(null);
    }

    // Create building (command)
    //[Command]
    public void CmdCreateBuilding()
    {
        if (BuildingHandler.active != null)
            BuildingHandler.active.CreateBuilding(building, hologram.position, hologram.rotation);
        else Debug.LogError("Scene does not have active building handler!");
    }

    // Delete building (command)
    //[Command]
    public void CmdDestroyBuilding()
    {
        if (BuildingHandler.active != null)
            BuildingHandler.active.RpcDestroyBuilding(hologram.position);
        else Debug.LogError("Scene does not have active building handler!");
    }

    // Sets the selected building (null to deselect)
    public void SetBuilding(Building building)
    {
        // Set tile 
        this.building = building;

        if (building != null)
        {
            // Get the tile sprite and set offset
            spriteRenderer.sprite = Sprites.GetSprite(building.name);
        }
        else spriteRenderer.sprite = Sprites.GetSprite("Empty");
    }

    // Uses the offset value from the Tile SO to center the object
    private void UpdatePosition()
    {
        // Update position to mouse pointer
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPosition;

        if (building != null) newPosition = new Vector2(5 * Mathf.Round(mousePos.x / 5) + building.offset.x, 5 * Mathf.Round(mousePos.y / 5) + building.offset.y);
        else newPosition = new Vector2(5 * Mathf.Round(mousePos.x / 5), 5 * Mathf.Round(mousePos.y / 5));

        hologram.position = newPosition;
    }

    // Rotates an object
    private void RotatePosition()
    {
        hologram.Rotate(0, 0, -90);
    }

    // Adjusts the alpha transparency of the SR component 
    private void AdjustTransparency()
    {
        // Switches
        if (spriteRenderer.color.a >= 1f)
            alphaHolder = -alphaAdjust;
        else if (spriteRenderer.color.a <= 0f)
            alphaHolder = alphaAdjust;

        // Set alpha
        spriteRenderer.color = new Color(1f, 1f, 1f, spriteRenderer.color.a + alphaHolder);
    }
}