using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    // If the enemy destroys a building, play this sound
    public AudioSource BuildingGoDeadSound;
    public Survival survival;

    // Contains all active enemies in the scene
    [System.Serializable]
    public class ActiveEnemies
    {
        // Constructor
        public ActiveEnemies(Transform Object, float Speed, int Damage)
        {
            this.Object = Object;
            this.Speed = Speed;
            this.Damage = Damage;

            ObjectClass = Object.GetComponent<EnemyClass>();
        }

        // Class variables
        public Transform Object { get; set; }
        public float Speed { get; set; }
        public int Damage { get; set; }
        public EnemyClass ObjectClass { get; set; }

    }
    public List<ActiveEnemies> Enemies;

    public LayerMask BuildingLayer;
    public bool isMenu = false;

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "Menu") isMenu = true;
    }


    // Handles enemy movement every frame
    public void Update()
    {
        if (isMenu)
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
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
        }
        else
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                try
                {
                    if (Enemies[i].ObjectClass.target == null) Enemies[i].ObjectClass.FindNearestDefence();
                    Enemies[i].Object.position += Enemies[i].Object.up * Enemies[i].Speed * Time.deltaTime;
                }
                catch
                {
                    Enemies.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    // Registers an enemy to then be handled by the controller 
    public void RegisterEnemy(Transform Object, float Speed, int Damage)
    {
        Enemies.Add(new ActiveEnemies(Object, Speed, Damage));
    }

    // Request information about an enemy using the transform attached
    public int RequestID(Transform obj)
    {
        for (int i = 0; i < Enemies.Count; i++)
            if (Enemies[i].Object == obj) return i;
        return -1;
    }

    // Called when a hit is detected in the updater 
    public void OnHit(int enemyID, Transform other)
    {
        if (isMenu)
        {
            Enemies[enemyID].ObjectClass.KillEntity();
            Enemies.RemoveAt(enemyID);
            return;
        }

        Vector3 pos = new Vector3(other.position.x, other.position.y, other.position.z);

        other.GetComponent<TileClass>().DamageTile(Enemies[enemyID].Damage);

        if (other != null && other.GetComponent<TileClass>().health > 0)
        {
            Enemies[enemyID].ObjectClass.KillEntity();
            Enemies.RemoveAt(enemyID);

            return;
        }

        BuildingGoDeadSound.Play();
        survival.SetLastHit(pos);
        return;
    }

    public Transform findClosest(Vector3 pos)
    {
        Transform result = null;
        float closest = float.PositiveInfinity;
        foreach (ActiveEnemies enemy in Enemies)
        {
            float distance = Vector2.Distance(enemy.Object.position, pos);
            if (distance < closest)
            {
                result = enemy.Object;
                closest = distance;
            }
        }
        return result;
    }
}
