using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTile : MonoBehaviour
{
    [HideInInspector] public Building building;
    [HideInInspector] public bool droneAssigned;
    [HideInInspector] public SpriteRenderer icon;

    public void Setup(Building building)
    {
        this.building = building;
        icon.sprite = Sprites.GetSprite(building.name);
    }

    public void ConstructBuilding()
    {
        InstantiationHandler.active.CreateBuilding(building, transform.position, transform.rotation);
        Recycler.AddRecyclable(transform);
    }
}
