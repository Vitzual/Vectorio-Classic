//using Mirror;
using UnityEngine;

// This script is ported from Automa.
// https://github.com/Vitzual/Automa

public class BuildingController : MonoBehaviour
{
    // Selected tile
    public Transform hologram;
    private Entity entity;
    private bool isEnemy = false;

    // Sprite values
    private SpriteRenderer spriteRenderer;
    private float alphaAdjust = 0.005f;
    private float alphaHolder;

    public void Start()
    {
        // Set event
        UIEvents.active.onEntityPressed += SetBuildable;

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
            if (entity != null) CmdCreateBuildable();
            else
            {
                // BaseTile holder = InstantiationHandler.active.TryGetBuilding(hologram.position);
                //if (holder != null) Events.active.BuildingClicked(holder);
            }
        }
        else if (Input.GetKey(Keybinds.rmb)) CmdDestroyBuilding();
        else if (Input.GetKeyDown(Keybinds.rotate)) RotatePosition();
        else if (Input.GetKeyDown(Keybinds.rmb)
            || Input.GetKeyDown(Keybinds.escape)) SetBuildable(null);
    }

    // Create building (command)
    //[Command]
    public void CmdCreateBuildable()
    {
        if (InstantiationHandler.active != null)
            InstantiationHandler.active.CreateEntity(entity, hologram.position, hologram.rotation, isEnemy);
        else Debug.LogError("Scene does not have active building handler!");
    }

    // Delete building (command)
    //[Command]
    public void CmdDestroyBuilding()
    {
        if (InstantiationHandler.active != null)
            InstantiationHandler.active.RpcDestroyBuilding(hologram.position);
        else Debug.LogError("Scene does not have active building handler!");
    }

    // Sets the selected building (null to deselect)
    public void SetBuildable(Entity entity, bool isEnemy = false)
    {
        // Set tile 
        this.entity = entity;
        this.isEnemy = isEnemy;

        if (entity != null)
        {
            // Get the tile sprite and set offset
            spriteRenderer.sprite = Sprites.GetSprite(entity.name);

            // Set the scale
            hologram.localScale = new Vector2(entity.hologramSize, entity.hologramSize);
        }
        else spriteRenderer.sprite = Sprites.GetSprite("Transparent");
    }

    // Uses the offset value from the Tile SO to center the object
    private void UpdatePosition()
    {
        // Update position to mouse pointer
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPosition;

        if (!isEnemy)
        {
            if (entity != null) newPosition = new Vector2(5 * Mathf.Round(mousePos.x / 5) + entity.offset.x, 5 * Mathf.Round(mousePos.y / 5) + entity.offset.y);
            else newPosition = new Vector2(5 * Mathf.Round(mousePos.x / 5), 5 * Mathf.Round(mousePos.y / 5));
        }
        else newPosition = mousePos;

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