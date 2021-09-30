using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    public static Instantiator active;
    public Variant variant;

    public void Awake()
    {
        active = this;
    }

    public GameObject CreateEntity(Entity entity, Transform transform)
    {
        // Create the tile
        GameObject lastObj = Instantiate(entity.obj, transform.position, transform.rotation);
        lastObj.name = entity.name;

        // Attempt to set enemy variant
        DefaultEnemy enemy = lastObj.GetComponent<DefaultEnemy>();
        if (enemy != null) enemy.variant = variant;

        // Set the health for the entity
        BaseEntity holder = lastObj.GetComponent<BaseEntity>();
        if (entity != null) 
        {
            holder.health = entity.health;
            holder.maxHealth = holder.health; 
        }

        // Setup entity
        lastObj.GetComponent<BaseEntity>().Setup();

        // Return object
        return lastObj;
    }
}
