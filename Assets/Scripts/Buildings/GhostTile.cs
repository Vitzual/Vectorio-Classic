using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTile : BaseTile
{
    // Ghos tile variables
    [HideInInspector] public Building building;
    [HideInInspector] public bool droneAssigned;
    [HideInInspector] public SpriteRenderer icon;

    // Sets the ghost tile building
    public void SetBuilding(Building building)
    {
        this.building = building;
        icon.sprite = Sprites.GetSprite(building.name);
    }

    // Called when drone reaches target
    public void CreateBuilding()
    {
        InstantiationHandler.active.CreateBuilding(building, transform.position, transform.rotation);
        Recycler.AddRecyclable(transform);
    }

    // Override all methods so as to not call them on Ghost tiles
    public override void DamageEntity(float dmg) { }
    public override void DestroyEntity() { }
    public override void HealEntity(float amount) { }
}
