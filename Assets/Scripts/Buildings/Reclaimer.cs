using UnityEngine;

public class Reclaimer : BaseTile
{
    // Internal placement variables
    [SerializeField] private LayerMask TileLayer;
    public Collider2D[] colliders;

    // Internal placement variables
    public Transform AOCB;
    public int size;

    // OUTDATED
    public override void DestroyEntity()
    {
        // Remove AOCB
        Destroy(AOCB.gameObject);

        // Run a check around surrounding tiles
        int xTile = (int)transform.position.x;
        int yTile = (int)transform.position.y;

        // Loop through all tiles and try to find drones
        for (int x = xTile - size; x <= xTile + size; x += 5)
        {
            for (int y = yTile - size; y <= yTile + size; y += 5)
            {
                BaseTile holder = InstantiationHandler.active.TryGetBuilding(new Vector2(x, y));
                if (holder != null && holder != this && holder.isSellable) holder.CheckNearbyEnergizers();
            }
        }

        // Run base destroy
        base.DestroyEntity();
    }
}
