using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TurretHandler : MonoBehaviour
{
    // Active class
    public static TurretHandler active;

    // Barrel class
    public List<TurretEntity> turretEntities;

    public void Start()
    {
        if (this != null)
            active = this;
    }

    public void Update()
    {
        for (int i=0; i<turretEntities.Count; i++)
        {
            if (turretEntities[i].barrel == null)
            {
                RemoveTurretEntity(turretEntities[i]);
                i--;
            }
            else if (turretEntities[i].hasTarget)
            {
                turretEntities[i].turret.RotateTurret(turretEntities[i]);
            }
        }
    }

    public void AddTurretEntity(Turret turret, Transform[] firePoints, Transform barrel, Vector2 position, GameObject bullet = null)
    {
        turretEntities.Add(new TurretEntity(turret, firePoints, barrel, position, bullet));
    }

    public void RemoveTurretEntity(TurretEntity turretEntity)
    {
        turretEntities.Remove(turretEntity);
    }
}
