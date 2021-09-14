using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DefaultEnemy : MonoBehaviour
{
    // Enemy scriptable
    public Enemy enemy;

    // Enemy stats
    public float health { get; set; }
    public float maxHealth { get; set; }

    public Transform rotator;

    // Sprite info
    public SpriteRenderer[] border;
    public SpriteRenderer[] fill;
    public TrailRenderer[] trail;

    // Start method
    public void Start()
    {
        health = enemy.health;
        maxHealth = health;

        foreach (SpriteRenderer a in border)
            a.material = enemy.variant.border;

        foreach (SpriteRenderer a in fill)
            a.material = enemy.variant.fill;

        foreach (TrailRenderer a in trail)
            a.material = enemy.variant.trail;

        Events.active.EnemySpawned(this, rotator);
    }

    public void DamageEnemy(float damage)
    {
        if (enemy.variant.TakeDamage(this, damage))
        {
            Destroy(gameObject);
        }
    }
}
