using UnityEngine;

public abstract class EnemyClass : MonoBehaviour
{

    // Enemy stats
    protected int health;
    protected int damage;
    protected float moveSpeed;
    protected int range;

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

    // Return damage
    public int getDamage()
    {
        return damage;
    }

    protected GameObject FindNearestEnemy()
    {
        var colliders = Physics2D.OverlapCircleAll(
            this.gameObject.transform.position, 
            range, 
            1 << LayerMask.NameToLayer("Enemy"));
        GameObject result = null;
        float closest = float.PositiveInfinity;

        foreach (Collider2D collider in colliders)
        {
            if (collider.tag != "Enemy") continue;
            float distance = (collider.transform.position - this.transform.position).sqrMagnitude;
            if (distance < closest) {
                result = collider.gameObject;
                closest = distance;
            }
        }
        return result;
    }

}
