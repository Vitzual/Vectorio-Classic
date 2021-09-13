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
    public SpriteRenderer[] border;
    public SpriteRenderer[] fill;
    public TrailRenderer[] trail;

    // Enemy target
    public Transform target;

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
    }

}
