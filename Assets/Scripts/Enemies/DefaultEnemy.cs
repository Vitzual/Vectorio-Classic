using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DefaultEnemy : MonoBehaviour, IDamageable
{
    // Enemy handler script
    public Enemy enemy;
    
    // Enemy ID
    public int ID;
    
    // Effect variables
    private Transform burning_effect;
    private Transform freezing_effect;
    private Transform poison_effect;
    public bool is_burning = false;
    public bool is_frozen = false;
    public bool is_poisoned = false;

    // Enemy stats
    public float health { get; set; }
    public float maxHealth { get; set; }
    public float damage;
    public float moveSpeed;
    public float explosiveRadius;
    public float explosiveDamage;
    public float rayLength;
    public bool isChaos;
    public bool effectImmunity = false;
    public GameObject[] spawnOnDeath;
    public int[] amountToSpawn;
    public ParticleSystem Effect;

    // Enemy vars
    public Transform PreferredTarget = null;
    public Transform target;
    protected int attackTimeout;



    // Damages the entity (IDamageable interface method)
    public bool DamageEntity(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            DestroyEntity();
            return true;
        }
        else return false;
    }

    // Destroys the entity (IDamageable interface method)
    public void DestroyEntity()
    {
        // Create the particle
        ParticleSystemRenderer holder = Instantiate(Resources.Load<ParticleSystem>("Particles/Death"),
            transform.position, Quaternion.identity).GetComponent<ParticleSystemRenderer>();

        if (enemy.material != null)
        {
            holder.material = enemy.material;
            holder.trailMaterial = enemy.material;
        }

        Destroy(gameObject);
    }

    // Heals the entity (IDamageable interface method)
    public void HealEntity(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }


    // Gets called when entering another defenses range or hitting the defense all together
    public void OnTriggerEnter2D(Collider2D collider)
    {
        
    }

    // Gets called when entering another defenses range or hitting the defense all together
    public void OnTriggerLeave2D(Collider2D collider)
    {
        
    }
}
