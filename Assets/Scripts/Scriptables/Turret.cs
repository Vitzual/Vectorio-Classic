using UnityEngine;

[CreateAssetMenu(fileName = "New Turret", menuName = "Buildings/Turret")]
public class Turret : Building
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
    public GameObject Bullet;


}
