using UnityEngine;

public abstract class TileClass : MonoBehaviour
{

    [SerializeField]
    protected ParticleSystem Effect;
    protected float maxhp = 1;
    public float health = 1;
    public int level = 1;
    public int cost = 1;

    // Abstract methods
    public abstract void DestroyTile();

    // On collision event
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            int enemyDamage = collision.gameObject.GetComponent<EnemyClass>().getDamage();
            collision.gameObject.GetComponent<EnemyClass>().KillEntity();
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

    public float GetPercentage()
    {
        return (health / maxhp) * 100;
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetCost()
    {
        return cost;
    }

}
