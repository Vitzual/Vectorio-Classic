using UnityEngine;

public class Energizer : BaseTile
{
    // Internal placement variables
    [SerializeField] private LayerMask TileLayer;
    public Collider2D[] colliders;

    // Internal placement variables
    public float radialCheck;
    public Transform AOCB;
    public Transform rotator;
    public float speed;

    // Update is called once per frame
    void Start()
    {
        GameObject.Find("Rotation Handler").GetComponent<RotationHandler>().RegisterRotator(rotator, speed);
    }

    // OUTDATED
    public override void DestroyEntity()
    {
        // Run a check around surrounding tiles
        int xTile = (int)transform.position.x;
        int yTile = (int)transform.position.y;

        // Loop through all tiles and try to find drones
        for (int x = xTile - 30; x <= xTile + 30; x += 5)
        {
            for (int y = yTile - 30; y <= yTile + 30; y += 5)
            {
                BaseTile holder = InstantiationHandler.active.TryGetBuilding(new Vector2(x, y));
                if (holder != null) holder.CheckNearbyEnergizers();
            }
        }

        // Run base destroy
        base.DestroyEntity();
    }
}
