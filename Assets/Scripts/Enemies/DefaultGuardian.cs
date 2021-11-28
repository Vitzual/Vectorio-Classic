using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGuardian : DefaultEnemy
{
    public Guardian guardian;
    bool entityDestroyed = false;

    public override void Setup()
    {
        health = guardian.health;
        maxHealth = guardian.health;

        Events.active.GuardianSpawned(this);
    }

    // Move towards target
    public override void MoveTowards(Transform obj, Transform target)
    {
        float step = guardian.moveSpeed * Time.deltaTime;
        obj.position = Vector2.MoveTowards(obj.position, target.position, step);

        GradualRotation();
    }

    // Destroy entity
    public override void DestroyEntity()
    {
        // Check if entity destroyed
        if (entityDestroyed) return;
        else entityDestroyed = true;

        // Create particle and set material / trail material
        if (guardian.particleEffect != null)
        {
            ParticleSystemRenderer holder = Instantiate(guardian.particleEffect, transform.position,
                Quaternion.identity).GetComponent<ParticleSystemRenderer>();
            holder.material = guardian.material;
            holder.trailMaterial = guardian.material;
        }

        // Invoke enemy death event
        Events.active.GuardianDestroyed(this);

        // Destroy object
        Destroy(gameObject);
    }

    // If a collision is detected, destroy the other entity and apply damage to self
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other is BoxCollider2D)
        {
            BaseTile building = other.GetComponent<BaseTile>();

            if (building != null)
                building.DestroyEntity();
        }
        else
        {
            DefaultTurret turret = other.GetComponent<DefaultTurret>();

            if (turret != null)
                turret.AddTarget(this);
        }
    }

    // If entity leaves defense range, remove self from target list
    public override void OnTriggerExit2D(Collider2D other)
    {
        DefaultTurret turret = other.GetComponent<DefaultTurret>();

        if (turret != null)
            turret.RemoveTarget(this);
    }

    // Get material
    public override Material GetMaterial()
    {
        return guardian.material;
    }
}
