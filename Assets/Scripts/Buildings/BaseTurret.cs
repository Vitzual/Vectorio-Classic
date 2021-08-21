using System.Collections.Generic;
using UnityEngine;

public class BaseTurret : DefaultBuilding, IAudible
{
    // Turret scriptable
    public Turret turret;

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
    protected GameObject Bullet;

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

    // Get methods
    public int GetDamage() { return damage; }
    public int GetRange() { return range; }
    public float GetRotationSpeed() { return rotationSpeed; }
    public float GetFireRate() { return fireRate; }
    public int GetBulletPierces() { return bulletPierces; }
    public int GetBulletAmount() { return bulletAmount; }
    public float GetBulletSpeed() { return bulletSpeed; }
    public float GetBulletSpread() { return bulletSpread; }
}
