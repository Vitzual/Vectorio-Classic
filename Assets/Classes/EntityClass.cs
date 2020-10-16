using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityClass : MonoBehaviour
{

    protected ParticleSystem DeathEffect;
    protected int health;
    protected int damage;

    // Give damage to entity, check health
    protected void DamageEntity()
    {
        health -= damage;
        if (IsAlive() == false)
        {
            KillEntity();
        }
    }

    // Check if entity still has health
    protected bool IsAlive()
    {
        if (health <= 0)
        {
            return false;
        } else
        {
            return true;
        }
    }

    // Kill entity
    protected void KillEntity()
    {
        Instantiate(DeathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


}
