using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTurret : MonoBehaviour
{
    // Bullet Handler
    private LayerMask layer;

    // Weapon variables
    public int range;
    public float rotationSpeed;
    public int bulletDamage = 1;
    public int bulletPierces = 1;
    public int bulletAmount = 1;
    public float bulletSpeed = 80;
    public float bulletSpread = 1;
    public bool targetLock = true;
    public float fireRate;
    public Transform[] FirePoints;
    public Transform Gun;
    public GameObject Bullet;
    public bool hasAudio = false;
    public HashSet<GameObject> nearbyEnemies = new HashSet<GameObject>();

    // Global variables
    protected float nextFire = 0;
    protected float timePassed = 0;
    protected bool hasTarget = false;
    public Transform target = null;
    protected float enemyAngle;
    protected float gunRotation;
    public bool isRotating = true;

    // 1 = Closest 
    // 2 = Strongest
    // 3 = Weakest
    // 4 = Furthest
    public List<Transform> targets;
    public int targettingMode = 1;

    protected CameraScroll cameraScript;

    // Gun shot particle
    public bool gunShotParticles = false;
    public ParticleSystem shotParticle;

    // Shot anim variables
    public bool animationEnabled = false;
    protected bool animPlaying = false;
    protected bool animRebound = false;
    public int animTracker;
    private int animHolder;
    public float animMovement = 4f;
}
