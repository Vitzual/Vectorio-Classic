using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunshipTurret : DefaultTurret
{
    // Parent
    [HideInInspector]
    public DroneController parent;
    public SpriteRenderer model;
    public bool manualFire = false;
    public bool canFire = false;
    public bool lockTurret = false;

    // Get input events
    public void Start()
    {
        if (manualFire)
            InputEvents.active.onLeftMousePressed += ManualFire;
    }

    // Update cooldown
    public void Update()
    {
        if (!canFire && cooldown >= 0)
            cooldown -= Time.deltaTime;
        else canFire = true;
    }

    // Setup the drone
    public override void Setup() 
    {
        lockTurret = parent.gunship.lockTurret;
    }

    // Manual target
    public void ManualFire()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!lockTurret) CalcRotation(position);

        if (canFire)
        {
            Shoot();
            canFire = false;
            cooldown = turret.cooldown / Research.firerateBoost;
        }
    }

    // Override bullet creation
    public override void CreateBullet(Vector2 position)
    {
        // Create bullet
        GameObject holder = Instantiate(turret.bullet.gameObject, position, cannon.rotation);
        holder.transform.rotation = cannon.rotation;
        holder.transform.Rotate(0f, 0f, Random.Range(-turret.bulletSpread, turret.bulletSpread));

        // Set bullet variables
        DefaultBullet bullet = holder.GetComponent<DefaultBullet>();

        // Setup bullet
        if (turret.useBulletSprite) bullet.Setup(turret, bulletModel);
        else bullet.Setup(turret);

        Events.active.BulletFired(bullet);
    }

    // Calculate rotation
    public void CalcRotation(Vector3 position)
    {
        // Calculate the rotation towards the enemy
        Vector3 dir = cannon.position - position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        cannon.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
    }
}
