using UnityEngine;

public abstract class EnemyClass : MonoBehaviour
{

    // Enemy stats
    protected int health;
    protected int damage;
    protected float moveSpeed;

    // Abstract methods
    public abstract void KillEntity();

    // Apply damage to entity
    public void DamageEntity(int dmgRecieved)
    {
        health -= dmgRecieved;
        if (health <= 0)
        {
            KillEntity();
        }
    }

}
