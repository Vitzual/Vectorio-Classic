using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains all active enemies in the scene
[System.Serializable]
public class ActiveEnemy
{
    // Constructor
    public ActiveEnemy(Transform obj, Enemy enemy, Variant variant)
    {
        this.obj = obj;
        this.enemy = enemy;
        this.variant = enemy.variant;
        target = null;
    }

    // Class variables
    public Transform obj;
    public Enemy enemy;
    public Variant variant;
    [HideInInspector] public ActiveTurret target;
    [HideInInspector] public bool hasTarget;
}