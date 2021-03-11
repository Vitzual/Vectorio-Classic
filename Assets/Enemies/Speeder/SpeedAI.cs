using UnityEngine;
using System.Collections;

public class SpeedAI : EnemyClass
{

    // On start, get rigidbody and assign death effect
    void Start()
    {
        body = this.GetComponent<Rigidbody2D>();
    }

    // Targetting system
    void Update()
    {
        BaseUpdate();

        // Find closest enemy 
        if (target == null) {
            target = FindNearestDefence();
        }
        if (target != null)
        {
            float distance = (target.transform.position - this.transform.position).sqrMagnitude;
            // Rotate towards current target
            Vector2 TargetPosition = new Vector2(target.gameObject.transform.position.x, target.gameObject.transform.position.y);

            // Move towards defense
            Vector2 lookDirection = TargetPosition - body.position;

            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            body.rotation = angle;
            lookDirection.Normalize();
            Movement = lookDirection;
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
        body.AddForce(Movement * moveSpeed);
    }
}
