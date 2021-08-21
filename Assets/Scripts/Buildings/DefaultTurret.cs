using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultTurret : BaseTurret
{
    // Layer of the turret (will grab from LayerManager.cs)
    private LayerMask layer;

    // Bullet handler script. This will be removed during refactor v3
    private BulletHandler bulletHandler;

    // The targetting mode of the turret
    // 1 = Closest | 2 = Strongest | 3 = Weakest | 4 = Furthest
    public int targettingMode = 1;

    // Start method set variables
    private void Start()
    {
        // Get required references 
        layer = LayerManager.getTurretLayer();
        SetCooldown(fireRate - Research.research_firerate);
        bulletHandler = GameObject.Find("Bullet Handler").GetComponent<BulletHandler>();
    }

    // Forces a turret to fire at the passed target
    public void ForceTarget(Transform target)
    {
        this.target = target;
        RotationHandler();
    }

    // Forces a turret to stop firing at the passed target (if it is the target)
    public void RemoveTarget(Transform target)
    {
        if (this.target == target) this.target = null;
    }

    // Forces a turret to recheck its targets
    public void ForceUpdate()
    {
        target = null;

        var colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, range + Research.research_range, layer);
        float closest = float.PositiveInfinity;

        foreach (Collider2D collider in colliders)
        {
            float distance = (collider.transform.position - this.transform.position).sqrMagnitude;
            if (distance < closest)
            {
                target = collider.transform;
                closest = distance;
            }
        }

        if (target == null) enabled = false;
        else RotationHandler();
    }

    public void PlayAudio()
    {
        float audioScale = CameraScroll.getZoom() / 1400f;
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sound/" + transform.name), gameObject.transform.position, Settings.soundVolume - audioScale);
    }

    protected void RotationHandler()
    {
        if (target != null)
        {
            // Set turret to rotating state
            isRotating = true;

            // Get target position relative to this entity
            Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);

            // Get the distance from the turret to the target
            Vector2 distance = targetPosition - new Vector2(gun.position.x, gun.position.y);

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
            float difference = targetAngle - (gun.rotation.eulerAngles.z);

            if ((difference < 0 || difference >= 180) && !(difference < -180))
            {
                // Calculate how far to rotate the turret given how long since the last frame
                float distanceToRotate = -rotationSpeed * Time.deltaTime;

                // If distance to rotate would rotate past the target only rotate the distance
                if (distanceToRotate < difference)
                {
                    isRotating = false;
                    distanceToRotate = difference;
                }

                // Rotate the turret
                gun.Rotate(Vector3.forward, distanceToRotate);
            }
            else if (!(difference <= 5 && difference >= -5))
            {
                // Calculate how far to rotate the turret given how long since the last frame
                float distanceToRotate = rotationSpeed * Time.deltaTime;

                // If distance to rotate would rotate past the target only rotate the distance
                if (distanceToRotate > difference)
                {
                    isRotating = false;
                    distanceToRotate = difference;
                }

                // Rotate the turret
                gun.Rotate(Vector3.forward, distanceToRotate);
            }
            else
            {
                gun.transform.eulerAngles = new Vector3(0, 0, targetAngle);
                isRotating = false;
            }
        }
        else { isRotating = false; ForceUpdate(); }
    }

    // Attempts to fire a bullet and returns true if fired
    protected bool Shoot(GameObject prefab, Transform pos, float multiplier = 1)
    {
        isRotating = true;
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            return false;
        }
        else
        {
            for (int i = 0; i < bulletAmount; i += 1)
                CreateBullet(prefab, pos, multiplier);
            SetCooldown(fireRate - Research.research_firerate);
            return true;
        }
    }

    // Createa a bullet object
    protected void CreateBullet(GameObject prefab, Transform pos, float multiplier = 1)
    {
        // if (hasShotParticle) Instantiate(shotParticle, FirePoints[0].position, Quaternion.Euler(0, 0, Gun.localEulerAngles.z + 180f));
        if (sound != null) PlaySound();
        //if (animationEnabled) animPlaying = true;

        pos.position = new Vector3(pos.position.x, pos.position.y, 0);
        GameObject bullet = Instantiate(prefab, pos.position, pos.rotation);
        bullet.transform.rotation = pos.rotation;
        bullet.transform.Rotate(0f, 0f, Random.Range(-bulletSpread, bulletSpread));

        float speed = Random.Range(bulletSpeed - 10, bulletSpeed + 10) * Research.research_bulletspeed;
        int pierces = bulletPierces + Research.research_pierce;
        int damage = this.damage + Research.research_damage;

        // Dependent on the bullet, register under the correct master script
        bulletHandler.RegisterBullet(bullet.transform, target, speed, pierces, damage);
    }

    public void SetCooldown(float cooldown)
    {
        if (cooldown <= 0.03f) this.cooldown = 0.03f;
        else this.cooldown = cooldown;
    }
}
