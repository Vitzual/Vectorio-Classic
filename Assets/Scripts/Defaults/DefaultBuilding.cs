using UnityEngine;
using Mirror;
using System.Collections.Generic;

[HideInInspector]
public class DefaultBuilding : DefaultEntity
{
    // IDamageable interface variables
    public Entity entity;

    public override void Setup()
    {
        health = entity.health;
        maxHealth = health;
    }
}
