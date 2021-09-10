using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Normal Enemy", menuName = "Enemy/Normal")]
public class Enemy : ScriptableObject
{
    // Enemy info
    public new string name;
    [TextArea] public string desc;
    public GameObject obj;
    public Sprite icon;

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
    public Material material;

    public virtual void Move(Transform obj)
    {
        obj.position += obj.up * moveSpeed * Time.deltaTime;
    }

    public virtual bool GiveDamage(DefaultBuilding building)
    {
        return building.DamageEntity(damage);
    }

    public virtual bool TakeDamage(DefaultEnemy enemy, int amount)
    {
        enemy.health -= amount;
        if (health <= 0)
        {
            Kill(enemy.transform);
            return true;
        }
        return false;
    }

    public virtual void Kill(Transform obj)
    {
        Debug.Log("Pretend there's a particle");
        Destroy(obj.gameObject);
    }
}
