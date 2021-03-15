using UnityEngine;

public class EnemyStaticAI : TileClass
{
    // Kill defense
    public override void DestroyTile()
    {
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
