using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGuardian : DefaultEnemy
{
    public Guardian guardian;

    public override void Setup()
    {
        material = guardian.material;
        health = guardian.health;
        maxHealth = guardian.health;

        Events.active.GuardianSpawned(this);
    }

    // Move towards target
    public override void MoveTowards(Transform obj, Transform target)
    {
        float step = guardian.moveSpeed * Time.deltaTime;
        obj.position = Vector2.MoveTowards(obj.position, target.position, step);

        Vector3 targetDir = target.position - transform.position;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * guardian.rotationSpeed);
    }

    // Destroy entity
    public override void DestroyEntity()
    {
        // Create particle and set material / trail material
        if (guardian.particleEffect != null)
        {
            ParticleSystemRenderer holder = Instantiate(guardian.particleEffect, transform.position,
                Quaternion.identity).GetComponent<ParticleSystemRenderer>();
            holder.material = material;
            holder.trailMaterial = material;
        }

        // Invoke enemy death event
        Events.active.GuardianDestroyed(this);

        // Update unlockables
        Buildables.UpdateEntityUnlockables(Unlockable.UnlockType.DestroyGuardianAmount, guardian, 1);

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
}
