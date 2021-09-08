using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretEntity 
{
    // Parameterized Constructor 
    public TurretEntity(Turret turret, Transform[] firePoints, Transform obj, GameObject bullet = null)
    {
        this.turret = turret;
        this.firePoints = firePoints;
        this.obj = obj;

        if (bullet == null)
            this.bullet = Resources.Load<GameObject>("Bullet/Default");
        else this.bullet = bullet;

        target = null;
        hasTarget = false;
    }

    // Turret scriptable
    public Turret turret;

    // Base turret object variables
    public Transform[] firePoints;
    public Transform obj;
    public GameObject bullet;

    // Boolean flags
    public Enemy target;
    public bool hasTarget;
}
