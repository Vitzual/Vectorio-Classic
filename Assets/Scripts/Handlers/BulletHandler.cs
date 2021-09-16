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
        public ActiveBullets(Transform obj, DefaultEnemy target, float speed, int piercing, float damage, float time, bool tracking, Material material)
        {
            this.obj = obj;
            this.target = target;
            this.speed = speed;
            this.piercing = piercing;
            this.damage = damage;
            this.time = time;
            this.tracking = tracking;
            this.material = material;
            ignore = new List<Transform>();
        }

        // Class variables
        public Transform obj;
        public DefaultEnemy target;
        public float speed;
        public int piercing;
        public float damage;
        public float time;
        public bool tracking;
        public Material material;

        public List<Transform> ignore;

    }
    public List<ActiveBullets> bullets;

    public LayerMask enemyLayer;
    private ParticleSystem particle;

    // Called when awoken
    public void Start()
    {
        // temp
        particle = Resources.Load<ParticleSystem>("Particles/Shot");
        Events.active.onBulletFired += RegisterBullet;
    }

    // Handles bullet movement and hit detection frame-by-frame
    public void Update()
    {
        for (int a = 0; a < bullets.Count; a++)
        {
            if (bullets[a].obj != null)
            {
                bullets[a].time -= Time.deltaTime;
                if (bullets[a].time <= 0)
                {
                    DestroyBullet(a, false);
                    a--;
                }
                else
                {
                    if (bullets[a].tracking && bullets[a].target != null)
                    {
                        float step = bullets[a].speed * Time.deltaTime;
                        bullets[a].obj.position = Vector2.MoveTowards(bullets[a].obj.position, bullets[a].target.transform.transform.position, step);
                    }
                    else
                    {
                        bullets[a].obj.position += bullets[a].obj.up * bullets[a].speed * Time.deltaTime;
                    }
                    RaycastHit2D hit = Physics2D.Raycast(bullets[a].obj.position, bullets[a].obj.up, 3f, enemyLayer);
                    if (hit.collider != null && !bullets[a].ignore.Contains(hit.collider.transform))
                        if (OnHit(a, hit.collider.transform)) { a--; continue; }
                }
            }
            else
            {
                bullets.RemoveAt(a);
                a--;
            }
        }
    }

    // Registers a bullet to be handled by the updater in this script
    public void RegisterBullet(Bullet bullet)
    {
        bullets.Add(new ActiveBullets(bullet.obj, bullet.target, bullet.speed, bullet.pierces, 
            bullet.damage, bullet.time, bullet.tracking, bullet.material));
    }

    // Called when a hit is detected in the updater 
    public bool OnHit(int bulletID, Transform other)
    {
        // Add the other transform to the ignore list for future collisions
        if (bullets[bulletID].target != null)
        {
            bullets[bulletID].ignore.Add(other);
            bullets[bulletID].target.DamageEntity(bullets[bulletID].damage);
        }
        bullets[bulletID].piercing--;
        bullets[bulletID].tracking = false;

        if (bullets[bulletID].piercing == 0)
        {
            DestroyBullet(bulletID, true);
            return true;
        }

        return false;
    }

    public void DestroyBullet(int bulletID, bool reverseEffect)
    {
        ParticleSystemRenderer holder = Instantiate(particle, bullets[bulletID].obj.position,
            bullets[bulletID].obj.rotation).GetComponent<ParticleSystemRenderer>();
        holder.transform.rotation *= Quaternion.Euler(0, 0, 180f);

        if (!reverseEffect)
        {
            holder.material = bullets[bulletID].material;
            holder.trailMaterial = bullets[bulletID].material;
        }
        else
        {
            holder.material = bullets[bulletID].target.variant.border;
            holder.trailMaterial = bullets[bulletID].target.variant.border;
        }

        Recycler.AddRecyclable(bullets[bulletID].obj);
        bullets.RemoveAt(bulletID);
    }

    public void DestroyAllBullets()
    {
        for(int i = 0; i < bullets.Count; i++)
            Destroy(bullets[i].obj.gameObject);
        bullets = new List<ActiveBullets>();
    }
}
