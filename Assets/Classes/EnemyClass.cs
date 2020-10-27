using UnityEngine;

public abstract class EnemyClass : MonoBehaviour
{

    // Enemy stats
    protected int health;
    protected int damage;
    protected float moveSpeed;
    protected int range;
    protected int worth;

    // Enemy vars
    protected GameObject target;

    // Abstract methods
    public abstract void KillEntity();

    // Apply damage to entity
    public void DamageEntity(int dmgRecieved)
    {
        health -= dmgRecieved;
        if (health <= 0)
        {
            GameObject.Find("Survival").GetComponent<Survival>().AddGold(worth);
            KillEntity();
        }
    }

    // Return damage
    public int getDamage()
    {
        return damage;
    }

    protected GameObject FindNearestDefence()
    {
        var colliders = Physics2D.OverlapCircleAll(
            this.gameObject.transform.position, 
            range, 
            1 << LayerMask.NameToLayer("Building"));
        GameObject result = null;
        float closest = float.PositiveInfinity;

        foreach (Collider2D collider in colliders)
        {
            float distance = (collider.transform.position - this.transform.position).sqrMagnitude;
            if (distance < closest) {
                result = collider.gameObject;
                closest = distance;
            }
        }
        return result;
    }

}
