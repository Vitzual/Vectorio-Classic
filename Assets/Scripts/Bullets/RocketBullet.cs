using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBullet : DefaultBullet
{
    // Track target and move
    public override void Move()
    {
        if (tracking && target != null) RotateToTarget();
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    // Rotate gunship
    public void RotateToTarget()
    {
        float angle = Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 25f * Time.deltaTime);
    }
}
