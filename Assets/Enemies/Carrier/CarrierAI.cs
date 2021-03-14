using UnityEngine;
using System.Collections;

public class CarrierAI : EnemyClass
{
    // Carriers will destroy any building they touch
    // Additional logic requires a Rigibody component be attached to this unit
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("test");
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Building"))
            collision.collider.GetComponent<TileClass>().DestroyTile();
    }
}
