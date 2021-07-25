using System.Collections.Generic;
using UnityEngine;

public class BaseTurret : DefaultBuilding, IAudible
{
    // IAudible interface variables
    public AudioClip sound { get; set; }

    // Base turret stat variables
    protected int damage = 0;
    protected int range = 0;
    protected float rotationSpeed = 0;
    protected float fireRate = 0;
    protected int bulletPierces = 0;
    protected int bulletAmount = 0;
    protected float bulletSpeed = 0;
    protected float bulletSpread = 0;

    // Base turret object variables
    public Transform[] FirePoints;
    public Transform Gun;
    public GameObject Bullet;

    // Base turret target variables
    public Transform target = null;
    public List<Transform> targets;

    // Base turret firing variables
    protected float cooldown = 0;
    protected bool isRotating = false;

    // IAudible sound method
    public void PlaySound()
    {
        float audioScale = CameraScroll.getZoom() / 1400f;
        AudioSource.PlayClipAtPoint(sound, gameObject.transform.position, Settings.soundVolume - audioScale);
    }

    // Set base turret variables
    public void InitTurretStats()
    {
        // Grab values from BuildingRegistrar
        BuildingRegistrar buildingRegistrar = ScriptHandler.buildingRegistrar;
        BuildingRegistrar.TurretStats turretStats = buildingRegistrar.GetTurretStats(transform);
        
        // Check to make sure the stats exist
        if (turretStats == null)
        {
            Debug.LogError("Could not find stats in registrar for " + transform.name);
            return;
        }

        // Set values returned from BuildingRegistrar 
        damage = turretStats.damage;
        range = turretStats.range;
        rotationSpeed = turretStats.rotationSpeed;
        fireRate = turretStats.fireRate;
        bulletPierces = turretStats.bulletPierces;
        bulletAmount = turretStats.bulletAmount;
        bulletSpeed = turretStats.bulletSpeed;
        bulletSpread = turretStats.bulletSpread;
        sound = turretStats.sound;
        
        
        if(turretStats.animEnabled) { Debug.Log("Register turret with anim enabled"); }
    }

    // Get methods
    public int getDamage() { return damage; }
    public int getRange() { return range; }
    public float getRotationSpeed() { return rotationSpeed; }
    public float getFireRate() { return fireRate; }
    public int getBulletPierces() { return bulletPierces; }
    public int getBulletAmount() { return bulletAmount; }
    public float getBulletSpeed() { return bulletSpeed; }
    public float getBulletSpread() { return bulletSpread; }
}
