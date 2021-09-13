using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Barrel 
{
    // Turret scriptable
    public Turret turret;

    // Base turret object variables
    public Transform[] firePoints;
    public Transform barrel;
    public GameObject bullet;
    [HideInInspector] public Vector2 position;

    // Boolean flags
    [HideInInspector] public Enemy enemy;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool hasTarget;

    // Cooldown
    [HideInInspector] public float cooldown;
}
