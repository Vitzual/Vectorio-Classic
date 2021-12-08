using Mirror;
using UnityEngine;

// This script is ported from Automa.
// https://github.com/Vitzual/Automa

public class BuildingController : NetworkBehaviour
{
    // Entity selected
    public static bool entitySelected = false;

    // Selected tile
    public Transform hologram, squareRadius, circleRadius;
    public Entity entity;
    public Variant variant;
    public Buildable buildable;
    public int metadata = -1;

    // Sprite values
    private SpriteRenderer spriteRenderer;
    private float alphaAdjust = 0.005f;
    private float alphaHolder;

    bool justDeselected = false;

    public void Start()
    {
        // Confirm user has authority
        if (!hasAuthority) return;
        else Debug.Log("[SERVER] Authority granted to instance " + transform.name);

        // Grab sprite renderer component
        spriteRenderer = hologram.GetComponent<SpriteRenderer>();

        // Set input events
        if (InputEvents.active != null)
        {
            Debug.Log("Assigning input events");
            InputEvents.active.onLeftMousePressed += TryCreateEntity;
            InputEvents.active.onRightMousePressed += TryDestroyBuilding;
            InputEvents.active.onRightMouseReleased += DisableJustDeselected;
            InputEvents.active.onEscapePressed += TryDeselectEntity;
            InputEvents.active.onPipettePressed += TryPipetteBuilding;
        }

        // Set UI events
        if (UIEvents.active != null) 
        {
            Debug.Log("Assigning interface events");
            UIEvents.active.onEntityPressed += SetEntity;
            UIEvents.active.onBuildablePressed += SetBuilding;
        }

        Debug.Log("Finished setting up controller instance");
    }

    public void Update()
    {
        // Confirm user has authority
        if (!hasAuthority) return;

        // Update position and sprite transparency
        UpdatePosition();
        AdjustTransparency();
    }

    public void TryClickBuilding()
    {
        if (StatsPanel.isOpen) return;

        BaseTile holder = InstantiationHandler.active.TryGetBuilding(hologram.position);
        if (holder != null) holder.OnClick();
    }

    public void TryPipetteBuilding()
    {
        if (StatsPanel.isOpen) return;

        BaseTile holder = InstantiationHandler.active.TryGetBuilding(hologram.position);
        if (holder != null && holder.isSellable)
        {
            Debug.Log("Pipetted " + holder.name + " with metadata " + holder.metadata);
            SetBuilding(holder.buildable, holder.metadata);
            Events.active.Pipette(holder);
        }
    }

    public void TryCreateEntity()
    {
        if (StatsPanel.isOpen) return;

        if (buildable != null) CreateEntity(buildable.building.InternalID, hologram.position, hologram.rotation, metadata);
        else TryClickBuilding();
    }

    [Command]
    public void CreateEntity(string entity_id, Vector2 position, Quaternion rotation, int metadata)
    {
        Syncer.active.SrvSyncBuildable(entity_id, position, rotation, metadata);
    }

    // Delete building (command)
    public void DestroyBuilding()
    {
        if (InstantiationHandler.active != null)
            InstantiationHandler.active.RpcDestroyBuilding(hologram.position);
        else Debug.LogError("Scene does not have active building handler!");
    }

    // Sets the selected entity (null to deselect)
    public void SetEntity(Entity entity, int metadata = -1)
    {
        this.metadata = metadata;
        entitySelected = entity != null;

        if (entitySelected)
        {
            Buildable buildable = Buildables.RequestBuildable(entity);
            if (buildable != null)
            {
                SetBuilding(buildable);
                return;
            }
        }

        circleRadius.gameObject.SetActive(false);
        squareRadius.gameObject.SetActive(false);

        buildable = null;
        this.entity = entity;
        UpdateSprite();
    }

    // Sets the selected building (null to deselect)
    public void SetBuilding(Buildable buildable, int metadata = -1)
    {
        this.metadata = metadata;
        entitySelected = buildable != null;

        if (entitySelected)
        {
            DefaultTurret turret = buildable.obj.GetComponent<DefaultTurret>();
            if (turret != null)
            {
                circleRadius.localScale = new Vector3(turret.turret.range * 2, 0, turret.turret.range * 2);
                circleRadius.gameObject.SetActive(true);
                squareRadius.gameObject.SetActive(false);
            }
            else
            {
                if (buildable.building.useSquareRange)
                {
                    squareRadius.localScale = new Vector2(buildable.building.squareRange, buildable.building.squareRange);
                    circleRadius.gameObject.SetActive(false);
                    squareRadius.gameObject.SetActive(true);
                }
                else if (buildable.building.useCircleRange)
                {
                    circleRadius.localScale = new Vector3(buildable.building.circleRange, 0, buildable.building.circleRange);
                    squareRadius.gameObject.SetActive(false);
                    circleRadius.gameObject.SetActive(true);
                }
                else
                {
                    circleRadius.gameObject.SetActive(false);
                    squareRadius.gameObject.SetActive(false);
                }
            }
            this.buildable = buildable;
            entity = buildable.building;
        }
        else
        {
            entity = null;
            this.buildable = null;
            circleRadius.gameObject.SetActive(false);
            squareRadius.gameObject.SetActive(false);
        }

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

        if (buildable != null)
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
        else if (!justDeselected) DestroyBuilding();
    }

    public void TryDeselectEntity() 
    {
        if (GuardianHandler.animInProgress) return;

        if (entity != null) 
        {
            if (NewInterface.active.CheckPanels())
                SetEntity(null);
        }
        else NewInterface.active.ToggleQuitMenu();
    }
    public void DeselectEntity() { SetEntity(null); }
    public void DisableJustDeselected() { justDeselected = false; }
}