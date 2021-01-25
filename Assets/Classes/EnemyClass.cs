using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public abstract class EnemyClass : MonoBehaviour
{

    // Effect variables
    private Transform burning_effect;
    private Transform freezing_effect;
    private Transform poison_effect;
    public bool is_burning = false;
    public bool is_frozen = false;
    public bool is_poisoned = false;

    // Enemy stats
    public int health;
    public int damage;
    public int range;
    public float moveSpeed;
    public int attackSpeed;
    public int worth;
    public int explosiveRadius;
    public int explosiveDamage;
    public bool effectImmunity = false;
    public GameObject[] spawnOnDeath;
    public int[] amountToSpawn;
    public ParticleSystem Effect;
    public Rigidbody2D body;

    // Enemy vars
    protected GameObject target;
    protected int attackTimeout;
    protected Vector2 Movement;

    // Attack Tile
    public void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionStay2D(collision);
    }

    // Damage building on collision
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            if (attackTimeout <= 0)
            {
                collision.gameObject.GetComponent<TileClass>().DamageTile(damage);
                attackTimeout = attackSpeed;

                // If bonus shield is unlocked, apply damage to entity
                if (Research.bonus_shield > 0)
                {
                    health -= Research.bonus_shield;
                    if (health <= 0) KillEntity();
                }
            }
        }
    }

    // Called by Update() of child classes
    public void BaseUpdate()
    {
        if (attackTimeout > 0) attackTimeout -= 1;
    }

    // Kill entity
    public void KillEntity()
    {
        // If menu scene, re instantiate the object
        if (SceneManager.GetActiveScene().name == "Menu") {
            Instantiate(Effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            // If has explosive damage, apply it to surrounding tiles
            if (explosiveRadius > 0 && explosiveDamage > 0)
            {
                var colliders = Physics2D.OverlapCircleAll(transform.position, explosiveRadius, 1 << LayerMask.NameToLayer("Building"));
                for (int i = 0; i < colliders.Length; i++)
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
                for (int a = 0; a < spawnOnDeath.Length; a++)
                {
                    for (int b = 0; b < amountToSpawn[a]; b++)
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
            GameObject.Find("Survival").GetComponent<Technology>().UpdateUnlock(gameObject.transform);
            Instantiate(Effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    // Apply damage to entity
    public void DamageEntity(int dmgRecieved)
    {
        // Apply damage and double if entity is poisoned
        if (is_poisoned) health -= dmgRecieved * 2;
        else health -= dmgRecieved;

        // Check health, if less then 0 kill entity
        if (health <= 0)
        {
             KillEntity();
        } 
        else if (!effectImmunity)
        {
            // Check to see if burning has been unlocked and if it should be applied
            if (!is_burning && Research.bonus_burning > 0)
            {
                if (Random.Range(0, 20) == 10)
                {
                    is_burning = true;
                    burning_effect = Instantiate(Resources.Load<Transform>("Effects/Fire"), transform.position, transform.rotation);
                    burning_effect.parent = transform;
                    StartCoroutine(ApplyBurning(Research.bonus_burning));
                }
            }

            // Check to see if freezing has been unlocked and if it should be applied
            if (!is_frozen && Research.bonus_freezing > 0)
            {
                if (Random.Range(0, 20) == 10)
                {
                    is_frozen = true;
                    freezing_effect = Instantiate(Resources.Load<Transform>("Effects/Ice"), transform.position, transform.rotation);
                    freezing_effect.parent = transform;
                    StartCoroutine(ApplyFreezing(Research.bonus_freezing));
                }
            }

            // Check to see if poisoning has been unlocked and if it should be applied
            if (!is_poisoned && Research.bonus_poisoning > 0)
            {
                if (Random.Range(0, 20) == 10)
                {
                    is_poisoned = true;
                    poison_effect = Instantiate(Resources.Load<Transform>("Effects/Poison"), transform.position, transform.rotation);
                    poison_effect.parent = transform;
                    StartCoroutine(ApplyPoisoning(Research.bonus_poisoning));
                }
            }
        }
    }

    // Apply burning effect
    public IEnumerator ApplyBurning(int amount)
    {
        // Wait for 1 second before applying burn
        yield return new WaitForSeconds(1);

        // Apply damage
        health -= 1;

        // Check health, if less then 0 kill entity
        if (health <= 0)
        {
            KillEntity();
        }

        // If entity still alive, check if still burning
        if (amount <= 0)
        {
            is_burning = false;
            Destroy(burning_effect.gameObject);
        }
        else StartCoroutine(ApplyBurning(amount - 1));
    }

    // Apply freezing effect
    public IEnumerator ApplyFreezing(int amount)
    {
        float speed_holder = moveSpeed;
        moveSpeed = moveSpeed / 2;
        yield return new WaitForSeconds(amount);
        Destroy(freezing_effect.gameObject);
        moveSpeed = speed_holder;
        is_frozen = false;
    }

    // Apply poisoning effect
    public IEnumerator ApplyPoisoning(int amount)
    {
        yield return new WaitForSeconds(amount);
        Destroy(poison_effect.gameObject);
        is_poisoned = false;
    }

    // Return damage
    public int getDamage()
    {
        return damage;
    }

    // Return moveSpeed
    public float getSpeed()
    {
        return moveSpeed;
    }

    // Set the move speed
    public void setSpeed(float a)
    {
        moveSpeed = a;
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
