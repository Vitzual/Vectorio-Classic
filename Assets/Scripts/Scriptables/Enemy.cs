using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies/Normal")]
public class Enemy : Entity
{
    // Enemy stats
    public float damage;
    public float moveSpeed;
    public float explosiveRadius;
    public float explosiveDamage;
    public float rayLength;
    public float rotationSpeed;

    // Spawn on death
    [System.Serializable]
    public class EnemySpawn
    {
        public Enemy enemy;
        public int amount;
        public float radius;
    }
    public EnemySpawn[] spawns;

    // Variant
    public Variant variant;

    // Set panel stats
    // This gets used to set the stats on the building menu panel
    public override void CreateStats(Panel panel)
    {
        panel.CreateStat(new Stat("Health", health, 0, Sprites.GetSprite("Health")));
        panel.CreateStat(new Stat("Damage", damage, 0, Sprites.GetSprite("Damage")));
        panel.CreateStat(new Stat("Speed", moveSpeed, 0, Sprites.GetSprite("Rotation Speed")));

        // Base method
        base.CreateStats(panel);
    }

    public virtual bool Rotate(Transform self, Transform target)
    {
        // Get target position relative to this entity
        Vector2 targetPosition = new Vector2(target.position.x, target.position.y);

        // Get the distance from the turret to the target
        Vector2 distance = targetPosition - new Vector2(self.position.x, self.position.y);

        // Get the angle between the gun position and the target position
        float targetAngle = Mathf.Atan(distance.y / distance.x) * Mathf.Rad2Deg + 90f;
        if (distance.x > 0) targetAngle += 180;

        // Correct for if target is directly above or below the turret
        if (distance.x == 0)
        {
            if (distance.y > 0) targetAngle = 0;
            else targetAngle = 180;
        }

        // Calculate the difference between the target angle and the current angle
        float difference = targetAngle - (self.rotation.eulerAngles.z);

        if ((difference < 0 || difference >= 180) && !(difference < -180))
        {
            // Calculate how far to rotate the turret given how long since the last frame
            float distanceToRotate = -rotationSpeed * Time.deltaTime;

            // If distance to rotate would rotate past the target only rotate the distance
            if (distanceToRotate < difference)
                distanceToRotate = difference;

            // Rotate the turret
            self.Rotate(Vector3.forward, distanceToRotate);
        }
        else if (!(difference <= 5 && difference >= -5))
        {
            // Calculate how far to rotate the turret given how long since the last frame
            float distanceToRotate = -rotationSpeed * Time.deltaTime;

            // If distance to rotate would rotate past the target only rotate the distance
            if (distanceToRotate > difference)
                distanceToRotate = difference;

            // Rotate the turret
            self.Rotate(Vector3.forward, distanceToRotate);
        }
        else
        {
            self.transform.eulerAngles = new Vector3(0, 0, targetAngle);
            return true;
        }
        return false;
    }
}
