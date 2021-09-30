using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGuardian : BaseEntity
{
    public Guardian guardian;
    [HideInInspector] public BaseTile target;
    
    public override void Setup()
    {
        material = guardian.material;

        Events.active.GuardianSpawned(this);
    }

    public virtual void MoveTowards(Transform obj, Transform target)
    {
        float step = guardian.moveSpeed * Time.deltaTime;
        obj.position = Vector2.MoveTowards(obj.position, target.position, step);

        Vector3 targetDir = target.position - transform.position;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * guardian.rotationSpeed);
    }

    // If a collision is detected, destroy the other entity and apply damage to self
    public virtual void OnTriggerEnter2D(Collider2D other)
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
    public virtual void OnTriggerExit2D(Collider2D other)
    {
        DefaultTurret turret = other.GetComponent<DefaultTurret>();

        if (turret != null)
            turret.RemoveTarget(this);
    }
}
