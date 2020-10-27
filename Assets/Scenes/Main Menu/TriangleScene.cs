using UnityEngine;
using System.Collections;

public class TriangleScene : EnemyClass
{
    // Model components
    [SerializeField]
    private ParticleSystem ChargeEffect;
    private Vector3 SpawnPosition;
    private Rigidbody2D Triangle;

    // Movement variables
    private Vector2 Movement;

    // On start, get rigidbody and assign death effect
    void Start()
    {
        Triangle = this.GetComponent<Rigidbody2D>();
    }

    // Targetting system
    void Update()
    {
        // Find closest enemy 
        var target = DefensePool.FindClosestDefense(transform.position);
        float distance = DefensePool.FindClosestPosition(transform.position);
        if (target != null)
        {
            // Rotate towards current target
            Vector2 TargetPosition = new Vector2(target.gameObject.transform.position.x, target.gameObject.transform.position.y);

            // Move towards defense
            Vector2 lookDirection = TargetPosition - Triangle.position;

            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            Triangle.rotation = angle;
            lookDirection.Normalize();
            Movement = lookDirection;
        } 
        else
        {
            Movement = new Vector2(0, 0);
        }
    }

    // Move entity towards target every frame
    private void FixedUpdate()
    {
        Triangle.AddForce(Movement * moveSpeed);
    }
}
