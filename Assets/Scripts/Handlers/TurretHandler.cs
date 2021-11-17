using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Mirror;

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
        if (Settings.paused) return;

        for (int i = 0; i < turretEntities.Count; i++)
        {
            if (turretEntities[i] == null)
            {
                turretEntities.Remove(turretEntities[i]);
                i--;
            }
            else if (turretEntities[i].target != null)
            {
                turretEntities[i].RotateTurret();
            }
            else if (!turretEntities[i].GetNewTarget())
            {
                turretEntities.Remove(turretEntities[i]);
            }
        }
    }

    public void AddTurretEntity(DefaultTurret turret)
    {
        turretEntities.Add(turret);
    }
}
