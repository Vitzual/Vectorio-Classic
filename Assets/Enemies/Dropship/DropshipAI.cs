using UnityEngine;
using System.Collections;

public class DropshipAI : EnemyClass
{
    // Dropships will destroy any building they touch
    // Additional logic requires a Rigibody component be attached to this unit
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            GameObject.Find("BuildingGoDeadSound").GetComponent<AudioSource>().Play();
            GameObject.Find("Survival").GetComponent<Survival>().SetLastHit(collision.collider.transform.position);
            collision.collider.GetComponent<TileClass>().DestroyTile();
        }
    }
}
