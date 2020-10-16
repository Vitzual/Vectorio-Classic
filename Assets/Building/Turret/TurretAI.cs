using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class TurretAI : MonoBehaviour
{
    // Turret AI variables
    [SerializeField]
    private Transform FirePoint;
    private Rigidbody2D turret;
    private Vector2 TargetPosition;

    // On start, grab RB2D component
    void Start()
    {
        turret = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Find closest enemy 
        var target = EnemyPool.FindClosestEnemy(transform.position);

        // Rotate turret towards target
        TargetPosition = new Vector2(target.gameObject.transform.position.x, target.gameObject.transform.position.y);
        Vector2 lookDirection = TargetPosition - turret.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        turret.rotation = angle;
    }
}
