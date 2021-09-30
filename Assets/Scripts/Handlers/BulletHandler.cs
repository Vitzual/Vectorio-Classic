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
        public ActiveBullets(DefaultBullet bullet, BaseEntity target)
        {
            this.bullet = bullet;
            obj = bullet.transform;

            if (target != null)
            {
                this.target = target;
                bullet.tracking = true;
            }
            else bullet.tracking = false;
        }

        // Class variables
        public DefaultBullet bullet;
        public Transform obj;
        public BaseEntity target;

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
        if (Settings.paused) return;

        for (int a = 0; a < bullets.Count; a++)
        {
            if (bullets[a].bullet != null && !bullets[a].bullet.recycling)
            {
                bullets[a].bullet.time -= Time.deltaTime;
                if (bullets[a].bullet.time <= 0)
                {
                    bullets[a].bullet.DestroyBullet(bullets[a].bullet.turret.material);
                    a--;
                }
                else
                {
                    if (bullets[a].bullet.tracking && bullets[a].target != null)
                    {
                        float step = bullets[a].bullet.speed * Time.deltaTime;
                        bullets[a].obj.position = Vector2.MoveTowards(bullets[a].obj.position, bullets[a].target.transform.transform.position, step);
                    }
                    else
                    {
                        bullets[a].obj.position += bullets[a].obj.up * bullets[a].bullet.speed * Time.deltaTime;
                    }
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
    public void RegisterBullet(DefaultBullet bullet, BaseEntity target)
    {
        bullets.Add(new ActiveBullets(bullet, target));
    }


    // Destroys all active bullets
    public void DestroyAllBullets()
    {
        for(int i = 0; i < bullets.Count; i++)
            Recycler.AddRecyclable(bullets[i].obj);
        bullets = new List<ActiveBullets>();
    }
}
