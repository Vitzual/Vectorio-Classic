using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies/Normal")]
public class Enemy : ScriptableObject
{
    // Enemy info
    public new string name;
    [TextArea] public string desc;
    public GameObject obj;

    // Sprite info
    public SpriteRenderer border;
    public SpriteRenderer fill;

    // Enemy stats
    public float health;
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
}
