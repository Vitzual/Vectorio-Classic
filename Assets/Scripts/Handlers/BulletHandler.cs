using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    // Contains all active bullets in scene
    public List<DefaultBullet> bullets;
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
            if (bullets[a] != null && !bullets[a].recycling)
            {
                bullets[a].time -= Time.deltaTime;
                if (bullets[a].time <= 0)
                {
                    bullets[a].DestroyBullet(bullets[a].turret.material);
                    a--;
                }
                else bullets[a].Move();
            }
            else
            {
                bullets.RemoveAt(a);
                a--;
            }
        }
    }

    // Registers a bullet to be handled by the updater in this script
    public void RegisterBullet(DefaultBullet bullet)
    {
        bullets.Add(bullet);
    }


    // Destroys all active bullets
    public void DestroyAllBullets()
    {
        for(int i = 0; i < bullets.Count; i++)
            bullets[i].DestroyBullet(bullets[i].turret.material);
        bullets = new List<DefaultBullet>();
    }
}
