using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class EnemyClass : MonoBehaviour
{

    // Enemy stats
    public int health;
    public int damage;
    public int range;
    public float moveSpeed;
    public int worth;
    public int explosiveRadius;
    public int explosiveDamage;
    public GameObject[] spawnOnDeath;
    public int[] amountToSpawn;
    public ParticleSystem Effect;

    // Enemy vars
    protected GameObject target;

    // Kill entity
    public void KillEntity()
    {
        // If menu scene, re instantiate the object
        if (SceneManager.GetActiveScene().name == "Menu") {
            var clone = Instantiate(this, transform.position, Quaternion.identity);
            clone.name = "Triangle";
        }

        // If has explosive damage, apply it to surrounding tiles
        if (explosiveRadius > 0 && explosiveDamage > 0)
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, explosiveRadius, 1 << LayerMask.NameToLayer("Building"));
            for (int i=0; i < colliders.Length; i++)
            {
                colliders[i].GetComponent<TileClass>().DamageTile(explosiveDamage);
            }
        }

        // If spawns on death, itterate through and spawn enemies
        if (spawnOnDeath.Length > 0)
        {
            if (amountToSpawn.Length != spawnOnDeath.Length)
            {
                Debug.LogError("Custom error #001\n- Mismatched array size!");
                return;
            }
            for (int a=0; a < spawnOnDeath.Length; a++)
            {
                for (int b=0; b < amountToSpawn[a]; b++)
                {
                    if (spawnOnDeath[a] == gameObject)
                    {
                        Debug.LogError("Custom error #002\n- Enemies cannot spawn themselves on death");
                    } 
                    else
                    {
                        Instantiate(spawnOnDeath[a], transform.position, Quaternion.identity);
                    }
                }
            }
        }

        // Instantiate death effect and destroy self
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

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
