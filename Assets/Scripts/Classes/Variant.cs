using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Variants", menuName = "Variants/Normal")]
public class Variant : ScriptableObject
{
    // Variant info
    public new string name;
    [TextArea] public string desc;

    // Variant modifiers
    public float healthModifier;
    public float damageModifier;
    public float speedModifier;

    // Spawn on death
    [System.Serializable]
    public class EnemySpawn
    {
        public Enemy enemy;
        public int amount;
        public float radius;
    }
    public EnemySpawn[] spawns;

    // Variant material 
    public Material border;
    public Material fill;

    public virtual void Move(Transform obj, float speed)
    {
        obj.position += obj.up * speed * Time.deltaTime;
    }

    public virtual bool GiveDamage(DefaultBuilding building, float damage)
    {
        return building.DamageEntity(damage);
    }

    public virtual bool TakeDamage(DefaultEnemy enemy, float amount)
    {
        enemy.health -= amount;
        if (enemy.health <= 0)
        {
            Kill(enemy.transform);
            return true;
        }
        return false;
    }

    public virtual void Kill(Transform obj)
    {
        Debug.Log("Pretend there's a particle");
        
    }
}
