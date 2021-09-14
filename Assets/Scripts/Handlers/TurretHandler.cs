using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TurretHandler : MonoBehaviour
{
    // Active class
    public static TurretHandler active;

    // Barrel class
    public List<DefaultTurret> turretEntities;

    public void Start()
    {
        turretEntities = new List<DefaultTurret>();

        if (this != null)
        {
            active = this;
            Events.active.onTurretRegistered += AddTurretEntity;
        }
    }

    public void Update()
    {
        foreach (DefaultTurret turret in turretEntities)
        {
            if (turret.transform == null)
            {
                turretEntities.Remove(turret);
                return;
            }
            else if (turret.target.transform != null)
            {
                turret.RotateTurret();
            }
            else if (!turret.GetNewTarget())
            {
                turretEntities.Remove(turret);
            }
        }
    }

    public void AddTurretEntity(DefaultTurret turret)
    {
        turretEntities.Add(turret);
    }
}
