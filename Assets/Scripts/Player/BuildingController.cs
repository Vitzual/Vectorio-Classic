//using Mirror;
using UnityEngine;

// This script is ported from Automa.
// https://github.com/Vitzual/Automa

public class BuildingController : MonoBehaviour
{
    // Selected tile
    public Transform hologram;
    private Entity entity;
    private Building building;

    // Sprite values
    private SpriteRenderer spriteRenderer;
    private float alphaAdjust = 0.005f;
    private float alphaHolder;

    bool justDeselected = false;

    public void Start()
    {
        // Confirm user has authority
        //if (!hasAuthority) return;

        // Grab sprite renderer component
        spriteRenderer = hologram.GetComponent<SpriteRenderer>();

        // Set input events
        if (InputEvents.active != null)
        {
            InputEvents.active.onLeftMousePressed += TryCreateEntity;
            InputEvents.active.onRightMousePressed += TryDestroyBuilding;
            InputEvents.active.onRightMouseReleased += DisableJustDeselected;
            InputEvents.active.onEscapePressed += TryDeselectEntity;
        }

        // Set UI events
        if (UIEvents.active != null) 
        { 
            UIEvents.active.onEntityPressed += SetEntity;
            UIEvents.active.onBuildingPressed += SetBuilding;
        }
    }

    public void Update()
    {
        // Confirm user has authority
        //if (!hasAuthority || Tablet.active) return;

        // Update position and sprite transparency
        UpdatePosition();
        AdjustTransparency();
    }

    public void TryCreateEntity()
    {
        if (entity != null) CmdCreateBuildable();
        else
        {
            BaseTile holder = InstantiationHandler.active.TryGetBuilding(hologram.position);
            if (holder != null) holder.OnClick();
        }
    }

    // Create building (command)
    //[Command]
    public void CmdCreateBuildable()
    {
        if (InstantiationHandler.active != null)
        {
            if (building != null) InstantiationHandler.active.CreateBuilding(building, hologram.position, hologram.rotation);
            else if (entity != null) InstantiationHandler.active.CreateEnemy(entity, hologram.position, hologram.rotation);
        }
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

    // Sets the selected entity (null to deselect)
    public void SetEntity(Entity entity)
    {
        building = null;
        this.entity = entity;
        UpdateSprite();
    }

    // Sets the selected building (null to deselect)
    public void SetBuilding(Building building)
    {
        this.building = building;
        entity = building;
        UpdateSprite();
    }

    // Updates the selected sprite
    public void UpdateSprite()
    {
        if (entity != null)
        {
            spriteRenderer.sprite = Sprites.GetSprite(entity.name);
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

        if (building != null)
        {
            if (entity != null) newPosition = new Vector2(5 * Mathf.Round(mousePos.x / 5) + entity.gridOffset.x, 5 * Mathf.Round(mousePos.y / 5) + entity.gridOffset.y);
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

    public void TryDestroyBuilding()
    {
        if (entity != null)
        {
            justDeselected = true;
            TryDeselectEntity();
        }
        else if (!justDeselected) CmdDestroyBuilding();
    }

    public void TryDeselectEntity() { if (!NewInterface.isOpen) SetEntity(null); }
    public void DeselectEntity() { SetEntity(null); }
    public void DisableJustDeselected() { justDeselected = false; }
}