using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletHandler : MonoBehaviour
{
    // Contains all active coins in scene
    [System.Serializable]
    public class ActiveBullets
    {
        // Constructor
        public ActiveBullets(Transform Object, EnemyBullet ObjectScript, float Speed, int Tracker, int Piercing, int Damage)
        {
            this.Object = Object;
            this.Speed = Speed;
            this.Tracker = Tracker;
            this.Piercing = Piercing;
            this.Damage = Damage;
            this.ObjectScript = ObjectScript;
        }

        // Class variables
        public Transform Object { get; set; }
        public EnemyBullet ObjectScript { get; set; }
        public float Speed { get; set; }
        public int Tracker { get; set; }
        public int Piercing { get; set; }
        public int Damage { get; set; }
        public List<Transform> IgnoreBuildings = new List<Transform>();

    }
    public List<ActiveBullets> Bullets;

    public LayerMask BuildingLayer;

    // Handles bullet movement and hit detection frame-by-frame
    public void Update()
    {
        for (int i = 0; i < Bullets.Count; i++)
            try
            {
                Bullets[i].Object.position += Bullets[i].Object.up * Bullets[i].Speed * Time.deltaTime;
                if (Bullets[i].Tracker == 4)
                {
                    Bullets[i].Tracker = 1;
                    RaycastHit2D hit = Physics2D.Raycast(Bullets[i].Object.position, Bullets[i].Object.up, 1.5f, BuildingLayer);
                    if (hit.collider != null && !Bullets[i].IgnoreBuildings.Contains(hit.collider.transform))
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
    public void RegisterBullet(Transform bullet, EnemyBullet ObjectScript, float speed, int pierces, int damage)
    {
        Bullets.Add(new ActiveBullets(bullet, ObjectScript, speed, 1, pierces, damage));
    }

    // Called when a hit is detected in the updater 
    public bool OnHit(int bulletID, Transform other)
    {
        // Add the other transform to the ignore list for future collisions
        Bullets[bulletID].IgnoreBuildings.Add(other);
        other.GetComponent<DefaultBuilding>().DamageEntity(Bullets[bulletID].Damage);

        Bullets[bulletID].Piercing--;
        if (Bullets[bulletID].Piercing == 0)
        {
            Bullets[bulletID].ObjectScript.collide();
            Bullets.RemoveAt(bulletID);
            return true;
        }
        return false;
    }
}
