using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Barrel 
{
    // Parameterized Constructor 
    public Barrel(Turret turret, Transform[] firePoints, Transform barrel, Vector2 position, GameObject bullet = null)
    {
        this.turret = turret;

        this.firePoints = firePoints;
        this.barrel = barrel;
        this.position = position;
        this.bullet = bullet;

        if (bullet == null)
            this.bullet = Resources.Load<GameObject>("Bullet/Default");
        else this.bullet = bullet;

        target = null;
        hasTarget = false;
        cooldown = turret.fireRate;
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

    // Cooldown
    public float cooldown;
}
