using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTile : BaseTile
{
    // Ghos tile variables
    [HideInInspector] public SpriteRenderer icon;
    public List<Droneport> nearbyPorts;
    public bool isFree = false;
    public bool droneAssigned = false;

    // Get the sprite renderer
    public void Awake()
    {
        icon = GetComponent<SpriteRenderer>();
    }

    // Sets the ghost tile building
    public void SetBuilding(Buildable buildable, int metadata = -1)
    {
        this.buildable = buildable;
        this.metadata = metadata;
        icon.sprite = Sprites.GetSprite(buildable.building.name);
        transform.localScale = new Vector2(buildable.building.hologramSize, buildable.building.hologramSize);

        Events.active.GhostPlaced(this);
    }

    // Called when drone reaches target
    public override void ResetTile()
    {
        // Remove cells from Tile grid
        if (InstantiationHandler.active != null)
        {
            // Remove ghost cells
            foreach (Vector2Int cell in cells)
                InstantiationHandler.active.tileGrid.RemoveCell(cell);
        }

        Events.active.GhostDestroyed(this);
        Destroy(gameObject);
    }

    // Reset ghost tile
    public void CreateBuilding()
    {
        // Create building and destroy this game object
        if (buildable != null) Server.active.SrvSyncGhost(runtimeID, buildable.building.InternalID, transform.position, transform.rotation, metadata);
    }

    // Override onClick
    public override void OnClick()
    {
        // Disable
    }

    // Override all methods so as to not call them on Ghost tiles
    public override void DamageEntity(float dmg) { }
    public override void HealEntity(float amount) { }

    // Override destroy entity
    public override void DestroyEntity()
    {
        // Remove cells
        if (InstantiationHandler.active != null)
        {
            foreach (Vector2Int cell in cells)
                InstantiationHandler.active.tileGrid.RemoveCell(cell);
        }

        // Refund cost
        if (droneAssigned) Resource.active.RefundResources(buildable.resources);
        Events.active.GhostDestroyed(this);

        // Create particle and destroy
        Destroy(gameObject);
    }
}
