using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{

    // Contains all active coins in scene
    [System.Serializable]
    public class ActiveEnemies
    {
        // Constructor
        public ActiveEnemies(Transform Object, float Speed)
        {
            this.Object = Object;
            this.Speed = Speed;
        }

        // Class variables
        public Transform Object { get; set; }
        public float Speed { get; set; }

    }
    public List<ActiveEnemies> Enemies;

    // Handles bullet movement 
    public void Update()
    {
        for (int i = 0; i < Enemies.Count; i++)
            try
            {
                Enemies[i].Object.position += Enemies[i].Object.up * Enemies[i].Speed * Time.deltaTime;
            }
            catch
            {
                Enemies.RemoveAt(i);
                i--;
            }
    }

    // Register bullet as an ActiveBullets 
    public void RegisterEnemy(Transform a, float b)
    {
        Enemies.Add(new ActiveEnemies(a, b));
    }
}
