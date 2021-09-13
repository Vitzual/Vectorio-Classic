using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TurretHandler : MonoBehaviour
{
    // Active class
    public static TurretHandler active;

    // Barrel class
    public Dictionary<DefaultTurret, ActiveTurret> turretEntities;

    public void Start()
    {
        if (this != null)
        {
            active = this;
            Events.active.onTurretPlaced += AddTurretEntity;
        }
    }

    public void Update()
    {
        foreach (KeyValuePair<DefaultTurret, ActiveTurret> entity in turretEntities)
        {
            if (entity.Value.barrel == null)
            {
                turretEntities.Remove(entity.Key);
                return;
            }
            else if (entity.Value.hasTarget)
                entity.Value.turret.RotateTurret(entity.Value);
        }
    }

    public void AddTurretEntity(DefaultTurret turret, ActiveTurret barrel)
    {
        turretEntities.Add(turret, barrel);
    }
}
