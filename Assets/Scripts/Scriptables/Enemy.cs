using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ScriptableObject
{
    public new string name;
    [TextArea] public string desc;

    public float health;
    public float damage;
    public float speed;

    public ParticleSystem particle;
    public Material material;

    public virtual void Move(Transform obj)
    {
        obj.position += obj.up * speed * Time.deltaTime;
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
        // Particle
        Destroy(obj.gameObject);
    }
}
