using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{

    [SerializeField]
    private Transform FirePoint;
    private Rigidbody2D turret;

    void Start()
    {
        turret = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Grab target variable
        TriangleAI target = TriangleAI.FindNearest(transform.position);

        // Rotate turret towards target
        Vector2 lookDirection = target.position - turret.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        turret.rotation = angle;

        // Shoot bullet


    }

}
