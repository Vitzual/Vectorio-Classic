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

    // Sprite info
    public SpriteRenderer border;
    public SpriteRenderer fill;
    public TrailRenderer trail;

    // Enemy target
    public Transform target;

    // Start method
    public void Start()
    {
        health = enemy.health;
        maxHealth = health;

        border.material = enemy.variant.border;
        fill.material = enemy.variant.fill;
        trail.material = enemy.variant.trail;
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
