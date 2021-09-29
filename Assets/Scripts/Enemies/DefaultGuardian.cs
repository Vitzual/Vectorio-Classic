using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGuardian : DefaultEntity
{
    public Guardian guardian;
    public Transform rotator;
    [HideInInspector] public DefaultBuilding target;
    
    public override void Setup()
    {
        material = guardian.material;

        health = guardian.health;
        maxHealth = health;

        Events.active.GuardianSpawned(this);
    }

    public virtual void MoveTowards(Transform obj, Transform target)
    {
        float step = guardian.moveSpeed * Time.deltaTime;
        obj.position = Vector2.MoveTowards(obj.position, target.position, step);
    }
}
