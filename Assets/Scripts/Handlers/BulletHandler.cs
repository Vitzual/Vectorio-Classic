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
        public ActiveBullets(Transform obj, ActiveEnemy target, float speed, int piercing, float damage, bool tracking)
        {
            this.obj = obj;
            this.target = target;
            this.speed = speed;
            this.piercing = piercing;
            this.damage = damage;
            this.tracking = tracking;

            ignore = new List<Transform>();
        }

        // Class variables
        public Transform obj;
        public ActiveEnemy target;
        public float speed;
        public int piercing;
        public float damage;
        public bool tracking;
        public List<Transform> ignore;

    }
    public List<ActiveBullets> bullets;

    public LayerMask enemyLayer;

    // Called when awoken
    public void Start()
    {
        Events.active.onBulletFired += RegisterBullet;
    }

    // Handles bullet movement and hit detection frame-by-frame
    public void Update()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (bullets[i].obj != null)
            {
                if (bullets[i].tracking && bullets[i].target.obj != null)
                {
                    float step = bullets[i].speed * Time.deltaTime;
                    bullets[i].obj.position = Vector2.MoveTowards(bullets[i].obj.position, bullets[i].target.obj.transform.position, step);
                }
                else
                {
                    bullets[i].obj.position += bullets[i].obj.up * bullets[i].speed * Time.deltaTime;
                }
                RaycastHit2D hit = Physics2D.Raycast(bullets[i].obj.position, bullets[i].obj.up, 5f, enemyLayer);
                if (hit.collider != null && !bullets[i].ignore.Contains(hit.collider.transform))
                    if (OnHit(i, hit.collider.transform)) { i--; continue; }
            }
            else
            {
                bullets.RemoveAt(i);
                i--;
            }
        }
    }

    // Registers a bullet to be handled by the updater in this script
    public void RegisterBullet(Bullet bullet)
    {
        bullets.Add(new ActiveBullets(bullet.obj, bullet.target, bullet.speed, bullet.pierces, bullet.damage, bullet.tracking));
    }

    // Called when a hit is detected in the updater 
    public bool OnHit(int bulletID, Transform other)
    {
        // Add the other transform to the ignore list for future collisions
        bullets[bulletID].ignore.Add(other);
        bullets[bulletID].target.script.DamageEnemy(bullets[bulletID].damage);
        bullets[bulletID].piercing--;

        if (bullets[bulletID].piercing == 0)
        {
            bullets.RemoveAt(bulletID);
            return true;
        }

        return false;
    }
}
