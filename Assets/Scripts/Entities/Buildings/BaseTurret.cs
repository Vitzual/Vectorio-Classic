using System.Collections.Generic;
using UnityEngine;

public class BaseTurret : TileClass, IAudible
{
    // IAudible interface variables
    public AudioClip sound { get; set; }

    // Base turret stat variables
    public int damage = 0;
    public int range = 0;
    public float rotationSpeed = 0;
    public float fireRate = 0;
    public int bulletPierces = 0;
    public int bulletAmount = 0;
    public float bulletSpeed = 0;
    public float bulletSpread = 0;

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
        BuildingRegistrar.TurretStats turretStats = buildingRegistrar.getTurretStats(transform);
        
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
    }
}
