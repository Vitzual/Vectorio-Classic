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
        turretEntities = new Dictionary<DefaultTurret, ActiveTurret>();

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
            else if (entity.Value.target.obj != null)
                entity.Value.turret.RotateTurret(entity.Value);
        }
    }

    public void AddTurretEntity(DefaultTurret turret, ActiveTurret barrel)
    {
        turretEntities.Add(turret, barrel);
    }
}
