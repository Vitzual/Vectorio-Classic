using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DefaultEnemy : MonoBehaviour, IDamageable
{
    // Enemy scriptable
    public Enemy enemy;

    // Enemy stats
    public float health { get; set; }
    public float maxHealth { get; set; }

    // Enemy target
    public Transform target;

    // Start method
    public void Start()
    {
        health = enemy.health;
        maxHealth = health;
    }

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
