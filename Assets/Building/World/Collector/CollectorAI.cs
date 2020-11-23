using UnityEngine;

public class CollectorAI : TileClass
{

    private bool powered = false;

    // Kill defense
    public override void DestroyTile()
    {
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
