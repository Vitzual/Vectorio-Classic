using UnityEngine;

public abstract class TileClass : MonoBehaviour
{

    [SerializeField]
    protected ParticleSystem Effect;
    protected int health = 1;
    protected int level = 1;

    // Abstract methods
    public abstract void DestroyTile();

    // On collision event
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            int enemyDamage = collision.gameObject.GetComponent<EnemyClass>().getDamage();
            DamageTile(enemyDamage);
        }
    }

    // Apply damage to entity
    public void DamageTile(int dmgRecieved)
    {
        health -= dmgRecieved;
        if (health <= 0)
        {
            DestroyTile();
        }
    }

}
