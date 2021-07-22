using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    // Contains all active coins in scene
    [System.Serializable]
    public class ActiveBullets
    {
        // Constructor
        public ActiveBullets(Transform Object, Transform Target, float Speed, int Tracker, int Piercing, int Damage)
        {
            this.Object = Object;
            this.Target = Target;
            this.Speed = Speed;
            this.Tracker = Tracker;
            this.Piercing = Piercing;
            this.Damage = Damage;
        }

        // Class variables
        public Transform Object { get; set; }
        public Transform Target { get; set; }
        public float Speed { get; set; }
        public int Tracker { get; set; }
        public int Piercing { get; set; }
        public int Damage { get; set; }
        public List<Transform> IgnoreEnemies = new List<Transform>();

    }
    public List<ActiveBullets> Bullets;

    public LayerMask EnemyLayer;
    public LayerMask EnemyBuildingLayer;
    private LayerMask LayerCast;

    // Called when awoken
    public void Start()
    {
        LayerCast = EnemyLayer | EnemyBuildingLayer;
    }

    // Handles bullet movement and hit detection frame-by-frame
    public void Update()
    {
        for (int i = 0; i < Bullets.Count; i++)
            try
            {
                Bullets[i].Object.position += Bullets[i].Object.up * Bullets[i].Speed * Time.deltaTime;
                if (Bullets[i].Tracker == 2)
                {
                    Bullets[i].Tracker = 1;
                    RaycastHit2D hit = Physics2D.Raycast(Bullets[i].Object.position, Bullets[i].Object.up, 1.5f, LayerCast);
                    if (hit.collider != null && !Bullets[i].IgnoreEnemies.Contains(hit.collider.transform))
                        if (OnHit(i, hit.collider.transform)) { i--; continue; }
                }
                else
                {
                    Bullets[i].Tracker+=1;
                    continue;
                }
            }
            catch
            {
                Bullets.RemoveAt(i);
                i--;
            }
    }

    // Registers a bullet to be handled by the updater in this script
    public void RegisterBullet(Transform bullet, Transform target, float speed, int pierces, int damage)
    {
        Bullets.Add(new ActiveBullets(bullet, target, speed, 1, pierces, damage));
    }

    // Called when a hit is detected in the updater 
    public bool OnHit(int bulletID, Transform other)
    {
        // Add the other transform to the ignore list for future collisions
        Bullets[bulletID].IgnoreEnemies.Add(other);

        // Get correct component
        switch (other.name)
        {
            case "Hive":
                other.GetComponent<SpawnerAI>().SpawnEnemy();
                break;
            case "Enemy Turret":
                other.GetComponent<EnemyTurretAI>().DamageTile(Bullets[bulletID].Damage);
                break;
            case "Enemy Wall":
                other.GetComponent<EnemyWallAI>().DamageTile(Bullets[bulletID].Damage);
                break;
            case "Enemy Mine":
                other.GetComponent<EnemyStaticAI>().DamageTile(Bullets[bulletID].Damage);
                break;
            default:
                other.GetComponent<EnemyClass>().DamageEntity(Bullets[bulletID].Damage);
                break;
        }

        Bullets[bulletID].Piercing--;
        if (Bullets[bulletID].Piercing == 0)
        {
            BulletClass bds = Bullets[bulletID].Object.GetComponent<BulletClass>();
            if (bds.IsParticleChangeable())
                if (other.name.Contains("Dark") || other.name == "The Kraken")
                    bds.SetHitEffect("Dark Particle");
                else if (other.name.Contains("Phantom"))
                    bds.SetHitEffect("Phantom Particle");
                else
                    bds.SetHitEffect("Enemy Particle");
            bds.collide();
            Bullets.RemoveAt(bulletID);
            return true;
        }
        return false;
    }
}
