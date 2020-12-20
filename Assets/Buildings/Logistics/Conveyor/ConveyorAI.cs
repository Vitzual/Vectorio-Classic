using UnityEngine;

public class ConveyorAI : TileClass
{
    public override void DestroyTile()
    {
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
