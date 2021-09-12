using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies/Normal")]
public class Enemy : Entity
{
    // Sprite info
    public SpriteRenderer border;
    public SpriteRenderer fill;

    // Enemy stats
    public float damage;
    public float moveSpeed;
    public float explosiveRadius;
    public float explosiveDamage;
    public float rayLength;

    // Spawn on death
    [System.Serializable]
    public class EnemySpawn
    {
        public Enemy enemy;
        public int amount;
        public float radius;
    }
    public EnemySpawn[] spawns;

    // Particle and materials
    public ParticleSystem particle;

    // MOVEMENT
    // Variants handle the movement of each enemy. However,
    // variants get passed the data container when their 
    // method is called, so if you want an enemy to override
    // that behaviour, you can call a method from there.
}
