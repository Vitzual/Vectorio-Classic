using UnityEngine;
using System.Collections;

public class SpeedAI : EnemyClass
{
    // Targetting system
    void Update()
    {
        BaseUpdate();

        // Find closest enemy 
        if (target == null)
        {
            target = FindNearestDefence();
        }
        if (target != null)
        {
            // Rotate towards current target
            Vector2 TargetPosition = new Vector2(target.gameObject.transform.position.x, target.gameObject.transform.position.y);

            // Move towards defense
            Vector2 lookDirection = TargetPosition - new Vector2(transform.position.x, transform.position.y);

            // Get the angle between the target and this transform
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
}
