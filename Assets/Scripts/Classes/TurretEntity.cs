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
    public Transform barrel;
    public Vector2 position;
    public GameObject bullet;

    // Boolean flags
    public Enemy enemy;
    public Transform target;
    public bool hasTarget;
}
