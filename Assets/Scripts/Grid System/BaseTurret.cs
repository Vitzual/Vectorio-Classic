using System.Collections.Generic;
using UnityEngine;

[HideInInspector]
public class BaseTurret : BaseBuilding, IAudible
{
    // Turret scriptable
    public Turret turret;

    // IAudible interface variables
    public AudioClip sound { get; set; }

    // Base turret stat variables
    public float damage = 0;
    public float range = 0;
    public float rotationSpeed = 0;
    public float fireRate = 0;
    public int bulletPierces = 0;
    public int bulletAmount = 0;
    public float bulletSpeed = 0;
    public float bulletSpread = 0;

    // Base turret object variables
    protected Transform[] firePoints;
    protected Transform gun;
    protected GameObject bullet;

    // Base turret target variables
    protected Transform target = null;
    protected List<Transform> targets;

    // Base turret firing variables
    protected float cooldown = 0;
    protected bool isRotating = false;

    // IAudible sound method
    public void PlaySound()
    {
        float audioScale = CameraScroll.getZoom() / 1400f;
        AudioSource.PlayClipAtPoint(sound, gameObject.transform.position, Settings.soundVolume - audioScale);
    }

    // Set the turret stats
    public void SetTurretStats()
    {
        if (turret == null)
        {
            Debug.LogError(transform.name + " does not have a scriptable attached to it!");
        }
        else
        {
            damage = turret.damage;
            range = turret.range;
            rotationSpeed = turret.rotationSpeed;
            fireRate = turret.fireRate;
            bulletPierces = (int)turret.bulletPierces;
            bulletAmount = (int)turret.bulletAmount;
            bulletSpeed = turret.bulletSpeed;
            bulletSpread = turret.bulletSpread;
        }

        SetBuildingStats();
    }

    // Get methods
    public Transform GetTarget() { return target; }
    public void AddTarget(Transform newTarget) { targets.Add(newTarget); }
    public float GetDamage() { return turret.damage; }
    public float GetRange() { return turret.range; }
    public float GetRotationSpeed() { return turret.rotationSpeed; }
    public float GetFireRate() { return turret.fireRate; }
    public int GetBulletPierces() { return (int)turret.bulletPierces; }
    public int GetBulletAmount() { return (int)turret.bulletAmount; }
    public float GetBulletSpeed() { return turret.bulletSpeed; }
    public float GetBulletSpread() { return turret.bulletSpread; }
}
