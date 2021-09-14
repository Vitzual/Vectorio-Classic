using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains all active enemies in the scene
[System.Serializable]
public class ActiveEnemy
{
    // Constructor
    public ActiveEnemy(Transform obj, DefaultEnemy script, Enemy enemy, Transform rotator)
    {
        this.obj = obj;
        this.script = script;
        this.enemy = enemy;
        this.rotator = rotator;

        target = null;
        variant = enemy.variant;
    }

    // Class variables
    public Transform obj;
    public Transform rotator;
    public DefaultEnemy script;

    public Enemy enemy;
    public Variant variant;
    public DefaultBuilding target;
}