using System.Collections.Generic;
using UnityEngine;

public class BaseTurret : TileClass, IAudible
{
    // IAudible interface variables
    public AudioClip sound { get; set; }

    // Base turret stat variables
    public int damage;
    public int range;
    public int rotationSpeed;
    public float fireRate;
    public int bulletPierces;
    public int bulletAmount;
    public float bulletSpeed;
    public float bulletSpread;

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
}
