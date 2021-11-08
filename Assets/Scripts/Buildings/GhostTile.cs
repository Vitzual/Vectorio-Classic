using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTile : BaseTile
{
    // Ghos tile variables
    [HideInInspector] public Building building;
    [HideInInspector] public SpriteRenderer icon;

    // Get the sprite renderer
    public void Awake()
    {
        icon = GetComponent<SpriteRenderer>();
    }

    // Sets the ghost tile building
    public void SetBuilding(Building building)
    {
        this.building = building;
        icon.sprite = Sprites.GetSprite(building.name);
    }

    // Called when drone reaches target
    public void CreateBuilding()
    {
        // Remove cells from Tile grid
        if (InstantiationHandler.active != null)
        {
            foreach (Vector2Int cell in cells)
                InstantiationHandler.active.tileGrid.RemoveCell(cell);

            // Create building and destroy this game object
            InstantiationHandler.active.CreateBuilding(building, transform.position, transform.rotation, false);
        }
        Destroy(gameObject);
    }

    // Override all methods so as to not call them on Ghost tiles
    public override void DamageEntity(float dmg) { }
    public override void HealEntity(float amount) { }
}
