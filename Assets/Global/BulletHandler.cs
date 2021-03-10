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
        public ActiveBullets(Transform Object, float Speed)
        {
            this.Object = Object;
            this.Speed = Speed;
        }

        // Class variables
        public Transform Object { get; set; }
        public float Speed { get; set; }

    }
    public List<ActiveBullets> Bullets;

    // Handles bullet movement 
    public void Update()
    {
        for (int i = 0; i < Bullets.Count; i++)
            try
            {
                Bullets[i].Object.position += Bullets[i].Object.up * Bullets[i].Speed * Time.deltaTime;
            }
            catch
            {
                Bullets.RemoveAt(i);
                i--;
            }
    }

    // Register bullet as an ActiveBullets 
    public void RegisterBullet(Transform a, float b)
    {
        Bullets.Add(new ActiveBullets(a, b));
    }
}
