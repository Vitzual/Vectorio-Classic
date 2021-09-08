using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHandler : MonoBehaviour
{
    // Active class
    public static TurretHandler active;

    // Barrel class
    public List<TurretEntity> turretEntities;

    public void Start()
    {
        if (this != null)
            active = this;
    }

    public void Update()
    {
        for (int i=0; i<turretEntities.Count; i++)
        {
            if (turretEntities[i].obj == null)
            {
                RemoveTurretEntity(turretEntities[i]);
                i--;
            }
            else if (turretEntities[i].hasTarget)
            {
                RotateTurret(turretEntities[i]);
            }
        }
    }

    public void RotateTurret(TurretEntity entity)
    {
        // Get target position relative to this entity
        Vector2 targetPosition = new Vector2(entity.target.position.x, entity.target.position.y);

        // Get the distance from the turret to the target
        Vector2 distance = targetPosition - new Vector2(entity.barrel.position.x, entity.barrel.position.y);

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
        float difference = targetAngle - (entity.barrel.rotation.eulerAngles.z);

        if ((difference < 0 || difference >= 180) && !(difference < -180))
        {
            // Calculate how far to rotate the turret given how long since the last frame
            float distanceToRotate = -entity.turret.rotationSpeed * Time.deltaTime;

            // If distance to rotate would rotate past the target only rotate the distance
            if (distanceToRotate < difference)
                distanceToRotate = difference;

            // Rotate the turret
            entity.barrel.Rotate(Vector3.forward, distanceToRotate);
        }
        else if (!(difference <= 5 && difference >= -5))
        {
            // Calculate how far to rotate the turret given how long since the last frame
            float distanceToRotate = -entity.turret.rotationSpeed * Time.deltaTime;

            // If distance to rotate would rotate past the target only rotate the distance
            if (distanceToRotate > difference)
                distanceToRotate = difference;

            // Rotate the turret
            entity.barrel.Rotate(Vector3.forward, distanceToRotate);
        }
        else entity.barrel.transform.eulerAngles = new Vector3(0, 0, targetAngle);
    }

    public void AddTurretEntity(Turret turret, Transform[] firePoints, Transform obj, GameObject bullet = null)
    {
        turretEntities.Add(new TurretEntity(turret, firePoints, obj, bullet));
    }

    public void RemoveTurretEntity(TurretEntity turretEntity)
    {
        turretEntities.Remove(turretEntity);
    }



















    /* Plays the recoil animation (IRecoilAnim interface method)
    public void PlayRecoilAnimation(Transform obj)
    {
        if (!animRebound)
        {
            animTracker -= 1;
            obj.localPosition -= obj.up * animMovement * Time.deltaTime;
            if (animTracker == animHolder / 2)
            {
                animTracker = 0;
                animRebound = true;
            }
        }
        else
        {
            animTracker += 1;
            obj.localPosition += obj.up * animMovement / 2 * Time.deltaTime;
            if (animTracker == animHolder)
            {
                obj.localPosition = new Vector2(0, 0);
                animRebound = false;
                animPlaying = false;
            }
        }
        return;
    }
    */
}
