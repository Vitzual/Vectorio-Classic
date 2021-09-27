using UnityEngine;
using Mirror;
using System.Collections.Generic;

[HideInInspector]
public class DefaultBuilding : DefaultEntity
{
    [HideInInspector]
    public List<Vector2Int> cells;

    public override void DestroyEntity()
    {
        if (BuildingSystem.active != null)
        {
            foreach (Vector2Int cell in cells)
                BuildingSystem.active.tileGrid.RemoveCell(cell);
        }

        if (particle != null)
            Instantiate(particle, transform.position, transform.rotation);
        Recycler.AddRecyclable(transform);
    }

    // If a collision is detected, destroy the other entity and apply damage to self
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Ree");

        DefaultEnemy enemy = other.GetComponent<DefaultEnemy>();

        if (enemy != null)
        {
            enemy.GiveDamage(this);
            if (this != null)
                enemy.DestroyEntity();
        }
    }
}
